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
using SC_M4.Utilities;

namespace SC_M4.Forms
{
    public partial class Actions_io : Form
    {
        private int item_id = -1;
        private TypeState typeState = TypeState.Create;
        public Actions_io(int item_id, TypeState typeState = TypeState.Create)
        {
            InitializeComponent();
            this.item_id = item_id;
            this.typeState = typeState;
        }

        private void Actions_io_Load(object sender, EventArgs e)
        {
            
            RenderDGV_IO();


            btnRect.Enabled = false;

            rdOff.Enabled = false;
            rdOn.Enabled = false;
            nAutoDelay.Enabled = false;
            btnLoadImage.Enabled = false;
            btnRect.Enabled = false;
            tServo.Enabled = false;
            nServo.Enabled = false;
            nThreshold.Enabled = false;
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
        private Camera camera;
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            camera?.Close();
            camera = new Camera();
            camera.Show();
            camera.OnSave += Camera_OnSave;
        }
        private string fileNameImage = string.Empty;
        private void Camera_OnSave(string fileName)
        {
            fileNameImage = fileName;
            string path = Path.Combine(Properties.Resources.path_images, fileName);
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = Image.FromStream(fs);
            }
        }

        private void type_Checked(object sender, EventArgs e)
        {
            var radioButton = (RadioButton)sender;

            rdOff.Enabled = false;
            rdOn.Enabled = false;
            nAutoDelay.Enabled = false;
            btnLoadImage.Enabled = false;
            btnRect.Enabled = false;
            tServo.Enabled = false;
            nServo.Enabled = false;
            nThreshold.Enabled = false;

            if (radioButton.Checked && radioButton == rdTypeManual)
            {
                rdOff.Enabled = true;
                rdOn.Enabled = true;
            }
            else if (radioButton.Checked && radioButton == rdTypeAuto)
            {
                nAutoDelay.Enabled = true;
            }
            else if (radioButton.Checked && radioButton == rdTypeImage)
            {
                btnLoadImage.Enabled = true;
                btnRect.Enabled = true;
                nThreshold.Enabled = true;

                if (typeState == TypeState.Create)
                {
                    btnRect.Enabled = false;
                }
                else if (typeState == TypeState.Update)
                {
                    btnRect.Enabled = true;
                }
            }
            else if (radioButton.Checked && radioButton == rdTypeServo)
            {
                tServo.Enabled = true;
                nServo.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Modules.Actions  actions = new Modules.Actions();
            if (typeState == TypeState.Create)
            {
                string name_details = string.Empty;
                // Validate rdType
                if (!rdTypeManual.Checked && !rdTypeAuto.Checked && !rdTypeImage.Checked && !rdTypeServo.Checked)
                {
                    MessageBox.Show("Please choose type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (rdTypeManual.Checked || rdTypeAuto.Checked)
                {
                    // Get id action IO
                    int id  =  int.Parse(dgvIO.SelectedRows[0].Cells["id"].Value.ToString());
                    actions.action_io_id = id;

                    if(rdTypeManual.Checked){
                        // Validate dgvIO selected
                        if (dgvIO.SelectedRows.Count == 0)
                        {
                            MessageBox.Show("Please choose IO", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (!rdOn.Checked && !rdOff.Checked)
                        {
                            MessageBox.Show("Please choose state", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        actions.type = (int)TypeAction.Manual;
                        actions.state = rdOn.Checked ? 1 : 0;
                        name_details = "Manual " + (actions.state == 1 ? "On" : "Off") + " " + dgvIO.SelectedRows[0].Cells["name"].Value.ToString() + " " + dgvIO.SelectedRows[0].Cells["hex"].Value.ToString();
                    }
                    else if(rdTypeAuto.Checked){
                        actions.type = (int)TypeAction.Auto;
                        actions.auto_delay = (int)nAutoDelay.Value;
                        name_details = "Auto " + actions.auto_delay + "s " + dgvIO.SelectedRows[0].Cells["name"].Value.ToString() + " " + dgvIO.SelectedRows[0].Cells["hex"].Value.ToString();
                    }
                }else if(rdTypeServo.Checked){
                    actions.type = (int)TypeAction.Servo;
                    actions.servo = (int)nServo.Value;
                    name_details = "Servo " + actions.servo;
                }else if(rdTypeImage.Checked){
                    // Validate image selected
                    if (string.IsNullOrEmpty(fileNameImage))
                    {
                        MessageBox.Show("Please choose image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    actions.type = (int)TypeAction.Image;
                    actions.image_name = fileNameImage;
                    actions.type_image = 0;
                    actions.threshold = (int)nThreshold.Value;
                    name_details = "Image " + fileNameImage;
                }
                actions.item_id = item_id;
                actions.name = name_details;
                actions.delay = (int)nDelay.Value;
                actions.Save();
            }
            else if (typeState == TypeState.Update)
            {
                
            }
            this.Close();
        }
    }
   
}
