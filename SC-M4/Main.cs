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
using System.Xml.Linq;
using SC_M4.Forms.Analyze;

using Microsoft.VisualBasic.FileIO;
using System.Web;

//using Windows.UI.Xaml.Controls;

namespace SC_M4
{
    public partial class Main : Form
    {
        private Language SelectedLang = null;
        public TControl cameraControl;

        public SelectionType SelectionType = SelectionType.Auto;
        public Main()
        {
            InitializeComponent();
            InitializeSerial();
        }

        public TCapture.Capture capture_1;
        public TCapture.Capture capture_2;

        public Bitmap bitmapCamera_01;
        public Bitmap bitmapCamera_02;

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

        private bool isStateReset;

        private bool isOCR1 = false;

        private System.Windows.Forms.Timer timerOCR;
        private ColorName _colorName;
        private void Main_Load(object sender, EventArgs e)
        {
            _colorName = new ColorName();

            foreach (ToolStripItem item in statusStripHome.Items)
            {
                item.Text = "";
            }

            this.Text = "SC-M4 v.5.00";

            timerOCR = new System.Windows.Forms.Timer(components);
            timerOCR.Interval = 700;
            timerOCR.Tick += TimerOCR_Tick;

            SelectedLang = new Language("en-US");
            // Create Folder
            CreateDirectoryIfNotExists(Properties.Resources.path_temp);
            CreateDirectoryIfNotExists(Properties.Resources.path_log);
            CreateDirectoryIfNotExists(Properties.Resources.path_images);

            LogWriter = new LogFile();
            LogWriter.path = Properties.Resources.path_log;

            Task.Run(() =>
            {
                //Create Database
                Modules.Models.CreateTable();
                Modules.Actions.CreateTable();
                Modules.ActionIO.CreateTable();
                Modules.Items.CreateTable();
                Modules.Rect.CreateTable();

                try
                {
                    // Enable Progress Bar
                    VisibleProgressBar(true);

                    var s = Setting.GetSettingRemove();
                    int i = 0;
                    if (s.Count > 0)
                    {
                        foreach (var set in s)
                        {
                            if (File.Exists(set.path_image))
                            {
                                File.Delete(set.path_image);
                                set.Delete();
                            }
                            i++;
                            // Update progress bar
                            SetProcessBar(i * 100 / s.Count);
                        }
                    }
                    // Move file log to Recycle Bin
                    string[] files = Directory.GetFiles(Properties.Resources.path_log);
                    // Reset i
                    i = 0;
                    foreach (string file in files)
                    {
                        FileInfo info = new FileInfo(file);
                        if (info.LastAccessTime < DateTime.Now.AddDays(-5)){
                            // Move file to Recycle Bin
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(file, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                        }

                        i++;
                        // Update progress bar
                        SetProcessBar(i * 100 / files.Length);

                    }

                    // Visible progress bar
                    deletedFileTemp();
                    VisibleProgressBar(false);
                }
                catch (Exception ex)
                {
                    LogWriter.SaveLog("Error delete file : " + ex.Message);
                    VisibleProgressBar(false);

                }

            });

            // Create Video Capture Object
            capture_1 = new TCapture.Capture();
            capture_1.OnFrameHeader += Capture_1_OnFrameHeader;
            capture_1.OnVideoStarted += Capture_1_OnVideoStarted;
            capture_1.OnVideoStop += Capture_1_OnVideoStop;
            capture_2 = new TCapture.Capture();
            capture_2.OnFrameHeader += Capture_2_OnFrameHeader;
            capture_2.OnVideoStarted += Capture_2_OnVideoStarted;
            capture_2.OnVideoStop += Capture_2_OnVideoStop;

            this.ActiveControl = txtEmployee;
            txtEmployee.Focus();

            LogWriter.SaveLog("Start Program");
            btRefresh.PerformClick();

            imageList = new List<System.Drawing.Image>();

            loadRect(0);
            loadRect(1);

            timerMain.Start();
      
            loadTableHistory();

            cbQrCode.Checked = Properties.Settings.Default.useQrCode;
            useQrCode = cbQrCode.Checked;
            checkBoxAutoFocus.Checked = Properties.Settings.Default.isAutoFocus;
        }

        private void VisibleProgressBar(bool input){
            
            // Check invoke required
            if (InvokeRequired)
            {
                Invoke(new Action(() => { VisibleProgressBar(input); }));
                return;
            }
            toolStripProgressBar.Visible = input;
            toolStripProgressBar.Value = 0;
        }

       private void SetProcessBar(int value)
        {
            // Check invoke required
            if (InvokeRequired)
            {
                Invoke(new Action(() => { SetProcessBar(value); }));
                return;
            }
            toolStripProgressBar.Value = value;
        }

        public static void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private void deletedFileTemp()
        {
            try
            {
                // Enable Progress Bar
                VisibleProgressBar(true);
                string _dir = Properties.Resources.path_temp;
                string[] files = Directory.GetFiles(_dir);
                int i = 0;
                foreach (string file in files)
                {
                    FileInfo info = new FileInfo(file);
                    if (info.LastAccessTime < DateTime.Now.AddHours(-24)){
                        // Move file to Recycle Bin
                        Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(file, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                    }
                    i++;
                    // Update progress bar
                    SetProcessBar(i * 100 / files.Length);
                }

                // Visible progress bar
                VisibleProgressBar(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void loadTableHistory()
        {

            if (InvokeRequired)
            {
                Invoke(new Action(() => { loadTableHistory(); }));
                return;
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("No", typeof(int));
            dt.Columns.Add("Employee", typeof(string));
            dt.Columns.Add("MasterSW", typeof(string));
            dt.Columns.Add("Software", typeof(string));
            dt.Columns.Add("Master_Model", typeof(string));
            dt.Columns.Add("Models", typeof(string));
            dt.Columns.Add("Color", typeof(string));
            dt.Columns.Add("Results", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Update", typeof(string));

            var history = History.GetHistory();
            int i = 1;
            foreach (var h in history)
            {
                dt.Rows.Add(h.id, i, h.name, h.master_sw, h.name_sw, h.master_lb, h.name_lb, h.color, h.result, h.description, h.updated_at);
                i++;
            }

            dataGridViewHistory.DataSource = dt;

            dataGridViewHistory.Columns["ID"].Visible = false;
            // 10% of the width of the DataGridView
            dataGridViewHistory.Columns["No"].Width = dataGridViewHistory.Width * 10 / 100;
            // last 20% of the width of the DataGridView
            dataGridViewHistory.Columns["Update"].Width = dataGridViewHistory.Width * 20 / 100;
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            RefreshVideoDevices();
            RefreshComboBoxWithList(comboBoxBaud, this.baudList);
            RefreshComboBoxWithList(comboBoxCOMPort, SerialPort.GetPortNames());
        }
        private void RefreshVideoDevices()
        {
            var videoDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).Select(device => device.Name).ToList();

            RefreshComboBoxWithList(cbDriveCam01, videoDevices);
            RefreshComboBoxWithList(cbDriveCam02, videoDevices);
        }
        private void RefreshComboBoxWithList(System.Windows.Forms.ComboBox comboBox, IList<string> items, bool selectLast = false)
        {
            int oldSelectedIndex = comboBox.SelectedIndex;

            comboBox.Items.Clear();
            comboBox.Items.AddRange(items.ToArray());

            if (comboBox.Items.Count <= 0) return;

            if (oldSelectedIndex > 0 && oldSelectedIndex < comboBox.Items.Count)
            {
                comboBox.SelectedIndex = oldSelectedIndex;
            }
            else
            {
                comboBox.SelectedIndex = selectLast ? comboBox.Items.Count - 1 : 0;
            }
        }


        public void saveRect(Rectangle rect, int _type)
        {
            if (_type == 0)
            {
                rect_1 = rect;
                Properties.Settings.Default.rect1_x = rect.X;
                Properties.Settings.Default.rect1_y = rect.Y;
                Properties.Settings.Default.rect1_w = rect.Width;
                Properties.Settings.Default.rect1_h = rect.Height;
            }
            else if (_type == 1)
            {
                rect_2 = rect;
                Properties.Settings.Default.rect2_x = rect.X;
                Properties.Settings.Default.rect2_y = rect.Y;
                Properties.Settings.Default.rect2_w = rect.Width;
                Properties.Settings.Default.rect2_h = rect.Height;
            }
            else if (_type == 2)
            {
                Properties.Settings.Default.color_x = rect.X;
                Properties.Settings.Default.color_y = rect.Y;
                Properties.Settings.Default.color_width = rect.Width;
                Properties.Settings.Default.color_high = rect.Height;
            }
            Properties.Settings.Default.Save();
        }

        public void loadRect(int _type)
        {
            if (_type == 0 && Properties.Settings.Default.rect1_x != 0)
            {
                rect_1 = new Rectangle(Properties.Settings.Default.rect1_x, Properties.Settings.Default.rect1_y, Properties.Settings.Default.rect1_w, Properties.Settings.Default.rect1_h);
                toolStripStatusRect1.Text = "Rect 1 : X=" + rect_1.X.ToString() + ", Y=" + rect_1.Y.ToString() + ", H=" + rect_1.Height.ToString() + ", W=" + rect_1.Width.ToString();
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

        private Thread thread;
        #region SATART
        private Task taskCam1;
        private Task taskCam2;
        private void btStartStop_Click(object sender, EventArgs e)
        {
            StartStopCamera();
        }
        private async void StartStopCamera()
        {
            if (IsTaskRunning(taskCam1) || IsTaskRunning(taskCam2))
                return;


            try
            {
                this.isStart = !this.isStart;
                if (this.isStart)
                {
                    ValidateInputs();
                    InitializeComponents();
                    await StartCameras();
                    await Task.Delay(1000);
                    PostStartActions();
                }
                else
                {
                    StopComponents();
                    ResetUIAfterStop();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ResetUIAfterStop()
        {

        }

        private void PostStartActions()
        {
            btStartStop.Text = "STOP";

            this.richTextBox1.Text = string.Empty;
            this.richTextBox2.Text = string.Empty;

            scrollablePictureBoxCamera01.Image?.Dispose();
            scrollablePictureBoxCamera02.Image?.Dispose();
            scrollablePictureBoxCamera01.Image = null;
            scrollablePictureBoxCamera02.Image = null;

            if (cameraControl != null)
            {
                cameraControl = null;
            }
            //cameraControl = new TClass.TControl(cbDriveCam02.SelectedIndex);

            //if (!checkBoxAutoFocus.Checked)
            //{
            //    cameraControl.set(driveindex_02);
            //    cameraControl.setFocus(Properties.Settings.Default.dFocus);
            //    cameraControl.setZoom(Properties.Settings.Default.dZoom);
            //    cameraControl.setPan(Properties.Settings.Default.dPan);
            //    cameraControl.setTilt(Properties.Settings.Default.dTilt);
            //    cameraControl.setExposure(Properties.Settings.Default.dExposure);

            //    nFocus.Value = cameraControl.fValue;
            //    nFocus.Value = 68 > cameraControl.fmax ? cameraControl.fmax : 68;
            //    nFocus.Maximum = cameraControl.fmax;
            //    nFocus.Minimum = cameraControl.fmin;
            //}

            btConnect.Text = "Disconnect";

            //timerOCR.Start();

            stopwatchManualTest.Restart();

            isStarted = true;
            isStateReset = true;
        }

        private bool IsTaskRunning(Task task)
        {
            if (task != null && task.Status == TaskStatus.Running)
            {
                Console.WriteLine($"Task {task.Id} is running");
                return true;
            }
            return false;
        }

        private void InitializeComponents()
        {
            _colorName = Properties.Settings.Default.isUseColorMaster ? new ColorName(MasterNTC.GetMasterNTC()) : new ColorName();
            replaceNames1?.Clear();
            replaceNames1 = Modules.ReplaceName.GetList(0);

            replaceNames2?.Clear();
            replaceNames2 = Modules.ReplaceName.GetList(1);

            btConnect.Text = "Opening...";
            pictureBoxCamera01.Image?.Dispose();
            pictureBoxCamera02.Image?.Dispose();
            pictureBoxCamera01.Image = Properties.Resources.Spinner_0_4s_800px;
            pictureBoxCamera02.Image = Properties.Resources.Spinner_0_4s_800px;

            // ... Other initialization logic
        }


        private void ValidateInputs()
        {
            if (string.IsNullOrEmpty(txtEmployee.Text))
            {
                SetError("Please input employee ID", txtEmployee);
                throw new ArgumentException("Please input employee ID");
            }

            if (cbDriveCam01.SelectedIndex == cbDriveCam02.SelectedIndex)
            {
                lbTitle.Text = "Please select camera drive!";
                throw new ArgumentException("Please select camera drive!");
            }

            if (cbDriveCam01.SelectedIndex == -1 || cbDriveCam02.SelectedIndex == -1)
            {
                throw new ArgumentException("Select a camera");
            }
        }

        private void SetError(string v, System.Windows.Forms.TextBox TextBox)
        {

            this.ActiveControl = TextBox;
            TextBox.Focus();
            lbTitle.Text = v; //
        }

        private async Task StartCameras()
        {
            if (capture_1.IsOpened) capture_1.Stop();
            if (capture_2.IsOpened) capture_2.Stop();

            driveindex_01 = cbDriveCam01.SelectedIndex;
            driveindex_02 = cbDriveCam02.SelectedIndex;
            lbTitle.Text = "Camera opening...";

            taskCam1 = Task.Run(() => capture_1.Start(driveindex_01));
            await taskCam1;

            taskCam2 = Task.Run(() => capture_2.Start(driveindex_02));
            await taskCam2;
            SerialConnect(comboBoxCOMPort.Text, int.Parse(comboBoxBaud.Text));

        }

        private void HandleException(Exception ex)
        {
            LogWriter.SaveLog("Error Start :" + ex.Message);
            MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            this.isStart = false;
            btStartStop.Text = "START";
            lbTitle.Text = "Camera close.";
            StopComponents();
        }

        private void StopComponents()
        {

            if (capture_1._isRunning)
                capture_1.Stop();

            if (capture_2._isRunning)
                capture_2.Stop();

            CloseSerialPortIfExists();

            //timerOCR.Stop();

            btStartStop.Text = "START";
            btConnect.Text = "Connect";
            pictureBoxCamera01.Image = null;
            pictureBoxCamera02.Image = null;

            this.richTextBox1.Text = string.Empty;
            this.richTextBox2.Text = string.Empty;


            scrollablePictureBoxCamera01.Image?.Dispose();
            scrollablePictureBoxCamera02.Image?.Dispose();

            scrollablePictureBoxCamera01.Image = null;
            scrollablePictureBoxCamera02.Image = null;

            lbTitle.Text = "Camera close.";
            lbTitle.ForeColor = Color.Black;
            lbTitle.BackColor = Color.Yellow;

            is_Blink_NG = false;

            pictureBoxCamera01.Image?.Dispose();
            pictureBoxCamera02.Image?.Dispose();

            pictureBoxCamera01.Image = null;
            pictureBoxCamera02.Image = null;

            stopwatchManualTest.Stop();
        }


        private History history;
        private bool is_Blink_NG = false;

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

            select_XY?.Close();
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

            select_XY?.Close();
            select_XY = new Select_X_Y(this, 1);
            select_XY.Show();
        }

        #endregion

        private void btConnect_Click(object sender, EventArgs e)
        {
            //btStartStop.PerformClick();
            StartStopCamera();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private async Task<string> performOCR(IList<System.Drawing.Image> imageList, string inputfilename, int index, Rectangle rect)
        {
            return await Task.Run(() =>
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
        private bool IsChangeSelectedMode = false;
        private void timerMain_Tick(object sender, EventArgs e)
        {
            if (!IsChangeSelectedMode)
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
                else if (lbTitle.BackColor != Color.Yellow && isStateReset && lbTitle.Text != "OK" && lbTitle.Text != "NG")
                {
                    lbTitle.BackColor = Color.Yellow;
                    lbTitle.ForeColor = Color.Black;
                }
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
                capture_2.AutoFocus();
            }
        }
        SettingModel setting;
        private void masterListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setting?.Close();
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
            return await Task.Run(() =>
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

        }
        NameList nameList;
        private void changeNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nameList?.Close();
            nameList = new NameList();
            nameList.Show();
        }
        private CameraControls cameraControlForm;
        private void cameraControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cameraControlForm?.Close();
            cameraControlForm = new CameraControls(this);
            cameraControlForm.Show();
        }

        private Forms.Key key;
        private void keyCAM1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            key?.Close();
            key = new Key(KeyForms.CAM1);
            key.Show();
        }

        private void keyCAM2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            key?.Close();
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
            catch (Exception ex)
            {
                LogWriter.SaveLog("cb Qr save :" + ex.Message);
            }
        }

        private void txtEmployee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btConnect.PerformClick();
            }
        }
        private ColorAverage colorAverage;
        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (scrollablePictureBoxCamera02.Image == null)
            {
                MessageBox.Show("Please select camera 2", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            select_XY?.Close();
            select_XY = new Select_X_Y(this, 2);
            select_XY.Show();
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

                        softwareBmp?.Dispose();
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

        private void checkBoxAutoFocus_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isAutoFocus = checkBoxAutoFocus.Checked;
            Properties.Settings.Default.Save();

            if (cameraControl != null && !checkBoxAutoFocus.Checked)
            {
                cameraControl.setFocus((int)nFocus.Value);
            }
        }
        private MasterColors masterColors;
        private void masterColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            masterColors?.Close();
            masterColors = new MasterColors();
            masterColors.Show();
        }

        private ItemList itemList;
        private void sTEPTESTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            itemList?.Close();
            itemList = new ItemList(this);
            itemList.Show();
        }
        private SearchLB searchModel;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            searchModel?.Close();
            searchModel = new SearchLB(SearchType.Models);
            searchModel.OnSelect += SearchModel_OnSelect;
            searchModel.Show();
        }

        private void SearchModel_OnSelect(string name)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    txtModel.Text = name;
                }));
                return;
            }
            txtModel.Text = name;
        }

        private ImageProcessing imageProcessing;
        private void commandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageProcessing?.Close();
            imageProcessing = new ImageProcessing(this);
            imageProcessing.Show();
        }

        private void lbTitle_TextChanged(object sender, EventArgs e)
        {
            if (manualTest != null)
            {
                manualTest.lbTitle.Text = lbTitle.Text;
            }
        }

        private void lbTitle_BackColorChanged(object sender, EventArgs e)
        {
            if (manualTest != null)
            {
                manualTest.lbTitle.BackColor = lbTitle.BackColor;
            }
        }
        private Forms.Show.ShowImage showImage;
        private void showDiffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(imageDiffShow == null) return;
            
            showImage?.Close();
            showImage = new Forms.Show.ShowImage();
            showImage.pictureBox1.Image?.Dispose();
            showImage.pictureBox1.Image = new Bitmap(imageDiffShow);
            showImage.Show();
        }
    }
}
