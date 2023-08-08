﻿using System;
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

        public static AverageColor GetAverageColor(Bitmap bmp)
        {
            try
            {
                if (bmp == null)
                {
                    throw new ArgumentNullException(nameof(bmp), "Bitmap is null");
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
                throw; // Re-throw the exception
            }
        }
    }
}
