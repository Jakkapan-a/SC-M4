using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SC_M4.Forms
{
    public partial class NameList : Form
    {
        public NameList()
        {
            InitializeComponent();
        }
        public int _type = 0;
        public int _id = -1;

        public bool _login = false;
        private Login login;
        private void NameList_Load(object sender, EventArgs e)
        {
            btnCam1.BackColor = Color.FromArgb(192, 192, 192);
            randersTable();
            toolStripStatusLabelType.Text = "Type :" + _type;

            login = new Login(this);
            login.ShowDialog();

            if (!_login)
            {
                this.Close();
            }
        }

        private void btnCam1_Click(object sender, EventArgs e)
        {
            btnCam1.BackColor = Color.FromArgb(192, 192, 192);
            btnCam2.BackColor = Color.FromArgb(253, 253, 253);
            _type = 0;
            toolStripStatusLabelType.Text = "Type :"+_type;
            randersTable();
        }

        private void btnCam2_Click(object sender, EventArgs e)
        {
            btnCam1.BackColor = Color.FromArgb(253, 253, 253);
            btnCam2.BackColor = Color.FromArgb(192, 192, 192);
            _type = 1;
            toolStripStatusLabelType.Text = "Type :" + _type;
            randersTable();
        }

        public void randersTable()
        {
            var list = Modules.ReplaceName.GetList(_type);
            dataGridView.DataSource = null;
            int i = 0;
            var data = (from item in list
                        select new
                        {
                            id = item.id,
                            No = ++i,
                            oldName = item.oldName,
                            newName = item.newName,
                            Type = item._type == 0 ? "Camera 1" : "Camera 2",
                            Update = item.updated_at
                        }).ToList();     

            dataGridView.DataSource = data;

            dataGridView.Columns["id"].Visible = false;
            dataGridView.Columns["No"].Width = dataGridView.Width / 10;
            dataGridView.Columns["Update"].Width = dataGridView.Width / 5;
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                _id = int.Parse(dataGridView.SelectedRows[0].Cells["id"].Value.ToString());
                toolStripStatusLabelId.Text = "ID :" + _id;
            }
        }
        NameListForms nameList;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (nameList != null)
            {
                nameList.Close();
                nameList.Dispose();
                nameList = null;
            }

            nameList = new NameListForms(this);
            nameList.Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_id == -1)
            {
                MessageBox.Show("Please select a row");
                return;
            }
            if (nameList != null)
            {
                nameList.Close();
                nameList.Dispose();
                nameList = null;
            }

            nameList = new NameListForms(this, _id);
            nameList.Show();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(_id == -1)
            {
                MessageBox.Show("Please select a row");
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this item?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Modules.ReplaceName.Delete(_id);
                randersTable();
            }
            
        }
    }
}
