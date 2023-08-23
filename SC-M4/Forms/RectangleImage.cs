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
    public partial class RectangleImage : Form
    {
        private int id = -1;
        public RectangleImage(int id)
        {
            InitializeComponent();
            this.id = id;
        }
        private string path = "";
        private string fileName = "";
        private Image image;
        private Image imageRect;


        private void RectangleImage_Load(object sender, EventArgs e)
        {
            if (id == -1) this.Close();
            Actions actions = Actions.Get(id);
            if (actions == null) this.Close();

            fileName = actions.image_name;

            if (fileName == null || fileName == "") this.Close();

            path = Path.Combine(Properties.Resources.path_images, fileName);
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                scrollablePictureBox1.Image?.Dispose();
                scrollablePictureBox1.Image = Image.FromStream(fs);
                image = Image.FromStream(fs);
            }

            nThreshold.Value = Properties.Settings.Default.Threshold;
            RandersDGV_Rect(id);
        }
        private void RenderRectangle(Graphics g, Rectangle rectangle, bool isHighlighted)
        {
            Color color = isHighlighted ? Color.Red : Color.Blue;
            g.DrawRectangle(new Pen(color, 2), rectangle);
        }
        private List<Rect> rects;
        private void RandersDGV_Rect(int id)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("id", typeof(int)),
                new DataColumn("No", typeof(int)),
                new DataColumn("Rectangle", typeof(string)),
                new DataColumn("Threshold", typeof(int)),
                new DataColumn("Date", typeof(string))
            });

            rects = Rect.GetByAction(id);
            int oldRow = dgvRect.SelectedRows.Count > 0 ? dgvRect.SelectedRows[0].Index : 0;

            using (Bitmap bmp = new Bitmap(image.Width, image.Height))
            {

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(image, 0, 0);
                    for (int i = 0; i < rects.Count; i++)
                    {
                        var item = rects[i];
                        dt.Rows.Add(item.id, i + 1, $"X:{item.x} , Y:{item.y} , W:{item.width} , H:{item.height}", item.threshold, item.updated_at);

                        Rectangle rectangle = new Rectangle(item.x, item.y, item.width, item.height);
                        RenderRectangle(g, rectangle, oldRow == i);
                    }
                }
                scrollablePictureBox1.Image?.Dispose();
                scrollablePictureBox1.Image = (Image)bmp.Clone();
            }

            dgvRect.DataSource = dt;
            dgvRect.ClearSelection();
            if (dgvRect.Rows.Count > 0)
            {
                dgvRect.Rows[Math.Min(oldRow, dgvRect.Rows.Count - 1)].Selected = true;
            }

            dgvRect.Columns["id"].Visible = false;
            dgvRect.Columns["No"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvRect.Columns["Rectangle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvRect.Columns["Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void DrawRectanglesOnImage()
        {
            if (rects == null) return;

            int oldRow = dgvRect.SelectedRows.Count > 0 ? dgvRect.SelectedRows[0].Index : 0;

            using (Bitmap bmp = new Bitmap(image.Width, image.Height))
            {

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(image, 0, 0);
                    for (int i = 0; i < rects.Count; i++)
                    {
                        var item = rects[i];
                        Rectangle rectangle = new Rectangle(item.x, item.y, item.width, item.height);
                        RenderRectangle(g, rectangle, oldRow == i);
                    }
                }
                scrollablePictureBox1.Image?.Dispose();
                scrollablePictureBox1.Image = (Image)bmp.Clone();
            }
        }

        private TypeState typeState = TypeState.Create;
        private void btnSave_Click(object sender, EventArgs e)
        {

            // Clear Rectangle
            scrollablePictureBox1.Refresh();
            if (typeState == TypeState.Update)
            {
                if (dgvRect.SelectedRows.Count == 0) return;
                int id = (int)dgvRect.SelectedRows[0].Cells["id"].Value;

                Rect rect = Rect.Get(id);
                if (rect == null)
                {
                    MessageBox.Show("Not found rect");
                    return;
                }
                rect.threshold = (int)nThreshold.Value;
                rect.Save();
            }
            else if (typeState == TypeState.Create)
            {
                Rectangle rectangle = scrollablePictureBox1.GetRect();
                if (rectangle == Rectangle.Empty)
                {
                    MessageBox.Show("Please select rectangle");
                    return;
                }

                Rect rect = new Rect();
                rect.action_id = id;
                rect.x = rectangle.X;
                rect.y = rectangle.Y;
                rect.width = rectangle.Width;
                rect.height = rectangle.Height;
                rect.threshold = (int)nThreshold.Value;
                rect.Save();
            }

            RandersDGV_Rect(id);
        }

        private void dgvRect_SelectionChanged(object sender, EventArgs e)
        {
            typeState = TypeState.Create;
            btnSave.Text = "Create";
            DrawRectanglesOnImage();
        }
        private bool IsEdit = false;
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvRect.SelectedRows.Count == 0) return;
            int id = (int)dgvRect.SelectedRows[0].Cells["id"].Value;

            Rect rect = Rect.Get(id);
            if (rect == null)
            {
                MessageBox.Show("Not found rect");
                return;
            }
            IsEdit = true;
            nThreshold.Value = rect.threshold;
            typeState = TypeState.Update;
            btnSave.Text = "Update";

            //RandersDGV_Rect(this.id);
            IsEdit = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRect.SelectedRows.Count == 0) return;

            // Ask for confirmation
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int id = (int)dgvRect.SelectedRows[0].Cells["id"].Value;

                Rect rect = Rect.Get(id);
                if (rect == null)
                {
                    MessageBox.Show("Not found rect");
                    return;
                }
                rect.Delete();
                RandersDGV_Rect(this.id);
            }
        }
    }
}
