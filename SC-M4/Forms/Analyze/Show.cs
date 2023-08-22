using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SC_M4.Forms.Analyze
{
    public partial class Show : Form
    {
        
        public Show(Image image)
        {
            InitializeComponent();
            pictureBox1.Image = image;
        }

        private void Show_Load(object sender, EventArgs e)
        {

        }
    }
}
