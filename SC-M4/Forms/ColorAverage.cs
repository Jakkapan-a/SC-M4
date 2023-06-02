using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SC_M4.Utilities.Heller;

namespace SC_M4.Forms
{
    public partial class ColorAverage: Form
    {
        public Main main;
        public ColorAverage(Main main)
        {
            InitializeComponent();
            this.main = main;
        }

        private void ColorAverage_Load(object sender, EventArgs e)
        {
            decimal max = 2500;
            nbX.Maximum = max;
            nbY.Maximum = max;
            nbWidth.Maximum = max;
            nbHeight.Maximum = max;

            decimal min = 0;
            nbX.Minimum = min;
            nbY.Minimum = min;
            nbWidth.Minimum = min;
            nbHeight.Minimum = min;

            rbTopLeft.Enabled = true;
            rbBottomRight.Enabled = false;
            rbTopRight.Enabled = false;
            rbBottomLeft.Enabled = false;

            switch ((AverageColorType)Properties.Settings.Default.ColorAverageType)
            {
                case AverageColorType.Full:
                    rbFull.Checked = true;
                    break;
                case AverageColorType.TopLeft:
                    rbTopLeft.Checked = true;
                    break;
                case AverageColorType.TopRight:
                    rbTopRight.Checked = true;
                    break;
                case AverageColorType.BottomLeft:
                    rbBottomLeft.Checked = true;
                    break;
                case AverageColorType.BottomRight:
                    rbBottomRight.Checked = true;
                    break;
            }
        }

        private void rbTopLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFull.Checked) {
                nbWidth.Enabled = false;
                nbHeight.Enabled = false;
                nbX.Enabled = false;
                nbY.Enabled = false;
                btnSelect.Enabled = false;
            }
            else
            {
                nbWidth.Enabled = true;
                nbHeight.Enabled = true;
                nbX.Enabled = true;
                nbY.Enabled = true;
                btnSelect.Enabled = true;
            }
        }
        public void setRect(Rectangle rect)
        {
            nbX.Value = rect.X;
            nbY.Value = rect.Y;
            nbWidth.Value = rect.Width;
            nbHeight.Value = rect.Height;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.color_x = (int)nbX.Value;
            Properties.Settings.Default.color_y = (int)nbY.Value;
            Properties.Settings.Default.color_width = (int)nbWidth.Value;
            Properties.Settings.Default.color_high = (int)nbHeight.Value;

            if (rbFull.Checked)
            {
                Properties.Settings.Default.ColorAverageType = (int)AverageColorType.Full;
            }
            else if(rbTopLeft.Checked)
            {
                Properties.Settings.Default.ColorAverageType = (int)AverageColorType.TopLeft;
            }

            Properties.Settings.Default.Save();

            this.Close();
        }
        private Select_XY_Color Select_XY_Color;
        private void btnSelect_Click(object sender, EventArgs e)
        {
            Select_XY_Color?.Dispose();
            Select_XY_Color = new Select_XY_Color(this);
            Select_XY_Color.Show();
        }
    }
}
