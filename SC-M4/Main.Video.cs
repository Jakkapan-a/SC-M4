using System;
using System.Collections.Generic;
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

        public Bitmap bmp2;
        public Bitmap bmp2_color;
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

                Rectangle rectangle = new Rectangle(Properties.Settings.Default.color_x, Properties.Settings.Default.color_y, Properties.Settings.Default.color_width, Properties.Settings.Default.color_high);
                using (Graphics g = Graphics.FromImage(scrollablePictureBoxCamera02.Image))
                {
                    g.DrawRectangle(new Pen(Color.Red, 2), rectangle);
                   
                }

                bmp2_color?.Dispose();
                bmp2_color = new Bitmap(rectangle.Width, rectangle.Height);
                using (Graphics gx = Graphics.FromImage(scrollablePictureBoxCamera02.Image))
                {

                    gx.DrawImage(bmp2_color, 0, 0, rectangle, GraphicsUnit.Pixel);
                }
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
                Invoke(new Action(() => Capture_1_OnFrameHeadler(bitmap)));
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
    }
}
