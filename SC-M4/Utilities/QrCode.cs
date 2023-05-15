using System.Drawing;
using ZXing;
namespace SC_M4.Utilities
{

    public class QrCode
    {
        public static Bitmap GenerateQrCode(string text, int width = 200, int height = 200)
        {
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = height,
                    Width = width
                }
            };
            return writer.Write(text);
        }

        public static string DecodeQRCode(Image qrCodeImage)
        {
            try
            {
                var reader = new BarcodeReader();
                using (var bitmap = qrCodeImage.Clone() as Bitmap)
                {
                    var result = reader.Decode(bitmap);
                    if (result != null)
                    {
                        return result.Text;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            catch
            {
                return "No QR Code found in the image";
            }

        }
    }
}