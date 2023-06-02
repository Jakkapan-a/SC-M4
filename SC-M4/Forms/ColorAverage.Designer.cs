namespace SC_M4.Forms
{
    partial class ColorAverage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorAverage));
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.rbTopLeft = new System.Windows.Forms.RadioButton();
            this.rbBottomLeft = new System.Windows.Forms.RadioButton();
            this.rbBottomRight = new System.Windows.Forms.RadioButton();
            this.rbTopRight = new System.Windows.Forms.RadioButton();
            this.rbFull = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nbWidth = new System.Windows.Forms.NumericUpDown();
            this.nbHeight = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.rbHeight = new System.Windows.Forms.Label();
            this.nbX = new System.Windows.Forms.NumericUpDown();
            this.nbY = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbY)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label1.Font = new System.Drawing.Font("Consolas", 15.75F);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "SET POINT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SC_M4.Properties.Resources.Point;
            this.pictureBox1.Location = new System.Drawing.Point(16, 81);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(217, 112);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // rbTopLeft
            // 
            this.rbTopLeft.AutoSize = true;
            this.rbTopLeft.Location = new System.Drawing.Point(16, 63);
            this.rbTopLeft.Name = "rbTopLeft";
            this.rbTopLeft.Size = new System.Drawing.Size(65, 17);
            this.rbTopLeft.TabIndex = 2;
            this.rbTopLeft.Text = "Top Left";
            this.rbTopLeft.UseVisualStyleBackColor = true;
            this.rbTopLeft.CheckedChanged += new System.EventHandler(this.rbTopLeft_CheckedChanged);
            // 
            // rbBottomLeft
            // 
            this.rbBottomLeft.AutoSize = true;
            this.rbBottomLeft.Location = new System.Drawing.Point(16, 195);
            this.rbBottomLeft.Name = "rbBottomLeft";
            this.rbBottomLeft.Size = new System.Drawing.Size(79, 17);
            this.rbBottomLeft.TabIndex = 2;
            this.rbBottomLeft.Text = "Bottom Left";
            this.rbBottomLeft.UseVisualStyleBackColor = true;
            this.rbBottomLeft.CheckedChanged += new System.EventHandler(this.rbTopLeft_CheckedChanged);
            // 
            // rbBottomRight
            // 
            this.rbBottomRight.AutoSize = true;
            this.rbBottomRight.Location = new System.Drawing.Point(147, 195);
            this.rbBottomRight.Name = "rbBottomRight";
            this.rbBottomRight.Size = new System.Drawing.Size(86, 17);
            this.rbBottomRight.TabIndex = 2;
            this.rbBottomRight.Text = "Bottom Right";
            this.rbBottomRight.UseVisualStyleBackColor = true;
            this.rbBottomRight.CheckedChanged += new System.EventHandler(this.rbTopLeft_CheckedChanged);
            // 
            // rbTopRight
            // 
            this.rbTopRight.AutoSize = true;
            this.rbTopRight.Location = new System.Drawing.Point(161, 63);
            this.rbTopRight.Name = "rbTopRight";
            this.rbTopRight.Size = new System.Drawing.Size(72, 17);
            this.rbTopRight.TabIndex = 2;
            this.rbTopRight.Text = "Top Right";
            this.rbTopRight.UseVisualStyleBackColor = true;
            this.rbTopRight.CheckedChanged += new System.EventHandler(this.rbTopLeft_CheckedChanged);
            // 
            // rbFull
            // 
            this.rbFull.AutoSize = true;
            this.rbFull.Location = new System.Drawing.Point(16, 227);
            this.rbFull.Name = "rbFull";
            this.rbFull.Size = new System.Drawing.Size(41, 17);
            this.rbFull.TabIndex = 2;
            this.rbFull.Text = "Full";
            this.rbFull.UseVisualStyleBackColor = true;
            this.rbFull.CheckedChanged += new System.EventHandler(this.rbTopLeft_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(16, 252);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(215, 3);
            this.label2.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 262);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Size";
            // 
            // nbWidth
            // 
            this.nbWidth.Location = new System.Drawing.Point(16, 359);
            this.nbWidth.Name = "nbWidth";
            this.nbWidth.Size = new System.Drawing.Size(97, 20);
            this.nbWidth.TabIndex = 5;
            this.nbWidth.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // nbHeight
            // 
            this.nbHeight.Location = new System.Drawing.Point(147, 359);
            this.nbHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nbHeight.Name = "nbHeight";
            this.nbHeight.Size = new System.Drawing.Size(86, 20);
            this.nbHeight.TabIndex = 5;
            this.nbHeight.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(158, 385);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 343);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Width";
            // 
            // rbHeight
            // 
            this.rbHeight.AutoSize = true;
            this.rbHeight.Location = new System.Drawing.Point(144, 343);
            this.rbHeight.Name = "rbHeight";
            this.rbHeight.Size = new System.Drawing.Size(38, 13);
            this.rbHeight.TabIndex = 7;
            this.rbHeight.Text = "Height";
            // 
            // nbX
            // 
            this.nbX.Location = new System.Drawing.Point(16, 307);
            this.nbX.Name = "nbX";
            this.nbX.Size = new System.Drawing.Size(97, 20);
            this.nbX.TabIndex = 5;
            this.nbX.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // nbY
            // 
            this.nbY.Location = new System.Drawing.Point(147, 307);
            this.nbY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nbY.Name = "nbY";
            this.nbY.Size = new System.Drawing.Size(86, 20);
            this.nbY.TabIndex = 5;
            this.nbY.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 291);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(144, 291);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Y";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(156, 227);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 6;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // ColorAverage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 419);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.rbHeight);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.nbY);
            this.Controls.Add(this.nbX);
            this.Controls.Add(this.nbHeight);
            this.Controls.Add(this.nbWidth);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbTopRight);
            this.Controls.Add(this.rbFull);
            this.Controls.Add(this.rbBottomRight);
            this.Controls.Add(this.rbBottomLeft);
            this.Controls.Add(this.rbTopLeft);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(268, 432);
            this.Name = "ColorAverage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ColorAverage";
            this.Load += new System.EventHandler(this.ColorAverage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton rbTopLeft;
        private System.Windows.Forms.RadioButton rbBottomLeft;
        private System.Windows.Forms.RadioButton rbBottomRight;
        private System.Windows.Forms.RadioButton rbTopRight;
        private System.Windows.Forms.RadioButton rbFull;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nbWidth;
        private System.Windows.Forms.NumericUpDown nbHeight;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label rbHeight;
        private System.Windows.Forms.NumericUpDown nbX;
        private System.Windows.Forms.NumericUpDown nbY;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSelect;
    }
}