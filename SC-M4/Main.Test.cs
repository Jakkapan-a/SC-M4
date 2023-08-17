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
                if (capture_1._isRunning && capture_2._isRunning && bitmapCamera_01 != null && bitmapCamera_02 != null && isStateReset && scrollablePictureBoxCamera01.Image != null && scrollablePictureBoxCamera02.Image != null)
                {
                    if (isStarted)
                    {
                        capture_2.setFocus((int)nFocus.Value);
                        isStarted = false;
                    }
                    IsChangeSelectedMode = false;
                    stopwatch.Reset();
                    ToggleDetectionStatus();

                    // Image 01 OCR 
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
                    if (result_1 != string.Empty && lb != -1)
                    {
                        // OCR 2
                        result_2 = string.Empty;
                        ocrResult2 = null;
                        ocrResult2 = GetOcrResultBitmap((Bitmap)scrollablePictureBoxCamera02.Image.Clone(), SelectedLang).Result;

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
                            loadTableHistory();
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
            }
            catch (Exception ex)
            {
                Invoke(new Action(() =>
                {
                    // One or more errors occurred.
                    toolStripStatusLabelError.Text = ex.Message;
                    toolStripStatusLabelError.ForeColor = Color.Red;
                    LogWriter.SaveLog("Error Test :" + ex.Message);
                }));
            }
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

        private void UpdateUIAndInvokeCommand(string status, Color color)
        {
            Invoke(new Action(() =>
            {
                lbTitle.Text = status;
                lbTitle.ForeColor = Color.White;
                lbTitle.BackColor = color;
            }));

            is_Blink_NG = status == "NG";
            serialPortIO.SerialCommand(status);
        }

        private ResultType CompareData(string txt_sw, string txt_lb)
        {
            LogWriter.SaveLog($"TXT Read : {PrepareLogMessage(txt_sw)}, {PrepareLogMessage(txt_lb)}");

            history = history ?? new History();

            if (txt_sw.IndexOf(Properties.Settings.Default.keyCAM1) == -1 || txt_lb.IndexOf(Properties.Settings.Default.keyCAM2) == -1)
                return ResultType.Error;

            var master_lb = MasterAll.GetMasterALLByLBName(txt_lb.Substring(0, txt_lb.IndexOf(Properties.Settings.Default.keyCAM2)).Replace("O", "0"));

            history.master_sw = "null";
            history.master_lb = "null";
            string description = "";
            string color_error = "";

            /*
             *  Colors name
             */
            Heller.AverageColor rgb;
            //AverageColor _color = new AverageColor();
            using (var bm = (Bitmap)bmp2_color.Clone())
            {
                rgb = Heller.GetAverageColor(bm);
            }

            string[] colorName = _colorName.Name(_colorName.RgbToHex(rgb.R, rgb.G, rgb.B));
            Console.WriteLine("Color Name : " + colorName[3]);
            LogWriter.SaveLog($"Color name :{colorName[3]},{colorName[1]},{colorName[2]} ,{colorName[0]}, R{rgb.R} G{rgb.G} B{rgb.B}");
            description += $"Color name :{colorName[3]},{colorName[1]},{colorName[2]} ,{colorName[0]}, R{rgb.R} G{rgb.G} B{rgb.B}";

            string color = colorName[3];
            foreach (var item in master_lb)
            {
                history.master_sw = item.nameSW;
                history.master_lb = item.nameModel;

                if (item.nameSW == txt_sw && color.Equals(item.color_name, StringComparison.OrdinalIgnoreCase))
                {
                    history.master_sw = item.nameSW;
                    history.master_lb = item.nameModel;

                    UpdateUIAndInvokeCommand("OK", Color.Green);

                    history.name = txtEmployee.Text.Trim();
                    history.name_lb = txt_lb;
                    history.name_sw = txt_sw;
                    history.result = "OK";
                    history.color = item.color_name + " - " + color;
                    history.description = "-";
                    history.Save();

                    isStateReset = false;
                    return ResultType.OK;
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
                    break;

                }
            }
            richTextBox2.Invoke(new Action(() =>
            {
                this.richTextBox2.Text += $"\n Color name :{colorName[3]},{colorName[1]},{colorName[2]} ,{colorName[0]}, R{rgb.R} G{rgb.G} B{rgb.B}";
            }));
            UpdateUIAndInvokeCommand("NG", Color.Red);

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

        #region Old
        private int Compare_Master(string txt_sw, string txt_lb)
        {
            // 0 = not fount, 1 = OK, 2 = NG
            int result = 0;

            LogWriter.SaveLog("TXT Read :" + txt_sw.Replace("\r", "").Replace("\n", "") + ", " + txt_lb.Replace("\r", "").Replace("\n", ""));

            if (history == null)
            {
                history = new History();
            }

            int swa = txt_sw.IndexOf(Properties.Settings.Default.keyCAM1);
            // If not found, IndexOf returns -1.
            if (swa == -1)
            {
                result = 0;
                return result;
            }

            int lb = txt_lb.IndexOf(Properties.Settings.Default.keyCAM2);
            // If not found, IndexOf returns -1.
            if (lb == -1)
            {
                // Return the original string.
                result = 0;
                return result;
            }

            var txt = txt_lb.Substring(0, lb);
            txt = txt.Replace("O", "0");
            var master_lb = MasterAll.GetMasterALLByLBName(txt);

            bool check = false;

            if (master_lb.Count > 0)
            {
                foreach (var item in master_lb)
                {
                    history.master_sw = item.nameSW;
                    history.master_lb = item.nameModel;
                    if (item.nameSW == txt_sw)
                    {
                        check = true;
                        break;
                    }
                }
            }
            else
            {
                history.master_sw = "null";
                history.master_lb = "null";
            }

            if (!check)
            {
                Invoke(new Action(() =>
                {
                    lbTitle.Text = "NG";
                    lbTitle.ForeColor = Color.White;
                    lbTitle.BackColor = Color.Red;
                }));
                is_Blink_NG = true;
                serialPortIO.SerialCommand("NG");
                result = 1;
            }
            else
            {

                Invoke(new Action(() =>
                {
                    lbTitle.Text = "OK";
                    lbTitle.ForeColor = Color.White;
                    lbTitle.BackColor = Color.Green;
                    //loadTableHistory();
                }));
                result = 2;
                serialPortIO.SerialCommand("OK");
            }
            history.name = txtEmployee.Text.Trim();
            history.name_lb = txt_lb;
            history.name_sw = txt_sw;
            history.result = check ? "OK" : "NG";
            history.Save();
            LogWriter.SaveLog("Result :" + history.result);
            LogWriter.SaveLog("SW :" + txt_lb);
            LogWriter.SaveLog("LABEL :" + txt_lb);
            isStateReset = false;

            return result;
        }
        #endregion
    }
}
