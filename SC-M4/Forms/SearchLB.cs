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
    public partial class SearchLB : Form
    {
        public SearchLB()
        {
            InitializeComponent();
        }

        public delegate void SetNameDoubleClickHandler(string name);
        public event SetNameDoubleClickHandler OnSelect;

        private void SearchModels_Load(object sender, EventArgs e)
        {
            RenderDGV();
        }

        private void RenderDGV(){
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("No", typeof(int));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("Date", typeof(string));


            int no = 0;
            foreach (var item in Modules.MasterLB.Get())
            {
                dt.Rows.Add(item.id, ++no, item.name, item.updated_at);
            }

            // Get old row selected
            int oldRow = 0;
            if (dgvModels.SelectedRows.Count > 0)
            {
                oldRow = dgvModels.SelectedRows[0].Index;
            }

            dgvModels.DataSource = dt;
            // Clear selection.
            dgvModels.ClearSelection();

            // Set new row selected
            if (dgvModels.Rows.Count > 0)
            {
                dgvModels.Rows[oldRow].Selected = true;
            }

            // Hide id column
            dgvModels.Columns["id"].Visible = false;

            // Set width for column
            dgvModels.Columns["no"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvModels.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvModels.Columns["Date"].Width = 100;

        }
        private void dgvModels_DoubleClick(object sender, EventArgs e)
        {
            OnSelect?.Invoke(dgvModels.SelectedRows[0].Cells["name"].Value.ToString());
            this.Close();
        }
    }
}
