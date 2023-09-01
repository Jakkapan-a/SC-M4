using SC_M4.Forms.Show;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_M4
{
    partial class Main
    {
        #region Video Capture
        private void Capture_2_OnVideoStop()
        {
            LogWriter.SaveLog("Video 2 Stop");
            // Clear Image
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    Capture_2_OnVideoStop();
                }));
                return;
            }

            pictureBoxCamera02.Image?.Dispose();
            pictureBoxCamera02.Image = null;

        }

        private void Capture_2_OnVideoStarted()
        {
            Console.WriteLine("Video 2 Start");
            LogWriter.SaveLog("Video 2 Start");
        }

        public Bitmap bmp2;
        public Bitmap bmp2_color;

        private void Capture_2_OnFrameHeader(Bitmap bitmap)
        {
            if (InvokeRequired)
            {
                bitmapCamera_02?.Dispose();
                bitmapCamera_02 = (Bitmap)bitmap.Clone();

                Invoke(new Action(() => Capture_2_OnFrameHeader(bitmap)));
                return;
            }


            if (!IsCapture)
            {
                pictureBoxCamera02.Image?.Dispose();
                pictureBoxCamera02.Image = (System.Drawing.Image)bitmap.Clone();
 
                if (rect_2 != Rectangle.Empty && isStateReset)
                {

                    bmp2?.Dispose();
                    bmp2 = new Bitmap(rect_2.Width, rect_2.Height);
                    using (Graphics g = Graphics.FromImage(bmp2))
                    {
                        g.DrawImage(bitmapCamera_02, 0, 0, rect_2, GraphicsUnit.Pixel);
                    }
                    scrollablePictureBoxCamera02.Image?.Dispose();
                    scrollablePictureBoxCamera02.Image = new Bitmap(bmp2); ;

                    Rectangle rectangle = new Rectangle(Properties.Settings.Default.color_x, Properties.Settings.Default.color_y, Properties.Settings.Default.color_width, Properties.Settings.Default.color_high);
                    using (Graphics g = Graphics.FromImage(scrollablePictureBoxCamera02.Image))
                    {
                        g.DrawRectangle(new Pen(Color.Red, 2), rectangle);
                    }

                    rectangle = new Rectangle(Properties.Settings.Default.color_x + 2, Properties.Settings.Default.color_y + 2, Properties.Settings.Default.color_width - 6, Properties.Settings.Default.color_high - 6);

                    bmp2_color?.Dispose();
                    bmp2_color = new Bitmap(rectangle.Width, rectangle.Height);
                    using (Graphics gx = Graphics.FromImage(bmp2_color))
                    {
                        gx.DrawImage(scrollablePictureBoxCamera02.Image, 0, 0, rectangle, GraphicsUnit.Pixel);
                    }
                    pgRGB.Image?.Dispose();
                    pgRGB.Image = new Bitmap(bmp2_color);
                    // Draw Rectangle to Image
                    using (Graphics g = Graphics.FromImage(pictureBoxCamera02.Image))
                    {
                        g.DrawRectangle(new Pen(Color.Red, 2), rect_2);
                    }
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
                    Capture_1_OnVideoStop();
                }));
                return;
            }

            pictureBoxCamera01.Image?.Dispose();
            pictureBoxCamera01.Image = null;

        }

        private void Capture_1_OnVideoStarted()
        {
            Console.WriteLine("Video 1 Start");
            LogWriter.SaveLog("Video 1 Start");
        }

        public Bitmap bmp1;
        private Stopwatch stopwatchManualTest = new Stopwatch();
        private bool IsCapture = false;
        private void Capture_1_OnFrameHeader(Bitmap bitmap)
        {
            if (InvokeRequired)
            {
                bitmapCamera_01?.Dispose();
                bitmapCamera_01 = (Bitmap)bitmap.Clone();
                Invoke(new Action(() => Capture_1_OnFrameHeader(bitmap)));
                return;
            }

            if(stopwatchManualTest == null)
            {
                stopwatchManualTest = new Stopwatch();
            }
            if (!IsCapture)
            {
                pictureBoxCamera01.Image?.Dispose();
                pictureBoxCamera01.Image = (System.Drawing.Image)bitmap.Clone();
                if (manualTest != null)
                {
                    manualTest.SetImage(bitmap);
                }
   
                if (rect_1 != Rectangle.Empty && isStateReset)
                {
                    bmp1?.Dispose();
                    bmp1 = new Bitmap(rect_1.Width, rect_1.Height);
                    using (Graphics g = Graphics.FromImage(bmp1))
                    {
                        g.DrawImage(bitmapCamera_01, 0, 0, rect_1, GraphicsUnit.Pixel);
                    }
                    scrollablePictureBoxCamera01.Image?.Dispose();
                    scrollablePictureBoxCamera01.Image = new Bitmap(bmp1);
                    // Draw Rectangle to Image
                    using (Graphics g = Graphics.FromImage(pictureBoxCamera01.Image))
                    {
                        g.DrawRectangle(new Pen(Color.Red, 2), rect_1);
                    }
                }
            }

            if (typeSelected == Utilities.TypeAction.Manual)
            {
                if (stopwatchManualTest.ElapsedMilliseconds > 1000)
                {
                    StartManualTest();
                    stopwatchManualTest.Restart();
                }
            }

        }

        #endregion
    }
}
