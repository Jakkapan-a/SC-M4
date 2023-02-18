using DirectShowLib;
using LogWriter;
using SC_M4.Forms;
using SC_M4.Modules;
using SC_M4.Ocr;
using SC_M4.OCR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Color = System.Drawing.Color;
using Windows.Media.Ocr;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using BitmapDecoder = Windows.Graphics.Imaging.BitmapDecoder;
using System.Drawing.Imaging;

namespace SC_M4
{
    public partial class Main : Form
    {
        private  Language SelectedLang = null;

        //private SerialPort serialPort;
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

        protected string curLangCode = "eng";
        protected IList<Image> imageList;
        protected string inputfilename;
        protected int imageIndex;
        protected float scaleX = 1f;
        protected float scaleY = 1f;
        protected string selectedPSM = "Auto"; // 3 - Fully automatic page segmentation, but no OSD (default)
        protected string selectedOEM = "3"; // Default

        private bool isStaetReset;
        private void Main_Load(object sender, EventArgs e)
        {

            foreach (ToolStripItem item in statusStripHome.Items)
            {
                item.Text = "";
            }

            SelectedLang = new Language("en-US");
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
            try
            {
                var s = Setting.GetSettingRemove();
                if (s.Count > 0)
                {
                    foreach (var set in s)
                    {
                        if (File.Exists(set.path_image))
                        {
                            File.Delete(set.path_image);
                            set.Delete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.SaveLog("Error delete file : " + ex.Message);
            }
            timerMain.Start();
            deletedFileTemp();
            loadTableHistory();

        }
        private async void deletedFileTemp()
        {
            try
            {
                string _dir = Properties.Resources.path_temp;
                string[] files = Directory.GetFiles(_dir);
                int i = 0;
                await Task.Delay(1);
                foreach (string file in files)
                {
                    i++;
                    FileInfo info = new FileInfo(file);
                    if (info.LastAccessTime < DateTime.Now.AddMinutes(-30))
                        info.Delete();
                    if (i > 200)
                        break;
                }
                i = 0;
                files.Reverse();
                foreach (string file in files)
                {
                    i++;
                    FileInfo info = new FileInfo(file);
                    if (info.LastAccessTime < DateTime.Now.AddMinutes(-30))
                        info.Delete();
                    if (i > 200)
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void loadTableHistory()
        {
            var list = History.GetHistory();
            dataGridViewHistory.DataSource = null;
            int i = 0;
            // Reverse the list to display the latest record first
            var data = (from p in list
                        select new
                        {
                            ID = p.id,
                            No = ++i,
                            Employee = p.name,
                            MasterSW = p.master_sw,
                            Software = p.name_sw,
                            Master_Model = p.master_lb,
                            Models = p.name_lb,
                            Results = p.result,
                            Update = p.created_at
                        }).ToList();
            dataGridViewHistory.DataSource = data;
            dataGridViewHistory.Columns["ID"].Visible = false;
            // 10% of the width of the DataGridView
            dataGridViewHistory.Columns["No"].Width = dataGridViewHistory.Width * 10 / 100;
            // last 20% of the width of the DataGridView
            dataGridViewHistory.Columns["Update"].Width = dataGridViewHistory.Width * 20 / 100;
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
            LogWriter.SaveLog("Video 2 Stop");
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
            LogWriter.SaveLog("Video 2 Start");
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
            if (rect_2 != Rectangle.Empty && isStaetReset)
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
            LogWriter.SaveLog("Video 1 Stop");
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
            LogWriter.SaveLog("Video 1 Start");
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
            //pictureBoxCamera01.Image = new Bitmap(@"\C:\\Users\\Jakkapan\\OneDrive\\Pictures\\aaaaaaa.jpg\");

            bitmapCamaera_01 = (Bitmap)pictureBoxCamera01.Image.Clone();
            if (rect_1 != Rectangle.Empty && isStaetReset)
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

        #region Start
        private void btStartStop_Click(object sender, EventArgs e)
        {
            this.isStart = !this.isStart;
            try
            {

                if (this.isStart)
                {

                    if (txtEmployee.Text == string.Empty)
                    {
                        this.ActiveControl = txtEmployee;
                        txtEmployee.Focus();
                        lbTitle.Text = "Please input employee ID"; //
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
                    serialConnect();

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

                    thread = new Thread(new ThreadStart(ProcessTesting));
                    thread.Start();
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

                    if (serialPort.IsOpen)
                        serialPort.Close();

                    btStartStop.Text = "START";
                    btConnect.Text = "Connect";
                    pictureBoxCamera01.Image = null;
                    pictureBoxCamera02.Image = null;

                    this.richTextBox1.Text = string.Empty;
                    this.richTextBox2.Text = string.Empty;



                    scrollablePictureBoxCamera01.Image = null;
                    scrollablePictureBoxCamera02.Image = null;
                    lbTitle.Text = "Camera close.";
                    lbTitle.ForeColor = Color.Black;
                    lbTitle.BackColor = Color.Yellow;
                    is_Blink_NG = false;

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

        private void ProcessTesting()
        {
            bool detection = false;
            string result_1 = string.Empty;
            string result_2 = string.Empty;
            isStaetReset = true;
            Stopwatch stopwatch = new Stopwatch();
            while (true)
            {
                if (capture_1._isRunning && capture_2._isRunning && bitmapCamaera_01 != null && bitmapCamaera_02 != null && isStaetReset)
                {
                    // 
                    stopwatch.Reset();
                    stopwatch.Start();
                    detection = !detection;
                    if (detection)
                    {
                        Invoke(new Action(() =>
                        {
                            lbTitle.Text = "Wiat for detect..";
                        }));
                    }
                    else
                    {
                        Invoke(new Action(() =>
                        {
                            lbTitle.Text = "Detecting...";
                        }));
                    }
                    // Image 02
                    imageList = new List<Image>();
                    imageList.Add((Image)scrollablePictureBoxCamera01.Image.Clone());

                    result_1 = performOCR(imageList, inputfilename, imageIndex, Rectangle.Empty);
                    
                    var a = result_1.IndexOf("-731");
                    result_1 = result_1.Substring(a + 1);
                    a = result_1.IndexOf("|731");
                    result_1 = result_1.Substring(a + 1);
                    result_1 = result_1.Replace("T31TM", "731TM");
                    result_1 = result_1.Replace("731THC", "731TMC");
                    result_1 = result_1.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace("\\", "").Replace("|", "").Replace(@"\", "");
                    richTextBox1.Invoke(new Action(() =>
                    {
                        this.richTextBox1.Text = string.Empty;
                        this.richTextBox1.Text = result_1.Trim();
                    }));
                    // Image 02
                    int lb = result_1.IndexOf("731TMC");
                    if (result_1 != string.Empty && lb != -1)
                    {
                        // OCR 2
                        result_2 = string.Empty;
                        //imageList = new List<Image>();
                        var ocr = OcrProcessor.GetOcrResultFromBitmap((Bitmap)scrollablePictureBoxCamera02.Image.Clone(), SelectedLang);
                        result_2 = ocr.Result.Text;
                        //result_2 = "9U7310TM063-01731TMCasfea";
                        result_2 = result_2.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
                        result_2 = Regex.Replace(result_2, "[^a-zA-Z,0-9,(),:,-]", "");

                        result_2 = result_2.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace("'", "").Replace("|", "").Replace(@"\", "");
                        result_2 = result_2.Replace(")9U", "9U");

                        richTextBox2.Invoke(new Action(() =>
                        {
                            this.richTextBox2.Text = string.Empty;
                            this.richTextBox2.Text = result_2.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
                        }));
                        int result = Compare_Master(this.richTextBox1.Text, this.richTextBox2.Text);
                        if (result == 1 || result == 2)
                        {
                            isStaetReset = false;
                        }
                    }

                    stopwatch.Stop();
                    Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
                    Invoke(new Action(() =>
                    {
                        toolStripStatusTime.Text = "Load " + stopwatch.ElapsedMilliseconds.ToString() + "ms";
                    }));
                }
                Thread.Sleep(1000);
            }
        }
        
        History history;

        private bool is_Blink_NG = false;
        private int Compare_Master(string txt_sw, string txt_lb)
        {
            // 0 = not fount, 1 = OK, 2 = NG
            int result = 0;

            LogWriter.SaveLog("TXT Read :" + txt_sw.Replace("\r","").Replace("\n", "") + ", " + txt_lb.Replace("\r", "").Replace("\n", ""));
            //lbTitle.Text;
            history = new History();
            //txt_lb = txt_lb.Replace("O", "0");
            int lb = txt_lb.IndexOf("731TMC");
            // If not found, IndexOf returns -1.
            if (lb == -1)
            {
                // Return the original string.
                result = 0;
                //Invoke(new Action(() =>
                //{
                //    this.richTextBox1.Text = string.Empty;
                //    this.richTextBox2.Text = string.Empty;
                //}));
                return result;
            }

            int swa = txt_sw.IndexOf("731TMC");
            // If not found, IndexOf returns -1.
            if (swa == -1)
            {
                result = 0;
                //Invoke(new Action(() =>
                //{
                //    this.richTextBox1.Text = string.Empty;
                //    this.richTextBox2.Text = string.Empty;
                //}));
                return result;
            }
            var txt = txt_lb.Substring(0, lb);
            txt = txt.Replace("O", "0");
            var master_lb = MasterAll.GetMasterALLByLBName(txt);

            bool check = false;
            if (master_lb.Count > 0)
            {
                foreach (var item in master_lb)
                {
                    history.master_sw = item.nameSW;
                    history.master_lb = item.nameModel;
                    if (item.nameSW == txt_sw)
                    {
                        check = true;
                        break;
                    }
                }
            }
            else
            {
                history.master_sw = "null";
                history.master_lb = "null";
            }

            if (!check)
            {
                Invoke(new Action(() =>
                {
                    lbTitle.Text = "NG";
                    lbTitle.ForeColor = Color.White;
                    lbTitle.BackColor = Color.Red;
                    //loadTableHistory();
                }));
                is_Blink_NG = true;
                serialCommand("NG");
             
                result = 1;
            }
            else
            {

                Invoke(new Action(() =>
                {
                    lbTitle.Text = "OK";
                    lbTitle.ForeColor = Color.White;
                    lbTitle.BackColor = Color.Green;
                    //loadTableHistory();
                }));
                result = 2;
                serialCommand("OK");
            }
            history.name = txtEmployee.Text.Trim();
            history.name_lb = txt_lb;
            history.name_sw = txt_sw;
            history.result = check ? "OK" : "NG";
            history.Save();
            LogWriter.SaveLog("Result :" + history.result);
            LogWriter.SaveLog("SW :" + txt_lb);
            LogWriter.SaveLog("LABEL :" + txt_lb);
            isStaetReset = false;
            Invoke(new Action(() =>
            {
                loadTableHistory();
            }));
            return result;
        }
        #endregion

        #region Serial Port 

        //private string serialportName = string.Empty;

        public void setSerialPort(string portName, string baud)
        {
            this.serialportName = portName;
            this.serialportBaud = baud;
        }

        private void serialConnect(string portName, int baud)
        {
            try
            {
                if (this.serialPort != null && this.serialPort.IsOpen)
                {
                    this.serialPort.Close();
                }
                //this.serialPort = new SerialPort();
                this.serialPort.PortName = portName;
                this.serialPort.BaudRate = baud;
                this.serialPort.Open();
                this.serialCommand("conn");
                Thread.Sleep(50);
                this.serialCommand("conn");
                this.toolStripStatusConnectSerialPort.Text = "Serial Connected"; // Serial Connected
                this.toolStripStatusConnectSerialPort.ForeColor = Color.Green;

            }
            catch (Exception ex)
            {
                LogWriter.SaveLog("Error :" + ex.Message);
                this.toolStripStatusConnectSerialPort.Text = "Serial Port: Disconnect"; // "Serial Port: Disconnect";
                this.toolStripStatusConnectSerialPort.ForeColor = Color.Red;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public void serialConnect()
        {

            if (this.serialportName == string.Empty || this.serialportBaud == string.Empty)
            {
               throw new Exception("Please select serial port and baud rate");
            }
            this.serialConnect(this.serialportName, int.Parse(this.serialportBaud));
        }

        public void serialCommand(string command)
        {
            if (this.serialPort.IsOpen)
            {
                this.serialPort.Write(">" + command + "<#");
                LogWriter.SaveLog("Serial send : " + command);
            }
        }

        private string readDataSerial = string.Empty;
        private string dataSerialReceived = string.Empty;
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                readDataSerial = this.serialPort.ReadExisting();
                this.Invoke(new EventHandler(dataReceived));
            }
            catch (Exception ex)
            {
                LogWriter.SaveLog("Error :" + ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataReceived(object sender, EventArgs e)
        {
            this.dataSerialReceived += readDataSerial;
            if (dataSerialReceived.Contains(">") && dataSerialReceived.Contains("<"))
            {
                string data = this.dataSerialReceived.Replace("\r", string.Empty).Replace("\n", string.Empty);
                data = data.Substring(data.IndexOf(">") + 1, data.IndexOf("<") - 1);
                this.dataSerialReceived = string.Empty;
                Console.WriteLine("RST : " + data);
                data = data.Replace(">", "").Replace("<", "");
                toolStripStatusSerialData.Text = "DATA :" + data;
                LogWriter.SaveLog("Serial Received : " + data);
                if (data == "rst" || data.Contains("rst"))
                {

                    isStaetReset = true;
                    is_Blink_NG = false;
                    if (capture_1.IsOpened && capture_1.IsOpened)
                    {
                        lbTitle.Text = "Wiat for detect...."; // Wiat for detect....
                    }
                    lbTitle.ForeColor = Color.Black;
                    lbTitle.BackColor = Color.Yellow;
                    richTextBox1.Text = "";
                    richTextBox2.Text = "";
                }
            }
            else if (!dataSerialReceived.Contains(">"))
            {
                this.dataSerialReceived = string.Empty;
            }
        }

        #endregion

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
            btStartStop.PerformClick();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (capture_1 != null)
            {
                if(capture_1.IsOpened)
                    capture_1.Stop();
                
                capture_1.Dispose();
            }

            if (capture_2 != null)
            {
                if (capture_2.IsOpened)
                    capture_2.Stop();

                capture_2.Dispose();
            }

            if (thread != null)
            {
                if (thread.IsAlive)
                    thread.Abort();
                thread.DisableComObjectEagerCleanup();
                thread = null;
            }
        }

        private string performOCR(IList<Image> imageList, string inputfilename, int index, Rectangle rect)
        {
            try
            {
                if (curLangCode.Trim().Length == 0)
                {
                    MessageBox.Show(this, "curLangCode = 0");
                    return "";
                }
                OCRImageEntity entity = new OCRImageEntity(imageList, inputfilename, index, rect, curLangCode);
                entity.ScreenshotMode = false;
                entity.Language = "eng";
                OCR<Image> ocrEngine = new OCRImages();
                ocrEngine.PageSegMode = selectedPSM;
                ocrEngine.OcrEngineMode = selectedOEM;
                ocrEngine.Language = entity.Language;

                IList<Image> images = entity.ClonedImages;

                string result = ocrEngine.RecognizeText(((List<Image>)images).GetRange(0, 1), entity.Inputfilename);
                return result;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Exclamation A00", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                LogWriter.SaveLog("A00 : " + ex.Message);
            }
            return "";
        }

        private void testOCRToolStripMenuItem_Click(object sender, EventArgs e)
        {
           //var  result_2 = OcrProcessor.GetOcrResultFromBitmap((Bitmap)scrollablePictureBoxCamera02.Image.Clone(),SelectedLang);

        }
        private bool toggle_blink_ng = false;
        private void timerMain_Tick(object sender, EventArgs e)
        {
            if (is_Blink_NG)
            {
                toggle_blink_ng = !toggle_blink_ng;
                if (toggle_blink_ng)
                {
                    lbTitle.BackColor = Color.Red;
                    lbTitle.ForeColor = Color.White;
                }
                else
                {
                    lbTitle.BackColor = Color.White;
                    lbTitle.ForeColor = Color.Red;
                }
            }
            else if (lbTitle.BackColor != Color.Yellow && isStaetReset && (lbTitle.Text != "OK" || lbTitle.Text != "NG"))
            {
                lbTitle.BackColor = Color.Yellow;
                lbTitle.ForeColor = Color.Black;
            }
        }

        private void numericUpDownFocus_ValueChanged(object sender, EventArgs e)
        {
            if (!checkBoxAutoFocus.Checked)
            {
                capture_2.SetFocus((int)numericUpDownFocus.Value);
            }
            else
            {
                capture_2.AutoFocus();
            }
        }
        SettingModel setting;
        private void masterListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (setting != null)
            {
                setting.Close();
                setting.Dispose();
            }

            setting = new SettingModel();
            setting.Show();
        }
    }

    #region TCapture
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
    #endregion

    /*
    public static class SettingHandler
    {
        public static Dictionary<string, string> DefaultSetting = new Dictionary<string, string>(){
            {"Language", ""},
            {"WrapText", "newLine"},
            {"IsTooltipShowed", "false"},
            {"RecentAccessFolderToken", ""}
        };

        public static ApplicationDataContainer GetSetting()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            //if no setting, set default value
            foreach (var item in DefaultSetting)
            {
                if (localSettings.Values[item.Key] == null)
                {
                    localSettings.Values[item.Key] = item.Value;
                }
            }

            return localSettings;
        }
    }
    */
    public static class OcrProcessor
    {
            public static IReadOnlyList<Language> GetOcrLangList()
            {
                return OcrEngine.AvailableRecognizerLanguages;
            }
        
            public static async Task<List<string>> GetText(SoftwareBitmap imageItem, Language SelectedLang)
            {
                //check item is null
                //check no selectedLang
                //check image is too large
                if (imageItem == null ||
                    SelectedLang == null ||
                    imageItem.PixelWidth > OcrEngine.MaxImageDimension ||
                    imageItem.PixelHeight > OcrEngine.MaxImageDimension
                    )
                {
                    return new List<string> { "" };
                }


                //check ocr image exist
                var ocrEngine = OcrEngine.TryCreateFromLanguage(SelectedLang);
                if (ocrEngine == null)
                {
                    return new List<string> { "" };
                }


                var ocrResult = await ocrEngine.RecognizeAsync(imageItem);


                List<string> textList = new List<string>() { };
                foreach (var line in ocrResult.Lines)
                {
                    textList.Add(line.Text);
                }
                return textList;
            }
        public async static Task<OcrResult> GetOcrResultFromBitmap(Bitmap scaledBitmap, Language selectedLanguage)
        {
            MemoryStream memory = new MemoryStream();

            scaledBitmap.Save(memory, ImageFormat.Bmp);
            memory.Position = 0;
            BitmapDecoder bmpDecoder = await BitmapDecoder.CreateAsync(memory.AsRandomAccessStream());
            SoftwareBitmap softwareBmp = await bmpDecoder.GetSoftwareBitmapAsync();

            await memory.FlushAsync();

            OcrEngine ocrEngine = OcrEngine.TryCreateFromLanguage(selectedLanguage);
            return await ocrEngine.RecognizeAsync(softwareBmp);
        }

    }
}
