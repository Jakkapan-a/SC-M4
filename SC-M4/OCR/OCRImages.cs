﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace SC_M4.OCR
{
    class OCRImages : OCR<Image>
    {
        /// <summary>
        /// Recognize text.
        /// </summary>
        /// <param name="images"></param>
        /// <param name="inputName">input filename</param>
        /// <param name="index"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public override string RecognizeText(IList<Image> images, string inputName)
        {
            IEnumerable<string> configs_file = new List<string>() { CONFIGS_FILE };

            using (TesseractEngine engine = new TesseractEngine(Datapath + "/tessdata/", Language, EngineMode, configs_file))
            {
                engine.SetVariable("tessedit_create_hocr", OutputFormat == "hocr" ? "1" : "0");
                ControlParameters(engine);
                Tesseract.PageSegMode psm = (PageSegMode)Enum.Parse(typeof(PageSegMode), PageSegMode);

                StringBuilder strB = new StringBuilder();
                int pageNum = 0;

                foreach (Image image in images)
                {
                    pageNum++;
                    using (Pix pix = ConvertBitmapToPix(image))
                    {
                        using (Page page = engine.Process(pix, inputName, psm))
                        {
                            string text = OutputFormat == "hocr" ? page.GetHOCRText(pageNum - 1) : page.GetText();

                            if (text == null) return String.Empty;
                            strB.Append(text);
                        }
                    }
                }

                return strB.ToString().Replace("\n", Environment.NewLine);
            }
        }
        /// <summary>
        /// Reads tessdata/configs/tess_configvars and SetVariable on Tesseract engine.
        /// This only works for non-init parameters (@see <a href="https://code.google.com/p/tesseract-ocr/wiki/ControlParams">ControlParams</a>).
        /// </summary>
        /// <param name="engine"></param>
        void ControlParameters(TesseractEngine engine)
        {
            string configsFilePath = Path.Combine(Datapath, "tessdata/configs/" + CONFIGVARS_FILE);
            if (!File.Exists(configsFilePath))
            {
                return;
            }

            string[] lines = File.ReadAllLines(configsFilePath);
            foreach (string line in lines)
            {
                if (!line.Trim().StartsWith("#"))
                {
                    try
                    {
                        string[] keyValuePair = line.Trim().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                        string value = keyValuePair[1];
                        if (value == "T" || value == "F")
                        {
                            engine.SetVariable(keyValuePair[0], value == "T" ? true : false);
                        }
                        else
                        {
                            engine.SetVariable(keyValuePair[0], keyValuePair[1]);
                        }
                    }
                    catch
                    {
                        //ignore and continue on
                    }
                }
            }
        }

        public override void ProcessFile(string filename)
        {
            List<IResultRenderer> resultRenderers = new List<IResultRenderer>();

            switch (OutputFormat)
            {
                case "text":
                    resultRenderers.Add(ResultRenderer.CreateTextRenderer(OutputFile));
                    break;
                case "hocr":
                    resultRenderers.Add(ResultRenderer.CreateHOcrRenderer(OutputFile));
                    break;
                case "pdf":
                    //resultRenderers.Add(ResultRenderer.CreatePdfRenderer(OutputFile, Datapath + "\\tessdata"));
                    resultRenderers.Add(ResultRenderer.CreatePdfRenderer(OutputFile, Datapath + "\\tessdata", true));
                    break;
            }

            using (IResultRenderer renderer = new AggregateResultRenderer(resultRenderers))
            {
                ProcessImageFile(renderer, filename);
            }
        }

        /// <summary>
        /// Processes a file using ResultRenderers.
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="filename"></param>
        private void ProcessImageFile(IResultRenderer renderer, string filename)
        {
            IEnumerable<string> configs_file = new List<string>() { CONFIGS_FILE };

            using (TesseractEngine engine = new TesseractEngine(Datapath, Language, EngineMode, configs_file))
            {
                var imageName = Path.GetFileNameWithoutExtension(filename);

                using (var pixA = LoadPixArray(filename))
                {
                    using (renderer.BeginDocument(imageName))
                    {
                        foreach (var pix in pixA)
                        {
                            using (var page = engine.Process(pix, imageName))
                            {
                                var addedPage = renderer.AddPage(page);
                            }
                        }
                    }
                }
            }
        }

        private PixArray LoadPixArray(string filename)
        {
            if (filename.ToLower().EndsWith(".tif") || filename.ToLower().EndsWith(".tiff"))
            {
                return PixArray.LoadMultiPageTiffFromFile(filename);
            }
            else
            {
                PixArray pixA = PixArray.Create(0);
                pixA.Add(Pix.LoadFromFile(filename));
                return pixA;
            }
        }

        /// <summary>
        /// Converts .NET Bitmap to Leptonica Pix.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private Pix ConvertBitmapToPix(Image image)
        {
            try
            {
                return PixConverter.ToPix((Bitmap)image);
            }
            catch
            {
                return ConvertBitmapToPixViaFile(image);
            }
        }


        /// <summary>
        /// Writes .NET image to file and read it back as Pix image. Works in all cases but not efficient.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private Pix ConvertBitmapToPixViaFile(Image image)
        {
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";
            image.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
            Pix pix = Pix.LoadFromFile(fileName);
            File.Delete(fileName);

            return pix;
        }

        /// <summary>
        /// Gets segmented regions at specified page iterator level.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public override List<Rectangle> GetSegmentedRegions(Bitmap image, PageIteratorLevel level)
        {
            using (var engine = new TesseractEngine(Datapath, Language, EngineMode))
            {
                using (var page = engine.Process(image))
                {
                    List<Rectangle> boxes = page.GetSegmentedRegions(level);
                    return boxes;
                }
            }
        }


    }
}
