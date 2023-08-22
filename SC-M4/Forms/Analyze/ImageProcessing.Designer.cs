namespace SC_M4.Forms.Analyze
{
    partial class ImageProcessing
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cAM1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cAM2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cAM1ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cAM2ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.analyzeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBoxMaster = new System.Windows.Forms.PictureBox();
            this.pictureBoxCurrent = new System.Windows.Forms.PictureBox();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrent)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 677);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1089, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingToolStripMenuItem,
            this.analyzeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1089, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getImageToolStripMenuItem,
            this.getCurrentToolStripMenuItem});
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.settingToolStripMenuItem.Text = "Setting";
            // 
            // getImageToolStripMenuItem
            // 
            this.getImageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cAM1ToolStripMenuItem,
            this.cAM2ToolStripMenuItem});
            this.getImageToolStripMenuItem.Name = "getImageToolStripMenuItem";
            this.getImageToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.getImageToolStripMenuItem.Text = "Get Master";
            // 
            // cAM1ToolStripMenuItem
            // 
            this.cAM1ToolStripMenuItem.Name = "cAM1ToolStripMenuItem";
            this.cAM1ToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.cAM1ToolStripMenuItem.Text = "CAM 1";
            this.cAM1ToolStripMenuItem.Click += new System.EventHandler(this.cAM1ToolStripMenuItem_Click);
            // 
            // cAM2ToolStripMenuItem
            // 
            this.cAM2ToolStripMenuItem.Name = "cAM2ToolStripMenuItem";
            this.cAM2ToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.cAM2ToolStripMenuItem.Text = "CAM 2";
            this.cAM2ToolStripMenuItem.Click += new System.EventHandler(this.cAM2ToolStripMenuItem_Click);
            // 
            // getCurrentToolStripMenuItem
            // 
            this.getCurrentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cAM1ToolStripMenuItem1,
            this.cAM2ToolStripMenuItem1});
            this.getCurrentToolStripMenuItem.Name = "getCurrentToolStripMenuItem";
            this.getCurrentToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.getCurrentToolStripMenuItem.Text = "Get Current";
            // 
            // cAM1ToolStripMenuItem1
            // 
            this.cAM1ToolStripMenuItem1.Name = "cAM1ToolStripMenuItem1";
            this.cAM1ToolStripMenuItem1.Size = new System.Drawing.Size(110, 22);
            this.cAM1ToolStripMenuItem1.Text = "CAM 1";
            this.cAM1ToolStripMenuItem1.Click += new System.EventHandler(this.cAM1ToolStripMenuItem1_Click);
            // 
            // cAM2ToolStripMenuItem1
            // 
            this.cAM2ToolStripMenuItem1.Name = "cAM2ToolStripMenuItem1";
            this.cAM2ToolStripMenuItem1.Size = new System.Drawing.Size(110, 22);
            this.cAM2ToolStripMenuItem1.Text = "CAM 2";
            this.cAM2ToolStripMenuItem1.Click += new System.EventHandler(this.cAM2ToolStripMenuItem1_Click);
            // 
            // analyzeToolStripMenuItem
            // 
            this.analyzeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compareToolStripMenuItem});
            this.analyzeToolStripMenuItem.Name = "analyzeToolStripMenuItem";
            this.analyzeToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.analyzeToolStripMenuItem.Text = "Analyze";
            // 
            // compareToolStripMenuItem
            // 
            this.compareToolStripMenuItem.Name = "compareToolStripMenuItem";
            this.compareToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.compareToolStripMenuItem.Text = "Compare";
            this.compareToolStripMenuItem.Click += new System.EventHandler(this.compareToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(12, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.pictureBoxMaster);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.pictureBoxCurrent);
            this.splitContainer1.Size = new System.Drawing.Size(1065, 495);
            this.splitContainer1.SplitterDistance = 532;
            this.splitContainer1.TabIndex = 2;
            // 
            // pictureBoxMaster
            // 
            this.pictureBoxMaster.BackColor = System.Drawing.Color.Black;
            this.pictureBoxMaster.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxMaster.Name = "pictureBoxMaster";
            this.pictureBoxMaster.Size = new System.Drawing.Size(467, 421);
            this.pictureBoxMaster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxMaster.TabIndex = 0;
            this.pictureBoxMaster.TabStop = false;
            // 
            // pictureBoxCurrent
            // 
            this.pictureBoxCurrent.BackColor = System.Drawing.Color.Black;
            this.pictureBoxCurrent.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxCurrent.Name = "pictureBoxCurrent";
            this.pictureBoxCurrent.Size = new System.Drawing.Size(467, 421);
            this.pictureBoxCurrent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxCurrent.TabIndex = 0;
            this.pictureBoxCurrent.TabStop = false;
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(12, 557);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(1065, 117);
            this.txtOutput.TabIndex = 3;
            this.txtOutput.Text = "";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1002, 528);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Deff";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ImageProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1089, 699);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ImageProcessing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImageProcessing";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ImageProcessing_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBoxMaster;
        private System.Windows.Forms.PictureBox pictureBoxCurrent;
        private System.Windows.Forms.ToolStripMenuItem getImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cAM1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cAM2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getCurrentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cAM1ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cAM2ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem analyzeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compareToolStripMenuItem;
    }
}