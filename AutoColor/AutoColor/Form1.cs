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
            int bandCount = ds.RasterCount;
            int xSize = ds.RasterXSize;
            int ySize = ds.RasterYSize;

            Band band = ds.GetRasterBand(1);
            DataType dt = band.DataType;
            int[] buf = new int[xSize * ySize];
            band.ReadRaster(0, 0, xSize, ySize, buf, xSize, ySize, 0, 0);
            int min = buf.Min();
            int max = buf.Max();
            double Average = buf.Average();
            Console.WriteLine("");
        }
        public  double myBiaoZhunC(double[] suzu)
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