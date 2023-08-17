using OpenCvSharp;
using SC_M4.Forms;
using SC_M4.Modules;
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

            // For each item
            foreach(var item in items){
                List<Actions> actions = Actions.GetListByItemId(item.id);
                foreach(var action in actions)
                {
                    SortingProcess(action, token);
                    Thread.Sleep(action.delay);
                }
            }

            Console.WriteLine($"Tset Done");
        }

        public void SortingProcess(Actions action, CancellationToken token)
        {

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
                    processOCR(token);
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
            // Send parameter
            serialPortIO.SerialCommand(templateData["Command_io"]);
            Thread.Sleep(action.auto_delay);
            // Set parameter
            templateData["Command_io"][6] = 0x00;
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
        private void ProcessTypeImage(Actions action, CancellationToken token)
        {
            using (Bitmap image = new Bitmap(bitmapCamera_01.Width, bitmapCamera_01.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmapCamera_01))
                {
                    g.DrawImage(bitmapCamera_01,0,0, bitmapCamera_01.Width, bitmapCamera_01.Height);
                }

                // Load master image
                string path = Path.Combine(Properties.Resources.path_images, action.image_name);
                if(!File.Exists(path))
                {
                    Console.WriteLine($"Image not found");
                    return;
                }
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using(Bitmap master = (Bitmap)Image.FromStream(fs))
                    {
                        List<Modules.Rect> rect = Modules.Rect.GetByAction(action.id);
                        if(rect == null || rect.Count == 0)
                        {
                            Console.WriteLine($"Rect not found");
                            return;
                        }
                        
                        foreach(var r in rect)
                        {

                            Console.WriteLine($"Rect: {r.x} {r.y} {r.width} {r.height}");

                            
                            // using (Bitmap template = new Bitmap(r.width, r.height))
                            // {
                            //     using (Graphics g = Graphics.FromImage(template))
                            //     {
                            //         g.DrawImage(master, new Rectangle(0, 0, r.width, r.height), new Rectangle(r.x, r.y, r.width, r.height), GraphicsUnit.Pixel);
                            //     }

                            //     if (CompareImages(image, template, 0.9))
                            //     {
                            //         Console.WriteLine($"Found image");
                            //         return;
                            //     }
                            // }
                        }
                    }
                }

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

    }
}


