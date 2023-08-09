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
        private TCapture.Capture capture;

        public delegate void OnSaveHandler(string fileName);
        public event OnSaveHandler OnSave;
        public Camera()
        {
            InitializeComponent();
            capture = new TCapture.Capture();
            capture.OnFrameHeader += Capture_OnFrameCaptured;
            capture.OnVideoStop += Capture_OnVideoStop;
        }

        private void Camera_Load(object sender, EventArgs e)
        {
            var videoDevices = new List<DsDevice>(DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice));
            foreach (var item in videoDevices)
            {
                cbCamera.Items.Add(item.Name);
            }
            if (cbCamera.Items.Count > 0)
            {
                cbCamera.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No camera found");
            }
        }

        private void Capture_OnFrameCaptured(Bitmap bitmap)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { Capture_OnFrameCaptured(bitmap); }));
                return;
            }

            scrollablePictureBox1.Image?.Dispose();
            scrollablePictureBox1.Image = (Image)bitmap.Clone();
        }
        private bool IsCapturing = false;
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsCapturing)
                {
                    StopCamera();
                }
                else
                {
                    StartCamera();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartCamera()
        {
            scrollablePictureBox1.Image?.Dispose();
            scrollablePictureBox1.Image = Properties.Resources.Spinner_0_4s_800px;
            capture.Start(cbCamera.SelectedIndex);
            btnConnect.Text = "Disconnect";
            IsCapturing = true;
        }

        private void StopCamera()
        {
            capture.Stop();
            btnConnect.Text = "Connect";
            IsCapturing = false;
        }

        private void Capture_OnVideoStop()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { Capture_OnVideoStop(); }));
                return;
            }
            scrollablePictureBox1.Image?.Dispose();
            scrollablePictureBox1.Image = null;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (IsCapturing)
            {
                StopCamera();
            }

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
            string newFileName = Guid.NewGuid().ToString() + ".jpg";
            newFileName = newFileName.Replace("-", "_");
            string path = Path.Combine(Properties.Resources.path_images, newFileName);
            // Save file
            scrollablePictureBox1.Image.Save(path);
            // Invoke event
            OnSave?.Invoke(newFileName);

            if (IsCapturing)
            {
                StopCamera();
            }

            // Close form
            this.Close();
        }
    }
}
