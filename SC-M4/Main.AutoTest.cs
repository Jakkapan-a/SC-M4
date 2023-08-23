﻿using OpenCvSharp;
using SC_M4.Forms;
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
            if (taskProcess != null && taskProcess.Status == TaskStatus.Running)
            {
                return;
            }

            cts_auto?.Dispose();
            cts_auto = new CancellationTokenSource();
            task_auto = Task.Run(() => DoWorkAutoTest(cts_auto.Token), cts_auto.Token);
        }
        private void StopAutoTest()
        {
            // Cancel task
            cts_auto.Cancel();
            
        }

        private List<ActionIO> actionIO;
        private void DoWorkAutoTest(CancellationToken token)
        {
            if (string.IsNullOrEmpty(txtModel.Text))
            {
                if (MessageBox.Show("Please input model name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    return;
                }

                return;
            }
            Console.WriteLine("Start Process...");
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
            // Start process
            ClearTxtBox();

            result_auto_test = ResultType.None;
            // For each item
            foreach (var item in items){
                List<Actions> actions = Actions.GetListByItemId(item.id);
                AppendTxtBox($"Start Test Item: {item.name}");
                foreach(var action in actions)
                {
                    AppendTxtBox($"Start Test Action: {action.name}");
                    SortingProcess(action, token);
                    // Check token is cancel
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    Thread.Sleep(action.delay);
                }                
                // Check token is cancel
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Cancel Process...");
                    AppendTxtBox($"Cancel Process...");
                    return;
                }
            }

            if(result_auto_test != ResultType.OK)
            {
                result_auto_test = ResultType.NG;
            }

            string result = result_auto_test == ResultType.OK ? "PASS" : "NG";

            AppendTxtBox($"Test Done " + (result_auto_test == ResultType.OK || result_auto_test == ResultType.OK ? "PASS" : "NG"));
            Console.WriteLine($"Test Done " + (result_auto_test == ResultType.OK || result_auto_test == ResultType.OK ? "PASS" : "NG"));
        }

        public void SortingProcess(Actions action, CancellationToken token,bool isGetIO = false)
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
                    while (result_auto_test == ResultType.None)
                    {
                        processOCR(token);
                        if(stopwatch.ElapsedMilliseconds > action.time_out)
                        {
                            result_auto_test = ResultType.NG;
                            break;
                        }

                        if (token.IsCancellationRequested)
                        {
                            Console.WriteLine("Cancel Process...");
                            return;
                        }
                    }
                    break;
            }
        }

        private void ProcessTypeAuto(Actions action,CancellationToken token)
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
        }


        private void ProcessTypeManual(Actions action, CancellationToken token)
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
            templateData["Command_io"][2] = 0X49;
            templateData["Command_io"][3] = 0x50;
            templateData["Command_io"][4] = value;
            templateData["Command_io"][6] = action.state == 1 ? (byte)0x01 : (byte)0x00; ;
            // Send parameter
            serialPortIO.SerialCommand(templateData["Command_io"]);
        }

        private void ProcessTypeServo(Actions action, CancellationToken token)
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
        }

        private Bitmap imageDiff;



        private void ProcessTypeImage(Actions action, CancellationToken token)
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
                using (Graphics g = Graphics.FromImage(bitmapCamera_01))
                {
                    g.DrawImage(bitmapCamera_01,0,0, bitmapCamera_01.Width, bitmapCamera_01.Height);
                }

                // Load master image
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using(Bitmap master = (Bitmap)Image.FromStream(fs))
                    {
                        List<Modules.Rect> rect = Modules.Rect.GetByAction(action.id);
                        if(rect == null || rect.Count == 0)
                        {
                            double maxVal;
                            OpenCvSharp.Point location;
                            // Compare 
                            CompareAndHandle(image, master, action.threshold, action);
                        }
                        else
                        {
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

                                        CompareAndHandle(comparator, template, action.threshold, action);
                                    }

                                }

                                if (token.IsCancellationRequested)
                                {
                                    return;
                                }
                            }
                        }                        
                       
                    }
                }

            }
        }

        private void CompareAndHandle(Bitmap comparator, Bitmap master, double threshold, Actions action)
        {
            double maxVal;
            OpenCvSharp.Point location;

            imageDiff?.Dispose();  // ทำลาย imageDiff ที่เก่าเมื่อจำเป็น
            imageDiff = ImageProcessing.CompareImages(comparator, master, out maxVal, out location);

            // Display ...

            if (maxVal > action.threshold)
            {
                // More code..
            }
            else
            {
                // More code..
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
                // if (maxVal > threshold) return true;
                return maxVal > threshold;
            }
        }
        // Append text to txtBoxResult
        private void AppendTxtBox(string txt_input)
        {
            if(txtBoxResult.InvokeRequired)
            {
                txtBoxResult.Invoke(new Action<string>(AppendTxtBox), new object[] { txt_input });
            }
            else
            {
                txtBoxResult.AppendText(txt_input + Environment.NewLine);
            }
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


