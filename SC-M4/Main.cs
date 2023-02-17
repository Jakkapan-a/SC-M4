using DirectShowLib;
using LogWriter;
using SC_M4.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

        public Rectangle rect_1;
        public Rectangle rect_2;
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
            LogWriter = new LogFile();
            LogWriter.path = Properties.Resources.path_log;
            LogWriter.SaveLog("Start Program");
            btRefresh.PerformClick();

            loadRect(0);
            loadRect(1);
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            var videoDevices = new List<DsDevice>(DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice));
            cbDriveCam01.Items.Clear();
            cbDriveCam02.Items.Clear();
            foreach (DsDevice device in videoDevices)
            {
                cbDriveCam01.Items.Add(device.Name);
                cbDriveCam02.Items.Add(device.Name);
            }

            if (cbDriveCam01.Items.Count > 0)
            {
                cbDriveCam01.SelectedIndex = 0;
                cbDriveCam02.SelectedIndex = 0;
            }

            comboBoxBaud.Items.Clear();
            comboBoxBaud.Items.AddRange(this.baudList);
            if (comboBoxBaud.Items.Count > 0)
                comboBoxBaud.SelectedIndex = comboBoxBaud.Items.Count - 1;

            comboBoxCOMPort.Items.Clear();
            comboBoxCOMPort.Items.AddRange(SerialPort.GetPortNames());
            if (comboBoxCOMPort.Items.Count > 0)
                comboBoxCOMPort.SelectedIndex = 0;
        }

        #region Video Capture

        private void Capture_2_OnVideoStop()
        {
            Console.WriteLine("Video 2 Stop");
            // Clear Image
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    pictureBoxCamera02.Image = null;
                }));
            }
            else
            {
                pictureBoxCamera02.Image = null;
            }
        }

        private void Capture_2_OnVideoStarted()
        {
            Console.WriteLine("Video 2 Start");
        }
        
        private Bitmap bmp2;
        
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
            if (rect_2 != Rectangle.Empty)
            {
                bmp2 = new Bitmap(rect_2.Width, rect_2.Height);
                using (Graphics g = Graphics.FromImage(bmp2))
                {
                    g.DrawImage(bitmapCamaera_02, 0, 0, rect_2, GraphicsUnit.Pixel);
                }
                scrollablePictureBoxCamera02.Image = bmp2;
                // Draw Rectangle to Image
                using (Graphics g = Graphics.FromImage(pictureBoxCamera02.Image))
                {
                    g.DrawRectangle(new Pen(Color.Red, 2), rect_2);
                }
            }
            pictureBoxCamera02.ResumeLayout();
        }

        private void Capture_1_OnVideoStop()
        {
            Console.WriteLine("Video 1 Stop");
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    pictureBoxCamera01.Image = null;
                }));
            }
            else
            {
                pictureBoxCamera01.Image = null;
            }
        }

        private void Capture_1_OnVideoStarted()
        {
            Console.WriteLine("Video 1 Start");
        }
        
        private Bitmap bmp1;
        
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
            if (rect_1 != Rectangle.Empty)
            {
                bmp1 = new Bitmap(rect_1.Width, rect_1.Height);
                using (Graphics g = Graphics.FromImage(bmp1))
                {
                    g.DrawImage(bitmapCamaera_01, 0, 0, rect_1, GraphicsUnit.Pixel);
                }
                scrollablePictureBoxCamera01.Image = bmp1;
                // Draw Rectangle to Image
                using (Graphics g = Graphics.FromImage(pictureBoxCamera01.Image))
                {
                    g.DrawRectangle(new Pen(Color.Red, 2), rect_1);
                }
            }
            pictureBoxCamera01.ResumeLayout();
        }

        #endregion
        
        public void saveRect(Rectangle rect, int _type)
        {
            if (_type == 0)
            {
                rect_1 = rect;
                Properties.Settings.Default.rect1_x = rect.X;
                Properties.Settings.Default.rect1_y = rect.Y;

                Properties.Settings.Default.rect1_w = rect.Width;
                Properties.Settings.Default.rect1_h = rect.Height;

                Properties.Settings.Default.Save();
            }
            else if (_type == 1)
            {
                rect_2 = rect;
                Properties.Settings.Default.rect2_x = rect.X;
                Properties.Settings.Default.rect2_y = rect.Y;

                Properties.Settings.Default.rect2_w = rect.Width;
                Properties.Settings.Default.rect2_h = rect.Height;
                
                
                Properties.Settings.Default.Save();
            }
        }

        public void loadRect(int _type)
        {
            if (_type == 0 && Properties.Settings.Default.rect1_x != 0)
            {
                rect_1 = new Rectangle(Properties.Settings.Default.rect1_x, Properties.Settings.Default.rect1_y, Properties.Settings.Default.rect1_w, Properties.Settings.Default.rect1_h);
                toolStripStatusRect1.Text = "Rect 1 : X=" + rect_1.X.ToString() + ", Y=" + rect_1.Y.ToString() + ", H=" + rect_1.Height.ToString()+ ", W=" + rect_1.Width.ToString();
            }
            else if (_type == 1 && Properties.Settings.Default.rect2_x != 0)
            {
                rect_2 = new Rectangle(Properties.Settings.Default.rect2_x, Properties.Settings.Default.rect2_y, Properties.Settings.Default.rect2_w, Properties.Settings.Default.rect2_h);
                toolStripStatusRect2.Text = "Rect 2 : X=" + rect_2.X.ToString() + ", Y=" + rect_2.Y.ToString() + ", H=" + rect_2.Height.ToString() + ", W=" + rect_2.Width.ToString();
            }
        }

        private bool isStart = false;
        private int driveindex_01 = -1;
        private int driveindex_02 = -1;
        private string serialportName = string.Empty;
        private string serialportBaud = string.Empty;
        private Thread thread;
        private void btStartStop_Click(object sender, EventArgs e)
        {
            this.isStart = !this.isStart;
            try
            {

                if (this.isStart)
                {

                    if (txtEmployee.Text == string.Empty)
                    {
                        lbTitle.Text = "Please input employee ID"; // STATUS_PROCES_10
                        throw new Exception("Please input employee ID");

                    }

                    if (cbDriveCam01.SelectedIndex == cbDriveCam02.SelectedIndex)
                    {
                        lbTitle.Text = "Please select camera drive!"; 
                        throw new Exception("Please select camera drive!");

                    }

                    if (this.cbDriveCam01.SelectedIndex == -1 || this.cbDriveCam02.SelectedIndex == -1)
                    {
                        throw new Exception(Properties.Resources.msg_select_camera);
                    }
                    if (txtEmployee.Text == string.Empty)
                    {
                        lbTitle.Text = "Please input employee ID"; // 
                        this.ActiveControl = txtEmployee;
                        this.txtEmployee.Focus();
                        throw new Exception("Please input employee ID");
                    }

                    this.serialportName = comboBoxCOMPort.Text;
                    this.serialportBaud = comboBoxBaud.Text;
                    //serialConnect();

                    if (capture_1.IsOpened)
                        capture_1.Stop();
                    if (capture_2.IsOpened)
                        capture_2.Stop();

                    driveindex_01 = cbDriveCam01.SelectedIndex;
                    driveindex_02 = cbDriveCam02.SelectedIndex;
                    Task.Factory.StartNew(() => capture_1.Start(driveindex_01));
                    Task.Factory.StartNew(() => capture_2.Start(driveindex_02));

                    lbTitle.Text = "Camera opening...";

                    btStartStop.Text = "STOP";
                    if (thread != null)
                    {
                        thread.Abort();
                        thread.DisableComObjectEagerCleanup();
                        thread = null;
                    }

                    //thread = new Thread(new ThreadStart(ProcessTesting));
                    //thread.Start();
                    this.richTextBox1.Text = string.Empty;
                    this.richTextBox2.Text = string.Empty;

                    scrollablePictureBoxCamera01.Image = null;
                    scrollablePictureBoxCamera02.Image = null;

                    btConnect.Text = "Disconnect";
                }
                else
                {
                    if (capture_1._isRunning)
                        capture_1.Stop();

                    if (capture_2._isRunning)
                        capture_2.Stop();

                    btStartStop.Text = "START";
                    btConnect.Text = "Connect";
                    pictureBoxCamera01.Image = null;
                    pictureBoxCamera02.Image = null;

                    this.richTextBox1.Text = string.Empty;
                    this.richTextBox2.Text = string.Empty;

                    scrollablePictureBoxCamera01.Image = null;
                    scrollablePictureBoxCamera02.Image = null;
                    lbTitle.Text = "Camera close.";
                    if (thread != null)
                    {
                        thread.Abort(true);
                    }
                }
                lbTitle.BackColor = Color.Yellow;
                lbTitle.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                LogWriter.SaveLog("Error Start :" + ex.Message);
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.isStart = false;
                btStartStop.Text = "START";
            }
        }

        #region SELECT X Y

        private Select_X_Y select_XY = null;
        private void selectXYCAM1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBoxCamera01.Image == null)
            {
                MessageBox.Show("Please open camera 1", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (select_XY != null)
            {
                select_XY.Close();
                select_XY = null;
            }

            select_XY = new Select_X_Y(this, 0);
            select_XY.Show();
        }

        private void selectXYCAM2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBoxCamera02.Image == null)
            {
                MessageBox.Show("Please select camera 2", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (select_XY != null)
            {
                select_XY.Close();
                select_XY = null;
            }

            select_XY = new Select_X_Y(this, 1);
            select_XY.Show();
        }

        #endregion

        private void btConnect_Click(object sender, EventArgs e)
        {

        }
    }


    public class TCapture
    {
        public class Capture
        {
            private Thread _thread;
            private OpenCvSharp.VideoCapture _videoCapture;

            public delegate void VideoCaptureError(string messages);
            public event VideoCaptureError OnError;

            public delegate void VideoFrameHeadler(Bitmap bitmap);
            public event VideoFrameHeadler OnFrameHeadler;

            public delegate void VideoCaptureStop();
            public event VideoCaptureStop OnVideoStop;

            public delegate void VideoCaptureStarted();
            public event VideoCaptureStarted OnVideoStarted;

            private bool _onStarted = false;

            public bool _isRunning { get; set; }

            private int _frameRate = 50;

            public int width { get; set; }
            public int height { get; set; }
            public int frameRate
            {
                get { return _frameRate; }
                set { _frameRate = 1000 / value; }
            }

            public bool IsOpened
            {
                get { return IsOpen(); }
            }
            public bool IsOpen()
            {
                if (_videoCapture != null && _videoCapture.IsOpened())
                {
                    return true;
                }
                return false;
            }

            public Capture()
            {
                width = 1280;
                height = 720;
            }
            public void Start(int device)
            {
                if (_videoCapture != null)
                {
                    _videoCapture.Dispose();
                }

                _videoCapture = new OpenCvSharp.VideoCapture(device);
                _videoCapture.Open(device);
                setFrame(width, height);
                _isRunning = true;
                _onStarted = true;
                if (_thread != null)
                {
                    _thread.Abort();
                }
                _thread = new Thread(FrameCapture);
                _thread.Start();
            }

            private void FrameCapture()
            {
                try
                {
                    while (_isRunning)
                    {

                        if (_videoCapture.IsOpened())
                        {
                            using (OpenCvSharp.Mat frame = _videoCapture.RetrieveMat())
                            {
                                if (frame.Empty())
                                {
                                    OnError?.Invoke("Frame is empty");
                                    continue;
                                }
                                using (Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame))
                                {
                                    OnFrameHeadler?.Invoke(bitmap);
                                }
                            }
                            if (_onStarted)
                            {
                                OnVideoStarted?.Invoke();
                                _onStarted = false;
                            }
                        }

                        Thread.Sleep(_frameRate);
                    }
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(ex.Message);
                }
            }

            public void setFrame(int width, int height)
            {
                _videoCapture.Set(OpenCvSharp.VideoCaptureProperties.FrameWidth, width);
                _videoCapture.Set(OpenCvSharp.VideoCaptureProperties.FrameHeight, height);
            }

            public void Stop()
            {
                _isRunning = false;
                if (_videoCapture != null)
                {
                    _videoCapture.Release();
                }
                OnVideoStop?.Invoke();
            }

            public void Resumed()
            {
                _isRunning = true;
                if (_thread != null)
                {
                    _thread.Abort();
                }
                _thread = new Thread(FrameCapture);
                _thread.Start();
            }

            public void Dispose()
            {
                _isRunning = false;
                if (_videoCapture != null)
                {
                    _videoCapture.Dispose();
                }
                if (_thread != null)
                {
                    _thread.Abort();
                }
            }

            // Get Focus
            public int GetFocus()
            {
                return (int)_videoCapture.Get(OpenCvSharp.VideoCaptureProperties.Focus);
            }
            // Set Focus 
            public void SetFocus(int value)
            {
                _videoCapture.Set(OpenCvSharp.VideoCaptureProperties.Focus, value);
            }
            // Auto Focus
            public void AutoFocus()
            {
                _videoCapture.Set(OpenCvSharp.VideoCaptureProperties.Focus, -1);
            }

            // Get Zoom
            public int GetZoom()
            {
                return (int)_videoCapture.Get(OpenCvSharp.VideoCaptureProperties.Zoom);
            }

            // Set Zoom
            public void SetZoom(int value)
            {
                _videoCapture.Set(OpenCvSharp.VideoCaptureProperties.Zoom, value);
            }
            // Get Exposure
            public int GetExposure()
            {
                return (int)_videoCapture.Get(OpenCvSharp.VideoCaptureProperties.Exposure);
            }

            // Set Exposure
            public void SetExposure(int value)
            {
                _videoCapture.Set(OpenCvSharp.VideoCaptureProperties.Exposure, value);
            }

            // Get Gain
            public int GetGain()
            {
                return (int)_videoCapture.Get(OpenCvSharp.VideoCaptureProperties.Gain);
            }

            // Set Gain
            public void SetGain(int value)
            {
                _videoCapture.Set(OpenCvSharp.VideoCaptureProperties.Gain, value);
            }

            // Set Brightness
            public void SetBrightness(int value)
            {
                _videoCapture.Set(OpenCvSharp.VideoCaptureProperties.Brightness, value);
            }

            // Set Contrast
            public void SetContrast(int value)
            {
                _videoCapture.Set(OpenCvSharp.VideoCaptureProperties.Contrast, value);
            }

            // Set Saturation
            public void SetSaturation(int value)
            {
                _videoCapture.Set(OpenCvSharp.VideoCaptureProperties.Saturation, value);
            }

        }
    }
}
