using DirectShowLib;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Windows.Markup;

using TClass;
using Windows.UI.Xaml.Controls;
using SC_M4.Utilities;
//using Windows.UI.Xaml.Controls;

namespace SC_M4
{
    public partial class Main : Form
    {
        private  Language SelectedLang = null;
        public TControl cameraControl;
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
        protected IList<System.Drawing.Image> imageList;
        protected string inputfilename;
        protected int imageIndex;
        protected float scaleX = 1f;
        protected float scaleY = 1f;
        protected string selectedPSM = "Auto"; // 3 - Fully automatic page segmentation, but no OSD (default)
        protected string selectedOEM = "3"; // Default

        private bool isStaetReset;

        private bool isOCR1 = false;

        private BackgroundWorker background;
        private System.Windows.Forms.Timer timerOCR;
       
        private void Main_Load(object sender, EventArgs e)
        {
            foreach (ToolStripItem item in statusStripHome.Items)
            {
                item.Text = "";
            }

            this.Text = "SC-M4 v.4.52";
            background = new BackgroundWorker();
            background.WorkerReportsProgress = true;
            background.WorkerSupportsCancellation = true;

            background.DoWork += Background_DoWork;
            background.RunWorkerCompleted += Background_RunWorkerCompleted;
            background.ProgressChanged += Background_ProgressChanged;

            timerOCR = new System.Windows.Forms.Timer(components);
            timerOCR.Interval = 700;

            timerOCR.Tick += TimerOCR_Tick;

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
            capture_1.OnFrameHeader += Capture_1_OnFrameHeadler;
            capture_1.OnVideoStarted += Capture_1_OnVideoStarted;
            capture_1.OnVideoStop += Capture_1_OnVideoStop;
            capture_2 = new TCapture.Capture();
            capture_2.OnFrameHeader += Capture_2_OnFrameHeadler;
            capture_2.OnVideoStarted += Capture_2_OnVideoStarted;
            capture_2.OnVideoStop += Capture_2_OnVideoStop;
            this.ActiveControl = txtEmployee;
            txtEmployee.Focus();
            LogWriter = new LogFile();
            LogWriter.path = Properties.Resources.path_log;
            LogWriter.SaveLog("Start Program");
            btRefresh.PerformClick();

            imageList = new List<System.Drawing.Image>();

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

            cbQrCode.Checked = Properties.Settings.Default.useQrCode;
            useQrCode = cbQrCode.Checked;
        }
        private Task taskProcess;
        private void TimerOCR_Tick(object sender, EventArgs e)
        {
            onTest();
        }

