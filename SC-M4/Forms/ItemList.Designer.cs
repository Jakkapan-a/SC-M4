namespace SC_M4.Forms
{
    partial class ItemList
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
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnItemUp = new System.Windows.Forms.Button();
            this.btnItemDown = new System.Windows.Forms.Button();
            this.btnItemDelete = new System.Windows.Forms.Button();
            this.btnItemEdit = new System.Windows.Forms.Button();
            this.btnItemSave = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.btnIODown = new System.Windows.Forms.Button();
            this.btnIOUp = new System.Windows.Forms.Button();
            this.btnIODelete = new System.Windows.Forms.Button();
            this.btnIOEdit = new System.Windows.Forms.Button();
            this.btnIONew = new System.Windows.Forms.Button();
            this.dgvIO = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.cbModels = new System.Windows.Forms.ComboBox();
            this.btnManage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIO)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "List :";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(12, 32);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnItemUp);
            this.splitContainer1.Panel1.Controls.Add(this.btnItemDown);
            this.splitContainer1.Panel1.Controls.Add(this.btnItemDelete);
            this.splitContainer1.Panel1.Controls.Add(this.btnItemEdit);
            this.splitContainer1.Panel1.Controls.Add(this.btnItemSave);
            this.splitContainer1.Panel1.Controls.Add(this.txtName);
            this.splitContainer1.Panel1.Controls.Add(this.dgvItems);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnIODown);
            this.splitContainer1.Panel2.Controls.Add(this.btnIOUp);
            this.splitContainer1.Panel2.Controls.Add(this.btnIODelete);
            this.splitContainer1.Panel2.Controls.Add(this.btnIOEdit);
            this.splitContainer1.Panel2.Controls.Add(this.btnIONew);
            this.splitContainer1.Panel2.Controls.Add(this.dgvIO);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Size = new System.Drawing.Size(776, 393);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.TabIndex = 2;
            // 
            // btnItemUp
            // 
            this.btnItemUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnItemUp.BackgroundImage = global::SC_M4.Properties.Resources.up_32;
            this.btnItemUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnItemUp.Location = new System.Drawing.Point(3, 334);
            this.btnItemUp.Name = "btnItemUp";
            this.btnItemUp.Size = new System.Drawing.Size(29, 23);
            this.btnItemUp.TabIndex = 2;
            this.btnItemUp.UseVisualStyleBackColor = true;
            this.btnItemUp.Click += new System.EventHandler(this.btnItemUp_Click);
            // 
            // btnItemDown
            // 
            this.btnItemDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnItemDown.BackgroundImage = global::SC_M4.Properties.Resources.down_32;
            this.btnItemDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnItemDown.Location = new System.Drawing.Point(38, 334);
            this.btnItemDown.Name = "btnItemDown";
            this.btnItemDown.Size = new System.Drawing.Size(29, 23);
            this.btnItemDown.TabIndex = 2;
            this.btnItemDown.UseVisualStyleBackColor = true;
            this.btnItemDown.Click += new System.EventHandler(this.btnItemDown_Click);
            // 
            // btnItemDelete
            // 
            this.btnItemDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnItemDelete.BackgroundImage = global::SC_M4.Properties.Resources.delete_32;
            this.btnItemDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnItemDelete.Location = new System.Drawing.Point(177, 334);
            this.btnItemDelete.Name = "btnItemDelete";
            this.btnItemDelete.Size = new System.Drawing.Size(29, 23);
            this.btnItemDelete.TabIndex = 2;
            this.btnItemDelete.UseVisualStyleBackColor = true;
            this.btnItemDelete.Click += new System.EventHandler(this.btnItemDelete_Click);
            // 
            // btnItemEdit
            // 
            this.btnItemEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnItemEdit.BackgroundImage = global::SC_M4.Properties.Resources.edit_property_32;
            this.btnItemEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnItemEdit.Location = new System.Drawing.Point(223, 334);
            this.btnItemEdit.Name = "btnItemEdit";
            this.btnItemEdit.Size = new System.Drawing.Size(29, 23);
            this.btnItemEdit.TabIndex = 2;
            this.btnItemEdit.UseVisualStyleBackColor = true;
            this.btnItemEdit.Click += new System.EventHandler(this.btnItemEdit_Click);
            // 
            // btnItemSave
            // 
            this.btnItemSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnItemSave.Location = new System.Drawing.Point(177, 360);
            this.btnItemSave.Name = "btnItemSave";
            this.btnItemSave.Size = new System.Drawing.Size(75, 23);
            this.btnItemSave.TabIndex = 2;
            this.btnItemSave.Text = "Save";
            this.btnItemSave.UseVisualStyleBackColor = true;
            this.btnItemSave.Click += new System.EventHandler(this.btnItemSave_Click);
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(6, 363);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(165, 20);
            this.txtName.TabIndex = 1;
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Location = new System.Drawing.Point(4, 29);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.ReadOnly = true;
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItems.Size = new System.Drawing.Size(250, 299);
            this.dgvItems.TabIndex = 0;
            this.dgvItems.SelectionChanged += new System.EventHandler(this.dgvItems_SelectionChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(3, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Items :";
            // 
            // btnIODown
            // 
            this.btnIODown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnIODown.BackgroundImage = global::SC_M4.Properties.Resources.down_32;
            this.btnIODown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnIODown.Location = new System.Drawing.Point(39, 360);
            this.btnIODown.Name = "btnIODown";
            this.btnIODown.Size = new System.Drawing.Size(28, 23);
            this.btnIODown.TabIndex = 2;
            this.btnIODown.UseVisualStyleBackColor = true;
            this.btnIODown.Click += new System.EventHandler(this.btnIODown_Click);
            // 
            // btnIOUp
            // 
            this.btnIOUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnIOUp.BackgroundImage = global::SC_M4.Properties.Resources.up_32;
            this.btnIOUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnIOUp.Location = new System.Drawing.Point(6, 360);
            this.btnIOUp.Name = "btnIOUp";
            this.btnIOUp.Size = new System.Drawing.Size(27, 23);
            this.btnIOUp.TabIndex = 2;
            this.btnIOUp.UseVisualStyleBackColor = true;
            this.btnIOUp.Click += new System.EventHandler(this.btnIOUp_Click);
            // 
            // btnIODelete
            // 
            this.btnIODelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIODelete.Location = new System.Drawing.Point(273, 363);
            this.btnIODelete.Name = "btnIODelete";
            this.btnIODelete.Size = new System.Drawing.Size(75, 23);
            this.btnIODelete.TabIndex = 2;
            this.btnIODelete.Text = "Delete";
            this.btnIODelete.UseVisualStyleBackColor = true;
            this.btnIODelete.Click += new System.EventHandler(this.btnIODelete_Click);
            // 
            // btnIOEdit
            // 
            this.btnIOEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIOEdit.Location = new System.Drawing.Point(354, 361);
            this.btnIOEdit.Name = "btnIOEdit";
            this.btnIOEdit.Size = new System.Drawing.Size(75, 23);
            this.btnIOEdit.TabIndex = 2;
            this.btnIOEdit.Text = "Edit";
            this.btnIOEdit.UseVisualStyleBackColor = true;
            this.btnIOEdit.Click += new System.EventHandler(this.btnIOEdit_Click);
            // 
            // btnIONew
            // 
            this.btnIONew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIONew.Location = new System.Drawing.Point(437, 360);
            this.btnIONew.Name = "btnIONew";
            this.btnIONew.Size = new System.Drawing.Size(75, 23);
            this.btnIONew.TabIndex = 2;
            this.btnIONew.Text = "New";
            this.btnIONew.UseVisualStyleBackColor = true;
            this.btnIONew.Click += new System.EventHandler(this.btnIONew_Click);
            // 
            // dgvIO
            // 
            this.dgvIO.AllowUserToAddRows = false;
            this.dgvIO.AllowUserToDeleteRows = false;
            this.dgvIO.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvIO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIO.Location = new System.Drawing.Point(3, 29);
            this.dgvIO.Name = "dgvIO";
            this.dgvIO.ReadOnly = true;
            this.dgvIO.RowHeadersVisible = false;
            this.dgvIO.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIO.Size = new System.Drawing.Size(509, 325);
            this.dgvIO.TabIndex = 0;
            this.dgvIO.DoubleClick += new System.EventHandler(this.btnIOEdit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(3, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "IO :";
            // 
            // cbModels
            // 
            this.cbModels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbModels.FormattingEnabled = true;
            this.cbModels.Location = new System.Drawing.Point(584, 6);
            this.cbModels.Name = "cbModels";
            this.cbModels.Size = new System.Drawing.Size(121, 21);
            this.cbModels.TabIndex = 3;
            this.cbModels.SelectedIndexChanged += new System.EventHandler(this.cbModels_SelectedIndexChanged);
            // 
            // btnManage
            // 
            this.btnManage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManage.Location = new System.Drawing.Point(711, 4);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(75, 23);
            this.btnManage.TabIndex = 2;
            this.btnManage.Text = "Manage";
            this.btnManage.UseVisualStyleBackColor = true;
            this.btnManage.Click += new System.EventHandler(this.btnManage_Click);
            // 
            // ItemList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cbModels);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnManage);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Name = "ItemList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ItemList";
            this.Load += new System.EventHandler(this.ItemList_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIO)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.DataGridView dgvIO;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnItemSave;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnIODelete;
        private System.Windows.Forms.Button btnIOEdit;
        private System.Windows.Forms.Button btnIONew;
        private System.Windows.Forms.Button btnItemDelete;
        private System.Windows.Forms.Button btnItemEdit;
        private System.Windows.Forms.Button btnItemUp;
        private System.Windows.Forms.Button btnItemDown;
        private System.Windows.Forms.ComboBox cbModels;
        private System.Windows.Forms.Button btnManage;
        private System.Windows.Forms.Button btnIODown;
        private System.Windows.Forms.Button btnIOUp;
    }
}