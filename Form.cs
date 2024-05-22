using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTG_Updater
{
    public partial class Form : System.Windows.Forms.Form
    {
        private string expectedHash = ""; // Declare expectedHash as a class-level field
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;

		[DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();

		// Event handler for MouseDown on the form
		private void Form_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}
		public Form()
		{
			InitializeComponent();
			this.AllowDrop = true; // Enable drag-and-drop on the form
			this.DragEnter += new DragEventHandler(Form_DragEnter);
			this.DragDrop += new DragEventHandler(Form_DragDrop);

			// 
			FormBorderStyle = FormBorderStyle.None;
			DoubleBuffered = true;


			updateButton.BringToFront();
			launchButton.BringToFront();
			statusLabel.BringToFront();
			progressBar.BringToFront();

			this.MouseDown += new MouseEventHandler(Form_MouseDown);

		}

		private void Form_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy; // Show copy cursor
			else
				e.Effect = DragDropEffects.None; // Show 'not allowed' cursor
		}

		private void Form_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			string droppedFile = files[0];

			if (Path.GetExtension(droppedFile).ToLowerInvariant() == ".exe")
			{
				string hash = CalculateSHA256Hash(droppedFile);
				string hashFileName = "hash.txt";
				string hashFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, hashFileName);

				File.WriteAllText(hashFilePath, hash); // Use the synchronous method
				statusLabel.Text = $"Hash calculated and saved to {hashFileName}";
			}
			else
			{
				statusLabel.Text = "Error: Please drop an .exe file.";
			}
		}


		private async void Form1_Load(object sender, EventArgs e)
        {
            await CheckForUpdates(); // Check for updates when the form loads
        }

        private async Task CheckForUpdates()
        {
            string hashUrl = "https://gaurdia.page/hash.txt";
            string executablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RTGaurdia", "RTGaurdia.exe");

            try
            {
                // Download the hash file from the web server
                this.expectedHash = await DownloadHashFile(hashUrl);

                if (File.Exists(executablePath))
                {
                    // Calculate hash of the existing file
                    string actualHash = CalculateSHA256Hash(executablePath);

                    if (actualHash == this.expectedHash)
                    {
                        statusLabel.Text = "Game is installed and up-to-date. Ready to launch!";
                        updateButton.Enabled = false;
                        launchButton.Enabled = true;
                    }
                    else
                    {
                        statusLabel.Text = "Game update available. Please update.";
                        updateButton.Enabled = true;
                        launchButton.Enabled = false;
                    }
                }
                else
                {
                    statusLabel.Text = "Game not found. Please update.";
                    updateButton.Enabled = true;
                    launchButton.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Error fetching hash: " + ex.Message;
                // You might want to handle this error differently, 
                // like disabling the update button or retrying the download
            }
        }

        private string CalculateSHA256Hash(string filename)
        {
            using (SHA256 sha256 = SHA256.Create())
            using (FileStream fileStream = File.OpenRead(filename))
            {
                byte[] hashBytes = sha256.ComputeHash(fileStream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToUpperInvariant();
            }
        }

        private async Task<string> DownloadHashFile(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                // Send GET request to the hash file URL
                HttpResponseMessage response = await client.GetAsync(url);

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content as a string (assuming the hash is plain text)
                string hash = await response.Content.ReadAsStringAsync();
                return hash.Trim(); // Trim any extra whitespace
            }
        }

		private async void updateButton_Click(object sender, EventArgs e)
		{
			string zipUrl = "https://gaurdia.page/update.zip";
			string zipPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "update.zip");
			string oldGameFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RTGaurdia");
			string tempGameFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"RTGaurdia_{Guid.NewGuid()}");

			try
			{
				// Disable buttons during update
				updateButton.Enabled = false;
				launchButton.Enabled = false;

				// Download the ZIP file with progress
				statusLabel.Text = "Downloading update...";
				await DownloadFileWithProgress(zipUrl, zipPath);

				// Rename existing RTGaurdia folder
				if (Directory.Exists(oldGameFolder))
				{
					Directory.Move(oldGameFolder, tempGameFolder); // Temporary rename
				}

				// Extract directly to the root folder
				statusLabel.Text = "Extracting update...";
				using (var client = new HttpClient())
				{
					var response = await client.GetAsync(zipUrl);
					response.EnsureSuccessStatusCode(); // Check for successful download

					using (var zipStream = await response.Content.ReadAsStreamAsync())
					using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
					{
						foreach (ZipArchiveEntry entry in archive.Entries)
						{
							if (entry.Length > 0) // Skip empty directories
							{
								// Directly extract to the root folder
								string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, entry.FullName);

								// Create directory if it doesn't exist
								Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

								try
								{
									entry.ExtractToFile(destinationPath, true);
								}
								catch (Exception extractionEx)
								{
									File.AppendAllText("error_log.txt", $"Error extracting file {entry.FullName}: {extractionEx.Message}\n{extractionEx.StackTrace}\n");
									// Consider rethrowing the exception or handling the error differently
								}
							}
						}
					}
				}

				// Delete the downloaded ZIP file
				File.Delete(zipPath);

				// Verify the extracted file's hash
				string extractedExecutablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RTGaurdia", "RTGaurdia.exe");
				if (File.Exists(extractedExecutablePath))
				{
					string downloadedHash = CalculateSHA256Hash(extractedExecutablePath);

					if (downloadedHash == this.expectedHash) // Compare with the class-level field
					{
						// Delete the temporary (renamed) folder
						if (Directory.Exists(tempGameFolder))
						{
							Directory.Delete(tempGameFolder, true);
						}

						statusLabel.Text = "Update complete! Ready to launch.";
						updateButton.Enabled = false;
						launchButton.Enabled = true;
					}
					else
					{
						statusLabel.Text = $"Error: Downloaded file hash ({downloadedHash}) doesn't match expected hash ({this.expectedHash}). Please try again.";
						updateButton.Enabled = true; // Re-enable update button in case of a mismatch
					}
				}
				else
				{
					statusLabel.Text = "Error: Extracted file not found.";
					updateButton.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				// Revert renaming if an error occurs
				if (Directory.Exists(tempGameFolder))
				{
					Directory.Move(tempGameFolder, oldGameFolder); // Move it back
				}

				File.AppendAllText("error_log.txt", $"Error updating: {ex.Message}\n{ex.StackTrace}\n");
				statusLabel.Text = "Error updating: Check error_log.txt for details";
				updateButton.Enabled = true;
			}
		}

		private bool IsFileLocked(string filePath)
		{
			try
			{
				using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
				{
					stream.Close();
				}
			}
			catch (IOException)
			{
				// The file is unavailable because it is:
				// - being written to
				// - or being processed by another thread
				// - or does not exist (has already been processed)
				return true;
			}

			//file is not locked
			return false;
		}

		private async Task DownloadFileWithProgress(string url, string destinationPath)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    using (Stream streamToWriteTo = File.Open(destinationPath, FileMode.Create)) // Corrected FileMode here
                    {
                        long totalBytes = response.Content.Headers.ContentLength ?? -1;
                        long totalBytesRead = 0;
                        byte[] buffer = new byte[8192];
                        int bytesRead;

                        while ((bytesRead = await streamToReadFrom.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await streamToWriteTo.WriteAsync(buffer, 0, bytesRead);
                            totalBytesRead += bytesRead;

                            // Update progress bar
                            if (totalBytes != -1)
                            {
                                double progressPercentage = (
                                double)totalBytesRead / totalBytes * 100;
                                progressBar.Value = (int)progressPercentage;
                            }
                        }
                    }
                }
            }
        }

        private void launchButton_Click(object sender, EventArgs e)
        {
            string executablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RTGaurdia", "RTGaurdia.exe");

            try
            {
                Process.Start(executablePath);

                // Optionally, close the updater after launching the game
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error launching the game: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void progressBar_Click(object sender, EventArgs e)
        {
            // Placeholder for any specific action when the progress bar is clicked
        }

        private void statusLabel_Click(object sender, EventArgs e)
        {
            // Placeholder for any specific action when the status label is clicked
        }

		private void closeButton_Click(object sender, EventArgs e)
		{
			this.Close();

		}

		private void title_Click(object sender, EventArgs e)
		{

		}
	}
}
