using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SC_M4.Utilities;

namespace SC_M4.Forms
{
    public partial class ItemList : Form
    {
        public ItemList()
        {
            InitializeComponent();
        }

        private void ItemList_Load(object sender, EventArgs e)
        {
            Modules.Actions.CreateTable();
            Modules.ActionIO.CreateTable();
            Modules.Items.CreateTable();
            Modules.Rect.CreateTable();

            RenderCBModels();
            // RenderDGVItem();
        }

        private void RenderDGVItem(int model_id)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("No", typeof(int));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            int no = 0;
            foreach (var item in Modules.Items.GetItemsByModelId(model_id))
            {
                dt.Rows.Add(item.id, ++no, item.name, item.updated_at);
            }

            // Get old row selected
            int oldRow = 0;
            if (dgvItems.SelectedRows.Count > 0)
            {
                oldRow = dgvItems.SelectedRows[0].Index;
            }
            // Render data to dgv
            dgvItems.DataSource = dt;
            // Clear selection
            dgvItems.ClearSelection();
            // Select old row
            if (dgvItems.Rows.Count > 0 && oldRow < dgvItems.Rows.Count)
            {
                dgvItems.Rows[oldRow].Selected = true;
            }
            else if (oldRow >= dgvItems.Rows.Count && dgvItems.Rows.Count > 0)
            {
                dgvItems.Rows[dgvItems.Rows.Count - 1].Selected = true;
            }
            // Enable/Disable button
            if (dgvItems.Rows.Count > 0)
            {
                btnItemUp.Enabled = true;
                btnItemDown.Enabled = true;
                btnItemDelete.Enabled = true;
                btnItemEdit.Enabled = true;
                btnIONew.Enabled = true;
            }
            else
            {
                btnItemUp.Enabled = false;
                btnItemDown.Enabled = false;
                btnItemDelete.Enabled = false;
                btnItemEdit.Enabled = false;
                btnIONew.Enabled = false;
            }

            // Hide id column
            dgvItems.Columns["id"].Visible = false;
            // Hide No column
            dgvItems.Columns["No"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Set width for name column
            dgvItems.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // Set width for date column
            dgvItems.Columns["Date"].Width = 100;

        }

