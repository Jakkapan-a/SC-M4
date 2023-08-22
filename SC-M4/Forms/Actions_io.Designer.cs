namespace SC_M4.Forms
{
    partial class Actions_io
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
            this.tProgressBarTest = new System.Windows.Forms.ToolStripProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.nPIN = new System.Windows.Forms.NumericUpDown();
            this.btnIODelete = new System.Windows.Forms.Button();
            this.btnIOEdit = new System.Windows.Forms.Button();
            this.btnIOSave = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.dgvIO = new System.Windows.Forms.DataGridView();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nDelay = new System.Windows.Forms.NumericUpDown();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.nThreshold = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.btnRect = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.nServo = new System.Windows.Forms.NumericUpDown();
            this.tServo = new System.Windows.Forms.TrackBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdOff = new System.Windows.Forms.RadioButton();
            this.rdOn = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nAutoDelay = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdCompareModel = new System.Windows.Forms.RadioButton();
            this.rdTypeImage = new System.Windows.Forms.RadioButton();
            this.rdTypeServo = new System.Windows.Forms.RadioButton();
            this.rdTypeAuto = new System.Windows.Forms.RadioButton();
            this.rdTypeManual = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPIN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIO)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nDelay)).BeginInit();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nServo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tServo)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nAutoDelay)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tProgressBarTest});
            this.statusStrip1.Location = new System.Drawing.Point(0, 521);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(718, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tProgressBarTest
            // 
            this.tProgressBarTest.Name = "tProgressBarTest";
            this.tProgressBarTest.Size = new System.Drawing.Size(100, 16);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(12, 38);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.nPIN);
            this.splitContainer1.Panel1.Controls.Add(this.btnIODelete);
            this.splitContainer1.Panel1.Controls.Add(this.btnIOEdit);
            this.splitContainer1.Panel1.Controls.Add(this.btnIOSave);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.txtName);
            this.splitContainer1.Panel1.Controls.Add(this.dgvIO);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox5);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox7);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox6);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(694, 448);
            this.splitContainer1.SplitterDistance = 240;
            this.splitContainer1.TabIndex = 2;
            // 
            // nPIN
            // 
            this.nPIN.Location = new System.Drawing.Point(45, 407);
            this.nPIN.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nPIN.Name = "nPIN";
            this.nPIN.Size = new System.Drawing.Size(100, 20);
            this.nPIN.TabIndex = 5;
            // 
            // btnIODelete
            // 
            this.btnIODelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIODelete.BackgroundImage = global::SC_M4.Properties.Resources.delete_32;
            this.btnIODelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnIODelete.Enabled = false;
            this.btnIODelete.Location = new System.Drawing.Point(160, 384);
            this.btnIODelete.Name = "btnIODelete";
            this.btnIODelete.Size = new System.Drawing.Size(31, 27);
            this.btnIODelete.TabIndex = 4;
            this.btnIODelete.UseVisualStyleBackColor = true;
            this.btnIODelete.Click += new System.EventHandler(this.btnIODelete_Click);
            // 
            // btnIOEdit
            // 
            this.btnIOEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIOEdit.BackgroundImage = global::SC_M4.Properties.Resources.edit_property_32;
            this.btnIOEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnIOEdit.Location = new System.Drawing.Point(197, 384);
            this.btnIOEdit.Name = "btnIOEdit";
            this.btnIOEdit.Size = new System.Drawing.Size(31, 27);
            this.btnIOEdit.TabIndex = 4;
            this.btnIOEdit.UseVisualStyleBackColor = true;
            this.btnIOEdit.Click += new System.EventHandler(this.btnIOEdit_Click);
            // 
            // btnIOSave
            // 
            this.btnIOSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIOSave.Enabled = false;
            this.btnIOSave.Location = new System.Drawing.Point(160, 417);
            this.btnIOSave.Name = "btnIOSave";
            this.btnIOSave.Size = new System.Drawing.Size(75, 23);
            this.btnIOSave.TabIndex = 4;
            this.btnIOSave.Text = "Save";
            this.btnIOSave.UseVisualStyleBackColor = true;
            this.btnIOSave.Click += new System.EventHandler(this.btnIOSave_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 420);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "PIN :";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 394);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Name :";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(45, 391);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(99, 20);
            this.txtName.TabIndex = 1;
            // 
            // dgvIO
            // 
            this.dgvIO.AllowUserToAddRows = false;
            this.dgvIO.AllowUserToDeleteRows = false;
            this.dgvIO.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvIO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIO.Location = new System.Drawing.Point(4, 3);
            this.dgvIO.Name = "dgvIO";
            this.dgvIO.ReadOnly = true;
            this.dgvIO.RowHeadersVisible = false;
            this.dgvIO.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIO.Size = new System.Drawing.Size(231, 375);
            this.dgvIO.TabIndex = 0;
            this.dgvIO.SelectionChanged += new System.EventHandler(this.dgvIO_SelectionChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.nDelay);
            this.groupBox5.Location = new System.Drawing.Point(8, 381);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(437, 52);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Next Delay";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(209, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "ms";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Delay";
            // 
            // nDelay
            // 
            this.nDelay.Location = new System.Drawing.Point(64, 19);
            this.nDelay.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.nDelay.Name = "nDelay";
            this.nDelay.Size = new System.Drawing.Size(139, 20);
            this.nDelay.TabIndex = 0;
            this.nDelay.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.nThreshold);
            this.groupBox7.Controls.Add(this.label8);
            this.groupBox7.Controls.Add(this.btnLoadImage);
            this.groupBox7.Controls.Add(this.btnRect);
            this.groupBox7.Controls.Add(this.pictureBox1);
            this.groupBox7.Location = new System.Drawing.Point(8, 245);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(437, 130);
            this.groupBox7.TabIndex = 2;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Image";
            // 
            // nThreshold
            // 
            this.nThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nThreshold.Location = new System.Drawing.Point(275, 36);
            this.nThreshold.Maximum = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.nThreshold.Name = "nThreshold";
            this.nThreshold.Size = new System.Drawing.Size(156, 20);
            this.nThreshold.TabIndex = 6;
            this.nThreshold.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(272, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Threshold";
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadImage.Location = new System.Drawing.Point(356, 101);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(75, 23);
            this.btnLoadImage.TabIndex = 4;
            this.btnLoadImage.Text = "Load";
            this.btnLoadImage.UseVisualStyleBackColor = true;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // btnRect
            // 
            this.btnRect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRect.Location = new System.Drawing.Point(275, 101);
            this.btnRect.Name = "btnRect";
            this.btnRect.Size = new System.Drawing.Size(75, 23);
            this.btnRect.TabIndex = 4;
            this.btnRect.Text = "Rect";
            this.btnRect.UseVisualStyleBackColor = true;
            this.btnRect.Click += new System.EventHandler(this.btnRect_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(6, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(223, 104);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.nServo);
            this.groupBox6.Controls.Add(this.tServo);
            this.groupBox6.Location = new System.Drawing.Point(8, 171);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(437, 68);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Servo Position";
            // 
            // nServo
            // 
            this.nServo.Location = new System.Drawing.Point(294, 19);
            this.nServo.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.nServo.Name = "nServo";
            this.nServo.Size = new System.Drawing.Size(88, 20);
            this.nServo.TabIndex = 1;
            this.nServo.ValueChanged += new System.EventHandler(this.nServo_ValueChanged);
            // 
            // tServo
            // 
            this.tServo.Location = new System.Drawing.Point(6, 17);
            this.tServo.Maximum = 180;
            this.tServo.Name = "tServo";
            this.tServo.Size = new System.Drawing.Size(291, 45);
            this.tServo.TabIndex = 0;
            this.tServo.ValueChanged += new System.EventHandler(this.tServo_ValueChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Location = new System.Drawing.Point(8, 171);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(414, 68);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Servo Position";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdOff);
            this.groupBox3.Controls.Add(this.rdOn);
            this.groupBox3.Location = new System.Drawing.Point(227, 89);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(218, 76);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Manual";
            // 
            // rdOff
            // 
            this.rdOff.AutoSize = true;
            this.rdOff.Location = new System.Drawing.Point(6, 42);
            this.rdOff.Name = "rdOff";
            this.rdOff.Size = new System.Drawing.Size(45, 17);
            this.rdOff.TabIndex = 0;
            this.rdOff.TabStop = true;
            this.rdOff.Text = "OFF";
            this.rdOff.UseVisualStyleBackColor = true;
            // 
            // rdOn
            // 
            this.rdOn.AutoSize = true;
            this.rdOn.Location = new System.Drawing.Point(6, 19);
            this.rdOn.Name = "rdOn";
            this.rdOn.Size = new System.Drawing.Size(41, 17);
            this.rdOn.TabIndex = 0;
            this.rdOn.TabStop = true;
            this.rdOn.Text = "ON";
            this.rdOn.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.nAutoDelay);
            this.groupBox2.Location = new System.Drawing.Point(8, 89);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(213, 76);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Auto";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(190, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "ms";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Delay :";
            // 
            // nAutoDelay
            // 
            this.nAutoDelay.Location = new System.Drawing.Point(64, 46);
            this.nAutoDelay.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.nAutoDelay.Name = "nAutoDelay";
            this.nAutoDelay.Size = new System.Drawing.Size(120, 20);
            this.nAutoDelay.TabIndex = 0;
            this.nAutoDelay.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdCompareModel);
            this.groupBox1.Controls.Add(this.rdTypeImage);
            this.groupBox1.Controls.Add(this.rdTypeServo);
            this.groupBox1.Controls.Add(this.rdTypeAuto);
            this.groupBox1.Controls.Add(this.rdTypeManual);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(437, 75);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Type";
            // 
            // rdCompareModel
            // 
            this.rdCompareModel.AutoSize = true;
            this.rdCompareModel.Location = new System.Drawing.Point(163, 19);
            this.rdCompareModel.Name = "rdCompareModel";
            this.rdCompareModel.Size = new System.Drawing.Size(104, 17);
            this.rdCompareModel.TabIndex = 1;
            this.rdCompareModel.TabStop = true;
            this.rdCompareModel.Text = "Software version";
            this.rdCompareModel.UseVisualStyleBackColor = true;
            // 
            // rdTypeImage
            // 
            this.rdTypeImage.AutoSize = true;
            this.rdTypeImage.Location = new System.Drawing.Point(91, 42);
            this.rdTypeImage.Name = "rdTypeImage";
            this.rdTypeImage.Size = new System.Drawing.Size(54, 17);
            this.rdTypeImage.TabIndex = 0;
            this.rdTypeImage.TabStop = true;
            this.rdTypeImage.Text = "Image";
            this.rdTypeImage.UseVisualStyleBackColor = true;
            this.rdTypeImage.CheckedChanged += new System.EventHandler(this.type_Checked);
            // 
            // rdTypeServo
            // 
            this.rdTypeServo.AutoSize = true;
            this.rdTypeServo.Location = new System.Drawing.Point(91, 19);
            this.rdTypeServo.Name = "rdTypeServo";
            this.rdTypeServo.Size = new System.Drawing.Size(53, 17);
            this.rdTypeServo.TabIndex = 0;
            this.rdTypeServo.TabStop = true;
            this.rdTypeServo.Text = "Servo";
            this.rdTypeServo.UseVisualStyleBackColor = true;
            this.rdTypeServo.CheckedChanged += new System.EventHandler(this.type_Checked);
            // 
            // rdTypeAuto
            // 
            this.rdTypeAuto.AutoSize = true;
            this.rdTypeAuto.Location = new System.Drawing.Point(6, 42);
            this.rdTypeAuto.Name = "rdTypeAuto";
            this.rdTypeAuto.Size = new System.Drawing.Size(47, 17);
            this.rdTypeAuto.TabIndex = 0;
            this.rdTypeAuto.TabStop = true;
            this.rdTypeAuto.Text = "Auto";
            this.rdTypeAuto.UseVisualStyleBackColor = true;
            this.rdTypeAuto.CheckedChanged += new System.EventHandler(this.type_Checked);
            // 
            // rdTypeManual
            // 
            this.rdTypeManual.AutoSize = true;
            this.rdTypeManual.Location = new System.Drawing.Point(6, 19);
            this.rdTypeManual.Name = "rdTypeManual";
            this.rdTypeManual.Size = new System.Drawing.Size(60, 17);
            this.rdTypeManual.TabIndex = 0;
            this.rdTypeManual.TabStop = true;
            this.rdTypeManual.Text = "Manual";
            this.rdTypeManual.UseVisualStyleBackColor = true;
            this.rdTypeManual.CheckedChanged += new System.EventHandler(this.type_Checked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(15, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "IO :";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(631, 495);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(631, 9);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 5;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogin.Location = new System.Drawing.Point(12, 492);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // Actions_io
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 543);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.MaximumSize = new System.Drawing.Size(734, 582);
            this.MinimumSize = new System.Drawing.Size(734, 582);
            this.Name = "Actions_io";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Actions_io";
            this.Load += new System.EventHandler(this.Actions_io_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nPIN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIO)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nDelay)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nServo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tServo)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nAutoDelay)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvIO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdOff;
        private System.Windows.Forms.RadioButton rdOn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdTypeServo;
        private System.Windows.Forms.RadioButton rdTypeAuto;
        private System.Windows.Forms.RadioButton rdTypeManual;
        private System.Windows.Forms.NumericUpDown nDelay;
        private System.Windows.Forms.NumericUpDown nAutoDelay;

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rdTypeImage;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.Button btnRect;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnIODelete;
        private System.Windows.Forms.Button btnIOEdit;
        private System.Windows.Forms.Button btnIOSave;
        private System.Windows.Forms.TrackBar tServo;
        private System.Windows.Forms.NumericUpDown nServo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nThreshold;
        private System.Windows.Forms.NumericUpDown nPIN;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.RadioButton rdCompareModel;
        private System.Windows.Forms.ToolStripProgressBar tProgressBarTest;
        private System.Windows.Forms.Button btnLogin;
    }
}