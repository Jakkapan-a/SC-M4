using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SC_M4.Utilities;

namespace SC_M4.Forms
{
    public partial class Actions_io : Form
    {
        public Actions_io()
        {
            InitializeComponent();
        }


        private void nServo_ValueChanged(object sender, EventArgs e)
        {
            var servo = (NumericUpDown)sender;
            if ((int)servo.Value != nServo.Value)
            {
                tServo.Value = (int)servo.Value;
            }
        }

        private void tServo_ValueChanged(object sender, EventArgs e)
        {
            var servo = (TrackBar)sender;
            if (servo.Value != (int)nServo.Value)
            {
                nServo.Value = servo.Value;
            }
        }

        private void Actions_io_Load(object sender, EventArgs e)
        {
            Modules.ActionIO.CreateTable();
            RenderDGV_IO();
        }

        private void RenderDGV_IO()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("No", typeof(int));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("Hex", typeof(string));
            dt.Columns.Add("Date", typeof(string));

            int no = 0;
            foreach (var item in Modules.ActionIO.Get())
            {
                dt.Rows.Add(item.id, ++no, item.name, item.hex, item.created_at);
            }

            // Get old row selected
            int oldRowSelected = dgvIO.SelectedRows.Count > 0 ? dgvIO.SelectedRows[0].Index : 0;

            // Render dgv
            dgvIO.DataSource = dt;

            // Clear selection 
            dgvIO.ClearSelection();

            // Hide id column
            dgvIO.Columns["id"].Visible = false;
            dgvIO.Columns["No"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Select old row
            if (dgvIO.Rows.Count > 0 && oldRowSelected <= dgvIO.Rows.Count - 1)
            {
                dgvIO.Rows[oldRowSelected].Selected = true;
            }
        }

        private void btnIOSave_Click(object sender, EventArgs e)
        {
            // Validate txtName, txtHex
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Please enter name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtHex.Text))
            {
                MessageBox.Show("Please enter hex", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var action_io = new Modules.ActionIO
            {
                name = txtName.Text.Trim(),
                hex = txtHex.Text.Trim(),
                type = 0,
                _values = 0
            };

            if (stateIO == TypeState.Update)
            {
                // Update
                action_io.id = int.Parse(dgvIO.SelectedRows[0].Cells["id"].Value.ToString());
                if (Modules.ActionIO.IsExist(action_io.name, action_io.id) || Modules.ActionIO.IsExistHex(action_io.hex, action_io.id))
                {
                    MessageBox.Show("Name or Hex is exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                action_io.Update();
            }
            else
            {
                // Save
                if (Modules.ActionIO.IsExist(action_io.name) || Modules.ActionIO.IsExistHex(action_io.hex))
                {
                    MessageBox.Show("Name or Hex is exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                action_io.Save();
            }

            // Render dgv
            RenderDGV_IO();

            // Clear form
            txtName.Text = "";
            txtHex.Text = "";
        }
        private TypeState stateIO = TypeState.Create;
        private void btnIOEdit_Click(object sender, EventArgs e)
        {
            // Get value from dgv
            if (dgvIO.SelectedRows.Count > 0)
            {
                // Set value to form
                txtName.Text = dgvIO.SelectedRows[0].Cells["name"].Value.ToString();
                txtHex.Text = dgvIO.SelectedRows[0].Cells["hex"].Value.ToString();
                stateIO = TypeState.Update;
                btnIOSave.Text = "Update";
            }
        }

        private void dgvIO_SelectionChanged(object sender, EventArgs e)
        {
            stateIO = TypeState.Create;
            btnIOSave.Text = "Save";
            // Clear form
            txtName.Text = "";
            txtHex.Text = "";
        }

        private void btnIODelete_Click(object sender, EventArgs e)
        {
            if (dgvIO.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var action_io = new Modules.ActionIO
                    {
                        id = int.Parse(dgvIO.SelectedRows[0].Cells["id"].Value.ToString())
                    };
                    action_io.Delete();
                    RenderDGV_IO();
                }
            }
        }
    }

    
}
