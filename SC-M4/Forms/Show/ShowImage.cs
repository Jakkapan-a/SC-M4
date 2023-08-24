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
                 // Load image
RandersImage();
        }

        private int index = 0;
        private int indexMax = 3;

        private string GetName(int index)
        {
            if(index == 0)
            {
                lbTitle.Text = "Difference";
                return "_Diff.jpg";
            }else if(index == 1)
            {
                lbTitle.Text = "Compare";
                return "_Comparator.jpg";
            }
            else if (index == 2)
            {
                lbTitle.Text = "Master";
                return "_Master.jpg";
            }
            
            return "_Master.jpg";
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if(index > 0)
            {
                index--;
            }
            else
            {
                index = indexMax;
            }

            // Load image
            RandersImage();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (index < indexMax)
            {
                index++;
            }
            else
            {
                index = 0;
            }
            // Load image
            RandersImage();
        }

        private void RandersImage(){
              // Search for the image in the query string from the path temp
            string path = Properties.Resources.path_temp;
            string[] files = System.IO.Directory.GetFiles(path);

            // Short the files by date and time
            // Array.Sort(files, new Comparison<string>((x, y) => DateTime.Compare(File.GetLastWriteTime(x), File.GetLastWriteTime(y))));
            Array.Sort(files, new Comparison<string>((x, y) => DateTime.Compare(File.GetLastWriteTime(y), File.GetLastWriteTime(x))));

            // Get last image and after the name is _Diff.jpg
            foreach (string file in files)
            {
                if (file.Contains(GetName(index)))
                {
                     using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                     {
                        pictureBox1.Image?.Dispose();
                        pictureBox1.Image = Image.FromStream(fs);
                     }
                     break;
                }
            }       
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Properties.Resources.path_temp; // ./system/temp
            // Get current folder
            string currentFolder = Directory.GetCurrentDirectory();
            // Open folder
            path = Path.Combine(currentFolder, path);
            System.Diagnostics.Process.Start(path);

            
        }
    }
}