        private async void onTest()
        {
            if (taskProcess != null && taskProcess.Status == TaskStatus.Running)
            {
                return;
            }
            taskProcess = Task.Run(() => processOCR());
            await taskProcess;
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


            if (cameraControl != null)
            {
                cameraControl = null;
            }
            cameraControl = new TClass.TControl(cbDriveCam02.SelectedIndex);
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

            pictureBoxCamera02.Image?.Dispose();
            pictureBoxCamera02.Image = (System.Drawing.Image)bitmap.Clone();
            bitmapCamaera_02?.Dispose();
            bitmapCamaera_02 = (Bitmap)pictureBoxCamera02.Image.Clone();
            if (rect_2 != Rectangle.Empty && isStaetReset)
            {

                bmp2?.Dispose();
                bmp2 = new Bitmap(rect_2.Width, rect_2.Height);
                using (Graphics g = Graphics.FromImage(bmp2))
                {
                    g.DrawImage(bitmapCamaera_02, 0, 0, rect_2, GraphicsUnit.Pixel);
                }
                scrollablePictureBoxCamera02.Image?.Dispose();
                scrollablePictureBoxCamera02.Image = bmp2;
                // Draw Rectangle to Image
                using (Graphics g = Graphics.FromImage(pictureBoxCamera02.Image))
                {
                    g.DrawRectangle(new Pen(Color.Red, 2), rect_2);
                }
            }
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
            if (InvokeRequired)
            {
                Invoke(new Action(()=>Capture_1_OnFrameHeadler(bitmap)));
                return;
            }
            pictureBoxCamera01.Image?.Dispose();
            pictureBoxCamera01.Image = (System.Drawing.Image)bitmap.Clone();
            
            bitmapCamaera_01?.Dispose();
            bitmapCamaera_01 = (Bitmap)pictureBoxCamera01.Image.Clone();
            if (rect_1 != Rectangle.Empty && isStaetReset)
            {
                bmp1?.Dispose();
                bmp1 = new Bitmap(rect_1.Width, rect_1.Height);
                using (Graphics g = Graphics.FromImage(bmp1))
                {
                    g.DrawImage(bitmapCamaera_01, 0, 0, rect_1, GraphicsUnit.Pixel);
                }
                scrollablePictureBoxCamera01.Image?.Dispose();
                scrollablePictureBoxCamera01.Image = bmp1;
                // Draw Rectangle to Image
                using (Graphics g = Graphics.FromImage(pictureBoxCamera01.Image))
                {
                    g.DrawRectangle(new Pen(Color.Red, 2), rect_1);
                }
            }
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
        private bool isStarted = false;
        private int driveindex_01 = -1;
        private int driveindex_02 = -1;
        private string serialportName = string.Empty;
        private string serialportBaud = string.Empty;
        private Thread thread;

        #region Start
        private List<ReplaceName> repleaceNames1 = new List<ReplaceName>();
        private List<ReplaceName> repleaceNames2 = new List<ReplaceName>();

        private Task taskCam1;
        private Task taskCam2;
        private async void btStartStop_Click(object sender, EventArgs e)
        {

            if(taskCam1 != null &&  taskCam1.Status == TaskStatus.Running)
            {
                Console.WriteLine("Task 1 is running");
                return;
            }

            if (taskCam2 != null && taskCam2.Status == TaskStatus.Running)
            {
                Console.WriteLine("Task 2 is running");
                return;
            }
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

                    repleaceNames1.Clear();
                    repleaceNames1 = Modules.ReplaceName.GetList(0);

                    repleaceNames2.Clear();
                    repleaceNames2 = Modules.ReplaceName.GetList(1);


                    driveindex_01 = cbDriveCam01.SelectedIndex;
                    driveindex_02 = cbDriveCam02.SelectedIndex;

                    lbTitle.Text = "Camera opening...";

                    btConnect.Text = "Opening...";
                    pictureBoxCamera01.Image?.Dispose();
                    pictureBoxCamera02.Image?.Dispose();
                    pictureBoxCamera01.Image = Properties.Resources.Spinner_0_4s_800px;
                    pictureBoxCamera02.Image = Properties.Resources.Spinner_0_4s_800px;

                    taskCam1 = Task.Run(()=> capture_1.Start(driveindex_01));

                    await taskCam1;

                    taskCam2 = Task.Run(() => capture_2.Start(driveindex_02));

                    await taskCam2;

                    btStartStop.Text = "STOP";

                    this.richTextBox1.Text = string.Empty;
                    this.richTextBox2.Text = string.Empty;

                    scrollablePictureBoxCamera01.Image = null;
                    scrollablePictureBoxCamera02.Image = null;

                    cameraControl.set(driveindex_02);
                    cameraControl.setFocus(Properties.Settings.Default.dFocus);
                    cameraControl.setZoom(Properties.Settings.Default.dZoom);
                    cameraControl.setPan(Properties.Settings.Default.dPan);
                    cameraControl.setTilt(Properties.Settings.Default.dTilt);
                    cameraControl.setExposure(Properties.Settings.Default.dExposure);

                    nFocus.Maximum = cameraControl.fmax;
                    nFocus.Minimum = cameraControl.fmin;
                    nFocus.Value = cameraControl.fValue;


                    btConnect.Text = "Disconnect";
                    if(background == null)
                    {
                        background = new BackgroundWorker();
                        background.WorkerReportsProgress = true;
                        background.WorkerSupportsCancellation = true;

                        background.DoWork += Background_DoWork;
                        background.RunWorkerCompleted += Background_RunWorkerCompleted;
                        background.ProgressChanged += Background_ProgressChanged;
                    }

                    timerOCR.Start();

                    checkBoxAutoFocus.Checked = false;
                    nFocus.Value = 68;

                    isStarted = true;
                    isStaetReset = true;
                }
                else
                {
                    if (capture_1._isRunning)
                        capture_1.Stop();

                    if (capture_2._isRunning)
                        capture_2.Stop();

                    if (serialPort.IsOpen)
                        serialPort.Close();

                    timerOCR.Stop();
                    if (background.IsBusy)
                    {
                        background.CancelAsync();
                        background.Dispose();
                    }

                    if(background != null)
                    {
                        background.Dispose();
                    }


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
                    pictureBoxCamera01.Image = null;
                    pictureBoxCamera02.Image = null;
                    //if (thread != null)
                    //{
                    //    thread.Abort(true);
                    //}
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
        private bool detection = false;
        private string result_1 = string.Empty;
        private string result_2 = string.Empty;

        private Stopwatch stopwatch = new Stopwatch();
        private OcrResult ocrResult1, ocrResult2;
        private bool useQrCode = false;
        private async void processOCR()
        {
            try
            {
                if (capture_1._isRunning && capture_2._isRunning && bitmapCamaera_01 != null && bitmapCamaera_02 != null && isStaetReset && scrollablePictureBoxCamera01.Image != null && scrollablePictureBoxCamera02.Image != null)
                {
                    if (isStarted)
                    {
                        capture_2.setFocus((int)nFocus.Value);
                        isStarted = false;
                    }
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
                    if (useQrCode)
                    {
                        result_1 = "";
                    }
                    else
                    {
                        imageList?.Clear();
                        imageList.Add((System.Drawing.Image)scrollablePictureBoxCamera01.Image.Clone());
                        result_1 = await performOCR(imageList, inputfilename, imageIndex, Rectangle.Empty);
                    }
                    var a = result_1.IndexOf("-731");
                    result_1 = result_1.Substring(a + 1);
                    a = result_1.IndexOf("|731");
                    result_1 = result_1.Substring(a + 1);
                    result_1 = result_1.Replace("T31TM", "731TM");
                    result_1 = result_1.Replace("731THC", "731TMC");
                    result_1 = result_1.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace("\\", "").Replace("|", "").Replace(@"\", "");
                    result_1 = result_1.Replace("7731TMC", "731TMC");
                    result_1 = result_1.Replace("731TMCO", "731TMC6").Replace("-I", "-1");
                    result_1 = result_1.Replace("-S-", "-5-");
                    result_1 = result_1.Replace("G.22", "G:22");

                    foreach (var item in repleaceNames1)
                    {
                        result_1 = result_1.Replace(item.oldName, item.newName);
                    }

                    if (isOCR1 && result_1 == string.Empty)
                    {
                        result_1 = Properties.Settings.Default.keyCAM1;
                        isOCR1 = false;
                    }

                    richTextBox1.Invoke(new Action(() =>
                    {
                        this.richTextBox1.Text = string.Empty;
                        this.richTextBox1.Text = result_1.Trim();

                    }));
                    // Image 02
                    int lb = result_1.IndexOf(Properties.Settings.Default.keyCAM1);
                    if (result_1 != string.Empty && lb != -1)
                    {
                        // OCR 2
                        result_2 = string.Empty;
                        ocrResult2 = null;
                        ocrResult2 = await GetOcrResultBitmap((Bitmap)scrollablePictureBoxCamera02.Image.Clone(), SelectedLang);
                        //var ocr = await _OCRScan.GetOcrResultFromBitmap((Bitmap)scrollablePictureBoxCamera02.Image.Clone(), SelectedLang);
                        result_2 = ocrResult2.Text;
                        result_2 = result_2.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
                        result_2 = Regex.Replace(result_2, "[^a-zA-Z,0-9,(),:,-]", "");

                        result_2 = result_2.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace("'", "").Replace("|", "").Replace(@"\", "");
                        result_2 = result_2.Replace("91J7", "9U7");
                        result_2 = result_2.Replace("-OO", "-00");
                        result_2 = result_2.Replace(")9U7", "9U7").Replace("\n", "");
                        result_2 = result_2.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
                        result_2 = ReplaceName(result_2);
                        foreach (var item in repleaceNames2)
                        {
                            result_2 = result_2.Replace(item.oldName, item.newName);
                        }
                        richTextBox2.Invoke(new Action(() =>
                        {
                            this.richTextBox2.Text = string.Empty;
                            this.richTextBox2.Text = result_2.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
                        }));
                        int result = Compare_Master(result_1, result_2);
                        if (result == 1 || result == 2)
                        {
                            isStaetReset = false;
                        }
                    }
                    //await Task.Delay(100);
                    stopwatch.Stop();
                    Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
                    Invoke(new Action(() =>
                    {
                        toolStripStatusTime.Text = "Load " + stopwatch.ElapsedMilliseconds.ToString() + "ms";
                        loadTableHistory();
                        toolStripStatusLabelError.ForeColor = Color.Green;
                    }));
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        toolStripStatusLabelError.Text = ex.Message;
                        toolStripStatusLabelError.ForeColor = Color.Red;
                    }));
                }
                else
                {
                    toolStripStatusLabelError.Text = ex.Message;
                    toolStripStatusLabelError.ForeColor = Color.Red;
                }
            }
        }

        private void Background_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            onTest();
            /*
            if (capture_1._isRunning && capture_2._isRunning && bitmapCamaera_01 != null && bitmapCamaera_02 != null && isStaetReset && scrollablePictureBoxCamera01.Image != null && scrollablePictureBoxCamera02.Image != null)
            {
                // Console.WriteLine("1");
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                }
                if (isStarted)
                {
                    capture_2.setFocus((int)numericUpDownFocus.Value);
                    isStarted = false;
                }
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
                // Console.WriteLine("2");
                var ocr_1 = await OcrProcessor.GetOcrResultFromBitmap(Sharpen((Bitmap)scrollablePictureBoxCamera01.Image), SelectedLang);
                //var ocr_1 = await _OCRScan.GetOcrResultFromBitmap(Sharpen((Bitmap)scrollablePictureBoxCamera01.Image), SelectedLang);
                result_1 = ocr_1.Text; // performOCR(imageList, inputfilename, imageIndex, Rectangle.Empty);
                var a = result_1.IndexOf("-731");
                result_1 = result_1.Substring(a + 1);
                a = result_1.IndexOf("|731");
                result_1 = result_1.Substring(a + 1);
                result_1 = result_1.Replace("T31TM", "731TM");
                result_1 = result_1.Replace("731THC", "731TMC");
                result_1 = result_1.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace("\\", "").Replace("|", "").Replace(@"\", "");
                result_1 = result_1.Replace("7731TMC", "731TMC");
                result_1 = result_1.Replace("731TMCO", "731TMC6").Replace("-I", "-1");
                result_1 = result_1.Replace("-S-", "-5-");
                result_1 = result_1.Replace("G.22", "G:22");

                foreach(var item in repleaceNames1)
                {
                    result_1 = result_1.Replace(item.oldName, item.newName);
                }

                if (isOCR1 && result_1 == string.Empty)
                {
                    result_1 = "731TMC";
                    isOCR1 = false;
                }

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
                    var ocr = await OcrProcessor.GetOcrResultFromBitmap((Bitmap)scrollablePictureBoxCamera02.Image.Clone(), SelectedLang);
                    //var ocr = await _OCRScan.GetOcrResultFromBitmap((Bitmap)scrollablePictureBoxCamera02.Image.Clone(), SelectedLang);
                    result_2 = ocr.Text;
                    result_2 = result_2.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
                    result_2 = Regex.Replace(result_2, "[^a-zA-Z,0-9,(),:,-]", "");

                    result_2 = result_2.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace("'", "").Replace("|", "").Replace(@"\", "");
                    result_2 = result_2.Replace("91J7", "9U7");
                    result_2 = result_2.Replace("-OO", "-00");
                    result_2 = result_2.Replace(")9U7", "9U7").Replace("\n", "");
                    result_2 = result_2.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
                    result_2 = ReplaceName(result_2);
                    foreach (var item in repleaceNames2)
                    {
                        result_2 = result_2.Replace(item.oldName, item.newName);
                    }
                    richTextBox2.Invoke(new Action(() =>
                    {
                        this.richTextBox2.Text = string.Empty;
                        this.richTextBox2.Text = result_2.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
                    }));
                    int result = Compare_Master(result_1, result_2);
                    if (result == 1 || result == 2)
                    {
                        isStaetReset = false;
                    }
                }
                // Console.WriteLine("5");

                await Task.Delay(100);
                stopwatch.Stop();
                Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
                Invoke(new Action(() =>
                {
                    toolStripStatusTime.Text = "Load " + stopwatch.ElapsedMilliseconds.ToString() + "ms";
                    loadTableHistory();
                }));
            }
            */
        }

