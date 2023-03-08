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
        private int _type = 0;
        private void NameList_Load(object sender, EventArgs e)
        {
            btnCam1.BackColor = Color.FromArgb(192, 192, 192);
            randerTable();
        }

        private void btnCam1_Click(object sender, EventArgs e)
        {
            btnCam1.BackColor = Color.FromArgb(192, 192, 192);
            btnCam2.BackColor = Color.FromArgb(253, 253, 253);
            _type = 0;
        }

        private void btnCam2_Click(object sender, EventArgs e)
        {
            btnCam1.BackColor = Color.FromArgb(253, 253, 253);
            btnCam2.BackColor = Color.FromArgb(192, 192, 192);
            _type = 1;
        }

        public void randerTable()
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
    }
}
