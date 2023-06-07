using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TClass;

namespace SC_M4.Utilities
{

    public enum PageType
    {
        None,
        Add,
        Update,
        SW,
        LB
    }
    public enum ResultType
    {
        None,
        Success,
        Fail,
        Pass,
        Error,
        OK,
        NG,
        NotFound,
    }
    public class Heller
    {
        public struct AverageColor
        {
            public int R;
            public int G;
            public int B;
        }

        public enum AverageColorType : int
        {            
            TopLeft = 0,
            TopRight = 1,
            BottomLeft = 2,
            BottomRight = 3,
            Center = 4,
            Full = 5,
        }

        public static AverageColor GetAverageColor(Bitmap bmp, AverageColorType type = AverageColorType.Full, int maxWidth = 100, int maxHeight = 100)
        {
            try
            {
                if (bmp == null)
                {
                    throw new ArgumentNullException(nameof(bmp), "Bitmap is null");
                }

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

                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        Color clr = bmp.GetPixel(x, y);

                        totalR += clr.R;
                        totalG += clr.G;
                        totalB += clr.B;
                    }
                }

                long totalPixels = bmp.Width * bmp.Height;

                AverageColor averageColor;
                averageColor.R = (int)(totalR / totalPixels);
                averageColor.G = (int)(totalG / totalPixels);
                averageColor.B = (int)(totalB / totalPixels);

                return averageColor;
            }
            catch (Exception ex)
            {
                // Replace this with your actual logging method
                //LogWriter.SaveLog($"Error in GetAverageColor: {ex.Message}");
                throw; // Re-throw the exception
            }
        }
    }
}
