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
        private int id = -1;
        private TypeState typeState = TypeState.Create;
        public delegate void OnSaveHandler();
        public event OnSaveHandler OnSave;
        public Actions_io(int item_id, TypeState typeState = TypeState.Create)
        {
            InitializeComponent();
            this.id = item_id;
            this.typeState = typeState;
            RenderDGV_IO();
        }

        private Modules.Actions action;
        private void Actions_io_Load(object sender, EventArgs e)
        {
            DisableAllControls();
            dgvIO.ClearSelection();

            if (typeState == TypeState.Update)
            {
                action = Modules.Actions.Get(id);
                SetActionControls();
                btnSave.Text = "Update";
            }
        }
        private void DisableAllControls()
        {
            btnRect.Enabled = rdOff.Enabled = rdOn.Enabled = nAutoDelay.Enabled = btnLoadImage.Enabled = btnRect.Enabled = tServo.Enabled = nServo.Enabled = nThreshold.Enabled = false;
        }
        private void SetActionControls()
        {
            switch ((TypeAction)action.type)
            {
                case TypeAction.Auto:
                    rdTypeAuto.Checked = true;
                    nAutoDelay.Value = action.auto_delay;
                    SelectRowById(action.action_io_id.ToString());
                    break;
                case TypeAction.Manual:
                    rdTypeManual.Checked = true;
                    if (action.state == 1) rdOn.Checked = true;
                    else rdOff.Checked = true;
                    SelectRowById(action.action_io_id.ToString());
                    break;
                case TypeAction.Servo:
                    rdTypeServo.Checked = true;
                    tServo.Value = action.servo;
                    break;
                case TypeAction.Image:
                    rdTypeImage.Checked = true;
                    fileNameImage = action.image_name;
                    Camera_OnSave(action.image_name);
                    break;
            }

            nDelay.Value = action.delay;
        }


        private void SelectRowById(string id)
        {
            for (int i = 0; i < dgvIO.Rows.Count; i++)
            {
                if (dgvIO.Rows[i].Cells["id"].Value.ToString() == id)
                {
                    dgvIO.Rows[i].Selected = true;
                    break;
                }
            }
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
            dt.Columns.Add("Pin", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            int no = 0;
            foreach (var item in Modules.ActionIO.Get())
            {
                dt.Rows.Add(item.id, ++no, item.name, item.pin, item.created_at);
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
            // Validate txtName, txtPin
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Please enter name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var action_io = new Modules.ActionIO
            {
                name = txtName.Text.Trim(),
                pin = (int)nPIN.Value,
                type = 0,
                _values = 0
            };

            if (stateIO == TypeState.Update)
            {
                // Update
                action_io.id = int.Parse(dgvIO.SelectedRows[0].Cells["id"].Value.ToString());
                if (Modules.ActionIO.IsExist(action_io.name, action_io.id) || Modules.ActionIO.IsExistPin(action_io.pin, action_io.id))
                {
                    MessageBox.Show("Name or Pin is exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                action_io.Update();
            }
            else
            {
                // Save
                if (Modules.ActionIO.IsExist(action_io.name) || Modules.ActionIO.IsExistPin(action_io.pin))
                {
                    MessageBox.Show("Name or Pin is exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                action_io.Save();
            }

            // Render dgv
            RenderDGV_IO();

            // Clear form
            txtName.Text = "";
            nPIN.Value = 0;
        }
        private TypeState stateIO = TypeState.Create;
        private void btnIOEdit_Click(object sender, EventArgs e)
        {
            // Get value from dgv
            if (dgvIO.SelectedRows.Count > 0)
            {
                // Set value to form
                txtName.Text = dgvIO.SelectedRows[0].Cells["name"].Value.ToString();
                nPIN.Value = (decimal)dgvIO.SelectedRows[0].Cells["Pin"].Value;
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
            nPIN.Value = 0;
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
          

            try
            {
                if (!rdTypeManual.Checked && !rdTypeAuto.Checked && !rdTypeImage.Checked && !rdTypeServo.Checked)
                {
                    MessageBox.Show("Please choose type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (typeState == TypeState.Create)
                {
                    SaveNewAction();
                }
                else if (typeState == TypeState.Update)
                {
                    UpdateAction();
                }
                OnSave?.Invoke();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveNewAction()
        {
            Modules.Actions actions = new Modules.Actions();
            string nameDetails = GetNameDetails(actions);
            if (nameDetails == null) return;

            actions.item_id = id; // Assuming 'id' is defined elsewhere in your class
            actions.name = nameDetails;
            actions.delay = (int)nDelay.Value;
            actions.Save();
        }
        private void UpdateAction()
        {
            string nameDetails = GetNameDetails(action);
            if (nameDetails == null) return;

            action.name = nameDetails;
            action.delay = (int)nDelay.Value;
            action.Update();
        }
        private string GetNameDetails(Modules.Actions actions)
        {
            // Validate and set action properties based on the selected type
            switch (GetSelectedType())
            {
                case TypeAction.Manual:
                    return HandleManualType(actions);
                case TypeAction.Auto:
                    return HandleAutoType(actions);
                case TypeAction.Servo:
                    actions.type = (int)TypeAction.Servo;
                    actions.servo = tServo.Value;
                    return "Servo " + actions.servo;
                case TypeAction.Image:
                    return HandleImageType(actions);
                default:
                    throw new Exception("Please choose type");
                    return null;
            }
        }

        private TypeAction GetSelectedType()
        {
            if (rdTypeManual.Checked) return TypeAction.Manual;
            if (rdTypeAuto.Checked) return TypeAction.Auto;
            if (rdTypeServo.Checked) return TypeAction.Servo;
            if (rdTypeImage.Checked) return TypeAction.Image;
            return default; // You may want to handle this case appropriately.
        }



        private string HandleManualType(Modules.Actions actions)
        {
            if (dgvIO.SelectedRows.Count == 0)
            {
                throw new Exception("Please choose IO");
            }
            if (!rdOn.Checked && !rdOff.Checked)
            {
                throw new Exception("Please choose state");
            }
            int id = int.Parse(dgvIO.SelectedRows[0].Cells["id"].Value.ToString());
            actions.action_io_id = int.Parse(dgvIO.SelectedRows[0].Cells["id"].Value.ToString());
            actions.type = (int)TypeAction.Manual;
            actions.state = rdOn.Checked ? 1 : 0;
            return "Manual " + (actions.state == 1 ? "On" : "Off") + " " + dgvIO.SelectedRows[0].Cells["name"].Value.ToString() + " " + dgvIO.SelectedRows[0].Cells["Pin"].Value.ToString();
        }

        private string HandleAutoType(Modules.Actions actions)
        {
            actions.type = (int)TypeAction.Auto;
            actions.action_io_id = int.Parse(dgvIO.SelectedRows[0].Cells["id"].Value.ToString());
            actions.auto_delay = (int)nAutoDelay.Value;
            return "Auto " + actions.auto_delay + "s " + dgvIO.SelectedRows[0].Cells["name"].Value.ToString() + " " + dgvIO.SelectedRows[0].Cells["Pin"].Value.ToString();
        }

        private string HandleImageType(Modules.Actions actions)
        {
            if (string.IsNullOrEmpty(fileNameImage))
            {
                // MessageBox.Show("Please choose image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception("Please choose image");
                // return null;
            }
            actions.type = (int)TypeAction.Image;
            actions.image_name = fileNameImage;
            actions.type_image = 0; // If there's specific logic to determine this, handle it here.
            actions.threshold = (int)nThreshold.Value;
            return "Image " + fileNameImage;
        }
        private Forms.RectangleImage rectangleImage;
        private void btnRect_Click(object sender, EventArgs e)
        {
            if(pictureBox1.Image == null)
            {
                MessageBox.Show("Please choose image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            rectangleImage?.Close();
            rectangleImage = new RectangleImage(action.id);
            rectangleImage.Show();
        }
    }

}
