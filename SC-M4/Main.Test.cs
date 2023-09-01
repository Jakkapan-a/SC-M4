using OpenCvSharp.Aruco;
using SC_M4.Modules;
using SC_M4.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Ocr;
using Windows.UI.Xaml.Shapes;
using static SC_M4.Utilities.Heller;
using Rectangle = System.Drawing.Rectangle;

namespace SC_M4
{
    partial class Main
    {

        private Task taskProcess;
        private void TimerOCR_Tick(object sender, EventArgs e)
        {
            onTest();
        }

        private async void onTest()
        {
            if (taskProcess != null && taskProcess.Status == TaskStatus.Running)
            {
                return;
            }

            try
            {
                taskProcess = Task.Run(() => processOCR(cts.Token), cts.Token);
                await taskProcess;
            }
            catch (Exception ex)
            {
                LogWriter.SaveLog(ex.Message);
            }
        }


        private List<ReplaceName> replaceNames1 = new List<ReplaceName>();
        private List<ReplaceName> replaceNames2 = new List<ReplaceName>();

        private void processOCR(CancellationToken token)
        {
            try
            { 
                #region Old code

                if (capture_1._isRunning && capture_2._isRunning && bitmapCamera_01 != null && bitmapCamera_02 != null && isStateReset && scrollablePictureBoxCamera01.Image != null && scrollablePictureBoxCamera02.Image != null)
                {
                    if (isStarted)
                    {
                        //capture_2.setFocus((int)nFocus.Value);
                        isStarted = false;
                    }
                    IsChangeSelectedMode = true;
                    stopwatch.Reset();
                    ToggleDetectionStatus();

                    IsCapture = true;
                    // Image 01 OCR 
                    if (useQrCode)
                    {
                        result_1 = QrCode.DecodeQRCode(scrollablePictureBoxCamera01.Image);
                    }
                    else
                    {
                        imageList?.Clear();
                        using (Bitmap image = new Bitmap(bmp1))
                        {
                            //ocrResult2 = GetOcrResultBitmap(image, SelectedLang).Result;
                            imageList.Add(image);
                            result_1 = performOCR(imageList, inputfilename, imageIndex, Rectangle.Empty).Result;
                        }


                    }

                    var a = result_1.IndexOf("-731");
                    result_1 = result_1.Substring(a + 1);
                    a = result_1.IndexOf("|731");
                    result_1 = result_1.Substring(a + 1);
                    result_1 = result_1.Replace("T31TM", "731TM");
                    result_1 = result_1.Replace("731THC", "731TMC");
                    result_1 = result_1.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace("\\", "").Replace("|", "").Replace(@"\", "");
                    result_1 = result_1.Replace("7731TMC", "731TMC");
                    result_1 = result_1.Replace("731TMCO", "731TMC6").Replace("-I", "-1");
                    result_1 = result_1.Replace("-S-", "-5-");
                    result_1 = result_1.Replace("G.22", "G:22");

                    result_1 = CleanAndReplaceResult(result_1, replaceNames1);

                    if (isOCR1 && result_1 == string.Empty)
                    {
                        result_1 = Properties.Settings.Default.keyCAM1;
                        isOCR1 = false;
                    }

                    richTextBox1.Invoke(new Action(() =>
                    {
                        this.richTextBox1.Text = string.Empty;
                        this.richTextBox1.Text = result_1.Trim();

                    }));
                    // Image 02
                    int lb = result_1.IndexOf(Properties.Settings.Default.keyCAM1);
                    if (result_1 != string.Empty && lb != -1 )
                    {
                        // OCR 2
                        result_2 = string.Empty;
                        ocrResult2 = null;

                        using(Bitmap image = new Bitmap(bmp2))
                        {
                            ocrResult2 = GetOcrResultBitmap(image, SelectedLang).Result;
                        }

                        result_2 = ocrResult2.Text;
                        result_2 = CleanAndReplaceText(result_2);
                        result_2 = CleanAndReplaceResult(result_2, replaceNames2);

                        richTextBox2.Invoke(new Action(() =>
                        {
                            this.richTextBox2.Text = string.Empty;
                            this.richTextBox2.Text = result_2.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
                        }));


                        ResultType result = CompareData(result_1, result_2);

                        if (result == ResultType.OK || result == ResultType.NG)
                        {
                            if(typeSelected == TypeAction.Manual)
                            {
                                RandersTableHistory();
                            }
                            isStateReset = false;
                        }
                    }

                    stopwatch.Stop();
                    Invoke(new Action(() =>
                    {
                        toolStripStatusTime.Text = "Load " + stopwatch.ElapsedMilliseconds.ToString() + "ms";
                        toolStripStatusLabelError.ForeColor = Color.Green;
                    }));
                }
                IsCapture = false;
                #endregion
            }
            catch (Exception ex)
            {
                HandleExceptionTest(ex);
                IsCapture = false;
            }
        }

        private void ProcessImage02OCR()
        {
            result_2 = string.Empty;
            ocrResult2 = GetOcrResultBitmap((Bitmap)scrollablePictureBoxCamera02.Image.Clone(), SelectedLang).Result;

            result_2 = CleanAndReplaceText(result_2);
            result_2 = CleanAndReplaceResult(result_2, replaceNames2);

            richTextBox2.Invoke(new Action(() =>
            {
                richTextBox2.Text = result_2.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
            }));
        }

        private void UpdateUIPostProcess()
        {
            stopwatch.Stop();
            Invoke(new Action(() =>
            {
                toolStripStatusTime.Text = "Load " + stopwatch.ElapsedMilliseconds.ToString() + "ms";
                toolStripStatusLabelError.ForeColor = Color.Green;
            }));
        }

        private void HandleExceptionTest(Exception ex)
        {
            Invoke(new Action(() =>
            {
                toolStripStatusLabelError.Text = ex.Message;
                toolStripStatusLabelError.ForeColor = Color.Red;
                LogWriter.SaveLog("Error Test :" + ex.Message);
            }));
        }
        private void InitializeProcess()
        {
            if (isStarted)
            {
                capture_2.setFocus((int)nFocus.Value);
                isStarted = false;
            }

            IsChangeSelectedMode = true;
            stopwatch.Reset();
            ToggleDetectionStatus();
        }

        private void ProcessImage01OCR()
        {
            if (useQrCode)
            {
                result_1 = QrCode.DecodeQRCode(scrollablePictureBoxCamera01.Image);
            }
            else
            {
                imageList?.Clear();
                imageList.Add((System.Drawing.Image)scrollablePictureBoxCamera01.Image.Clone());
                result_1 = performOCR(imageList, inputfilename, imageIndex, Rectangle.Empty).Result;
            }

            result_1 = CleanImage01OCRResult(result_1);
            result_1 = CleanAndReplaceResult(result_1, replaceNames1);

            UpdateRichTextBox1(result_1.Trim());
        }
        private bool IsResult01Valid()
        {
            int lb = result_1.IndexOf(Properties.Settings.Default.keyCAM1);
            return result_1 != string.Empty && lb != -1;
        }

        private string CleanImage01OCRResult(string text)
        {
            var adjustments = new List<(string, string)>
            {
                ("-731", ""),
                ("|731", ""),
                ("T31TM", "731TM"),
                ("731THC", "731TMC"),
                ("7731TMC", "731TMC"),
                ("731TMCO", "731TMC6"),
                ("-I", "-1"),
                ("-S-", "-5-"),
                ("G.22", "G:22"),
                // Add more if needed...
            };

            foreach (var (find, replace) in adjustments)
            {
                text = text.Replace(find, replace);
            }

            return text.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace("\\", "").Replace("|", "").Replace(@"\", "");
        }

        private void UpdateRichTextBox1(string text)
        {
            richTextBox1.Invoke(new Action(() =>
            {
                richTextBox1.Text = text;
            }));
        }


        private bool PreProcessChecks()
        {
            return capture_1._isRunning && capture_2._isRunning && bitmapCamera_01 != null && bitmapCamera_02 != null && isStateReset &&
            scrollablePictureBoxCamera01.Image != null && scrollablePictureBoxCamera02.Image != null;
        }

        private string CleanAndReplaceResult(string result, List<ReplaceName> replaceNames)
        {
            // Perform replacements
            result = result.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace("\\", "").Replace("|", "").Replace(@"\", "");

            foreach (var item in replaceNames)
            {
                result = result.Replace(item.oldName, item.newName);
            }

            return result;
        }

        private string CleanAndReplaceText(string input)
        {
            // Initial cleanup: Remove white spaces, newlines, carriage returns and tabs
            input = input.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
            // Retain only alphanumerics and some symbols
            input = Regex.Replace(input, "[^a-zA-Z0-9(),:,-]", "");

            // Special replacements
            input = input.Replace("91J7", "9U7");
            input = input.Replace("-OO", "-00");
            input = input.Replace(")9U7", "9U7");

            // Define dictionary for pattern replacements
            var replacements = new Dictionary<string, string>()
            {
                {"731OTM", "7310TM"},
                {"3O1731", "3-01731"},
                {"4O1731", "4-01731"},
                {"5O1731", "5-01731"},
                {"6O3731", "6-03731"},
                {"2O1731", "2-01731"},
                {"7O1731", "7-01731"},
                {"5O2731", "5-02731"},
                {"4O4731", "4-04731"},
                {"8OO731", "8-00731"},
                {"8OA731", "8-0A731"},
                {"9OA731", "9-0A731"},
                {"6O1731", "6-01731"},
                {"7OA731", "7-0A731"},
                {"7OO731", "7-00731"},
                {"2OA731", "2-0A731"},
            };

            // Perform pattern replacements
            foreach (var pair in replacements)
            {
                input = input.Replace(pair.Key, pair.Value);
                input = input.Replace(pair.Key.Insert(1, "-"), pair.Value); // insert "-" after first character
            }

            return input;
        }
        private bool isDetectionRunning = false;
        private void ToggleDetectionStatus()
        {
            isDetectionRunning = !isDetectionRunning;
            string statusText = isDetectionRunning ? "Detecting..." : "Wait for detect..";
            Invoke((Action)(() => lbTitle.Text = statusText));
        }

        private string PrepareLogMessage(string txt)
        {
            return txt.Replace("\r", "").Replace("\n", "");
        }

        private void UpdateUIAndInvoke(string status, Color color)
        {
            Invoke(new Action(() =>
            {
                lbTitle.Text = status;
                lbTitle.ForeColor = Color.Black;
                lbTitle.BackColor = color;
            }));

            is_Blink_NG = status == "NG";

        }
        private string updateVoltageB = "";
        private string updateAmpB = "";
        private string updateVoltageV = "";
        private string updateAmpV = "";

        private ResultType CompareData(string txt_sw, string txt_lb)
        {
            LogWriter.SaveLog($"TXT Read : {PrepareLogMessage(txt_sw)}, {PrepareLogMessage(txt_lb)}");
            LogWriter.SaveLog($"New History OBJ");
            var history = new History();

            if (txt_sw.IndexOf(Properties.Settings.Default.keyCAM1) == -1 || txt_lb.IndexOf(Properties.Settings.Default.keyCAM2) == -1)
                return ResultType.Error;

            LogWriter.SaveLog($"Qury master data");
            var master_lb = MasterAll.GetMasterALLByLBName(txt_lb.Substring(0, txt_lb.IndexOf(Properties.Settings.Default.keyCAM2)).Replace("O", "0"));

            string description = "";
            string color_error = "";

            /*
             *  Colors name
             */
            Heller.AverageColor rgb;
            LogWriter.SaveLog($"GetColor RGB");
            using (var bm = new Bitmap(bmp2_color))
            {
                rgb = Heller.GetAverageColor(bm);
            }

            LogWriter.SaveLog($"Check RGB");
            string[] colorName = _colorName.Name(_colorName.RgbToHex(rgb.R, rgb.G, rgb.B));
            description += currentVoltageB + "," + currentAmpB+"," + currentVoltageV +"," + currentAmpV+",";

            updateAmpB = currentAmpB;
            updateVoltageB = currentVoltageB;
            updateAmpV = currentAmpV;
            updateVoltageV = currentVoltageV;

            LogWriter.SaveLog($"Color name :{colorName[3]},{colorName[1]},{colorName[2]} ,{colorName[0]}, R{rgb.R} G{rgb.G} B{rgb.B}");
            description += $"Color name :{colorName[3]},{colorName[1]},{colorName[2]} ,{colorName[0]}, R{rgb.R} G{rgb.G} B{rgb.B}";

            string color = colorName[3];
            LogWriter.SaveLog($"TXT Compaare : {PrepareLogMessage(txt_sw)}, {PrepareLogMessage(txt_lb)}");
            foreach (var item in master_lb)
            {
                history.master_sw = item.nameSW;
                history.master_lb = item.nameModel;

                if (item.nameSW.Contains(txt_sw) && color.Equals(item.color_name, StringComparison.OrdinalIgnoreCase))
                {
                    history.master_sw = item.nameSW;
                    history.master_lb = item.nameModel;

                    if(IsMinMax(4.0,5.0,voltageV) &&  IsMinMax(450.0,850.0,ampB)){
                        if(typeSelected == TypeAction.Manual){
                            UpdateUIAndInvoke("OK", Color.Green);
                            // 02 42 49 50 32 30 03
                            int pin = 50;
                            byte value = ((byte)pin);
                            templateData["Command_io"][0] = 0x02;
                            templateData["Command_io"][1] = 0x43;
                            templateData["Command_io"][2] = 0X49;
                            templateData["Command_io"][3] = 0x50;
                            templateData["Command_io"][4] = value;
                            templateData["Command_io"][6] = (byte)0x01;
                            string hex = string.Empty;
                            foreach (var b in templateData["Command_io"])
                            {
                                hex += b.ToString("X2") + " ";
                            }
                            LogWriter.SaveLog($"Command : {hex}");
                            // Send parameter
                            serialPortIO.SerialCommand(templateData["Command_io"]);
                        }
                        else
                        {
                            result_auto_test = ResultType.OK;
                        }             

                        history.name = txtEmployee.Text.Trim();
                        history.name_lb = txt_lb;
                        history.name_sw = txt_sw;
                        history.result = "OK";
                        history.color = item.color_name + " - " + color;
                        history.description = description;
                        history.Save();
                        isStateReset = false;
                        return ResultType.OK;
                    }else{
                        if(!IsMinMax(4.0,5.0,voltageV) && IsMinMax(450.0,850.0,ampB)){
                            description += $"Voltage NG : {voltageV}, Min : 4.0, Max : 5.0";
                        }else if(!IsMinMax(450.0,850.0,ampB) && IsMinMax(4.0,5.0,voltageV) ){
                            description += $"Amp NG : {ampB}, Min : 450.0, Max : 850.0";
                        }else{  
                            description += $"Voltage NG : {voltageV}, Min : 4.0, Max : 5.0";
                            description += $"Amp NG : {ampB}, Min : 450.0, Max : 850.0";
                        }
                    }
                }
                else if (item.nameSW == txt_sw)
                {
                    history.name_lb = txt_lb;
                    history.name_sw = txt_sw;
                    description += "Found in " + item.nameModel + " - " + item.nameSW + "";
                    if (color.Equals(item.color_name, StringComparison.OrdinalIgnoreCase))
                    {
                        description += " - Color OK";
                        color_error = item.color_name + " - " + color + " OK";
                    }
                    else
                    {
                        description += " - Color NG";
                        string col = item.color_name == string.Empty ? "No color" : item.color_name;
                        color_error = col + " - " + color + " NG";
                    }

                    if(IsMinMax(4.0,5.0,voltageV) &&  IsMinMax(450.0,850.0,ampB))
                    {
                        description += $"Voltage and Amp are OK";
                    }else
                    {
                        if (!IsMinMax(4.0, 5.0, voltageV) && IsMinMax(450.0, 850.0, ampB))
                        {
                            description += $"Voltage NG : {voltageV}, Min : 4.0, Max : 5.0";
                        }
                        else if (!IsMinMax(450.0, 850.0, ampB) && IsMinMax(4.0, 5.0, voltageV))
                        {
                            description += $"Amp NG : {ampB}, Min : 450.0, Max : 850.0";
                        }
                        else
                        {
                            description += $"Voltage NG : {voltageV}, Min : 4.0, Max : 5.0";
                            description += $"Amp NG : {ampB}, Min : 450.0, Max : 850.0";
                        }
                    }
                    break;
                }
            }
            richTextBox2.Invoke(new Action(() =>
            {
                this.richTextBox2.Text += $"\n Color name :{colorName[3]},{colorName[1]},{colorName[2]} ,{colorName[0]}, R{rgb.R} G{rgb.G} B{rgb.B}";
                this.richTextBox2.Text += description;
            }));
            if(typeSelected == TypeAction.Manual){
                UpdateUIAndInvoke("NG", Color.Red);
            }

            history.name = txtEmployee.Text.Trim();
            history.name_lb = txt_lb;
            history.name_sw = txt_sw;
            history.description = description;
            history.color = color_error;
            history.result = "NG";
            history.Save();

            isStateReset = false;
            return ResultType.NG;
        }

        private bool IsMinMax(dynamic min,dynamic max,dynamic value)
        {
            if (value >= min && value <= max)
            {
                return true;
            }
            return false;
        }
    }
}
