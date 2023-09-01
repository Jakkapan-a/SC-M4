namespace SC_M4.Forms.Show
{
    partial class ManualTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManualTest));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbTitle = new System.Windows.Forms.Label();
            this.lbVoltage = new System.Windows.Forms.Label();
            this.lbAmp = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(13, 68);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(792, 471);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lbTitle
            // 
            this.lbTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTitle.BackColor = System.Drawing.Color.Yellow;
            this.lbTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lbTitle.Location = new System.Drawing.Point(13, 9);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(716, 56);
            this.lbTitle.TabIndex = 1;
            this.lbTitle.Text = "--------------";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbVoltage
            // 
            this.lbVoltage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbVoltage.AutoSize = true;
            this.lbVoltage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbVoltage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbVoltage.ForeColor = System.Drawing.Color.Blue;
            this.lbVoltage.Location = new System.Drawing.Point(736, 13);
            this.lbVoltage.Name = "lbVoltage";
            this.lbVoltage.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.lbVoltage.Size = new System.Drawing.Size(22, 19);
            this.lbVoltage.TabIndex = 2;
            this.lbVoltage.Text = "-";
            // 
            // lbAmp
            // 
            this.lbAmp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAmp.AutoSize = true;
            this.lbAmp.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbAmp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbAmp.ForeColor = System.Drawing.Color.Red;
            this.lbAmp.Location = new System.Drawing.Point(736, 41);
            this.lbAmp.Name = "lbAmp";
            this.lbAmp.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.lbAmp.Size = new System.Drawing.Size(22, 19);
            this.lbAmp.TabIndex = 2;
            this.lbAmp.Text = "-";
            // 
            // ManualTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(817, 551);
            this.Controls.Add(this.lbAmp);
            this.Controls.Add(this.lbVoltage);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ManualTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ManualTest";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ManualTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label lbTitle;
        public System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Label lbVoltage;
        public System.Windows.Forms.Label lbAmp;
    }
}