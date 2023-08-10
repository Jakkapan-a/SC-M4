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
        private byte[] bytes = new byte[6];
        private TypeAction typeSelected = TypeAction.Manual;
        private Dictionary<string, byte[]> templateData;
        private void InitializeSerial()
        {
            // Default byte data
            Array.Clear(bytes, 0, bytes.Length);
            templateData = new Dictionary<string, byte[]>();
            templateData.Add("Query_Mode", new byte[8] { 0x02, 0x51, 0x4D, 0x00, 0x00, 0x00, 0x00, 0x03 });
            templateData.Add("Query_Status", new byte[8] { 0x02, 0x51, 0x53, 0x00, 0x00, 0x00, 0x00, 0x03 });
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
            if ((dataReceived[1] == 0x52 || dataReceived[1] == 0x55) && dataReceived[2] == 0x4D)
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
                    typeSelected = TypeAction.Manual;
                }

            }

            Console.WriteLine(" Mode : " + (typeSelected == TypeAction.Auto ? "Auto" : typeSelected == TypeAction.Manual ? "Manual" : "None"));
            foreach (var d in dataReceived)
            {
                // Print to hex
                Console.Write(d.ToString("X2") + ", ");
            }
        }
        #endregion
    }
}
