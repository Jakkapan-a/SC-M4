using SC_M4.Modules;
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
    public partial class SettingModel : Form
    {
        public SettingModel()
        {
            InitializeComponent();
        }

        public int id_sw = -1;
        public int id_lb = -1;
        Edit_Items _Items;
        Rename rename;
        private void Setting_Load(object sender, EventArgs e)
        {
            RandersTable();
            // Clear status bar
            foreach (ToolStripItem item in statusStrip1.Items)
            {
                if (item is ToolStripStatusLabel)
                {
                    item.Text = "";
                }
            }
        }

        public void RandersTable()
        {

            var all = MasterAll.GetMasterAll();

            dgvModels.DataSource = null;

            int i = 0;

            var data = (from a in all
                        select new
                        {
                            ID_SW = a.id_sw,
                            ID_LB = a.id_lb,
                            No = ++i,
                            Name_SW = a.nameSW,
                            Name_Model = a.nameModel,
                            Updated = a.updated_at
                        }).ToList();

            dgvModels.DataSource = data;
            dgvModels.Columns[0].Visible = false;
            dgvModels.Columns[1].Visible = false;
            dgvModels.Columns[2].Width = dgvModels.Width / 10;


        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            _Items?.Dispose();
            _Items = new Edit_Items(this);
            _Items.Show();
        }
        private void dataGridViewReport_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvModels.SelectedRows.Count > 0)
            {
                id_sw = int.Parse(dgvModels.SelectedRows[0].Cells[0].Value.ToString());
                id_lb = int.Parse(dgvModels.SelectedRows[0].Cells[1].Value.ToString());
                toolStripStatusID_SW.Text = "SW :" + id_sw.ToString();
                toolStripStatusID_LB.Text = "LB :" + id_lb.ToString();
            }
        }

        private void renameSWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rename?.Dispose();
            if (id_sw == 0)
            {
                MessageBox.Show("Can't rename this software name", "Rename Software", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            rename = new Rename(this, Utilities.PageType.SW);
            rename.EventReloadData += Rename_EventReloadData;
            rename.Show();
        }

        private void Rename_EventReloadData()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Rename_EventReloadData()));
                return;
            }

            RandersTable();
        }

        private void renameModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rename?.Dispose();
            if (id_lb == 0)
            {
                MessageBox.Show("Can't rename this model name", "Rename Model", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            rename = new Rename(this, Utilities.PageType.LB);
            rename.EventReloadData += Rename_EventReloadData;
            rename.Show();
        }

        private void deleteModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete this model?", "Delete Model", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var data = MasterLB.GetMasterLB(id_lb);
                if (data.Count > 0)
                {
                    data[0].Delete();
                }
                RandersTable();
                MessageBox.Show("Delete model success", "Delete Model", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void deleteSWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete this software?", "Delete Software", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var model = MasterLB.GetMasterLBBySW(id_sw);
                foreach (var item in model)
                {
                    item.Delete();
                }
                var data = MasterSW.GetMasterSW(id_sw);
                if (data.Count > 0)
                {
                    data[0].Delete();
                    RandersTable();
                }
                RandersTable();
                MessageBox.Show("Delete software success", "Delete Software", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnEdit.PerformClick();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearch.Text;
            // Clear selection
            dgvModels.ClearSelection();
            // Search all column in dgvModels
            foreach (DataGridViewRow row in dgvModels.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value.ToString().ToLower().Contains(search.ToLower()))
                    {
                        cell.Selected = true;
                        break;
                    }
                }
            }
        }
    }
}
