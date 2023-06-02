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
    public partial class Edit_Items : Form
    {
        SettingModel settingModel= null;
        public Edit_Items()
        {
            InitializeComponent();
        }
        public Edit_Items(SettingModel setting)
        {
            InitializeComponent();
            this.settingModel = setting;
        }
        Rename rename;

        public int id_sw = 0;
        public int id_lb = 0;
        private void Add_Item_Load(object sender, EventArgs e)
        {
            loadTable_SW();
            // Clear status bar
            foreach (ToolStripItem item in statusStrip.Items)
            {
                item.Text = "";
            }
        }
        public void reloadTableSetting()
        {
            if(settingModel != null)
            {
                settingModel.loadTable();
            }
        }
        public void loadTable_SW()
        {
            dataGridViewSW.DataSource = null;

            int i = 0;
            var data = (from a in MasterSW.GetMasterSW()
                        select new
                        {
                            ID = a.id,
                            No = ++i,
                            Name = a.name,
                            Updated = a.updated_at
                        }).ToList();
            dataGridViewSW.DataSource = data;
            dataGridViewSW.Columns[0].Visible = false;
            dataGridViewSW.Columns[1].Width = dataGridViewSW.Width / 10;
            // last column
            dataGridViewSW.Columns[2].Width = dataGridViewSW.Width / 2;
            dataGridViewSW.Columns[3].Width = dataGridViewSW.Width / 3;
            reloadTableSetting();
        }
        public void loadTable_LB()
        {
            //dataGridViewLB.DataSource = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("No", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Color_Name", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            var masterLB = MasterLB.GetMasterLBBySW(id_sw);
            
            for(int i = 0; i < masterLB.Count; i++)
            {
                dt.Rows.Add(masterLB[i].id, i + 1, masterLB[i].name, masterLB[i].color_name, masterLB[i].updated_at);
            }
            dataGridViewLB.DataSource = dt;
            dataGridViewLB.Columns["id"].Visible = false;
            dataGridViewLB.Columns["No"].Width = dataGridViewLB.Width / 10;

            reloadTableSetting();
        }

        private void dataGridViewSW_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewSW.SelectedRows.Count > 0)
            {
                id_sw = int.Parse(dataGridViewSW.SelectedRows[0].Cells[0].Value.ToString());
                loadTable_LB();
                toolStripStatusID_SW.Text = "ID_SW: " + id_sw;
            }
        }

        private void btnAddSW_Click(object sender, EventArgs e)
        {
            MasterSW masterSW = new MasterSW();
            masterSW.name = txtInputSW.Text.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
            if (MasterSW.IsExist(masterSW.name))
            {
                MessageBox.Show("SW Name is already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            masterSW.Save();
            MessageBox.Show("SW Name is added", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtInputSW.Text = "";
            loadTable_SW();
            reloadTableSetting();
        }

        private void btnAddLB_Click(object sender, EventArgs e)
        {
            try
            {
                MasterLB masterLB = new MasterLB();
                masterLB.master_sw_id = id_sw;
                masterLB.name = txtInputLB.Text.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
                masterLB.color_name = cbColor.SelectedItem.ToString();
                if (MasterLB.IsExist(masterLB.name))
                {
                    MessageBox.Show("Model Name is already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                masterLB.Save();
                MessageBox.Show("Model Name is added", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInputLB.Text = "";
                loadTable_LB();
                reloadTableSetting();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewLB_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewLB.SelectedRows.Count > 0)
            {
                id_lb = int.Parse(dataGridViewLB.SelectedRows[0].Cells[0].Value.ToString());
                toolStripStatusID_LB.Text = "ID_LB: " + id_lb;
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rename?.Dispose();
            rename = new Rename(this, 0);
            rename.ShowDialog();
        }

        private void renameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            rename?.Dispose();
            rename = new Rename(this, 1);
            rename.ShowDialog();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete this Model?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MasterLB.Delete(id_lb);
                loadTable_LB();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete this Software?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var data = MasterLB.GetMasterLBBySW(id_sw);
                foreach (var item in data)
                {
                    MasterLB.Delete(item.id);
                }
                MasterSW.Delete(id_sw);
                loadTable_SW();
            }
        }
    }
}
