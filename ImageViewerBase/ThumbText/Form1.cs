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
        int ii;
        int picBoxSize = 256;
        int ThumbSize = 256;
        List<string> photos = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                photos = openFileDialog1.FileNames.ToList();
                UpdateItems();
            }
        }

        #region PicBoxItems
        void UpdateItems()
        {
            flowLayoutPanel1.Controls.Clear();
            creatPicBoxes();
            upDataThrmMaps();
        }


        void creatPicBoxes()
        {
            PictureBox abox = null;
            for (int i = 0; i < photos.Count; i++)
            {
                string photoPath = photos[i];
                //如果已存在，则返回此实例
                if (flowLayoutPanel1.Controls.ContainsKey(photoPath))
                    abox = flowLayoutPanel1.Controls[photoPath] as PictureBox;
                else
                {
                    abox = new PictureBox()
                    {
                        BackColor = SystemColors.ScrollBar,
                        BackgroundImageLayout = ImageLayout.Zoom,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Margin = new Padding(2),
                        Name = photoPath,
                        Size = new Size(picBoxSize, picBoxSize)
                    };
                    abox.MouseClick += Abox_MouseClick;
                    abox.MouseDoubleClick += Abox_MouseDoubleClick;
                }
                flowLayoutPanel1.Controls.Add(abox);
            }
        }

        private void Abox_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void Abox_MouseClick(object sender, MouseEventArgs e)
        {

        }

        #endregion


        #region ThumbMap
        Thread getThumbMap_SubThread;
        void upDataThrmMaps()
        {
            if (getThumbMap_SubThread == null || getThumbMap_SubThread.ThreadState == ThreadState.Stopped)
            {
                getThumbMap_SubThread = new Thread(new ThreadStart(() => getThumbMapFromImage(callback_setToPic)));
                getThumbMap_SubThread.Start();
            }
            else
                MessageBox.Show("正在提取缩略图");
        }
        void getThumbMapFromImage(Func<string, string, bool> setToPic)
        {
            for (ii = 0; ii < photos.Count; ii++)
            {
                int i = ii;//界面点击会改变此值
                string imgPaht = photos[i];
                string thumbPath = getThumbPath(imgPaht, "hh");
                //如果thumbPath不存在则创建缩略图
                if (!File.Exists(thumbPath))
                {
                    Image aMap = Image.FromFile(imgPaht);
                    //缩略图尺寸
                    int thuWidth = aMap.Width > aMap.Height ? ThumbSize : ThumbSize * aMap.Height / aMap.Width;
                    int thuHieght = aMap.Width > aMap.Height ? ThumbSize * aMap.Height / aMap.Width : ThumbSize;
                    //创建缩略图
                    Image thumbMap = aMap.GetThumbnailImage(thuWidth, thuHieght, null, new IntPtr());
                    //创建文件夹，保存缩略图
                    if (Directory.Exists(Path.GetDirectoryName(thumbPath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(thumbPath));
                    try { thumbMap.Save(thumbPath, ImageFormat.Jpeg); } catch (Exception ex) { MessageBox.Show(ex.Message); };
                    aMap.Dispose();
                    thumbMap.Dispose();
                }
                //回调返回false,说明主线程断了，则break掉此线程的循环
                if (!setToPic(imgPaht, thumbPath)) break;
            }
        }
        bool callback_setToPic(string imgPath, string ThumbPath)
        {
            bool res = false;
            try
            {
                (flowLayoutPanel1.Controls[imgPath] as PictureBox).Image = Image.FromFile(ThumbPath);
                res = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return res;
        }
        string getThumbPath(string imgPaht, string ProName)
        {
            string imgName = Path.GetFileNameWithoutExtension(imgPaht) + ".jpg";
            string dir = Path.GetFileName(Path.GetDirectoryName(imgPaht));
            string appPath = Application.LocalUserAppDataPath;
            string res = $"{appPath}\\Thumbnail\\{ProName}\\{dir}\\{imgName}";
            return res;
        }
        #endregion
    }
}
