using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TClass;

namespace SC_M4.IO
{
    public class SerialPortIO : SerialPort
    {
        private const byte STX = 0x02;
        private const byte EOT = 0x03;

        /// <summary>
        /// Sends a serial command using a string.
        /// </summary>
        /// <param name="command">The command string to send.</param>
        public void SerialCommand(string command)
        {
            if (string.IsNullOrEmpty(command)) throw new ArgumentNullException(nameof(command));

            byte[] checksum = Encoding.ASCII.GetBytes(command);

            List<byte> bytes = new List<byte> { STX };

            bytes.AddRange(checksum);
            bytes.Add(EOT);

            WriteToSerialPort(bytes.ToArray());
        }

        /// <summary>
        /// Sends a serial command using a byte array.
        /// </summary>
        /// <param name="bytes">The byte array to send.</param>
        public void SerialCommand(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) throw new ArgumentNullException(nameof(bytes));

            WriteToSerialPort(bytes);
        }

        /// <summary>
        /// Sends a serial command using a byte list.
        /// </summary>
        /// <param name="bytes">The byte list to send.</param>
        public void SerialCommand(List<byte> bytes)
        {
            if (bytes == null || bytes.Count == 0) throw new ArgumentNullException(nameof(bytes));

            WriteToSerialPort(bytes.ToArray());
        }

        /// <summary>
        /// Writes data to the serial port.
        /// </summary>
        /// <param name="bytes">The byte array to write to the port.</param>
        private void WriteToSerialPort(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) throw new ArgumentNullException(nameof(bytes));
            // Is the port open?
            if (this.IsOpen)
            {
                this.Close();
                this.Open();
                Console.Write("Send : ");
                foreach (var d in bytes)
                {
                    // Print to hex
                    Console.Write($"{d.ToString("X2")}, ");
                }
                Console.WriteLine("---------------------");
                this.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