        private void RenderDGV_IO(int item_id)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("No", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            int no = 0;
            foreach (var item in Modules.Actions.GetListByItemId(item_id))
            {
                dt.Rows.Add(item.id, ++no, item.name, GetTypeAction((TypeAction)item.type), item.state, item.updated_at);
            }

            // Get old row selected
            int oldRow = 0;
            if (dgvIO.SelectedRows.Count > 0)
            {
                oldRow = dgvIO.SelectedRows[0].Index;
            }

            // Render data to dgv
            dgvIO.DataSource = dt;

            // Clear selection
            dgvIO.ClearSelection();

            // Select old row
            if (dgvIO.Rows.Count > 0 && oldRow < dgvIO.Rows.Count)
            {
                dgvIO.Rows[oldRow].Selected = true;
            }
            else if (oldRow >= dgvIO.Rows.Count && dgvIO.Rows.Count > 0)
            {
                dgvIO.Rows[dgvIO.Rows.Count - 1].Selected = true;
            }

            // Enable/Disable button
            if (dgvIO.Rows.Count > 0)
            {
                btnIOUp.Enabled = true;
                btnIODown.Enabled = true;
                btnIODelete.Enabled = true;
                btnIOEdit.Enabled = true;
            }
            else
            {
                btnIOUp.Enabled = false;
                btnIODown.Enabled = false;
                btnIODelete.Enabled = false;
                btnIOEdit.Enabled = false;
            }

            // Hide id column
            dgvIO.Columns["id"].Visible = false;
            // Hide No column
            dgvIO.Columns["No"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Set width for name column
            dgvIO.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvIO.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvIO.Columns["Value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Set width for date column
            dgvIO.Columns["Date"].Width = 100;

        }

        private string GetTypeAction(TypeAction type)
        {
            switch (type)
            {
                case TypeAction.Auto:
                    return "Auto";
                case TypeAction.Manual:
                    return "Manual";
                case TypeAction.Image:
                    return "Image";
                default:
                    return "Servo";
            }
        }
        private TypeState typeStateItem = TypeState.Create;

        private void btnItemSave_Click(object sender, EventArgs e)
        {
            // Validate txtName
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Please enter name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Validate cbModels
            if (cbModels.Items.Count == 0)
            {
                MessageBox.Show("Please create model first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Validate cbModels
            if (cbModels.SelectedIndex == -1)
            {
                MessageBox.Show("Please select model", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                // Check if item is exist
                if (typeStateItem == TypeState.Update)
                {
                    int item_id = (int)dgvItems.SelectedRows[0].Cells["id"].Value;
                    // Validate name is exist
                    if (Modules.Items.IsExist(txtName.Text, item_id))
                    {
                        MessageBox.Show("Name is exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Modules.Items item = Modules.Items.Get(item_id);
                    item.name = txtName.Text;
                    item.Update();

                }
                else if (typeStateItem == TypeState.Create)
                {
                    // Validate name is exist
                    if (Modules.Items.IsExist(txtName.Text))
                    {
                        MessageBox.Show("Name is exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Modules.Items items = new Modules.Items()
                    {
                        name = txtName.Text,
                        model_id = (int)cbModels.SelectedValue
                    };

                    items.Save();
                }

                RenderDGVItem(model_id);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnItemEdit_Click(object sender, EventArgs e)
        {
            // Get value from dgv
            if (dgvItems.SelectedRows.Count > 0)
            {
                // Set value to form
                txtName.Text = dgvItems.SelectedRows[0].Cells["name"].Value.ToString();
                typeStateItem = TypeState.Update;
                btnItemSave.Text = "Update";
            }
        }
        private void btnItemDelete_Click(object sender, EventArgs e)
        {
            // Get value from dgv
            if (dgvItems.SelectedRows.Count > 0)
            {
                // Ask user to confirm
                DialogResult dialogResult = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    // Set value to form
                    int item_id = (int)dgvItems.SelectedRows[0].Cells["id"].Value;
                    Modules.Items item = Modules.Items.Get(item_id);
                    item.Delete();
                    RenderDGVItem(model_id);
                    ClearForm();
                }

            }
        }
        private void ClearForm()
        {
            txtName.Text = "";
            typeStateItem = TypeState.Create;
            btnItemSave.Text = "Save";
        }

        private ManageModels manageModels;
        private void btnManage_Click(object sender, EventArgs e)
        {
            manageModels?.Close();
            manageModels = new ManageModels();
            manageModels.Show();
            manageModels.OnRenderDGV_Models += ManageModels_OnRenderDGV_Models;
        }

        private void ManageModels_OnRenderDGV_Models()
        {
            Console.WriteLine("ManageModels_OnRenderDGV_Models");
            RenderCBModels();
        }
        private int model_id = -1;
        private void cbModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if cbModels is empty
            if (cbModels.Items.Count == 0)
            {
                return;
            }
            // if No item selected
            if (cbModels.SelectedIndex == -1)
            {
                return;
            }

            if (cbModels.SelectedValue is int model_id)
            {
                this.model_id = model_id;
                RenderDGVItem(model_id);
            }

        }

        private void RenderCBModels()
        {
            // old index
            int oldIndex = cbModels.SelectedIndex;

            // Render data to cbModels
            List<Modules.Models> models = Modules.Models.Get();
            cbModels.DataSource = models;
            cbModels.DisplayMember = "name";
            cbModels.ValueMember = "id";
            // Select old index
            if (cbModels.Items.Count > 0 && oldIndex < cbModels.Items.Count && oldIndex != -1)
            {
                cbModels.SelectedIndex = oldIndex;
            }
            else if (cbModels.Items.Count > 0)
            {
                cbModels.SelectedIndex = oldIndex;
                cbModels.SelectedIndex = 0;
            }
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            // Press Enter
            if (e.KeyCode == Keys.Enter)
            {
                btnItemSave_Click(sender, e);
            }
        }

        private void btnItemUp_Click(object sender, EventArgs e)
        {
            MoveItem(true);
        }

        private void btnItemDown_Click(object sender, EventArgs e)
        {
            MoveItem(false);
        }

        private void MoveItem(bool up)
        {
            // Ensure a row is selected
            if (dgvItems.SelectedRows.Count == 0) return;

            int rowIndex = dgvItems.SelectedRows[0].Index;

            // If selected row is first row and moving up, or last row and moving down
            if ((rowIndex == 0 && up) || (rowIndex == dgvItems.Rows.Count - 1 && !up))
            {
                return;
            }

            // Set value to form
            int item_id = (int)dgvItems.SelectedRows[0].Cells["id"].Value;
            if (up)
            {
                // Set selected row to previous row
                dgvItems.Rows[rowIndex - 1].Selected = true;
                // Move item up
                Modules.Items.SetUp(item_id);
            }
            else
            {
                // Set selected row to next row
                dgvItems.Rows[rowIndex + 1].Selected = true;
                // Move item down
                Modules.Items.SetDown(item_id);
            }

            RenderDGVItem(model_id);
        }

        private void dgvItems_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count > 0)
            {
                // Get item_id
                int item_id = (int)dgvItems.SelectedRows[0].Cells["id"].Value;
                // Render dgvIO
                RenderDGV_IO(item_id);
            }

        }

        private Actions_io actions_io;
        private void btnIONew_Click(object sender, EventArgs e)
        {
            // Get item_id
            if (dgvItems.SelectedRows.Count > 0)
            {
                int id = (int)dgvItems.SelectedRows[0].Cells["id"].Value;

                actions_io?.Close();
                actions_io = new Actions_io(id);
                actions_io.FormClosed += Actions_io_FormClosed;
                actions_io.Show();
            }
        }

        private void Actions_io_OnSave()
        {
          
        }

        private void Actions_io_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (dgvItems.SelectedRows.Count > 0)
            {
                // Get item_id
                int item_id = (int)dgvItems.SelectedRows[0].Cells["id"].Value;
                // Render dgvIO
                RenderDGV_IO(item_id);
            }
        }

        private void btnIOUp_Click(object sender, EventArgs e)
        {
            MoveIO(true);
        }

        private void btnIODown_Click(object sender, EventArgs e)
        {
            MoveIO(false);
        }

        private void MoveIO(bool up){
            // Ensure a row is selected
            if (dgvIO.SelectedRows.Count == 0) return;

            int rowIndex = dgvIO.SelectedRows[0].Index;

            // If selected row is first row and moving up, or last row and moving down
            if ((rowIndex == 0 && up) || (rowIndex == dgvIO.Rows.Count - 1 && !up))
            {
                return;
            }

            // Set value to form
            int io_id = (int)dgvIO.SelectedRows[0].Cells["id"].Value;
            int item_id = (int)dgvItems.SelectedRows[0].Cells["id"].Value;
            if (up)
            {
                // Set selected row to previous row
                dgvIO.Rows[rowIndex - 1].Selected = true;
                // Move item up
                Modules.Actions.SetUp(io_id);
            }
            else
            {
                // Set selected row to next row
                dgvIO.Rows[rowIndex + 1].Selected = true;
                // Move item down
                Modules.Actions.SetDown(io_id);
            }

            RenderDGV_IO(item_id);
        }

        private void btnIOEdit_Click(object sender, EventArgs e)
        {
            actions_io?.Close();
            int io_id = (int)dgvIO.SelectedRows[0].Cells["id"].Value;
            actions_io = new Actions_io(io_id, TypeState.Update);
            actions_io.FormClosed += Actions_io_FormClosed;
            actions_io.Show();
        }

        private void btnIODelete_Click(object sender, EventArgs e)
        {
            // Ask user to confirm
            DialogResult dialogResult = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                // Set value to form
                int io_id = (int)dgvIO.SelectedRows[0].Cells["id"].Value;
                Modules.Actions.Delete(io_id);
                int item_id = (int)dgvItems.SelectedRows[0].Cells["id"].Value;
                RenderDGV_IO(item_id);
            }
        }
    }
}
