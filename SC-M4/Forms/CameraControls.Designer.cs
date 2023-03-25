namespace SC_M4.Forms
{
    partial class CameraControls
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
            this.btSave = new System.Windows.Forms.Button();
            this.nExposure = new System.Windows.Forms.NumericUpDown();
            this.nTilt = new System.Windows.Forms.NumericUpDown();
            this.nPan = new System.Windows.Forms.NumericUpDown();
            this.nZoom = new System.Windows.Forms.NumericUpDown();
            this.nFocus = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tExposure = new System.Windows.Forms.TrackBar();
            this.tTilt = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.tPan = new System.Windows.Forms.TrackBar();
            this.tZoom = new System.Windows.Forms.TrackBar();
            this.tFocus = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.nExposure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nTilt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nFocus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tExposure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tTilt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tPan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tFocus)).BeginInit();
            this.SuspendLayout();
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(265, 303);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 21;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // nExposure
            // 
            this.nExposure.Location = new System.Drawing.Point(274, 272);
            this.nExposure.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nExposure.Name = "nExposure";
            this.nExposure.Size = new System.Drawing.Size(65, 20);
            this.nExposure.TabIndex = 20;
            this.nExposure.ValueChanged += new System.EventHandler(this._ValueChanged);
            // 
            // nTilt
            // 
            this.nTilt.Location = new System.Drawing.Point(274, 227);
            this.nTilt.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nTilt.Name = "nTilt";
            this.nTilt.Size = new System.Drawing.Size(65, 20);
            this.nTilt.TabIndex = 19;
            this.nTilt.ValueChanged += new System.EventHandler(this._ValueChanged);
            // 
            // nPan
            // 
            this.nPan.Location = new System.Drawing.Point(274, 184);
            this.nPan.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nPan.Name = "nPan";
            this.nPan.Size = new System.Drawing.Size(65, 20);
            this.nPan.TabIndex = 18;
            this.nPan.ValueChanged += new System.EventHandler(this._ValueChanged);
            // 
            // nZoom
            // 
            this.nZoom.Location = new System.Drawing.Point(274, 133);
            this.nZoom.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nZoom.Name = "nZoom";
            this.nZoom.Size = new System.Drawing.Size(65, 20);
            this.nZoom.TabIndex = 17;
            this.nZoom.ValueChanged += new System.EventHandler(this._ValueChanged);
            // 
            // nFocus
            // 
            this.nFocus.Location = new System.Drawing.Point(274, 77);
            this.nFocus.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nFocus.Name = "nFocus";
            this.nFocus.Size = new System.Drawing.Size(65, 20);
            this.nFocus.TabIndex = 16;
            this.nFocus.ValueChanged += new System.EventHandler(this._ValueChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.label3.Font = new System.Drawing.Font("Cascadia Code Light", 15.75F, System.Drawing.FontStyle.Italic);
            this.label3.Location = new System.Drawing.Point(21, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(310, 49);
            this.label3.TabIndex = 15;
            this.label3.Text = "Camera Cobtrol";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 278);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Exposure";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(44, 233);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Tilt";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Pan";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Zoom";
            // 
            // tExposure
            // 
            this.tExposure.Location = new System.Drawing.Point(75, 270);
            this.tExposure.Maximum = 1000;
            this.tExposure.Name = "tExposure";
            this.tExposure.Size = new System.Drawing.Size(193, 45);
            this.tExposure.TabIndex = 8;
            this.tExposure.ValueChanged += new System.EventHandler(this._ValueChanged);
            // 
            // tTilt
            // 
            this.tTilt.Location = new System.Drawing.Point(75, 225);
            this.tTilt.Maximum = 1000;
            this.tTilt.Name = "tTilt";
            this.tTilt.Size = new System.Drawing.Size(193, 45);
            this.tTilt.TabIndex = 7;
            this.tTilt.ValueChanged += new System.EventHandler(this._ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Focus";
            // 
            // tPan
            // 
            this.tPan.Location = new System.Drawing.Point(75, 182);
            this.tPan.Maximum = 1000;
            this.tPan.Name = "tPan";
            this.tPan.Size = new System.Drawing.Size(193, 45);
            this.tPan.TabIndex = 6;
            this.tPan.ValueChanged += new System.EventHandler(this._ValueChanged);
            // 
            // tZoom
            // 
            this.tZoom.Location = new System.Drawing.Point(75, 131);
            this.tZoom.Maximum = 1000;
            this.tZoom.Name = "tZoom";
            this.tZoom.Size = new System.Drawing.Size(193, 45);
            this.tZoom.TabIndex = 9;
            this.tZoom.ValueChanged += new System.EventHandler(this._ValueChanged);
            // 
            // tFocus
            // 
            this.tFocus.Location = new System.Drawing.Point(75, 75);
            this.tFocus.Maximum = 1000;
            this.tFocus.Name = "tFocus";
            this.tFocus.Size = new System.Drawing.Size(193, 45);
            this.tFocus.TabIndex = 5;
            this.tFocus.ValueChanged += new System.EventHandler(this._ValueChanged);
            // 
            // CameraControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 338);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.nExposure);
            this.Controls.Add(this.nTilt);
            this.Controls.Add(this.nPan);
            this.Controls.Add(this.nZoom);
            this.Controls.Add(this.nFocus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tExposure);
            this.Controls.Add(this.tTilt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tPan);
            this.Controls.Add(this.tZoom);
            this.Controls.Add(this.tFocus);
            this.MaximumSize = new System.Drawing.Size(367, 377);
            this.MinimumSize = new System.Drawing.Size(367, 377);
            this.Name = "CameraControls";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Camera Control";
            this.Load += new System.EventHandler(this.CameraControls_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nExposure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nTilt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nFocus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tExposure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tTilt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tPan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tFocus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.NumericUpDown nExposure;
        private System.Windows.Forms.NumericUpDown nTilt;
        private System.Windows.Forms.NumericUpDown nPan;
        private System.Windows.Forms.NumericUpDown nZoom;
        private System.Windows.Forms.NumericUpDown nFocus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar tExposure;
        private System.Windows.Forms.TrackBar tTilt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar tPan;
        private System.Windows.Forms.TrackBar tZoom;
        private System.Windows.Forms.TrackBar tFocus;
    }
}