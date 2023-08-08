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
    public partial class Actions_io : Form
    {
        public Actions_io()
        {
            InitializeComponent();
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

        private void Actions_io_Load(object sender, EventArgs e)
        {

        }


    }
}
