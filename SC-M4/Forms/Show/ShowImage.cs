using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace SC_M4.Forms.Show
{
    public partial class ShowImage : Form
    {
        public ShowImage()
        {
            InitializeComponent();
        }

        public void SetImage(Bitmap bitmap)
        {
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = new Bitmap(bitmap);
        }

        private void ShowImage_Load(object sender, EventArgs e)
        {
            // Search for the image in the query string from the path temp
            string path = Properties.Resources.path_temp;
            string[] files = System.IO.Directory.GetFiles(path);
            // Get last image and after the name is _Diff.jpg
            foreach (string file in files)
            {
                if (file.Contains("_Diff.jpg"))
                {
                     using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                     {
                        pictureBox1.Image = Image.FromStream(fs);
                     }
                     break;
                }
            }
            
        }
    }
}
