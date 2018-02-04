using System;
using System.Drawing;
using System.Windows.Forms;

namespace byWinFormPic
{
    class PlayPictuerBox : PictureBox
    {
        Rectangle imgRec;
        Rectangle drawRec;
        Point cleckP;
        bool isFull;
        bool isMove;
        public Action<string> p = (a) => { };
        public PlayPictuerBox()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.White;
            Paint += this_Paint;
            MouseClick += this_MouseClick;
            MouseDoubleClick += this_MouseDoubleClick;
            MouseDown += this_MouseDown;
            MouseMove += this_MouseMove;
            MouseUp += this_MouseUp;
            MouseEnter += (object s, EventArgs e) => { Focus(); };
            MouseWheel += this_MouseWheel;
        }
        public void setImage(string imgFile)
        {
            Image = Image.FromFile(imgFile);
            imgRec = new Rectangle(new Point(), Image.Size);
            fixToWindow();
        }
        //按下：记录鼠标点击时的窗口坐标
        void this_MouseDown(object sender, MouseEventArgs e)
        {
            cleckP = e.Location;
            isMove = e.Button == MouseButtons.Left;
        }
        //移动：拖动图像，切换光标样式
        void this_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMove)
            {
                Cursor = Cursors.NoMove2D;
                drawRec.X += e.X - cleckP.X;
                drawRec.Y += e.Y - cleckP.Y;
                cleckP = e.Location;
                Refresh();
            }
            if (Image != null)
            {
                double zo = 1.0 * Image.Width / drawRec.Width;
                p($"imgX = {(int)(e.X * zo - drawRec.X * zo + 1)} , imgY = {(int)(e.Y * zo - drawRec.Y * zo + 1)},zoom = {drawRec.Width * 1.0 / Image.Width}");
            }
        }
        //弹起：切换光标样式
        void this_MouseUp(object sender, MouseEventArgs e)
        {
            isMove = false;
            Cursor = Cursors.Arrow;
            //范围控制
            if (drawRec.X > Size.Width)//右
                drawRec.X = Width - drawRec.Size.Width;
            if (drawRec.Y > Size.Height)//下
                drawRec.Y = Height - drawRec.Size.Height;
            if (drawRec.X < -drawRec.Size.Width)//左
                drawRec.X = 0;
            if (drawRec.Y < -drawRec.Size.Height)//上
                drawRec.Y = 0;
            Refresh();

        }
        //单击：拾取图像坐标
        void this_MouseClick(object sender, MouseEventArgs e)
        {

        }
        //双击：切换屏幕适配，全像素
        void this_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left
                && e.X > drawRec.X && e.X < drawRec.X + drawRec.Size.Width
                && e.Y > drawRec.Y && e.Y < drawRec.Y + drawRec.Size.Height)
            {
                if (isFull)
                {
                    fixToWindow();
                    isFull = false;
                }
                else
                {
                    fullPixce(e.Location);
                    isFull = true;
                }
            }
        }
        //滚轮
        void this_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Image == null) return;
            float zo = e.Delta > 0 ? 1f / 0.8f : 0.8f;
            if (drawRec.Width * zo < 100 || drawRec.Width * zo / Image.Width > 70) return;

            int nW = (int)(drawRec.Width * zo);
            int nH = nW * Image.Height / Image.Width;
            int nX = (int)((e.X - drawRec.X) * zo);
            int nY = (int)((e.Y - drawRec.Y) * zo);
            Point newLoc = new Point(e.X - nX, e.Y - nY);
            Size newSize = new Size(nW, nH);
            drawRec = new Rectangle(newLoc, newSize);
            drawRec = new Rectangle(newLoc, newSize);

            Refresh();
        }
        //重绘
        void this_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            if (Image != null)
            {
                e.Graphics.Clear(Color.White);
                e.Graphics.DrawImage(Image, drawRec, imgRec, GraphicsUnit.Pixel);
            }
        }
        //适配窗口
        void fixToWindow()
        {
            if (Image == null) return;
            drawRec = new Rectangle();

            if (Width * 1.0 / Height > Image.Width * 1.0 / Image.Height)
            {
                ///this的宽高比大于图像，则使用高计算缩放
                ///      _____________________
                ///     |     |        |     |
                ///     |     |        |     |
                ///     |     |        |     |
                ///     ``````````````````````
                ///     绘宽 = 图宽 x (this高 / 图高) , 绘高 = this高
                ///     loc.x = this中心 - 绘宽/2 , loc.y = 0
                drawRec.Size = new Size((int)(Image.Width * Height / Image.Height * 1.0), Height);
                drawRec.Location = new Point(Width / 2 - drawRec.Size.Width / 2, 0);
            }
            else
            {
                ///this的宽高比小于等于图像，则使用宽计算缩放
                ///     ___________________
                ///     |__________________|
                ///     |                  |
                ///     |                  |
                ///     |__________________|
                ///     |                  |
                ///     ````````````````````
                ///     绘宽 = this宽 ，绘高 = 图高 x （picBoc宽 / 图宽）
                ///     loc.x = 0 , loc.y =  h中心 - 绘高/2
                drawRec.Size = new Size(Width, (int)(Image.Height * Width / Image.Width * 1.0));
                drawRec.Location = new Point(0, Height / 2 - drawRec.Size.Height / 2);
            }

            Refresh();
        }
        //全像素显示
        void fullPixce(Point p)
        {
            if (Image == null) return;
            //点击位置转为图像坐标
            double zo = 1.0 * Image.Width / drawRec.Size.Width;
            int imgX = (int)(p.X * zo - drawRec.X * zo);
            int imgY = (int)(p.Y * zo - drawRec.Y * zo);

            //把点击位置移到窗口中心
            //Point loc = new Point( Size.Width / 2 - imgX,  Size.Height / 2 - imgY);
            //以点击位置为中心放大
            Point loc1 = new Point(p.X - imgX, p.Y - imgY);

            drawRec = new Rectangle(loc1, Image.Size);
            Refresh();
        }
    }
}
