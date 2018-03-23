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
        public Form2()
        {
            InitializeComponent();
        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap bMap = new Bitmap(openFileDialog1.FileName);
                BitmapData bmpData = bMap.LockBits(new Rectangle(Point.Empty, bMap.Size), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                byte[] buf = new byte[bmpData.Height * bmpData.Stride];
                Marshal.Copy(bmpData.Scan0, buf, 0, buf.Length);
                edit(ref buf);
                Marshal.Copy(buf, 0, bmpData.Scan0, buf.Length);
                bMap.UnlockBits(bmpData);
                bMap.Save(Path.Combine(Path.GetDirectoryName(openFileDialog1.FileName), Path.GetFileNameWithoutExtension(openFileDialog1.FileName), "_Edit.jpg"));
                bMap.Dispose();
            }
        }
        void edit(ref byte[] buf)//BGR
        {
            List<int[]> acters = new List<int[]>();
            int[] acB = new int[256];
            int[] acG = new int[256];
            int[] acR = new int[256];
            for (int u = 0; u < buf.Length; u++)
            {
                if (u % 3 == 0)//B
                    acB[buf[u]]++;
                else if (u % 3 == 1)//G
                    acG[buf[u]]++;
                else if (u % 3 == 2)//R
                    acR[buf[u]]++;
            }
            
            int filterCount = buf.Length / 6000;
            byte minValue = 0;
            byte maxValue = 255;

            for (int k = 0; k < acB.Length; k++)
            {
                if (acB[k] > filterCount && acB[k] < acB[k + 1])
                {
                    minValue = (byte)k;
                    break;
                }
            }
            for (int k = acB.Length - 1; k >= 0; k--)
            {
                if (acB[k] > filterCount && acB[k] < acB[k - 1])
                {
                    maxValue = (byte)k;
                    break;
                }
            }
            float zo = 255 * 1f / (maxValue - minValue);
            for (int i = 0; i < buf.Length; i += 3)
            {
                int res = (int)Math.Round((buf[i] - minValue) * zo);
                res = res < 0 ? 0 : res > 255 ? 255 : res;
                buf[i] = (byte)res;
            }
        }
    }
}
