using SC_M4.Forms.Show;
using SC_M4.IO;
using SC_M4.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Markup;

namespace SC_M4
{
    public partial class Main
    {
        private TypeAction typeSelected = TypeAction.Manual;
        private Dictionary<string, byte[]> templateData;
        private SerialPortIO serialPortIO;
        private void InitializeSerial()
        {
            serialPortIO = new SerialPortIO();
            serialPortIO.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);
            // Default byte data
            templateData = new Dictionary<string, byte[]>();
            templateData.Add("Query_Mode", new byte[8] { 0x02, 0x51, 0x4D, 0x00, 0x00, 0x00, 0x00, 0x03 });
            templateData.Add("Query_Status", new byte[8] { 0x02, 0x51, 0x53, 0x00, 0x00, 0x00, 0x00, 0x03 });
            templateData.Add("Command", new byte[8] { 0x02, 0x43, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03 });
            templateData.Add("Command_io", new byte[8] { 0x02, 0x43, 0x49, 0x00, 0x00, 0x00, 0x00, 0x03 });
            templateData.Add("Command_MES", new byte[8] { 0x02, 0x43, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03 });
        }
        #region Serial Port 

        private void SerialConnect(string portName, int baud)
        {
            CloseSerialPortIfExists();
            ConfigureSerialPort(portName, baud);
            AttemptSerialConnection();
        }
        private void ConfigureSerialPort(string portName, int baud)
        {
            this.serialPortIO.PortName = portName;
            this.serialPortIO.BaudRate = baud;
        }

        private void CloseSerialPortIfExists()
        {
            if (this.serialPortIO != null && this.serialPortIO.IsOpen)
            {
                this.serialPortIO.Close();
            }
        }
        private void AttemptSerialConnection()
        {
            try
            {
                this.serialPortIO.Open();
                serialPortIO.SerialCommand(templateData["Query_Mode"]);
                UpdateConnectionStatus("Serial Connected", Color.Green);
            }
            catch (Exception ex)
            {
                LogWriter.SaveLog("Error :" + ex.Message);
                UpdateConnectionStatus("Serial Port: Disconnect", Color.Red);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateConnectionStatus(string text, Color color)
        {
            this.toolStripStatusConnectSerialPort.Text = text;
            this.toolStripStatusConnectSerialPort.ForeColor = color;
        }

        private delegate void UpdateDataReceived(byte[] data);
        private List<byte> _dataBuffer = new List<byte>();

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] bytes = new byte[this.serialPortIO.BytesToRead];
                this.serialPortIO.Read(bytes, 0, bytes.Length);
                _dataBuffer.AddRange(bytes);

                Console.WriteLine("Receive : ");
                string receive = string.Empty;
                foreach (var d in _dataBuffer)
                {
                    // Print to hex
                    Console.Write(d.ToString("X2") + ", ");
                    receive += d.ToString("X2") +", ";
                }
                LogWriter.SaveLog("Receive : "+ receive);
                Console.WriteLine(" ");
                ProcessDataBuffer();

            }
            catch (Exception ex)
            {
                LogWriter.SaveLog("Error :" + ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessDataBuffer()
        {
            // Loop until we no longer have both STX and EOT in the buffer
            while (_dataBuffer.Contains((byte)0x02) && _dataBuffer.Contains((byte)0x03))
            {
                int indexSTX = _dataBuffer.IndexOf((byte)0x02);
                int indexEOT = _dataBuffer.IndexOf((byte)0x03);

                // Check index of STX and EOT
                if (indexSTX < indexEOT)
                {
                    // Extract message
                    var message = _dataBuffer.GetRange(indexSTX, indexEOT - indexSTX + 1).ToArray();
                    //DecodeDataReceived(message);

                    this.Invoke(new UpdateDataReceived(DataReceived), message);
                    // Remove processed data from buffer
                    _dataBuffer.RemoveRange(0, indexEOT + 1);
                }
                else
                {
                    // Remove corrupted data (if EOT appears before STX)
                    _dataBuffer.RemoveRange(0, indexSTX + 1);
                }
            }
        }


        private void DataReceived(byte[] data)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDataReceived(DataReceived), data);
                return;
            }

