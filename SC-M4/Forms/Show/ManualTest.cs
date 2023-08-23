using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SC_M4.Forms.Show
{
    public partial class ManualTest : Form
    {
        public ManualTest()
        {
            InitializeComponent();
        }

        private void ManualTest_Load(object sender, EventArgs e)
        {

        }

        public void SetImage(Bitmap bitmap)
        {
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = new Bitmap(bitmap);
        }
    }
}
