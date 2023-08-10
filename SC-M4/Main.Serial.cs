using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SC_M4
{
    public partial class Main
    {
        private byte[] bytes = new byte[4];
        private void InitializeSerial()
        {
            // Default byte data
            Array.Clear(bytes, 0, bytes.Length);
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
                byte[] bytesToSend = new byte[bytes.Length + 2];
                bytesToSend[0] = 0x02; // Start STX
                // 
                Array.Copy(bytes, 0, bytesToSend, 1, bytes.Length);
                bytesToSend[bytesToSend.Length - 1] = 0x03; // End EOT
                // Send
                this.serialPort.Write(bytesToSend, 0, bytesToSend.Length);
            }
        }
        
        private string readDataSerial = string.Empty;
        private string dataSerialReceived = string.Empty;

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //try
            //{
            //    readDataSerial = this.serialPort.ReadExisting();
            //    this.Invoke(new EventHandler(dataReceived));
            //}
            //catch (Exception ex)
            //{
            //    LogWriter.SaveLog("Error :" + ex.Message);
            //    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void dataReceived(object sender, EventArgs e)
        {
            //this.dataSerialReceived += readDataSerial;
            //if (dataSerialReceived.Contains(">") && dataSerialReceived.Contains("<"))
            //{
            //    string data = this.dataSerialReceived.Replace("\r", string.Empty).Replace("\n", string.Empty);
            //    data = data.Substring(data.IndexOf(">") + 1, data.IndexOf("<") - 1);
            //    this.dataSerialReceived = string.Empty;
            //    Console.WriteLine("RST : " + data);
            //    data = data.Replace(">", "").Replace("<", "");
            //    toolStripStatusSerialData.Text = "DATA :" + data;
            //    LogWriter.SaveLog("Serial Received : " + data);
            //    if (data == "rst" || data.Contains("rst"))
            //    {
            //        isStateReset = true;
            //        is_Blink_NG = false;
            //        if (capture_1.IsOpened && capture_1.IsOpened)
            //        {
            //            lbTitle.Text = "Wiat for detect...."; // Wiat for detect....
            //        }
            //        lbTitle.ForeColor = Color.Black;
            //        lbTitle.BackColor = Color.Yellow;
            //        richTextBox1.Text = "";
            //        richTextBox2.Text = "";
            //    }
            //}
            //else if (!dataSerialReceived.Contains(">"))
            //{
            //    this.dataSerialReceived = string.Empty;
            //}
        }

        #endregion
    }
}