            // Find data is STX and EOT 
            if (data.Any<byte>(d => d == 0x02) && data.Any<byte>(d => d == 0x03))
            {
                // Find index of STX and EOT
                int indexSTX = Array.IndexOf(data, (byte)0x02);
                int indexEOT = Array.IndexOf(data, (byte)0x03);
                // Check index of STX and EOT
                if (indexSTX >= 0 && indexEOT >= 0)
                {
                    // Check index of STX and EOT
                    if (indexSTX < indexEOT)
                    {
                        // Get data between STX and EOT
                        byte[] dataReceived = new byte[indexEOT - indexSTX + 1];
                        Array.Copy(data, indexSTX, dataReceived, 0, dataReceived.Length);
                        DecodeDataReceivedTile(dataReceived);
                    }
                }
            }
        }

        private void DecodeDataReceivedTile(byte[] dataReceived)
        {
            // Tile data received
            switch (dataReceived[1])
            {
                case 0x43:
                    // 0X43 is Command
                    DataReceivedCommand(dataReceived);
                    break;
                case 0x52:
                    // 0X52 is Response
                    DataReceivedResponseOrUpdate(dataReceived);
                    break;
                case 0x55:
                    // 0X55 is Update
                    DataReceivedResponseOrUpdate(dataReceived);
                    break;
            }


        }

        private void DataReceivedCommand(byte[] dataReceived)
        {
            switch (dataReceived[2])
            {
                case 0x49:
                    if (dataReceived[3] == 0x53 && dataReceived[6] == 0x00)
                    {
                        LogWriter.SaveLog("Command : START");

                        StartAutoTest();
                    }
                    else if (dataReceived[3] == 0x53 && dataReceived[6] != 0x00)
                    {
                        LogWriter.SaveLog("Command : STOP, " + dataReceived[6].ToString("X2"));
                        StopAutoTest();
                    }
                    break;
            }
        }
        // Subtitle
        private void DataReceivedResponseOrUpdate(byte[] dataReceived)
        {
            IsChangeSelectedMode = true;
            switch (dataReceived[2])
            {
                case 0x4D:
                    ModeSelector(dataReceived);
                    break;
                case 0x53:
                    ModeReset(dataReceived);
                    break;
                case 0x56: //V Voltage
                    VoltageUpdate(dataReceived);
                    break;
                case 0x43: //A Amp
                    AmpUpdate(dataReceived);
                    break;
            }
        }


        private void VoltageUpdate(byte[] dataReceived)
        {
            byte[] voltageByte = new byte[4] { dataReceived[3], dataReceived[4], dataReceived[5], dataReceived[6] };
            int voltage = BitConverter.ToInt32(voltageByte, 0);

            lbVoltage.Text = $"{voltage}V";
        }

        private void AmpUpdate(byte[] dataReceived)
        {
            byte[] ampByte = new byte[4] { dataReceived[3], dataReceived[4], dataReceived[5], dataReceived[6] };
            int amp = BitConverter.ToInt32(ampByte, 0);

            lbAmp.Text = $"{amp}A";
        }
        private ManualTest manualTest;
        private void ModeSelector(byte[] dataReceived)
        {
            // Check data is Mode Auto or not 
            if (dataReceived[6] == 0x40)
            {
                typeSelected = TypeAction.None;
                stopwatchManualTest.Stop();
                manualTest?.Close();
                lbTitle.BackColor = Color.Gray;
            }
            else if (dataReceived[6] == 0x41)
            {
                typeSelected = TypeAction.Auto;
                stopwatchManualTest.Stop();
                manualTest?.Close();
                lbTitle.BackColor = Color.Orange;
                RandersTableHistoryAuto();
            }
            // Check data is Mode Manual
            else if (dataReceived[6] == 0x42)
            {
                if (stopwatchManualTest == null)
                {
                    stopwatchManualTest = new Stopwatch();
                }
                lbTitle.BackColor = Color.Yellow;
                RandersTableHistory();

                stopwatchManualTest.Restart();
                manualTest?.Close();
                manualTest = new ManualTest();
                manualTest.Show();
                typeSelected = TypeAction.Manual;
            }
            IsCapture = false;
            // Set title 
            lbTitle.Text = "Mode : " + (typeSelected == TypeAction.Auto ? "Auto" : typeSelected == TypeAction.Manual ? "Manual" : "None");
            Console.WriteLine(" Mode : " + (typeSelected == TypeAction.Auto ? "Auto" : typeSelected == TypeAction.Manual ? "Manual" : "None"));

        }

        private void ModeReset(byte[] dataReceived)
        {
            // Check data is reset
            if(dataReceived[3] == 0x52)
            {
                isStateReset = true;
                is_Blink_NG = false;
                if (dataReceived[6] == 0x02)
                {
                    lbTitle.Text = "Ready....";
                }else if (dataReceived[6] == 0x01)
                {
                    lbTitle.Text = "Empty!";
                }
                lbTitle.ForeColor = Color.Black;
                lbTitle.BackColor = (typeSelected == TypeAction.Auto ? Color.Orange : typeSelected == TypeAction.Manual ? Color.Yellow : Color.Gray);
                richTextBox1.Text = "";
                richTextBox2.Text = "";
            }
        }
        #endregion
    }
}
