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
        ThumbViewer tv;
        public Form1()
        {
            InitializeComponent();
            toolStripButton1_Click(null, null);
        }

        //初始化
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tv = new ThumbViewer(Controls);
                tv.vb_ItemDoubleClick += Tv_MouseDoubleClick;
                tv.vb_Photos = openFileDialog1.FileNames.ToList();
                tv.vb_picBoxSize = 128;
                tv.vb_ThumbMapSize = 0;
                //tv.TempDir = @"C:\temp\aaa\";
                //tv.proName = "test99";
                tv.vb_TempDir = null;
                tv.vb_UpdataItems();
            }
        }


        private void Tv_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Text = tv.vb_Selected.First().Key;
        }
    }
}
