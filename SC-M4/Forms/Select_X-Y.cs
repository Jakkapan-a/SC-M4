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
            timerVideo.Start();
        }

        private void timerVideo_Tick(object sender, EventArgs e)
        {
            if(_type == 0 && main.bitmapCamaera_01 != null)
            {
                scrollablePictureBox1.Image = (Image)main.bitmapCamaera_01.Clone();
            }else if (_type == 1 && main.bitmapCamaera_02 != null)
            {
                scrollablePictureBox1.Image = (Image)main.bitmapCamaera_02.Clone();
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
    }
}
