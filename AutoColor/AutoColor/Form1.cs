using OSGeo.GDAL;
using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
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
            bitmapTest();
        }
        
        void bitmapTest()
        {
            Bitmap bMap = new Bitmap(@"E:\test\DSC00661.JPG");
            BitmapData bmpData = bMap.LockBits(new Rectangle(Point.Empty, bMap.Size), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //计算 rgb 数组大小
            byte[] buf  = new byte[bmpData.Height * bmpData.Stride];
            //将位图的数据拷贝到byColorInfoSrc中 rgb数据
            Marshal.Copy(bmpData.Scan0, buf, 0, buf.Length);
       
          // do something...

            //操作完之后 数据拷贝回去
            Marshal.Copy(buf, 0, bmpData.Scan0, buf.Length);
            bMap.UnlockBits(bmpData);//解除锁定
          
        }

        void sejie()
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

//https://stackoverflow.com/questions/4747428/getting-rgb-array-from-image-in-c-sharp 读写图像
//https://bbs.csdn.net/topics/391862868