        private void Background_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void Background_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }



        private string ReplaceName(string input)
        {
            input = input.Replace("731OTM", "7310TM");

            input = input.Replace("3O1731", "3-01731");

            input = input.Replace("3-O1731", "3-01731");

            input = input.Replace("4O1731", "4-01731");
            input = input.Replace("4-O1731", "4-01731");

            input = input.Replace("5O1731", "5-01731");
            input = input.Replace("5-O1731", "5-01731");

            input = input.Replace("6O3731", "6-03731");
            input = input.Replace("6-O3731", "6-03731");

            input = input.Replace("2O1731", "2-01731");
            input = input.Replace("2-O1731", "2-01731");

            input = input.Replace("7O1731", "7-01731");
            input = input.Replace("7-O1731", "7-01731");

            input = input.Replace("5O2731", "5-02731");
            input = input.Replace("5-O2731", "5-02731");

            input = input.Replace("4O4731", "4-04731");
            input = input.Replace("4-O4731", "4-04731");

            input = input.Replace("8OO731", "8-00731");
            input = input.Replace("8-OO731", "8-00731");

            input = input.Replace("8OO731", "8-00731");
            input = input.Replace("8-OO731", "8-00731");

            input = input.Replace("8OA731", "8-0A731");
            input = input.Replace("8-OA731", "8-0A731");

            input = input.Replace("9OA731", "9-0A731");
            input = input.Replace("9-OA731", "9-0A731");

            input = input.Replace("6O1731", "6-01731");
            input = input.Replace("6-O1731", "6-01731");

            input = input.Replace("7OA731", "7-0A731");
            input = input.Replace("7-OA731", "7-0A731");
            
            input = input.Replace("7OO731", "7-00731");
            input = input.Replace("7-OO731", "7-00731");

            input = input.Replace("2OA731", "2-0A731");
            input = input.Replace("2-OA731", "2-0A731");
            return input;
        }
        History history;
        private bool is_Blink_NG = false;
        private int Compare_Master(string txt_sw, string txt_lb)
        {
            // 0 = not fount, 1 = OK, 2 = NG
            int result = 0;

            LogWriter.SaveLog("TXT Read :" + txt_sw.Replace("\r","").Replace("\n", "") + ", " + txt_lb.Replace("\r", "").Replace("\n", ""));
            //lbTitle.Text;
            if(history != null)
            {
                history = null;
            }
            history = new History();


            int swa = txt_sw.IndexOf(Properties.Settings.Default.keyCAM1);
            // If not found, IndexOf returns -1.
            if (swa == -1)
            {
                result = 0;
                return result;
            }

            int lb = txt_lb.IndexOf(Properties.Settings.Default.keyCAM2);
            // If not found, IndexOf returns -1.
            if (lb == -1)
            {
                // Return the original string.
                result = 0;
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
                throw new Exception("Please select serial port and baud rate");
            
            this.serialConnect(this.serialportName, int.Parse(this.serialportBaud));
        }

        public void serialCommand(string command)
        {
            if (this.serialPort.IsOpen)
            {
                this.serialPort.Write(">" + command + "<#");
                LogWriter.SaveLog("Serial send : " + command);
                toolStripStatusSerialData.Text = "DATA :" + command;
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
        }

        private async Task<string> performOCR(IList<System.Drawing.Image> imageList, string inputfilename, int index, Rectangle rect)
        {
            return await Task.Run(() => {
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
                    OCR<System.Drawing.Image> ocrEngine = new OCRImages();
                    ocrEngine.PageSegMode = selectedPSM;
                    ocrEngine.OcrEngineMode = selectedOEM;
                    ocrEngine.Language = entity.Language;

                    IList<System.Drawing.Image> images = entity.ClonedImages;

                    string result = ocrEngine.RecognizeText(((List<System.Drawing.Image>)images).GetRange(0, 1), entity.Inputfilename);
                    return result;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, "Exclamation A00", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    LogWriter.SaveLog("A00 : " + ex.Message);
                }
                return "";
            });
        }

        private void testOCRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isOCR1 = true;
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
            else if (lbTitle.BackColor != Color.Yellow && isStaetReset && lbTitle.Text != "OK" && lbTitle.Text != "NG")
            {
                lbTitle.BackColor = Color.Yellow;
                lbTitle.ForeColor = Color.Black;
            }
        }

