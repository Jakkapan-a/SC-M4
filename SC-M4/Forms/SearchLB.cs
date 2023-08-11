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
        // Use an enum for clarity.
        private SearchType searchType = SearchType.MasterLB;

        public SearchLB(SearchType type = SearchType.MasterLB)
        {
            InitializeComponent();
            this.searchType = type;
        }

        public delegate void SetNameDoubleClickHandler(string name);
        public event SetNameDoubleClickHandler OnSelect;

        private void SearchModels_Load(object sender, EventArgs e)
        {
            RenderDGV();
        }
        private void RenderDGV()
        {
            var dt = CreateDataTable();

            PopulateDataTable(dt);

            RenderDataGridView(dt);
        }
        private DataTable CreateDataTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("No", typeof(int));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("Date", typeof(string));

            return dt;
        }
        private void PopulateDataTable(DataTable dt)
        {
            int no = 0;
            IEnumerable<IDataItem> items = null;

            switch (searchType)
            {
                case SearchType.MasterLB:
                    items = (IEnumerable<IDataItem>)Modules.MasterLB.Get();
                    break;
                case SearchType.Models:
                    items = (IEnumerable<IDataItem>)Modules.Models.Get();
                    break;
            }

            foreach (var item in items)
            {
                dt.Rows.Add(item.id, ++no, item.name, item.updated_at);
            }
        }

     
        private void RenderDataGridView(DataTable dt)
        {
            int oldRow = 0;

            if (dgvModels.SelectedRows.Count > 0)
            {
                oldRow = dgvModels.SelectedRows[0].Index;
            }

            dgvModels.DataSource = dt;

            dgvModels.ClearSelection();

            if (dgvModels.Rows.Count > 0)
            {
                dgvModels.Rows[oldRow].Selected = true;
            }

            dgvModels.Columns["id"].Visible = false;
            dgvModels.Columns["no"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvModels.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvModels.Columns["Date"].Width = 100;
        }

        //private void RenderDGV(){
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("id", typeof(int));
        //    dt.Columns.Add("No", typeof(int));
        //    dt.Columns.Add("name", typeof(string));
        //    dt.Columns.Add("Date", typeof(string));


        //    int no = 0;
        //    if(type == 0)
        //    {
        //        foreach (var item in Modules.MasterLB.Get())
        //        {
        //            dt.Rows.Add(item.id, ++no, item.name, item.updated_at);
        //        }
        //    }else if(type == 1)
        //    {
        //        foreach (var item in Modules.Models.Get())
        //        {
        //            dt.Rows.Add(item.id, ++no, item.name, item.updated_at);
        //        }
        //    }
        //    // Get old row selected
        //    int oldRow = 0;
        //    if (dgvModels.SelectedRows.Count > 0)
        //    {
        //        oldRow = dgvModels.SelectedRows[0].Index;
        //    }

        //    dgvModels.DataSource = dt;
        //    // Clear selection.
        //    dgvModels.ClearSelection();

        //    // Set new row selected
        //    if (dgvModels.Rows.Count > 0)
        //    {
        //        dgvModels.Rows[oldRow].Selected = true;
        //    }

        //    // Hide id column
        //    dgvModels.Columns["id"].Visible = false;

        //    // Set width for column
        //    dgvModels.Columns["no"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //    dgvModels.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        //    dgvModels.Columns["Date"].Width = 100;

        //}
        private void dgvModels_DoubleClick(object sender, EventArgs e)
        {
            if(dgvModels.SelectedRows.Count > 0){
                OnSelect?.Invoke(dgvModels.SelectedRows[0].Cells["name"].Value.ToString());
                this.Close();
            }
        }
    }
    public interface IDataItem
    {
        int id { get; }
        string name { get; }
        string updated_at { get; }
    }

    public enum SearchType
    {
        MasterLB,
        Models
    }
}
