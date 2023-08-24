using SC_M4.Modules;
using SC_M4.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SC_M4.Forms
{
    public partial class MasterColors : Form
    {
        public MasterColors()
        {
            InitializeComponent();
        }

        string[] colors = new string[] { "Red", "Orange", "Yellow", "Green", "Blue", "Violet", "Brown", "Black", "Grey", "White" };
        private void MasterColors_Load(object sender, EventArgs e)
        {
            cbUseData.Checked = Properties.Settings.Default.isUseColorMaster;

            cbColor.Items.Clear();
            cbColor.Items.AddRange(colors);
            var color = new MasterNTC();
            RenderTable();
        }
        private bool isRender = false;
        private int oldSelectedItems = -1;
        private int id = 0;
        private void RenderTable()
        {
            isRender = true;
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("No", typeof(int));
            dt.Columns.Add("Hex", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Color", typeof(string));
            dt.Columns.Add("Updated At", typeof(DateTime));

            var colors = MasterNTC.GetMasterNTC();
            int no = 1;
            foreach (var color in colors)
            {
                dt.Rows.Add(color.id, no, color.hex, color.name, color.color, color.updated_at);
                no++;
            }

            dgvMasterColors.DataSource = dt;
            dgvMasterColors.Columns["id"].Visible = false;
            dgvMasterColors.Columns["No"].Width = 50;
            dgvMasterColors.Columns["Hex"].Width = 100;
            dgvMasterColors.Columns["Name"].Width = 200;
            dgvMasterColors.Columns["Color"].Width = 100;
            dgvMasterColors.Columns["Updated At"].Width = 150;
            // Set color
            foreach (DataGridViewRow row in dgvMasterColors.Rows)
            {
                row.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#" + row.Cells["Hex"].Value.ToString());
                // Set font color
                if (row.DefaultCellStyle.BackColor.GetBrightness() < 0.5)
                {
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }

            // Clear selection
            dgvMasterColors.ClearSelection();
            if (oldSelectedItems != -1 && oldSelectedItems < dgvMasterColors.Rows.Count)
            {
                dgvMasterColors.Rows[oldSelectedItems].Selected = true;
            }
            else if (dgvMasterColors.SelectedRows.Count > 0)
            {
                dgvMasterColors.Rows[0].Selected = true;
            }
            // Scroll to selected row
            if (dgvMasterColors.SelectedRows.Count > 0)
            {
                dgvMasterColors.FirstDisplayedScrollingRowIndex = dgvMasterColors.SelectedRows[0].Index;
            }
            isRender = false;
        }
        private void dgvMasterColors_SelectionChanged(object sender, EventArgs e)
        {
            //if(isRender) return;

            if (dgvMasterColors.SelectedRows.Count > 0)
            {
                id = Convert.ToInt32(dgvMasterColors.SelectedRows[0].Cells["id"].Value);
                txtHex.Text = dgvMasterColors.SelectedRows[0].Cells["Hex"].Value.ToString();
                txtName.Text = dgvMasterColors.SelectedRows[0].Cells["Name"].Value.ToString();
                string color = dgvMasterColors.SelectedRows[0].Cells["Color"].Value.ToString();
                cbColor.SelectedIndex = cbColor.FindStringExact(color);
                toolStripStatusLabel_Id.Text = "ID : " + id;
                oldSelectedItems = dgvMasterColors.SelectedRows[0].Index;
            }
        }
        private void txtHex_TextChanged(object sender, EventArgs e)
        {
            pictureBoxColor.BackColor = ColorTranslator.FromHtml("#" + txtHex.Text);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (id != 0)
            {
                MasterNTC.UpdateColor(id, cbColor.SelectedItem.ToString());
                UpdateTable();
            }
        }

        private void UpdateTable()
        {
            var master_ntc = MasterNTC.GetMasterNTC(id);
            if (master_ntc.Count > 0)
            {
                foreach (DataGridViewRow row in dgvMasterColors.Rows)
                {
                    if ((int)row.Cells["id"].Value == id)
                    {
                        row.Cells["Color"].Value = master_ntc[0].color;
                        row.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#" + master_ntc[0].hex);
                        // Set font color
                        if (row.DefaultCellStyle.BackColor.GetBrightness() < 0.5)
                        {
                            row.DefaultCellStyle.ForeColor = Color.White;
                        }
                        else
                        {
                            row.DefaultCellStyle.ForeColor = Color.Black;
                        }
                        break;
                    }
                }
            }
        }

        private Task task;
        private async void btnFactoryReset_Click(object sender, EventArgs e)
        {
            // Check if user really want to reset
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to reset all colors?", "Factory Reset", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (task != null && !task.IsCompleted)
                {
                    return;
                }
                toolStripProgressBar1.Visible = true;
                toolStripProgressBar1.Value = 0;
                task = FactoryReset();
                await task;

                RenderTable();
                toolStripProgressBar1.Visible = false;
            }

        }
        private void UpdateProgress(int progress)
        {
            // Check if invocation is required,
            // otherwise it can be done on the current thread
            if (InvokeRequired)
                Invoke(new Action(() => toolStripProgressBar1.Value = progress));
            else
                toolStripProgressBar1.Value = progress;
        }
        private void _FactoryReset()
        {
            var ntc_name = new ColorName();
            int count = ntc_name.ntcNames.Count;
            int i = 0;
            var master_ntc = new MasterNTC();
            foreach (var color in ntc_name.ntcNames)
            {
                if (MasterNTC.isHexExist(color[0]))
                {
                    MasterNTC.UpdateByHex(color[0], color[2]);
                }
                else if (!MasterNTC.isHexExist(color[0]))
                {
                    master_ntc.hex = color[0];
                    master_ntc.name = color[1];
                    master_ntc.color = color[2];
                    master_ntc.Save();
                }
                i++;
                // Update progress bar invoke
                // Update progress bar
                UpdateProgress((int)((float)i / count * 100));
            }

            // Ensure progress bar is full when finished
            UpdateProgress(100);
        }

        public async Task FactoryReset()
        {
            await Task.Run(() => _FactoryReset());
        }

        private void cbUseData_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isUseColorMaster = cbUseData.Checked;
            Properties.Settings.Default.Save();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Check txtSearchHex is hex or not
            string hex = txtSearchHex.Text.ToUpper();
            if (hex.Length != 6)
            {
                MessageBox.Show("Hex must be 6 characters", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!IsHex(hex))
            {
                MessageBox.Show("Hex must be hex characters", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool found = false;
            // Clear selection
            dgvMasterColors.ClearSelection();

            foreach (DataGridViewRow row in dgvMasterColors.Rows)
            {
                if (row.Cells["Hex"].Value.Equals(hex))
                {
                    row.Selected = true;
                    dgvMasterColors.FirstDisplayedScrollingRowIndex = row.Index;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                MessageBox.Show("Hex not found", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private bool IsHex(string hex) => System.Text.RegularExpressions.Regex.IsMatch(hex, @"\A\b[0-9a-fA-F]+\b\Z");
        private void txtSearchHex_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }
        private Task taskSave;
        private async void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<MasterNTC> colors = MasterNTC.GetMasterNTC();
            if (colors != null && colors.Count > 0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV files (*.csv)|*.csv";
                    sfd.DefaultExt = "csv";
                    sfd.AddExtension = true;

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        toolStripProgressBar1.Visible = true;
                        toolStripProgressBar1.Value = 0;
                        taskSave = SaveFile(sfd, colors);
                        await taskSave;
                        toolStripProgressBar1.Visible = false;

                        // Open the location of the file
                        System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + sfd.FileName + "\"");

                    }
                }
            }
        }

        public async Task SaveFile(SaveFileDialog sfd, List<MasterNTC> colors)
        {
            await Task.Run(() => _SaveFile(sfd,colors));
        }

        private void _SaveFile(SaveFileDialog sfd, List<MasterNTC> colors)
        {
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(sfd.FileName))
            {
                int i =0;
                // Write header (if needed)
                writer.WriteLine("id,hex,name,color,created_at,updated_at"); // Replace Property1, Property2, etc. with your actual properties
                foreach (var item in colors)
                {
                    writer.WriteLine($"{item.id},{item.hex},{item.name},{item.color},{item.created_at},{item.updated_at}"); // Replace .Property1, .Property2, etc. with your actual properties
                    i++;
                   
                    // Update progress bar
                    UpdateProgress((int)((float)i / colors.Count * 100));
                }
            }
        }
      private Task taskImport;
        private async void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Import from CSV"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                toolStripProgressBar1.Visible = true;
                toolStripProgressBar1.Value = 0;
                taskImport = ImportFile(openFileDialog);
                await taskImport;
                toolStripProgressBar1.Visible = false;   

                // Randers Table
                RenderTable();
            }
        }

         public async Task ImportFile(OpenFileDialog openFileDialog)
        {
            await Task.Run(() => _ImportFile(openFileDialog));
        }

        private void _ImportFile(OpenFileDialog openFileDialog)
        {
            string[] lines = File.ReadAllLines(openFileDialog.FileName);

            List<MasterNTC> colors = new List<MasterNTC>();

            // Assuming first line is header, start from second line
            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');

                MasterNTC color = new MasterNTC
                {
                    id = Convert.ToInt32(values[0]),
                    hex = values[1],
                    name = values[2],
                    color = values[3],
                    created_at = values[4],
                    updated_at = values[5]
                };
                colors.Add(color);

                // Update progress bar
                UpdateProgress((int)((float)i / lines.Length * 100));
            }

            // Update to database
            foreach(var color in colors){
                if(MasterNTC.isIDExist(color.id)){
                    color.Update();
                }
                // Update progress bar
                UpdateProgress((int)((float)color.id / colors.Count * 100));
            }
        }
    }
}
