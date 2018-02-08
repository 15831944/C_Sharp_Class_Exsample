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

namespace ThumbText
{
    class ThumbViewer : FlowLayoutPanel
    {
        /// <summary>
        /// 图标大小
        /// </summary>
        public int picBoxSize { set; get; }
        /// <summary>
        /// 缩略图大小,如果不需要生成大缩略图，设置为0
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
        /// 缓存路径,不保存本地缓存设置为 null，默认%temp%
        /// </summary>
        public string TempDir { set; get; }
        /// <summary>
        /// 项目名称，参于定义缩略图保存目录，可以为空
        /// </summary>
        public string proName { set; get; }
        /// <summary>
        /// 触发图标的双击事件
        /// </summary>
        public Action<object, MouseEventArgs> itemDoubleClick = (a, e) => { };
        /// <summary>
        ///  触发图标的单击事件
        /// </summary>
        public Action<object, MouseEventArgs> itemClick = (a, e) => { };

        #region 私有
        Label lab_1;
        Label lab_2;
        Thread getThumbMap_SubThread;
        int ii;
        #endregion

        /// <summary>
        /// 初始化一个缩略图流览器对
        /// </summary>
        /// <param name="c">父对象的Controls</param>
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
            ThumbMapSize = 512;
            photos = new List<string>();
            selected = new Dictionary<string, PictureBox>();
            TempDir = Path.GetTempPath();
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
                getThumbMap_SubThread = new Thread(new ThreadStart(() => getThumbMapFromImage(callback_setPathToPic, callback_setIMGToPic)));
                getThumbMap_SubThread.Start();
            }
        }
        void getThumbMapFromImage(Func<string, string, bool> setPathToPic, Func<string, Image, bool> setImgToPic)
        {
            for (ii = 0; ii < photos.Count; ii++)
            {
                int i = ii;//界面点击会改变此值
                string imgPaht = photos[i];
                string smallThumb = getThumbPath(imgPaht, true);

                if (smallThumb != null)
                {
                    //保存本地缓存

                    if (!File.Exists(smallThumb))
                    {
                        //如果thumbPath不存在则创建缩略图

                        Image aMap = Image.FromFile(imgPaht);
                        //缩略图尺寸
                        int thuWidth = aMap.Width > aMap.Height ? picBoxSize : picBoxSize * aMap.Height / aMap.Width;
                        int thuHieght = aMap.Width > aMap.Height ? picBoxSize * aMap.Height / aMap.Width : picBoxSize;

                        //创建缩略图
                        Image thumbMap = aMap.GetThumbnailImage(thuWidth, thuHieght, null, new IntPtr());

                        //创建文件夹，保存缩略图
                        if (!Directory.Exists(Path.GetDirectoryName(smallThumb)))
                            Directory.CreateDirectory(Path.GetDirectoryName(smallThumb));
                        try { thumbMap.Save(smallThumb, ImageFormat.Jpeg); } catch (Exception ex) { MessageBox.Show(ex.Message); };

                        aMap.Dispose();
                        thumbMap.Dispose();
                        GC.Collect();
                    }

                    //回调返回false,说明主线程断了，则break掉此线程的循环
                    if (!setPathToPic(imgPaht, smallThumb)) break;

                    //创建大图
                    if (ThumbMapSize > 0)
                        new Thread(new ThreadStart(() => { setPathToPic(imgPaht, drawBigThumb(imgPaht)); })).Start();
                }
                else
                {
                    //不保存本地缓存

                    Image aMap = Image.FromFile(imgPaht);
                    int thuWidth = aMap.Width > aMap.Height ? picBoxSize : picBoxSize * aMap.Height / aMap.Width;
                    int thuHieght = aMap.Width > aMap.Height ? picBoxSize * aMap.Height / aMap.Width : picBoxSize;
                    Image thumbMap = aMap.GetThumbnailImage(thuWidth, thuHieght, null, new IntPtr());
                    aMap.Dispose(); GC.Collect();
                    if (!setImgToPic(imgPaht, thumbMap)) break;
                    if (ThumbMapSize > 0)
                        new Thread(new ThreadStart(() => { setImgToPic(imgPaht, drawBigIMGThumb(imgPaht)); })).Start();
                }
            }
        }
        /// <summary>
        /// 绘制高精缩略图，返回其路径，保存本地缓存时用
        /// </summary>
        /// <param name="imgFile"></param>
        /// <returns></returns>
        string drawBigThumb(string imgFile)
        {
            string bigThumb = getThumbPath(imgFile, false);
            Image image = Image.FromFile(imgFile);
            //缩略图尺寸
            int thuWidth = image.Width > image.Height ? ThumbMapSize : ThumbMapSize * image.Height / image.Width;
            int thuHieght = image.Width > image.Height ? ThumbMapSize * image.Height / image.Width : ThumbMapSize;

            Bitmap bmp = new Bitmap(thuWidth, thuHieght);
            Graphics gr = Graphics.FromImage(bmp);
            {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            }
            Rectangle thumbRec = new Rectangle(0, 0, thuWidth, thuHieght);
            gr.DrawImage(image, thumbRec, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            bmp.Save(bigThumb, ImageFormat.Jpeg);

            bmp.Dispose();
            image.Dispose();
            GC.Collect();
            return bigThumb;
        }
        /// <summary>
        /// 绘制高精缩略图，返回其image实例，不保存本地缓存时用
        /// </summary>
        /// <param name="imgFile"></param>
        /// <returns></returns>
        Image drawBigIMGThumb(string imgFile)
        {
            string bigThumb = getThumbPath(imgFile, false);
            Image image = Image.FromFile(imgFile);
            //缩略图尺寸
            int thuWidth = image.Width > image.Height ? ThumbMapSize : ThumbMapSize * image.Height / image.Width;
            int thuHieght = image.Width > image.Height ? ThumbMapSize * image.Height / image.Width : ThumbMapSize;

            Bitmap bmp = new Bitmap(thuWidth, thuHieght);
            Graphics gr = Graphics.FromImage(bmp);
            {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            }
            Rectangle thumbRec = new Rectangle(0, 0, thuWidth, thuHieght);
            gr.DrawImage(image, thumbRec, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

            image.Dispose();
            GC.Collect();
            return bmp as Image;
        }
        /// <summary>
        /// 回调，把缩略图赋值给picBox
        /// </summary>
        /// <param name="imgPath"></param>
        /// <param name="ThumbPath"></param>
        /// <returns></returns>
        bool callback_setPathToPic(string imgPath, string ThumbPath)
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
        bool callback_setIMGToPic(string imgPath, Image ThumbPath)
        {
            bool res = false;
            try
            {
                (Controls[imgPath] as PictureBox).Image = ThumbPath;
                res = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return res;
        }
        /// <summary>
        /// 格式化缩略图保存目录
        /// </summary>
        /// <param name="imgPaht"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        string getThumbPath(string imgPaht, bool s)
        {
            //用户设置为null,不保存缓存文件
            if (TempDir == null) return null;

            //Project目录
            string root = proName == null ? Path.Combine(TempDir, "AM_Thumbnail") : Path.Combine(TempDir, "AM_Thumbnail", proName);

            //照片的上一级目录 D:/dir/文件名.jpg
            string dir = Path.GetFileName(Path.GetDirectoryName(imgPaht));

            //文件名.jpg
            string imgName = Path.GetFileNameWithoutExtension(imgPaht) + (s ? "_s.jpg" : "_b.jpg");

            //合并路径
            return Path.Combine(root, dir, imgName);
        }
        /// <summary>
        /// 更新成员选择状态
        /// </summary>
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
            // Size = new Size(400, Size.Height);
            itemDoubleClick(sender, e);
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
            itemClick(sender, e);
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
