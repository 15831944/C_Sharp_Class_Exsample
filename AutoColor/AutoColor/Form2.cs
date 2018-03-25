using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoColor
{
    public partial class Form2 : Form
    {
        List<int[]> acters;
        List<Point> minMax;
        int fa = 6000;
        List<Pen> pens = new List<Pen>() { new Pen(Color.Blue), new Pen(Color.Green), new Pen(Color.Red), new Pen(Color.Black) };
        public Form2()
        {
            InitializeComponent();
        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                Bitmap bMap = new Bitmap(pictureBox1.Image);
                BitmapData bmpData = bMap.LockBits(new Rectangle(Point.Empty, bMap.Size), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                byte[] buf = new byte[bmpData.Height * bmpData.Stride];
                Marshal.Copy(bmpData.Scan0, buf, 0, buf.Length);

                //拉伸色阶
                //edit(ref buf);
                gamma(ref buf);
                getInfo(buf);
                Marshal.Copy(buf, 0, bmpData.Scan0, buf.Length);
                bMap.UnlockBits(bmpData);
                pictureBox2.Image = bMap;
                pictureBox3.Refresh();
                //string savePath = Path.Combine(
                //    Path.GetDirectoryName(openFileDialog1.FileName),
                //    Path.GetFileNameWithoutExtension(openFileDialog1.FileName) + "_Edit.jpg");
                //bMap.Save(savePath);
                //bMap.Dispose();
            }
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="buf"></param>
        void getInfo(byte[] buf)
        {
            acters = new List<int[]>() { new int[256], new int[256], new int[256] };
            for (int u = 0; u < buf.Length; u++)
            {
                if (u % 3 == 0) acters[0][buf[u]]++;
                else if (u % 3 == 1) acters[1][buf[u]]++;
                else if (u % 3 == 2) acters[2][buf[u]]++;
            }
        }
        /// <summary>
        /// 拉伸色阶
        /// </summary>
        /// <param name="buf"></param>
        void edit(ref byte[] buf)
        {
            minMax = new List<Point>();
            int filterCount = buf.Length / (fa * 3);
            for (int band = 0; band < acters.Count; band++)
            {
                int minValue = 0;
                for (; minValue < acters[band].Length; minValue++)
                    if (acters[band][minValue] > filterCount && acters[band][minValue] < acters[band][minValue + 1])
                        break;
                int maxValue = acters[band].Length - 1;
                for (; maxValue >= 0; maxValue--)
                    if (acters[band][maxValue] > filterCount && acters[band][maxValue] < acters[band][maxValue - 1])
                        break;
                minMax.Add(new Point(minValue, maxValue));
                float zo = 255f / (maxValue - minValue);
                for (int i = band; i < buf.Length; i += 3)
                {
                    int res = (int)Math.Round((buf[i] - minValue) * zo);
                    buf[i] = (byte)(res < 0 ? 0 : res > 255 ? 255 : res);
                }
            }
        }
        /// <summary>
        /// 对数增强
        /// </summary>
        /// <param name="buf"></param>
        void duishu(ref byte[] buf)
        {
            var c = 255 / Math.Log10(256);
            for (int i = 0; i < buf.Length; i++)
            {
                double kk = Math.Log10(buf[i] + 1) * c;
                int res = (int)kk;
                buf[i] = (byte)(res < 0 ? 0 : res > 255 ? 255 : res);
            }
        }
        /// <summary>
        /// 指数增强
        /// </summary>
        /// <param name="buf"></param>
        void zhishu(ref byte[] buf)
        {
            float c = 2f / 255;
            for (int i = 0; i < buf.Length; i++)
            {
                byte ori = buf[i];
                int res = (int)Math.Round(buf[i] * buf[i] * c);
                buf[i] = (byte)(res < 0 ? 0 : res > 255 ? 255 : res);
            }
        }
        /// <summary>
        /// gamma变换
        /// </summary>
        /// <param name="buf"></param>
        void gamma(ref byte[] buf)
        {
            for (int i = 0; i < buf.Length; i++)
            {
                byte ori = buf[i];
                int  res = (int)(Math.Pow(ori / 255f, 1) * 255);
                buf[i] = (byte)(res < 0 ? 0 : res > 255 ? 255 : res);
            }
        }
        private void Form2_Layout(object sender, LayoutEventArgs e)
        {
            splitContainer2.SplitterDistance = Width / 2;
        }

        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            if (acters == null) return;
            float zo = pictureBox3.Width / 256f;

            for (int i = 0; i < acters.Count; i++)
            {
                int[] ac = acters[i];
                float fo = pictureBox3.Height * 1f / ac.Max();
                for (int r = 0; r < ac.Length - 1; r++)
                {
                    e.Graphics.DrawLine(pens[i],
                        new PointF(r * zo, pictureBox3.Height - ac[r] * fo),
                        new PointF((r + 1) * zo, pictureBox3.Height - ac[r + 1] * fo));
                }
                if (minMax != null)
                {
                    e.Graphics.DrawLine(pens[i], minMax[i].X * zo, 0, minMax[i].X * zo, pictureBox3.Height);
                    e.Graphics.DrawLine(pens[i], minMax[i].Y * zo, 0, minMax[i].Y * zo, pictureBox3.Height);
                    e.Graphics.DrawString($"min:{minMax[i].X},max{minMax[i].Y}", new Font("宋体", 12), new SolidBrush(pens[i].Color), new PointF(10, 10 + i * 20));
                }
            }
            ///  va[r] / ac.max  = y / pic.width
            ///  y =  pic.w * ac[r] / ac.max   fo = pic.h /  ac.max 
            ///  y = ac[r] * fo
        }
    }
}
