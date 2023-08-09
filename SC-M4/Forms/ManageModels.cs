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
    public partial class ManageModels : Form
    {
        public ManageModels()
        {
            InitializeComponent();
        }
        // Create Event Handler RenderDGV_Models
        public delegate void RenderDGV_ModelsHandler();
        public event RenderDGV_ModelsHandler OnRenderDGV_Models;

        private void ManageModels_Load(object sender, EventArgs e)
        {
            Modules.Models.CreateTable();
            RenderDGV_Models();
        }

        private void RenderDGV_Models()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("No", typeof(int));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Date", typeof(string));

            int no = 0;
            foreach (var item in Modules.Models.Get())
            {
                dt.Rows.Add(item.id, ++no, item.name, item.description, item.created_at);
            }

            // Get old row selected
            int oldRow = 0;
            if (dgvModels.SelectedRows.Count > 0)
            {
                oldRow = dgvModels.SelectedRows[0].Index;
            }

            dgvModels.DataSource = dt;

            // Clear selection
            dgvModels.ClearSelection();

            // Set new row selected
            if (dgvModels.Rows.Count > 0)
            {
                dgvModels.Rows[oldRow].Selected = true;
            }


            if (dgvModels.Rows.Count > 0)
            {
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }

            // Set column width
            dgvModels.Columns["id"].Visible = false;
            dgvModels.Columns["No"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvModels.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvModels.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvModels.Columns["Date"].Width = 100;

            // Set column header text
            dgvModels.Columns["No"].HeaderText = "No";
            dgvModels.Columns["name"].HeaderText = "Name";
            dgvModels.Columns["Description"].HeaderText = "Description";
            dgvModels.Columns["Date"].HeaderText = "Date";

            // Set row header text
            OnRenderDGV_Models?.Invoke();
        }

        private TypeState typeState = TypeState.Create;
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate txtName and txtDescription
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Name is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                MessageBox.Show("Description is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var model = new Modules.Models
            {
                name = txtName.Text,
                description = txtDescription.Text
            };

            if (typeState == TypeState.Update)
            {
                model.id = int.Parse(dgvModels.SelectedRows[0].Cells["id"].Value.ToString());
                model.updated_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (Modules.Models.IsExist(model.name, model.id))
                {
                    MessageBox.Show("Name is exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                model.Update();
            }
            else if (typeState == TypeState.Create)
            {
                model.created_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (Modules.Models.IsExist(model.name))
                {
                    MessageBox.Show("Name is exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                model.Save();
            }

            RenderDGV_Models();

            ClearForm();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Get value from dgv
            if (dgvModels.SelectedRows.Count > 0)
            {
                // Set value to form
                txtName.Text = dgvModels.SelectedRows[0].Cells["name"].Value.ToString();
                txtDescription.Text = dgvModels.SelectedRows[0].Cells["description"].Value.ToString();
                typeState = TypeState.Update;

                // Change Text btnSave
                btnSave.Text = "Update";
            }
        }

        private void dgvModels_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvModels.SelectedRows.Count > 0)
            {
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }

            typeState = TypeState.Create;
            // Clear form
            ClearForm();
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtDescription.Text = "";
            // Change Text btnSave
            btnSave.Text = "Save";
        }

        public string SetTxtName
        {
            set
            {
                txtName.Text = value;
            }
        }

        private SC_M4.Forms.SearchLB searchModels;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            searchModels?.Close();
            searchModels = new SC_M4.Forms.SearchLB();
            searchModels.OnSelect += SearchModels_OnSelect;
            searchModels.Show();
        }

        private void SearchModels_OnSelect(string name)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    txtName.Text = name;
                }));
                return;
            }
            txtName.Text = name;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvModels.SelectedRows.Count > 0)
            {
                // MessageBox ask user want to delete
                DialogResult dialogResult = MessageBox.Show("Are you sure want to delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {

                    var model = new Modules.Models
                    {
                        id = int.Parse(dgvModels.SelectedRows[0].Cells["id"].Value.ToString())
                    };

                    model.Delete();

                    RenderDGV_Models();

                    ClearForm();
                }
            }
        }
    }
}
