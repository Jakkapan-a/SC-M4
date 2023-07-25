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
    public partial class Select_X_Y : Form
    {
        private Main main;
        private int _type = -1;
        public Select_X_Y(Main m, int _type)
        {
            InitializeComponent();
            main = m;
            this._type = _type;
        }

        private void Select_X_Y_Load(object sender, EventArgs e)
        {
            cbColor.Visible = false;
            if (_type == 0)
            {
                this.Text = "SELECT X-Y CAMERA 1"; // SELECT X-Y Camera
                this.lbTitle.Text = "SELECT X-Y CAMERA 1";
            }
            else if (_type == 1)
            {
                this.Text = "SELECT X-Y CAMERA 2"; // SELECT X-Y Camera
                this.lbTitle.Text = "SELECT X-Y CAMERA 2";
            }
            else if (_type == 3)
            {
                this.Text = "SELECT X-Y COLOR "; // SELECT X-Y Camera
                this.lbTitle.Text = "SELECT X-Y COLOR";
                cbColor.Visible = true;
                cbColor.Checked = Properties.Settings.Default.isColors;
            }
            timerVideo.Start();
        }

        private void timerVideo_Tick(object sender, EventArgs e)
        {
            if(_type == 0 && main.bitmapCamera_01 != null)
            {
                scrollablePictureBox1.Image?.Dispose();
                scrollablePictureBox1.Image = (Image)main.bitmapCamera_01.Clone();
            }else if (_type == 1 && main.bitmapCamera_02 != null)
            {
                scrollablePictureBox1.Image?.Dispose();
                scrollablePictureBox1.Image = (Image)main.bitmapCamera_02.Clone();
            }
            else if (_type ==2 && main.scrollablePictureBoxCamera02.Image != null)
            {
                scrollablePictureBox1.Image?.Dispose();
                scrollablePictureBox1.Image = (Image)main.scrollablePictureBoxCamera02.Image.Clone();
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            Rectangle rect = scrollablePictureBox1.GetRect();
            if(rect != Rectangle.Empty)
            {
               this.main.saveRect(rect, _type);
               this.Close();
            }
            else
            {
                MessageBox.Show("Please select X-Y area!!!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void cbColor_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isColors = cbColor.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
