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

                edit(ref buf);

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
        /// 拉伸色阶
        /// </summary>
        /// <param name="buf"></param>
        void edit(ref byte[] buf)
        {
            //获取直方
            acters = new List<int[]>() { new int[256], new int[256], new int[256] };
            for (int u = 0; u < buf.Length; u += 3)
            {
                acters[0][buf[u]]++;
                acters[1][buf[u + 1]]++;
                acters[2][buf[u + 2]]++;
            }
            //获取截断区
            minMax = new List<Point>();
            int filterCount = buf.Length / (fa * 3);
            for (int band = 0; band < acters.Count; band++)
            {
                int[] ac = acters[band];
                int miv = 0, mav = 255;
                for (; miv <= 255; miv++)
                    if (ac[miv] > filterCount && ac[miv] < ac[miv + 1] && ac[miv + 1] < ac[miv + 2])
                        break;
                for (; mav >= 0; mav--)
                    if (ac[mav] > filterCount && ac[mav] < ac[mav - 1] && ac[mav - 1] < ac[mav - 2])
                        break;
                minMax.Add(new Point(miv, mav));
            }

            //保护机制，触发后不对源数据做任何处理，保护域10/255
            if (minMax.Exists(p => (p.X < 10 && p.Y > 254)  //截断区与启始区过近，表示原图质量OK，不需要做处理
                      || p.X >= 254 || p.Y <= 10)) return;  //截断区过大，保留区不足以用于拉伸，表示原图质量过差，没有处理的意义

            //分波段拉伸
            for (int band = 0; band < minMax.Count; band++)
            {
                float zo = 255f / (minMax[band].Y - minMax[band].X);
                for (int i = band; i < buf.Length; i += 3)
                {
                    int res = (int)Math.Round((buf[i] - minMax[band].X) * zo);
                    buf[i] = (byte)(res < 0 ? 0 : res > 255 ? 255 : res);
                }
            }

            //gamma变换
            List<double> gammas = new List<double>() { 0, 0, 0 };
            for (int i = 0; i < buf.Length; i += 3)
            {
                gammas[0] += buf[i];
                gammas[1] += buf[i + 1];
                gammas[2] += buf[i + 2];
            }
            for (int i = 0; i < gammas.Count; i++)
                gammas[i] = (gammas[i] / (buf.Length / 3)) / 100;
            this.Text = $"gammas: B:{gammas[0]:f3},G:{gammas[1]:n3},R:{gammas[2]:f3}";
            for (int i = 0; i < buf.Length; i++)
            {
                double gam = gammas[i % 3];
                buf[i] = (byte)(int)(Math.Pow(buf[i] / 255f, gam) * 255);
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
            float c = 1f / 255;
            for (int i = 0; i < buf.Length; i++)
            {
                byte ori = buf[i];
                int res = (int)Math.Round(buf[i] * buf[i] * c);
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
        /// <summary>
        /// 最小二乘法
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        PointF zx2cf(List<PointF> points)
        {
            float x2 = 0, y = 0, x = 0, xy = 0, a = 0, b = 0;
            for (int i = 0; i < points.Count; i++)
            {
                x2 += points[i].X * points[i].X;
                y += points[i].Y;
                x += points[i].X;
                xy += points[i].X * points[i].Y;
            }
            a = (points.Count * xy - x * y) / (points.Count * x2 - x * x);
            b = (x2 * y - x * xy) / (points.Count * x2 - x * x);
            return new PointF(a, b);
        }
    }
}
