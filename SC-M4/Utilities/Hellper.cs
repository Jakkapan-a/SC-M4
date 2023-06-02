using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SC_M4.Utilities
{
    public class Heller
    {
        public struct AverageColor
        {
            public int R;
            public int G;
            public int B;
        }

        public enum AverageColorType
        {
            Full,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Center
        }

        public static AverageColor GetAverageColor(Bitmap bmp, AverageColorType type = AverageColorType.Full, int maxWidth = 100, int maxHeight = 100)
        {
            // Resize bitmap if necessary
            if (bmp.Width > maxWidth || bmp.Height > maxHeight)
            {
                float scale = Math.Min((float)maxWidth / bmp.Width, (float)maxHeight / bmp.Height);

                int newWidth = (int)(bmp.Width * scale);
                int newHeight = (int)(bmp.Height * scale);

                bmp = (Bitmap)bmp.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);
            }

            // Then continue with the previous code...
            long totalR = 0;
            long totalG = 0;
            long totalB = 0;

            var lockObject = new object();

            Parallel.For(0, bmp.Width, x =>
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color clr = bmp.GetPixel(x, y);
                    lock (lockObject)
                    {
                        totalR += clr.R;
                        totalG += clr.G;
                        totalB += clr.B;
                    }
                }
            });

            long totalPixels = bmp.Width * bmp.Height;

            AverageColor averageColor;
            averageColor.R = (int)(totalR / totalPixels);
            averageColor.G = (int)(totalG / totalPixels);
            averageColor.B = (int)(totalB / totalPixels);

            return averageColor;
        }
    }
}
