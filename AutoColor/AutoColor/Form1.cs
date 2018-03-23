using OSGeo.GDAL;
using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoColor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Gdal.AllRegister();
            Ogr.RegisterAll();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dataset ds = Gdal.Open(@"E:\test\DSC00661.JPG", Access.GA_ReadOnly);
            int xSize = ds.RasterXSize;
            int ySize = ds.RasterYSize;
            List<int[]> bufs = new List<int[]>();
            for (int band = 0; band < ds.RasterCount; band++)
            {
                int[] buf = new int[xSize * ySize];
                ds.GetRasterBand(band + 1).ReadRaster(0, 0, xSize, ySize, buf, xSize, ySize, 0, 0);
                bufs.Add(buf);
            }
            numbers(bufs);
            ds.Dispose();

        }

        void numbers(List<int[]> bufs)
        {
            List<int[]> acters = new List<int[]>();
            for (int band = 0; band < 3; band++)
            {
                int[] ac = new int[256];
                int[] buf = bufs[band];
                for (int valus = 0; valus < buf.Length; valus++)
                    ac[buf[valus]]++;

                int filterCount = buf.Length / 6000;
                int minValue = 0;
                int maxValue = 255;
                for (int k = 0; k < ac.Length; k++)
                {
                    if (ac[k] > filterCount && ac[k] < ac[k + 1])
                    {
                        minValue = k;
                        break;
                    }
                }
                for (int k = ac.Length - 1; k >= 0; k--)
                {
                    if (ac[k] > filterCount && ac[k] < ac[k - 1])
                    {
                        maxValue = k;
                        break;
                    }
                }
            }
        }




        public double myBiaoZhunC(int[] suzu)
        {
            double pingjunzhi = suzu.Average();
            double temp = 0;
            for (int i = 0; i < suzu.Length; i++)
            {
                temp += (suzu[i] - pingjunzhi) * (suzu[i] - pingjunzhi);
            }
            return Math.Sqrt(temp / suzu.Length);
        }

    }
}
// http://blog.csdn.net/u010771437/article/details/45371141