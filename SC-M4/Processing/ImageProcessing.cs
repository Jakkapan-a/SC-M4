using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_M4.Processing
{
    public class ImageProcessing
    {
        public static double CompareImages(Bitmap imgBitmap, Bitmap templateBitmap)
        {
            using (Mat img = OpenCvSharp.Extensions.BitmapConverter.ToMat(imgBitmap))
            using (Mat template = OpenCvSharp.Extensions.BitmapConverter.ToMat(templateBitmap))
            using (Mat result = new Mat())
            {
                Cv2.MatchTemplate(img, template, result, TemplateMatchModes.CCoeffNormed);
                Cv2.MinMaxLoc(result, out double minVal, out double maxVal, out _, out _);  // ใช้ discards สำหรับตัวแปรที่ไม่จำเป็น
                return maxVal;
            }
        }

        public static double CompareImages(Bitmap imgBitmap, Bitmap templateBitmap, out OpenCvSharp.Point location)
        {
            using (Mat img = OpenCvSharp.Extensions.BitmapConverter.ToMat(imgBitmap))
            using (Mat template = OpenCvSharp.Extensions.BitmapConverter.ToMat(templateBitmap))
            using (Mat result = new Mat())
            {
                Cv2.MatchTemplate(img, template, result, TemplateMatchModes.CCoeffNormed);
                Cv2.MinMaxLoc(result, out double minVal, out double maxVal, out OpenCvSharp.Point minLoc, out OpenCvSharp.Point maxLoc);  //
                location = maxLoc;
                return maxVal;
            }
        }


        public static double CompareImages(Bitmap imgBitmap, Bitmap templateBitmap, out Image image){
            using (Mat img = OpenCvSharp.Extensions.BitmapConverter.ToMat(imgBitmap))
            using (Mat template = OpenCvSharp.Extensions.BitmapConverter.ToMat(templateBitmap))
            using (Mat result = new Mat())
            {
                Cv2.MatchTemplate(img, template, result, TemplateMatchModes.CCoeffNormed);
                Cv2.MinMaxLoc(result, out double minVal, out double maxVal, out OpenCvSharp.Point minLoc, out OpenCvSharp.Point maxLoc);  // 
                
                using (Mat diff = new Mat())
                {
                    Cv2.Absdiff(img, template, diff);
                    image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(diff);
                }

                return maxVal;
            }
        }


        public static Bitmap CompareImages(Bitmap imgBitmap, Bitmap templateBitmap, out double maxVal)
        {
            using (Mat img = OpenCvSharp.Extensions.BitmapConverter.ToMat(imgBitmap))
            using (Mat template = OpenCvSharp.Extensions.BitmapConverter.ToMat(templateBitmap))
            using (Mat result = new Mat())
            {
                Cv2.MatchTemplate(img, template, result, TemplateMatchModes.CCoeffNormed);
                Cv2.MinMaxLoc(result, out double minVal, out maxVal, out OpenCvSharp.Point minLoc, out OpenCvSharp.Point maxLoc);

                using (Mat diff = new Mat())
                {
                    Cv2.Absdiff(img, template, diff);
                    return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(diff);
                }
            }
        }

        public static Bitmap CompareImages(Bitmap imgBitmap, Bitmap templateBitmap, out double maxVal, out OpenCvSharp.Point location)
        {
            using (Mat img = OpenCvSharp.Extensions.BitmapConverter.ToMat(imgBitmap))
            using (Mat template = OpenCvSharp.Extensions.BitmapConverter.ToMat(templateBitmap))
            using (Mat result = new Mat())
            {
                Cv2.MatchTemplate(img, template, result, TemplateMatchModes.CCoeffNormed);
                Cv2.MinMaxLoc(result, out double minVal, out maxVal, out OpenCvSharp.Point minLoc, out location);

                using (Mat diff = new Mat())
                {
                    Cv2.Absdiff(img, template, diff);
                    return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(diff);
                }
            }
        }

        public static Bitmap CropImage(Bitmap imgBitmap, Rectangle cropArea)
        {
            using (Mat img = OpenCvSharp.Extensions.BitmapConverter.ToMat(imgBitmap))
            using (Mat cropped = new Mat(img, new Rect(cropArea.X, cropArea.Y, cropArea.Width, cropArea.Height)))
            {
                return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(cropped);
            }
        }

        public static Bitmap CropImage(Bitmap imgBitmap, OpenCvSharp.Point location, OpenCvSharp.Size size)
        {
            using (Mat img = OpenCvSharp.Extensions.BitmapConverter.ToMat(imgBitmap))
            using (Mat cropped = new Mat(img, new Rect(location.X, location.Y, size.Width, size.Height)))
            {
                return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(cropped);
            }
        }

        public static Bitmap CropImage(Bitmap imgBitmap, OpenCvSharp.Point location, int width, int height)
        {
            using (Mat img = OpenCvSharp.Extensions.BitmapConverter.ToMat(imgBitmap))
            using (Mat cropped = new Mat(img, new Rect(location.X, location.Y, width, height)))
            {
                return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(cropped);
            }
        }

        public static Bitmap CropImage(Bitmap imgBitmap, int x, int y, int width, int height)
        {
            using (Mat img = OpenCvSharp.Extensions.BitmapConverter.ToMat(imgBitmap))
            using (Mat cropped = new Mat(img, new Rect(x, y, width, height)))
            {
                return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(cropped);
            }
        }

        public static Bitmap CropImage(Bitmap imgBitmap, Rectangle cropArea, int padding)
        {
            using (Mat img = OpenCvSharp.Extensions.BitmapConverter.ToMat(imgBitmap))
            using (Mat cropped = new Mat(img, new Rect(cropArea.X - padding, cropArea.Y - padding, cropArea.Width + padding * 2, cropArea.Height + padding * 2)))
            {
                return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(cropped);
            }
        }

    
    }
}
