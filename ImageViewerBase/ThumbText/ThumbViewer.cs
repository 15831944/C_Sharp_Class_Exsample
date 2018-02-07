using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Form;

namespace ThumbText
{
    class ThumbViewer : FlowLayoutPanel
    {
        /// <summary>
        /// 图标大小
        /// </summary>
        public int picBoxSize { set; get; }
        /// <summary>
        /// 缩略图大小
        /// </summary>
        public int ThumbMapSize { set; get; }
        /// <summary>
        /// 成果集合
        /// </summary>
        public List<string> photos { set; get; }
        /// <summary>
        /// 选择集
        /// </summary>
        public Dictionary<string, PictureBox> selected { set; get; }
        /// <summary>
        /// 缓存路径
        /// </summary>
        public string TempDir { set; get; }
        /// <summary>
        /// 项目名称，参于定义缩略图保存目录，可以为空
        /// </summary>
        public string proName { set; get; }

        #region 私有
        Label lab_1;
        Label lab_2;
        Thread getThumbMap_SubThread;
        int ii;
        #endregion
        /// <summary>
        /// 初始化一个缩略图流览器对
        /// </summary>
        /// <param name="c"></param>
        public ThumbViewer(ControlCollection c)
        {
            #region Buttons
            lab_1 = new Label()
            {
                Text = "+",
                ForeColor = Color.Blue,
                Location = new Point(c.Owner.Size.Width - 60, 40),
                Size = new Size(10, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            lab_1.MouseClick += Lab_1_MouseClick;
            lab_2 = new Label()
            {
                Text = "-",
                ForeColor = Color.Blue,
                Location = new Point(c.Owner.Size.Width - 40, 40),
                Size = new Size(10, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            lab_2.MouseClick += Lab_2_MouseClick;
            c.Add(lab_1);
            c.Add(lab_2);
            #endregion

            #region flowPayoutPanel

            AutoScroll = true;
            Dock = DockStyle.Fill;
            Name = "flowLayoutPanel1";
            Location = new Point(0, 24);
            Size = new Size(782, 568);
            TabIndex = 0;
            MouseEnter += ThumbViewer_MouseEnter;
            MouseWheel += ThumbViewer_MouseWheel;
            MouseClick += ThumbViewer_MouseClick;
            c.Add(this);
            #endregion

            picBoxSize = 128;
            ThumbMapSize = 128;
            photos = new List<string>();
            selected = new Dictionary<string, PictureBox>();
        }

        #region 逻辑
        //设置要显示的照片
        public void upDataItems()
        {
            Controls.Clear();
            creatPicBoxes();
            upDataThrmMaps();
        }
        //创建成员
        void creatPicBoxes()
        {
            PictureBox abox = null;
            for (int i = 0; i < photos.Count; i++)
            {
                string photoPath = photos[i];
                //如果已存在，则返回此实例
                if (Controls.ContainsKey(photoPath))
                    abox = Controls[photoPath] as PictureBox;
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
                Controls.Add(abox);
            }
        }
        //创建缩略图
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
                string thumbPath = getThumbPath(imgPaht);
                //如果thumbPath不存在则创建缩略图
                if (!File.Exists(thumbPath))
                {
                    Image aMap = Image.FromFile(imgPaht);
                    //缩略图尺寸
                    int thuWidth = aMap.Width > aMap.Height ? ThumbMapSize : ThumbMapSize * aMap.Height / aMap.Width;
                    int thuHieght = aMap.Width > aMap.Height ? ThumbMapSize * aMap.Height / aMap.Width : ThumbMapSize;
                    //创建缩略图
                    Image thumbMap = aMap.GetThumbnailImage(thuWidth, thuHieght, null, new IntPtr());
                    //创建文件夹，保存缩略图
                    if (!Directory.Exists(Path.GetDirectoryName(thumbPath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(thumbPath));
                    try { thumbMap.Save(thumbPath, ImageFormat.Jpeg); } catch (Exception ex) { MessageBox.Show(ex.Message); };
                    aMap.Dispose();
                    thumbMap.Dispose();
                }
                //回调返回false,说明主线程断了，则break掉此线程的循环
                if (!setToPic(imgPaht, thumbPath)) break;
            }
        }
        void drawBigThumb(string imgFile)
        {
            Image image = Image.FromFile(imgFile);
            Rectangle thumbRec = new Rectangle(0, 0, 44, 55);
            Bitmap bmp = new Bitmap(thumbRec.Width, thumbRec.Height);
            Graphics gr = Graphics.FromImage(bmp);
            {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            }
            gr.DrawImage(image, thumbRec, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            bmp.Save("", ImageFormat.Jpeg);

            bmp.Dispose();
            image.Dispose();
            GC.Collect();
        }


        bool callback_setToPic(string imgPath, string ThumbPath)
        {
            bool res = false;
            try
            {
                (Controls[imgPath] as PictureBox).Image = Image.FromFile(ThumbPath);
                res = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return res;
        }
        bool callback_setToPic(string imgPath, Image thMap)
        {
            bool res = false;
            try
            {
                (Controls[imgPath] as PictureBox).Image = thMap;
                res = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return res;
        }
        string getThumbPath(string imgPaht)
        {
            //根目录，判断用户是否设置，如果没有，就用系统%temp%目录
            string root = TempDir ?? Path.GetTempPath();

            //Project目录
            root = proName == null ? Path.Combine(TempDir, "AM_Thumbnail") : Path.Combine(TempDir, "AM_Thumbnail", proName);

            //照片的上一级目录 D:/dir/文件名.jpg
            string dir = Path.GetFileName(Path.GetDirectoryName(imgPaht));

            //文件名.jpg
            string imgName = Path.GetFileNameWithoutExtension(imgPaht) + ".jpg";

            //合并路径
            return Path.Combine(root, dir, imgName);
        }
        void updataSelection()
        {
            foreach (PictureBox item in Controls)
            {
                item.BackColor = SystemColors.ScrollBar;
            }
            foreach (PictureBox item in selected.Values)
            {
                item.BackColor = Color.DarkGray;
            }
        }

        #endregion

        #region 交互
        //双击
        private void Abox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Size = new Size(400, Size.Height);
        }
        //点击
        private void Abox_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            int picID = Controls.GetChildIndex(pic);
            //点到未提取缩略图的pic时，启动从当前pic开始向后提取
            if (pic.Image == null)
            {
                ii = picID - 1 >= 0 ? picID - 1 : 0;
                upDataThrmMaps();
            }
            //Ctrl 加减选
            if (ModifierKeys == Keys.Control)
            {
                if (selected.ContainsKey(pic.Name))
                    selected.Remove(pic.Name);
                else
                    selected[pic.Name] = pic;
            }
            //shift 范围选
            else if (ModifierKeys == Keys.Shift)
            {
                //求范围索引
                /// 选择集中的第一个成员是否小于当前选择对象
                /// 是，说明向后选择，选择集中的第一个成员为start
                /// 否，说明从后向前选，选择集中的最后一个成员为start
                int start = Controls.GetChildIndex(selected.First().Value) < picID ?
                    Controls.GetChildIndex(selected.First().Value) :
                    Controls.GetChildIndex(selected.Last().Value);
                int i = start < picID ? start : picID;
                int ii = start > picID ? start : picID;
                selected.Clear();
                for (; i <= ii; i++)
                {
                    PictureBox a = Controls[i] as PictureBox;
                    selected[a.Name] = a;
                }
            }
            else
            {
                selected.Clear();
                selected[pic.Name] = pic;
            }
            updataSelection();
        }
        //滚轮
        private void ThumbViewer_MouseWheel(object sender, MouseEventArgs e)
        {
            AutoScrollPosition = new Point(0, this.VerticalScroll.Value - e.Delta / 2);
        }
        private void ThumbViewer_MouseEnter(object sender, EventArgs e)
        {
            Focus();
        }
        //未点到照片，清空选择
        private void ThumbViewer_MouseClick(object sender, MouseEventArgs e)
        {
            selected.Clear();
            updataSelection();
        }
        //放大缩小图标
        private void Lab_2_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (PictureBox pic in Controls)
            {
                if (pic.Size.Width < 64)
                {
                    lab_2.ForeColor = Color.Gray;
                    break;
                }
                pic.Size = new Size((int)(pic.Size.Width * 0.8), (int)(pic.Size.Height * 0.8));
                lab_1.ForeColor = Color.Blue;
            }
        }
        private void Lab_1_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (PictureBox pic in Controls)
            {
                if (pic.Size.Width > 600)
                {
                    lab_1.ForeColor = Color.Gray;
                    break;
                }
                pic.Size = new Size((int)(pic.Size.Width / 0.8), (int)(pic.Size.Height / 0.8));

            }
            lab_2.ForeColor = Color.Blue;
        }
        #endregion
    }
}
