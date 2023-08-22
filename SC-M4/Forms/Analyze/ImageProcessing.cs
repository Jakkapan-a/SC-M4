using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SC_M4.Forms.Analyze
{
    public partial class ImageProcessing : Form
    {
        private Main main;
        private Bitmap result_diff;

        public ImageProcessing(Main main)
        {
            InitializeComponent();
            this.main = main;
        }

        private void ImageProcessing_Load(object sender, EventArgs e)
        {

        }

        public double CompareImages(Bitmap imgBitmap, Bitmap templateBitmap)
        {
            using (Mat img = OpenCvSharp.Extensions.BitmapConverter.ToMat(imgBitmap))
            using (Mat template = OpenCvSharp.Extensions.BitmapConverter.ToMat(templateBitmap))
            using (Mat result = new Mat())
            {
                Cv2.MatchTemplate(img, template, result, TemplateMatchModes.CCoeffNormed);
                Cv2.MinMaxLoc(result, out double minVal, out double maxVal, out _, out _);  // ใช้ discards สำหรับตัวแปรที่ไม่จำเป็น
                using (Mat diff = new Mat())
                {
                    Cv2.Absdiff(img, template, diff);
                    result_diff?.Dispose();
                    result_diff = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(diff);
                }
                return maxVal;
            }
        }

        private void cAM1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBoxMaster.Image?.Dispose();
            pictureBoxMaster.Image = (Image)main.bitmapCamera_01.Clone();
        }

        private void cAM2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBoxMaster.Image?.Dispose();
            pictureBoxMaster.Image = (Image)main.bitmapCamera_02.Clone();
        }

        private void cAM1ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pictureBoxCurrent.Image?.Dispose();
            pictureBoxCurrent.Image = (Image)main.bitmapCamera_01.Clone();
        }

        private void cAM2ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pictureBoxCurrent.Image?.Dispose();
            pictureBoxCurrent.Image = (Image)main.bitmapCamera_02.Clone();
        }

        private void compareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double result = CompareImages((Bitmap)pictureBoxCurrent.Image, (Bitmap)pictureBoxMaster.Image);
            txtOutput.AppendText($"Result :{result} \n");
        }

        private Show show;
        private void button1_Click(object sender, EventArgs e)
        {
            show?.Close();
            show = new Show(result_diff);
            show.Show();
        }
    }
}
