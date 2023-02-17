using LogWriter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SC_M4
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        public TCapture.Capture capture_1;
        public TCapture.Capture capture_2;

        public Bitmap bitmapCamaera_01;
        public Bitmap bitmapCamaera_02;

        LogFile LogWriter;
        public string[] baudList = { "9600", "19200", "38400", "57600", "115200" };



        private void Main_Load(object sender, EventArgs e)
        {
            // Create Folder
            if (!Directory.Exists(Properties.Resources.path_temp))
                Directory.CreateDirectory(Properties.Resources.path_temp);
            if (!Directory.Exists(Properties.Resources.path_log))
                Directory.CreateDirectory(Properties.Resources.path_log);
            if (!Directory.Exists(Properties.Resources.path_images))
                Directory.CreateDirectory(Properties.Resources.path_images);
            // Create Video Capture Object
            capture_1 = new TCapture.Capture();
            capture_1.OnFrameHeadler += Capture_1_OnFrameHeadler;
            capture_1.OnVideoStarted += Capture_1_OnVideoStarted;
            capture_1.OnVideoStop += Capture_1_OnVideoStop;
            capture_2 = new TCapture.Capture();
            capture_2.OnFrameHeadler += Capture_2_OnFrameHeadler;
            capture_2.OnVideoStarted += Capture_2_OnVideoStarted;
            capture_2.OnVideoStop += Capture_2_OnVideoStop;
            this.ActiveControl = txtEmployee;
            txtEmployee.Focus();

            loadDrive();
        }

        public void loadDrive(){
            string[] driveList = System.IO.Directory.GetLogicalDrives();
            foreach (string drive in driveList)
            {
                cbDriveCam01.Items.Add(drive);
                cbDriveCam02.Items.Add(drive);
            }
        }

        private void Capture_2_OnVideoStop()
        {
            Console.WriteLine("Video 1 Stop");
        }

        private void Capture_2_OnVideoStarted()
        {
            Console.WriteLine("Video 1 Start");
        }

        private void Capture_2_OnFrameHeadler(Bitmap bitmap)
        {
            if (pictureBoxCamera02.InvokeRequired)
            {
                pictureBoxCamera02.Invoke(new Action(() => Capture_2_OnFrameHeadler(bitmap)));
                return;
            }

            pictureBoxCamera02.SuspendLayout();
            pictureBoxCamera02.Image = new Bitmap(bitmap);
            bitmapCamaera_02 = (Bitmap)pictureBoxCamera02.Image.Clone();
            pictureBoxCamera02.ResumeLayout();
        }

        private void Capture_1_OnVideoStop()
        {
            Console.WriteLine("Video 1 Stop");
        }

        private void Capture_1_OnVideoStarted()
        {
            Console.WriteLine("Video 1 Start");
        }

        private void Capture_1_OnFrameHeadler(Bitmap bitmap)
        {
            if (pictureBoxCamera01.InvokeRequired)
            {
                pictureBoxCamera01.Invoke(new Action(()=>Capture_1_OnFrameHeadler(bitmap)));
                return;
            }

            pictureBoxCamera01.SuspendLayout();
            pictureBoxCamera01.Image = new Bitmap(bitmap);
            bitmapCamaera_01 = (Bitmap)pictureBoxCamera01.Image.Clone();
            pictureBoxCamera01.ResumeLayout();
        }

        private void btStartStop_Click(object sender, EventArgs e)
        {

        }

     
    }
}
