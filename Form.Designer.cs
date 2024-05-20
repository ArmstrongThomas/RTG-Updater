namespace RTG_Updater
{
    partial class Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form));
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.updateButton = new System.Windows.Forms.Button();
			this.launchButton = new System.Windows.Forms.Button();
			this.statusLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// progressBar
			// 
			this.progressBar.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.progressBar.Location = new System.Drawing.Point(11, 91);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(273, 33);
			this.progressBar.TabIndex = 0;
			this.progressBar.Click += new System.EventHandler(this.progressBar_Click);
			// 
			// updateButton
			// 
			this.updateButton.Location = new System.Drawing.Point(11, 35);
			this.updateButton.Name = "updateButton";
			this.updateButton.Size = new System.Drawing.Size(131, 50);
			this.updateButton.TabIndex = 1;
			this.updateButton.Text = "Download Update";
			this.updateButton.UseVisualStyleBackColor = true;
			this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
			// 
			// launchButton
			// 
			this.launchButton.Location = new System.Drawing.Point(149, 35);
			this.launchButton.Name = "launchButton";
			this.launchButton.Size = new System.Drawing.Size(135, 50);
			this.launchButton.TabIndex = 2;
			this.launchButton.Text = "Launch RTG";
			this.launchButton.UseVisualStyleBackColor = true;
			this.launchButton.Click += new System.EventHandler(this.launchButton_Click);
			// 
			// statusLabel
			// 
			this.statusLabel.AutoSize = true;
			this.statusLabel.Location = new System.Drawing.Point(8, 9);
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(115, 13);
			this.statusLabel.TabIndex = 3;
			this.statusLabel.Text = "INFORMATION HERE";
			this.statusLabel.Click += new System.EventHandler(this.statusLabel_Click);
			// 
			// Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.AntiqueWhite;
			this.ClientSize = new System.Drawing.Size(295, 135);
			this.Controls.Add(this.statusLabel);
			this.Controls.Add(this.launchButton);
			this.Controls.Add(this.updateButton);
			this.Controls.Add(this.progressBar);
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Return to Gaurdia - Updater";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button launchButton;
        private System.Windows.Forms.Label statusLabel;
    }
}
