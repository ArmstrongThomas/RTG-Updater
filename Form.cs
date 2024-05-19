using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Linq;

namespace RTG_Updater
{
    public partial class Form : System.Windows.Forms.Form
    {
        private string expectedHash = ""; // Declare expectedHash as a class-level field

		public Form()
		{
			InitializeComponent();
			this.AllowDrop = true; // Enable drag-and-drop on the form
			this.DragEnter += new DragEventHandler(Form_DragEnter);
			this.DragDrop += new DragEventHandler(Form_DragDrop);
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
				string hashFileName = Path.GetFileNameWithoutExtension(droppedFile) + "_hash.txt";
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
            string extractPath = AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                // Disable buttons during update
                updateButton.Enabled = false;
                launchButton.Enabled = false;

                // Download the ZIP file with progress
                statusLabel.Text = "Downloading update...";
                await DownloadFileWithProgress(zipUrl, zipPath);

                // Extract the ZIP file
                statusLabel.Text = "Extracting update...";
                ZipFile.ExtractToDirectory(zipPath, extractPath); // Overwrite existing files

                // Delete the downloaded ZIP file
                File.Delete(zipPath);

                // Verify the extracted file's hash
                string extractedExecutablePath = Path.Combine(extractPath, "RTGaurdia", "RTGaurdia.exe");
                if (File.Exists(extractedExecutablePath)) // Check if the file exists
                {
                    string downloadedHash = CalculateSHA256Hash(extractedExecutablePath);

                    if (downloadedHash == this.expectedHash) // Compare with the class-level field
                    {
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
                statusLabel.Text = "Error updating: " + ex.Message;
                updateButton.Enabled = true; // Re-enable in case of error
            }
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



    }
}
