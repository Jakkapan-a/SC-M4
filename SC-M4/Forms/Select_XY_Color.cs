using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TConstrols;

namespace SC_M4.Forms
{
    public partial class Select_XY_Color : Form
    {
        private ColorAverage colorAverage;
        public Select_XY_Color(ColorAverage colorAverage)
        {
            InitializeComponent();
            this.colorAverage = colorAverage;
        }

        private void Select_XY_Color_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Rectangle rect = scrollablePictureBox1.GetRect();

            if (rect != Rectangle.Empty)
            {
                this.colorAverage.setRect(rect);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select X-Y area!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (colorAverage.main.bmp2 != null)
            {
                scrollablePictureBox1.Image?.Dispose();
                scrollablePictureBox1.Image = (Image)colorAverage.main.bmp2.Clone();
            }
        }

        private void Select_XY_Color_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }
    }
}
