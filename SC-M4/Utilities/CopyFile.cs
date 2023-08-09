using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_M4.Utilities
{
    public class CopyFileArguments
    {
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
    }

    public static class CopyFile
    {
        public static void CopyFileWithProgress(string sourcePath, string destinationPath, BackgroundWorker worker)
        {
            byte[] buffer = new byte[1024 * 1024]; // 1MB buffer
            int bytesRead;
            long totalBytesRead = 0;

            using (FileStream sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
                long fileSize = sourceStream.Length;

                using (FileStream destinationStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
                {
                    while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // Use Buffer.BlockCopy to ensure all bytes are copied
                        byte[] tempBuffer = new byte[bytesRead];
                        Buffer.BlockCopy(buffer, 0, tempBuffer, 0, bytesRead);
                        destinationStream.Write(tempBuffer, 0, bytesRead);

                        totalBytesRead += bytesRead;

                        int progressPercentage = (int)(((double)totalBytesRead / fileSize) * 100);
                        worker.ReportProgress(progressPercentage);
                    }
                }
            }
        }
    }
}
