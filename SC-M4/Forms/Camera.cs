using DirectShowLib;
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
using TClass;

namespace SC_M4.Forms
{
    public partial class Camera : Form
    {
        public event EventHandler<EventArgs> OnSave;
        private Main main;
        public Camera(Main main)
        {
            InitializeComponent();
            this.main = main;
        }

        private string[] cameras = { "CAM 1", "CAM 2" };
        private void Camera_Load(object sender, EventArgs e)
        {
            foreach (var item in cameras)
            {
                cbCamera.Items.Add(item);
            }
            if (cbCamera.Items.Count > 0)
            {
                cbCamera.SelectedIndex = 0;
            }
           
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // Get Image
                scrollablePictureBox1.Image?.Dispose();
                scrollablePictureBox1.Image = null;

                if (cbCamera.SelectedIndex == 0)
                {
                    if(main.bitmapCamera_01 != null)
                    {
                        scrollablePictureBox1.Image = new Bitmap(main.bitmapCamera_01);
                    }
                }
                else
                {
                    if (main.bitmapCamera_02 != null)
                    {
                    
                        scrollablePictureBox1.Image = new Bitmap(main.bitmapCamera_02);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Check if file is image
                if (!openFileDialog.FileName.EndsWith(".jpg") && !openFileDialog.FileName.EndsWith(".jpeg") && !openFileDialog.FileName.EndsWith(".png"))
                {
                    MessageBox.Show("File is not image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Open file readonly
                using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    // Check if file is empty
                    if (fs.Length == 0)
                    {
                        MessageBox.Show("File is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Set file to picturers
                    scrollablePictureBox1.Image?.Dispose();
                    scrollablePictureBox1.Image = Image.FromStream(fs);
                }
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
         
            if (scrollablePictureBox1.Image == null)
            {
                MessageBox.Show("No image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(Properties.Resources.path_images))
            {
                Directory.CreateDirectory(Properties.Resources.path_images);
            }


            string newFileName = Guid.NewGuid().ToString() + ".jpg";
            newFileName = newFileName.Replace("-", "_");
            string path = Path.Combine(Properties.Resources.path_images, newFileName);
            // Save file
            scrollablePictureBox1.Image.Save(path);
            // Invoke event
            OnSave?.Invoke(newFileName,EventArgs.Empty);
            // Close form
            this.Close();
        }
    }
}
