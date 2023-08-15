using SC_M4.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SC_M4
{
    public partial class Main
    {
        private TypeAction typeSelected = TypeAction.Manual;
        private Dictionary<string, byte[]> templateData;
        private void InitializeSerial()
        {
            // Default byte data
            templateData = new Dictionary<string, byte[]>();
            templateData.Add("Query_Mode", new byte[8] { 0x02, 0x51, 0x4D, 0x00, 0x00, 0x00, 0x00, 0x03 });
            templateData.Add("Query_Status", new byte[8] { 0x02, 0x51, 0x53, 0x00, 0x00, 0x00, 0x00, 0x03 });
            templateData.Add("Command", new byte[8] { 0x02, 0x43, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03 });
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
            this.serialPort.PortName = portName;
            this.serialPort.BaudRate = baud;
        }

        public void SerialCommand(string command)
        {
            SerialCommand(Encoding.ASCII.GetBytes(">" + command + "<#"));
            LogWriter.SaveLog("Serial send : " + command);
            toolStripStatusSerialData.Text = "DATA :" + command;
        }

        public void SerialCommand(byte[] bytes) => WriteToSerialPort(bytes);

        public void SerialCommand(List<byte> bytes) => WriteToSerialPort(bytes.ToArray());

        private void CloseSerialPortIfExists()
        {
            if (this.serialPort != null && this.serialPort.IsOpen)
            {
                this.serialPort.Close();
            }
        }
        private void AttemptSerialConnection()
        {
            try
            {
                this.serialPort.Open();
                SerialCommand(templateData["Query_Mode"]);
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


        private void WriteToSerialPort(byte[] bytes)
        {
            if (this.serialPort.IsOpen)
            {
                this.serialPort.Write(bytes, 0, bytes.Length);
            }
        }
        private delegate void UpdateDataReceived(byte[] data);

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // Read data from serial port (byte[])
                byte[] bytes = new byte[this.serialPort.BytesToRead];
                this.serialPort.Read(bytes, 0, bytes.Length);
                // Convert byte[] to string

                this.Invoke(new UpdateDataReceived(DataReceived), bytes);
            }
            catch (Exception ex)
            {
                LogWriter.SaveLog("Error :" + ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        DecodeDataReceived(dataReceived);
                    }
                }
            }
        }

        private void DecodeDataReceived(byte[] dataReceived)
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
                    DataReceivedResponse(dataReceived);
                    break;
                case 0x55:
                    // 0X55 is Update
                    DataReceivedResponse(dataReceived);
                    break;
            }

            foreach (var d in dataReceived)
            {
                // Print to hex
                Console.Write(d.ToString("X2") + ", ");
            }
        }

        private void DataReceivedCommand(byte[] dataReceived)
        {
            switch(dataReceived[2]){
                case 0x49:
                    if(dataReceived[3] == 0x53 && dataReceived[6] == 0x00){
                        Console.WriteLine("Command : IS - IO START");
                        StartAutoTest();
                    }else if(dataReceived[3] == 0x53 && dataReceived[6] != 0x00){
                        Console.WriteLine("Command : IS - IO STOP, " + dataReceived[6].ToString("X2"));
                        StopAutoTest();
                    }
                break;
            }
        }

        private void DataReceivedResponse(byte[] dataReceived)
        {
            // Check data is Mode 0x52 or 0x55 is Response and Update Mode
            if (dataReceived[2] == 0x4D)
            {
                // Check data is Mode Auto 
                if (dataReceived[6] == 0x40)
                {
                    typeSelected = TypeAction.None;
                }
                else if (dataReceived[6] == 0x41)
                {
                    typeSelected = TypeAction.Auto;

                }
                // Check data is Mode Manual
                else if (dataReceived[6] == 0x42)
                {
                    stopwatchManualTest.Reset();
                    typeSelected = TypeAction.Manual;
                }
                // Set title 
                lbTitle.Text = "Mode : " + (typeSelected == TypeAction.Auto ? "Auto" : typeSelected == TypeAction.Manual ? "Manual" : "None");
                Console.WriteLine(" Mode : " + (typeSelected == TypeAction.Auto ? "Auto" : typeSelected == TypeAction.Manual ? "Manual" : "None"));
                txtDebug.Text += " Mode : " + (typeSelected == TypeAction.Auto ? "Auto" : typeSelected == TypeAction.Manual ? "Manual" : "None") + "\r\n";
            }

        }
        #endregion
    }
}