        private void numericUpDownFocus_ValueChanged(object sender, EventArgs e)
        {
            if (!checkBoxAutoFocus.Checked)
            {
                cameraControl.setFocus((int)nFocus.Value);
            }
            else
            {
                //capture_2.AutoFocus();
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
        /// <summary>
        /// Sharpens an image.
        /// http://stackoverflow.com/questions/903632/sharpen-on-a-bitmap-using-c-sharp
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        Bitmap sharpenImage;

        public Bitmap Sharpen(Bitmap image)
        {
            sharpenImage?.Dispose();
            sharpenImage = (Bitmap)image.Clone();

            int filterWidth = 3;
            int filterHeight = 3;
            int width = image.Width;
            int height = image.Height;

            // Create sharpening filter.
            double[,] filter = new double[filterWidth, filterHeight];
            filter[0, 0] = filter[0, 1] = filter[0, 2] = filter[1, 0] = filter[1, 2] = filter[2, 0] = filter[2, 1] = filter[2, 2] = -1;
            filter[1, 1] = 9;

            double factor = 1.0;
            double bias = 0.0;

            Color[,] result = new Color[image.Width, image.Height];

            // Lock image bits for read/write.
            BitmapData pbits = sharpenImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Declare an array to hold the bytes of the bitmap.
            int bytes = pbits.Stride * height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(pbits.Scan0, rgbValues, 0, bytes);

            int rgb;
            // Fill the color array with the new sharpened color values.
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    double red = 0.0, green = 0.0, blue = 0.0;

                    for (int filterX = 0; filterX < filterWidth; filterX++)
                    {
                        for (int filterY = 0; filterY < filterHeight; filterY++)
                        {
                            int imageX = (x - filterWidth / 2 + filterX + width) % width;
                            int imageY = (y - filterHeight / 2 + filterY + height) % height;

                            rgb = imageY * pbits.Stride + 3 * imageX;

                            red += rgbValues[rgb + 2] * filter[filterX, filterY];
                            green += rgbValues[rgb + 1] * filter[filterX, filterY];
                            blue += rgbValues[rgb + 0] * filter[filterX, filterY];
                        }
                        int r = Math.Min(Math.Max((int)(factor * red + bias), 0), 255);
                        int g = Math.Min(Math.Max((int)(factor * green + bias), 0), 255);
                        int b = Math.Min(Math.Max((int)(factor * blue + bias), 0), 255);

                        result[x, y] = Color.FromArgb(r, g, b);
                    }
                }
            }

            // Update the image with the sharpened pixels.
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    rgb = y * pbits.Stride + 3 * x;

                    rgbValues[rgb + 2] = result[x, y].R;
                    rgbValues[rgb + 1] = result[x, y].G;
                    rgbValues[rgb + 0] = result[x, y].B;
                }
            }

            // Copy the RGB values back to the bitmap.
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, pbits.Scan0, bytes);
            // Release image bits.
            sharpenImage.UnlockBits(pbits);
            return sharpenImage;
        }

        public async Task<Bitmap> SharpenAsync(Bitmap image)
        {
            return await Task.Run(() => { 
                sharpenImage?.Dispose();
                sharpenImage = (Bitmap)image.Clone();

                int filterWidth = 3;
                int filterHeight = 3;
                int width = image.Width;
                int height = image.Height;

                // Create sharpening filter.
                double[,] filter = new double[filterWidth, filterHeight];
                filter[0, 0] = filter[0, 1] = filter[0, 2] = filter[1, 0] = filter[1, 2] = filter[2, 0] = filter[2, 1] = filter[2, 2] = -1;
                filter[1, 1] = 9;

                double factor = 1.0;
                double bias = 0.0;

                Color[,] result = new Color[image.Width, image.Height];

                // Lock image bits for read/write.
                BitmapData pbits = sharpenImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                // Declare an array to hold the bytes of the bitmap.
                int bytes = pbits.Stride * height;
                byte[] rgbValues = new byte[bytes];

                // Copy the RGB values into the array.
                System.Runtime.InteropServices.Marshal.Copy(pbits.Scan0, rgbValues, 0, bytes);

                int rgb;
                // Fill the color array with the new sharpened color values.
                for (int x = 0; x < width; ++x)
                {
                    for (int y = 0; y < height; ++y)
                    {
                        double red = 0.0, green = 0.0, blue = 0.0;

                        for (int filterX = 0; filterX < filterWidth; filterX++)
                        {
                            for (int filterY = 0; filterY < filterHeight; filterY++)
                            {
                                int imageX = (x - filterWidth / 2 + filterX + width) % width;
                                int imageY = (y - filterHeight / 2 + filterY + height) % height;

                                rgb = imageY * pbits.Stride + 3 * imageX;

                                red += rgbValues[rgb + 2] * filter[filterX, filterY];
                                green += rgbValues[rgb + 1] * filter[filterX, filterY];
                                blue += rgbValues[rgb + 0] * filter[filterX, filterY];
                            }
                            int r = Math.Min(Math.Max((int)(factor * red + bias), 0), 255);
                            int g = Math.Min(Math.Max((int)(factor * green + bias), 0), 255);
                            int b = Math.Min(Math.Max((int)(factor * blue + bias), 0), 255);

                            result[x, y] = Color.FromArgb(r, g, b);
                        }
                    }
                }

                // Update the image with the sharpened pixels.
                for (int x = 0; x < width; ++x)
                {
                    for (int y = 0; y < height; ++y)
                    {
                        rgb = y * pbits.Stride + 3 * x;

                        rgbValues[rgb + 2] = result[x, y].R;
                        rgbValues[rgb + 1] = result[x, y].G;
                        rgbValues[rgb + 0] = result[x, y].B;
                    }
                }

                // Copy the RGB values back to the bitmap.
                System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, pbits.Scan0, bytes);
                // Release image bits.
                sharpenImage.UnlockBits(pbits);
                return sharpenImage;
            });
        }
        /// <summary>
        /// Clones a bitmap using DrawImage.
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap CloneImage(Bitmap bmp)
        {
            Bitmap bmpNew = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format24bppRgb);
            bmpNew.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);
            using (Graphics g = Graphics.FromImage(bmpNew))
            {
                g.DrawImage(bmp, 0, 0);
            }

            return bmpNew;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (capture_1 != null)
            {
                if (capture_1.IsOpened)
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

            timerOCR.Stop();

            if(background.IsBusy)
                background.Dispose();

        }
        NameList nameList;
        private void changeNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(nameList != null)
            {
                nameList.Close();
                nameList.Dispose();
                nameList = null;
            }

            nameList = new NameList();
            nameList.Show();
        }
        private CameraControls cameraControlForm;
        private void cameraControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cameraControlForm != null)
            {
                cameraControlForm.Close();
            }

            cameraControlForm = new CameraControls(this);
            cameraControlForm.Show();
        }

        private Forms.Key key;
        private void keyCAM1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(key != null)
            {
                key.Close();
            }

            key = new Key(KeyForms.CAM1);
            key.Show();
        }

        private void keyCAM2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (key != null)
            {
                key.Close();
            }

            key = new Key(KeyForms.CAM2);
            key.Show();
        }

        private void cbQrCode_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.useQrCode = cbQrCode.Checked;
                Properties.Settings.Default.Save();
                useQrCode = cbQrCode.Checked;

            }
            catch(Exception ex)
            {
                LogWriter.SaveLog("cb Qr save :" + ex.Message);
            }
        }

        public Task<OcrResult> GetOcrResultBitmap(Bitmap scaledBitmap, Language selectedLanguage)
        {
            var tcs = new TaskCompletionSource<OcrResult>();

            Thread staThread = new Thread(() =>
            {
                try
                {
                    using (MemoryStream memory = new MemoryStream())
                    {
                        scaledBitmap.Save(memory, ImageFormat.Bmp);
                        memory.Position = 0;

                        BitmapDecoder bmpDecoder = BitmapDecoder.CreateAsync(memory.AsRandomAccessStream()).AsTask().Result;
                        SoftwareBitmap softwareBmp = bmpDecoder.GetSoftwareBitmapAsync().AsTask().Result;

                        OcrEngine ocrEngine = OcrEngine.TryCreateFromLanguage(selectedLanguage);

                        // Run the RecognizeAsync call in a separate thread to allow message pumping
                        OcrResult result = ocrEngine.RecognizeAsync(softwareBmp).AsTask().Result;

                        softwareBmp.Dispose();
                        ocrEngine = null;
                        tcs.SetResult(result);
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();

            return tcs.Task;
        }


    }

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
            using (MemoryStream memory = new MemoryStream())
            {
                scaledBitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;

                BitmapDecoder bmpDecoder = await BitmapDecoder.CreateAsync(memory.AsRandomAccessStream());
                SoftwareBitmap softwareBmp = await bmpDecoder.GetSoftwareBitmapAsync();

                OcrEngine ocrEngine = OcrEngine.TryCreateFromLanguage(selectedLanguage);

                // Run the RecognizeAsync call in a separate thread to allow message pumping
                OcrResult result = await Task.Run(async () => await ocrEngine.RecognizeAsync(softwareBmp));
                scaledBitmap.Dispose();
                softwareBmp.Dispose();
                ocrEngine = null;
                return result;
            }
        }


    }

}
