using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageViewerBase
{
    public partial class Dex : Form
    {
        public Dex()
        {
            InitializeComponent();
            this.pictureEdit1.Image = Image.FromFile(@"E:\Automesh\Automesh v1.0\工程\测试数据\images\DSC00154.JPG");
          
        }
    }
}
