using OpenCvSharp;
using SC_M4.Forms;
using SC_M4.Forms.Show;
using SC_M4.Modules;
using SC_M4.Processing;
using SC_M4.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.UI;
using ZXing.Aztec.Internal;

namespace SC_M4
{
    public partial class Main
    {
        private CancellationTokenSource cts_auto = new CancellationTokenSource();
        private Task task_auto;
        private ResultType result_auto_test = ResultType.None;

        public void InitializeAutoTest()
        {

        }

        private void StartAutoTest()
        {
            if (task_auto != null && task_auto.Status == TaskStatus.Running)
            {
                AppendTxtBox("Task is run");
                return;
            }

            cts_auto?.Dispose();
            cts_auto = new CancellationTokenSource();
            task_auto = Task.Run(() => DoWorkAutoTest(cts_auto.Token), cts_auto.Token);
        }
        private void StopAutoTest()
        {
            // Cancel task
            cts_auto?.Cancel();

        }

        private List<ActionIO> actionIO;
        private void DoWorkAutoTest(CancellationToken token)
        {
            IsCapture = false;
            if (string.IsNullOrEmpty(txtModel.Text))
            {
                if (MessageBox.Show("Please input model name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    return;
                }

                return;
            }
            AppendTxtBox("Start Process...");
            IsChangeSelectedMode = false;
            // 
            actionIO = ActionIO.Get();

            var model = Models.Get(txtModel.Text);
            // 1. Check model
            if (model == null)
            {
                if (MessageBox.Show("Model not found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    return;
                }
            }

            // Get Item
            var items = Items.GetItemsByModelId(model.id);
            if (items == null || items.Count == 0)
            {
                if (MessageBox.Show("Item not found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    return;
                }
            }
            Invoke(new Action(() =>
            {
                this.richTextBox1.Text = string.Empty;
                this.richTextBox2.Text = string.Empty;
            }));

            // Start process
            ClearTxtBox();
            UpdateUIAndInvoke("START", System.Drawing.Color.Yellow);
            is_Blink_NG = false;
            result_auto_test = ResultType.None;
            string title = string.Empty;
            int i = 0;
            // For each item
            foreach (var item in items)
            {
                List<Actions> actions = Actions.GetListByItemId(item.id);
                AppendTxtBox($"Start Test Item: {item.name}");
                UpdateUIAndInvoke($"{item.name} {(++i)}/{items.Count()}", System.Drawing.Color.Yellow);
                foreach (var action in actions)
                {
                    AppendTxtBox($"Start Test Action: {action.name}");
                    SortingProcess(action, token);
                    // Check token is cancel
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    // Sleep next
                    Thread.Sleep(action.delay);
                    if (result_auto_test == ResultType.NG)
                    {
                        title = $" Action: {action.name}";
                        break;
                    }
                }

                if (result_auto_test == ResultType.NG)
                {
                    break;
                }
                // Check token is cancel
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Cancel Process...");
                    AppendTxtBox($"Cancel Process...");
                    CloseAllIO();
                    return;
                }
            }
            Thread.Sleep(100);
            if (result_auto_test == ResultType.NG)
            {
                IsChangeSelectedMode = false;
                UpdateUIAndInvoke("NG", System.Drawing.Color.Red);
            }

            if (result_auto_test == ResultType.OK || result_auto_test == ResultType.None)
            {
                // 02 43 49 50 32 00 01 03 MES ON
                templateData["Command_MES"][0] = 0x02; // 02 STX
                templateData["Command_MES"][1] = 0x43; // 43
                templateData["Command_MES"][2] = 0X49;
                templateData["Command_MES"][3] = 0x50;
                templateData["Command_MES"][4] = 0x32; // 50
                templateData["Command_MES"][6] = 0x01;
                templateData["Command_MES"][7] = 0x03; // 03 ETX   

                string hex = string.Empty;
                foreach (var b in templateData["Command_MES"])
                {
                    hex += b.ToString("X2") + " ";
                }
                AppendTxtBox($"Command: {hex}");
                // Send parameter
                serialPortIO.SerialCommand(templateData["Command_MES"]);
                IsChangeSelectedMode = true;
                // Update
                UpdateUIAndInvoke("PASS", System.Drawing.Color.Green);
            }

            AppendTxtBox($"Test Done " + (result_auto_test == ResultType.OK || result_auto_test == ResultType.OK ? "PASS" : "NG"));
            Console.WriteLine($"Test Done " + (result_auto_test == ResultType.OK || result_auto_test == ResultType.OK ? "PASS" : "NG"));
            IsCapture = false;
            CloseAllIO();

        }

        private void CloseAllIO()
        {
            if (actionIO == null) return;
            // Close All IO
            foreach (var item in actionIO)
            {
                int pin = item.pin;

                byte value = ((byte)pin);
                templateData["Command_io"][0] = 0x02; // 02 STX
                templateData["Command_io"][1] = 0x43; // 43
                templateData["Command_io"][2] = 0X49;
                templateData["Command_io"][3] = 0x50;
                templateData["Command_io"][4] = value;
                templateData["Command_io"][6] = 0x00;

                string hex = string.Empty;
                foreach (var b in templateData["Command_io"])
                {
                    hex += b.ToString("X2") + " ";
                }
                AppendTxtBox($"Command: {hex}");
                // Send parameter
                serialPortIO.SerialCommand(templateData["Command_io"]);
                Thread.Sleep(100);
            }
        }

        public void SortingProcess(Actions action, CancellationToken token, bool isGetIO = false)
        {
            if (isGetIO)
            {
                actionIO = ActionIO.Get();
            }
            if (token.IsCancellationRequested)
            {
                return;
            }

            switch ((Utilities.TypeAction)action.type)
            {
                case Utilities.TypeAction.Auto:
                    ProcessTypeAuto(action, token);
                    break;
                case Utilities.TypeAction.Manual:
                    ProcessTypeManual(action, token);
                    break;
                case Utilities.TypeAction.Servo:
                    ProcessTypeServo(action, token);
                    break;
                case Utilities.TypeAction.Image:
                    ProcessTypeImage(action, token);
                    break;
                case Utilities.TypeAction.Compare:
                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();
                    result_auto_test = ResultType.None;

                    int retries = 3;
                    while (retries > 0)
                    {
                        try
                        {
                            while (result_auto_test == ResultType.None)
                            {
                                if (token.IsCancellationRequested)
                                {
                                    break;
                                }

                                processOCR(token);

                                if (stopwatch.ElapsedMilliseconds > action.time_out)
                                {
                                    result_auto_test = ResultType.NG;
                                    break;
                                }

                                if (token.IsCancellationRequested)
                                {
                                    Console.WriteLine("Cancel Process...");
                                    break;
                                }

                                AppendTxtBox($"Time Out:{(stopwatch.ElapsedMilliseconds / 1000)}s");
                                isStateReset = true;
                                Thread.Sleep(500);
                                if (token.IsCancellationRequested)
                                {
                                    break;
                                }
                            }

                            break;
                        }
                        catch (Exception ex)
                        {
                            HandleExceptionTest(ex);
                            AppendTxtBox($"ERROR :{ex.Message}");
                            retries--;
                        }

                        if (retries == 0)
                        {
                            AppendTxtBox("Failed after 3 attempts.");
                        }
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                    break;
            }
        }
        private void ProcessTypeAuto(Actions action, CancellationToken token)
        {
            int retries = 3;
            while (retries > 0)
            {
                try
                {
                    // Set parameter
                    ActionIO iO = actionIO.FirstOrDefault(x => x.id == action.action_io_id);
                    if (iO == null)
                    {
                        Console.WriteLine($"Action IO not found");
                        return;
                    }

                    int pin = iO.pin;
                    byte value = ((byte)pin);
                    templateData["Command_io"][0] = 0X02;
                    templateData["Command_io"][1] = 0x43;
                    templateData["Command_io"][2] = 0X49;
                    templateData["Command_io"][3] = 0x50;
                    templateData["Command_io"][4] = value;
                    templateData["Command_io"][6] = 0x01;

                    string hex = string.Empty;
                    foreach (var b in templateData["Command_io"])
                    {
                        hex += b.ToString("X2") + " ";
                    }
                    AppendTxtBox($"Command: {hex}");
                    // Send parameter
                    serialPortIO.SerialCommand(templateData["Command_io"]);
                    Thread.Sleep(action.auto_delay);

                    // OFF
                    // Set parameter
                    templateData["Command_io"][6] = 0x00;
                    hex = string.Empty;
                    foreach (var b in templateData["Command_io"])
                    {
                        hex += b.ToString("X2") + " ";
                    }
                    AppendTxtBox($"Command: {hex}");
                    // Send parameter
                    serialPortIO.SerialCommand(templateData["Command_io"]);
                    Thread.Sleep(100);
                    break;
                }
                catch (Exception ex)
                {
                    HandleExceptionTest(ex);
                    AppendTxtBox($"ERROR :{ex.Message}");
                    retries--;
                }
                if (token.IsCancellationRequested)
                {
                    break;
                }
            }

            if (retries == 0)
            {
                AppendTxtBox("Failed after 3 attempts.");
            }
        }


        private void ProcessTypeManual(Actions action, CancellationToken token)
        {
            int retries = 3;
            while (retries > 0)
            {
                try
                {
                    // Set parameter
                    ActionIO iO = actionIO.FirstOrDefault(x => x.id == action.action_io_id);
                    if (iO == null)
                    {
                        Console.WriteLine($"Action IO not found");
                        return;
                    }
                    int pin = iO.pin;
                    byte value = ((byte)pin);
                    templateData["Command_io"][0] = 0X02;
                    templateData["Command_io"][1] = 0x43;
                    templateData["Command_io"][2] = 0X49;
                    templateData["Command_io"][3] = 0x50;
                    templateData["Command_io"][4] = value;
                    templateData["Command_io"][6] = action.state == 1 ? (byte)0x01 : (byte)0x00;

                    string hex = string.Empty;
                    foreach (var b in templateData["Command_io"])
                    {
                        hex += b.ToString("X2") + " ";
                    }
                    AppendTxtBox($"Command: {hex}");

                    // Send parameter
                    serialPortIO.SerialCommand(templateData["Command_io"]);
                    break;
                }
                catch (Exception ex)
                {
                    HandleExceptionTest(ex);
                    AppendTxtBox($"ERROR :{ex.Message}");
                    retries--;
                }
                if (token.IsCancellationRequested)
                {
                    break;
                }

            }

            if (retries == 0)
            {
                AppendTxtBox("Failed after 3 attempts.");
            }
        }

        private void ProcessTypeServo(Actions action, CancellationToken token)
        {
            int retries = 3;
            while (retries > 0)
            {
                try
                {

                    // Set parameter           
                    int pin = 4;
                    byte value = ((byte)pin);
                    templateData["Command_io"][2] = 0X49;
                    templateData["Command_io"][3] = 0x50;
                    templateData["Command_io"][4] = value;
                    templateData["Command_io"][6] = (byte)action.servo;
                    // Send parameter
                    serialPortIO.SerialCommand(templateData["Command_io"]);

                    break;

                }
                catch (Exception ex)
                {
                    HandleExceptionTest(ex);
                    AppendTxtBox($"ERROR :{ex.Message}");
                    retries--;

                }

                if (token.IsCancellationRequested)
                {
                    break;
                }
            }

            if (retries == 0)
            {
                AppendTxtBox("Failed after 3 attempts.");
            }
        }

        private Bitmap imageDiff;

        private void ProcessTypeImage(Actions action, CancellationToken token)
        {
            int retries = 3;
            while (retries > 0)
            {
                try
                {
                    // Check master image
                    string path = Path.Combine(Properties.Resources.path_images, action.image_name);
                    if (!File.Exists(path))
                    {
                        Console.WriteLine($"Image not found");
                        return;
                    }
                    // Load image
                    using (Bitmap image = new Bitmap(bitmapCamera_01.Width, bitmapCamera_01.Height))
                    {
                        IsCapture = true;
                        Thread.Sleep(50);
                        using (Graphics g = Graphics.FromImage(image))
                        {
                            g.DrawImage(bitmapCamera_01, 0, 0, bitmapCamera_01.Width, bitmapCamera_01.Height);
                        }
                        IsCapture = false;
                        // Load master image
                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            using (Bitmap master = (Bitmap)Image.FromStream(fs))
                            {
                                List<Modules.Rect> rect = Modules.Rect.GetByAction(action.id);

                                IsCapture = true;
                                // Compare 
                                Invoke(new Action(() =>
                                {
                                    pictureBoxCamera01.Image?.Dispose();
                                    pictureBoxCamera01.Image = new Bitmap(image);


                                    pictureBoxCamera02.Image?.Dispose();
                                    pictureBoxCamera02.Image = new Bitmap(master);
                                }));

                                CompareAndHandle(image, master, action.threshold, action);

                                imageDiffShow?.Dispose();
                                imageDiffShow = new Bitmap(imageDiff.Width, imageDiff.Height);
                                using (Graphics g = Graphics.FromImage(imageDiffShow))
                                {
                                    g.DrawImage(imageDiff, 0, 0, imageDiffShow.Width, imageDiffShow.Height);
                                }


                                IsCapture = false;
                                if (rect != null && rect.Count > 0)
                                {
                                    IsCapture = true;
                                    foreach (var r in rect)
                                    {
                                        Console.WriteLine($"Rect: {r.x} {r.y} {r.width} {r.height}");
                                        using (Bitmap template = new Bitmap(r.width, r.height))
                                        {
                                            using (Graphics g = Graphics.FromImage(template))
                                            {
                                                Rectangle rectangle = new Rectangle(r.x, r.y, r.width, r.height);
                                                g.DrawImage(master, 0, 0, rectangle, GraphicsUnit.Pixel);
                                            }

                                            using (Bitmap comparator = new Bitmap(r.width, r.height))
                                            {
                                                using (Graphics g = Graphics.FromImage(comparator))
                                                {
                                                    Rectangle rectangle = new Rectangle(r.x, r.y, r.width, r.height);
                                                    g.DrawImage(image, 0, 0, rectangle, GraphicsUnit.Pixel);
                                                }

                                                Invoke(new Action(() =>
                                                {
                                                    pictureBoxCamera01.Image?.Dispose();
                                                    pictureBoxCamera01.Image = new Bitmap(comparator);


                                                    pictureBoxCamera02.Image?.Dispose();
                                                    pictureBoxCamera02.Image = new Bitmap(template);
                                                }));


                                                CompareAndHandle(comparator, template, r.threshold, action);
                                                Thread.Sleep(50);
                                            }

                                        }

                                        if (token.IsCancellationRequested || result_auto_test == ResultType.NG)
                                        {
                                            break;
                                        }
                                    }
                                    IsCapture = false;
                                }
                            }
                        }
                    }
                    break;
                }
                catch (Exception ex)
                {
                    HandleExceptionTest(ex);
                    AppendTxtBox($"ERROR :{ex.Message}");
                    retries--;
                }
            }

            if (retries == 0)
            {
                AppendTxtBox("Failed after 3 attempts.");
            }

        }

        private Bitmap imageDiffShow;

        private void CompareAndHandle(Bitmap comparator, Bitmap master, double threshold, Actions action)
        {
            int retries = 3;
            while (retries > 0)
            {
                try
                {
                    double maxVal;
                    OpenCvSharp.Point location;

                    imageDiff?.Dispose();  // ทำลาย imageDiff ที่เก่าเมื่อจำเป็น
                    imageDiff = ImageProcessing.CompareImages(comparator, master, out maxVal, out location);
                    // maxVal = ImageProcessing.CompareImages(comparator, master);
                    // Display ...
                    AppendTxtBox($"Compare : {(maxVal * 100):F2} > {action.threshold}");

                    UpdateUIAndInvoke($"Different: {(maxVal * 100):F2} > {action.threshold}", System.Drawing.Color.Yellow);


                    if ((maxVal * 100) > action.threshold)
                    {
                        result_auto_test = ResultType.OK;
                    }
                    else
                    {
                        string file_name = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}";
                        // Difference image
                        string name = file_name + "_Diff.jpg";
                        string path = Path.Combine(Properties.Resources.path_temp, $"{name}.jpg");
                        imageDiff?.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                        LogWriter.SaveLog(name);
                        // Comparator image
                        name = file_name + "_Comparator.jpg";
                        path = Path.Combine(Properties.Resources.path_temp, $"{name}.jpg");
                        comparator?.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                        LogWriter.SaveLog(name);
                        // Master image
                        name = file_name + "_Master.jpg";
                        path = Path.Combine(Properties.Resources.path_temp, $"{name}.jpg");
                        master?.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                        LogWriter.SaveLog(name);

                        result_auto_test = ResultType.NG;
                    }
                    break;
                }
                catch (Exception ex)
                {

                    HandleExceptionTest(ex);
                    AppendTxtBox($"ERROR :{ex.Message}");
                    retries--;
                }
            }

            if (retries == 0)
            {
                AppendTxtBox("Failed after 3 attempts.");
            }

        }

        public bool CompareImages(Bitmap imgBitmap, Bitmap templateBitmap, double threshold)
        {
            using (Mat img = OpenCvSharp.Extensions.BitmapConverter.ToMat(imgBitmap))
            using (Mat template = OpenCvSharp.Extensions.BitmapConverter.ToMat(templateBitmap))
            using (Mat result = new Mat())
            {
                Cv2.MatchTemplate(img, template, result, TemplateMatchModes.CCoeffNormed);
                Cv2.MinMaxLoc(result, out double minVal, out double maxVal, out _, out _);  // ใช้ discards สำหรับตัวแปรที่ไม่จำเป็นต้องใช้
                return maxVal > threshold;
            }
        }
        // Append text to txtBoxResult
        private void AppendTxtBox(string txt_input)
        {
            if (txtBoxResult.InvokeRequired)
            {
                txtBoxResult.Invoke(new Action<string>(AppendTxtBox), new object[] { txt_input });

                return;
            }

            txtBoxResult.AppendText(txt_input + Environment.NewLine);
            // Scroll to end
            txtBoxResult.SelectionStart = txtBoxResult.Text.Length;
            txtBoxResult.ScrollToCaret();

        }

        // Clear text to txtBoxResult
        private void ClearTxtBox()
        {
            if (txtBoxResult.InvokeRequired)
            {
                txtBoxResult.Invoke(new Action(ClearTxtBox));
            }
            else
            {
                // Save at path_log
                string path = Path.Combine(Properties.Resources.path_log, $"TEST_LOG{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
                File.WriteAllText(path, txtBoxResult.Text);
                txtBoxResult.Clear();
            }
        }
    }
}


