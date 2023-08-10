using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;

namespace SC_M4.OCR
{
    public static class OcrProcessor
    {
        public static IReadOnlyList<Language> GetOcrLangList()
        {
            return OcrEngine.AvailableRecognizerLanguages;
        }

        public static async Task<List<string>> GetText(SoftwareBitmap imageItem, Language SelectedLang)
        {
            //check item is null
            //check no selectedLang
            //check image is too large
            if (imageItem == null ||
                SelectedLang == null ||
                imageItem.PixelWidth > OcrEngine.MaxImageDimension ||
                imageItem.PixelHeight > OcrEngine.MaxImageDimension
                )
            {
                return new List<string> { "" };
            }


            //check ocr image exist
            var ocrEngine = OcrEngine.TryCreateFromLanguage(SelectedLang);
            if (ocrEngine == null)
            {
                return new List<string> { "" };
            }


            var ocrResult = await ocrEngine.RecognizeAsync(imageItem);


            List<string> textList = new List<string>() { };
            foreach (var line in ocrResult.Lines)
            {
                textList.Add(line.Text);
            }
            return textList;
        }
        public async static Task<OcrResult> GetOcrResultFromBitmap(Bitmap scaledBitmap, Language selectedLanguage)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                scaledBitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;

                BitmapDecoder bmpDecoder = await BitmapDecoder.CreateAsync(memory.AsRandomAccessStream());
                SoftwareBitmap softwareBmp = await bmpDecoder.GetSoftwareBitmapAsync();

                OcrEngine ocrEngine = OcrEngine.TryCreateFromLanguage(selectedLanguage);

                // Run the RecognizeAsync call in a separate thread to allow message pumping
                OcrResult result = await Task.Run(async () => await ocrEngine.RecognizeAsync(softwareBmp));
                scaledBitmap?.Dispose();
                softwareBmp?.Dispose();
                ocrEngine = null;
                return result;
            }
        }
    }
}
