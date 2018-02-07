using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThumbText
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ThumbViewer tv = new ThumbViewer(Controls);

                tv.photos = openFileDialog1.FileNames.ToList();
                tv.picBoxSize = 128;
                tv.ThumbMapSize = 128;
                tv.TempDir = @"D:\temp";
                tv.proName = "test99";

                tv.upDataItems();
            }
        }
    }
}
