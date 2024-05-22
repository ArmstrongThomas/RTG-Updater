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
			this.closeButton = new System.Windows.Forms.Label();
			this.title = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// progressBar
			// 
			this.progressBar.BackColor = System.Drawing.Color.Honeydew;
			this.progressBar.Location = new System.Drawing.Point(25, 134);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(288, 33);
			this.progressBar.TabIndex = 0;
			this.progressBar.Click += new System.EventHandler(this.progressBar_Click);
			// 
			// updateButton
			// 
			this.updateButton.BackColor = System.Drawing.Color.Ivory;
			this.updateButton.FlatAppearance.BorderColor = System.Drawing.Color.Honeydew;
			this.updateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.updateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.updateButton.ForeColor = System.Drawing.SystemColors.Desktop;
			this.updateButton.Location = new System.Drawing.Point(25, 78);
			this.updateButton.Name = "updateButton";
			this.updateButton.Size = new System.Drawing.Size(131, 50);
			this.updateButton.TabIndex = 1;
			this.updateButton.Text = "Download";
			this.updateButton.UseVisualStyleBackColor = false;
			this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
			// 
			// launchButton
			// 
			this.launchButton.BackColor = System.Drawing.Color.Ivory;
			this.launchButton.FlatAppearance.BorderColor = System.Drawing.Color.Honeydew;
			this.launchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.launchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.launchButton.ForeColor = System.Drawing.SystemColors.Desktop;
			this.launchButton.Location = new System.Drawing.Point(178, 78);
			this.launchButton.Name = "launchButton";
			this.launchButton.Size = new System.Drawing.Size(135, 50);
			this.launchButton.TabIndex = 2;
			this.launchButton.Text = "Launch";
			this.launchButton.UseVisualStyleBackColor = false;
			this.launchButton.Click += new System.EventHandler(this.launchButton_Click);
			// 
			// statusLabel
			// 
			this.statusLabel.AutoSize = true;
			this.statusLabel.BackColor = System.Drawing.Color.Transparent;
			this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statusLabel.ForeColor = System.Drawing.Color.Ivory;
			this.statusLabel.Location = new System.Drawing.Point(22, 51);
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(127, 15);
			this.statusLabel.TabIndex = 3;
			this.statusLabel.Text = "INFORMATION HERE";
			this.statusLabel.Click += new System.EventHandler(this.statusLabel_Click);
			// 
			// closeButton
			// 
			this.closeButton.AutoSize = true;
			this.closeButton.BackColor = System.Drawing.Color.Transparent;
			this.closeButton.Font = new System.Drawing.Font("Lucida Sans Unicode", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.closeButton.ForeColor = System.Drawing.Color.Red;
			this.closeButton.Location = new System.Drawing.Point(311, 6);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(19, 18);
			this.closeButton.TabIndex = 4;
			this.closeButton.Text = "x";
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// title
			// 
			this.title.AutoSize = true;
			this.title.BackColor = System.Drawing.Color.Transparent;
			this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.title.ForeColor = System.Drawing.Color.Ivory;
			this.title.Location = new System.Drawing.Point(79, 15);
			this.title.Name = "title";
			this.title.Size = new System.Drawing.Size(175, 24);
			this.title.TabIndex = 5;
			this.title.Text = "Legends of Gaurdia";
			this.title.Click += new System.EventHandler(this.title_Click);
			// 
			// Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.AntiqueWhite;
			this.BackgroundImage = global::RTG_Updater.Properties.Resources.bg;
			this.ClientSize = new System.Drawing.Size(336, 185);
			this.Controls.Add(this.title);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.statusLabel);
			this.Controls.Add(this.launchButton);
			this.Controls.Add(this.updateButton);
			this.Controls.Add(this.progressBar);
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Legends of Gaurdia - Updater";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button launchButton;
        private System.Windows.Forms.Label statusLabel;
		private System.Windows.Forms.Label closeButton;
		private System.Windows.Forms.Label title;
	}
}
