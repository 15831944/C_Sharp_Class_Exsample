using System;
using System.Drawing;
using System.Windows.Forms;

namespace byWinFormPic
{
    class PalyPictuerBox
    {
        PictureBox picBox;
        Rectangle imgRec;
        Rectangle drawRec;
        Point cleckP;
        bool isFull;
        public PalyPictuerBox()
        {

        }
        public void setImage(string imgFile)
        {
            picBox.Image = Image.FromFile(imgFile);
            imgRec = new Rectangle(new Point(), picBox.Image.Size);
            fixToWindow();
        }
        void initPicBox()
        {
            picBox = new PictureBox() { Dock = DockStyle.Fill, BackColor = Color.White };
            picBox.Paint += picBox_Paint;
            picBox.MouseClick += picBox_MouseClick;
            picBox.MouseDoubleClick += picBox_MouseDoubleClick;
            picBox.MouseDown += picBox_MouseDown;
            picBox.MouseMove += picBox_MouseMove;
            picBox.MouseUp += picBox_MouseUp;
            picBox.MouseEnter += (object s, EventArgs e) => { picBox.Focus(); };
            picBox.MouseWheel += picBox_MouseWheel;

        }
        //按下：记录鼠标点击时的窗口坐标
        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            cleckP = e.Location;
        }
        //移动：拖动图像，切换光标样式
        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                picBox.Cursor = Cursors.NoMove2D;
                drawRec.X += e.X - cleckP.X;
                drawRec.Y += e.Y - cleckP.Y;
                cleckP = e.Location;
                picBox.Refresh();
            }
        }
        //弹起：切换光标样式
        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {
            picBox.Cursor = Cursors.Arrow;
            //范围控制
            if (drawRec.X > picBox.Size.Width)//右
                drawRec.X = picBox.Width - drawRec.Size.Width;
            if (drawRec.Y > picBox.Size.Height)//下
                drawRec.Y = picBox.Height - drawRec.Size.Height;
            if (drawRec.X < -drawRec.Size.Width)//左
                drawRec.X = 0;
            if (drawRec.Y < -drawRec.Size.Height)//上
                drawRec.Y = 0;
            picBox.Refresh();
        }
        //单击：拾取图像坐标
        private void picBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (picBox.Image == null) return;
            double zo = 1.0 * picBox.Image.Width / drawRec.Size.Width;
            // this.Text = $"imgX = {(int)(e.X * zo - drawRec.X * zo + 1)} , imgY = {(int)(e.Y * zo - drawRec.Y * zo + 1)}";
        }
        //双击：切换屏幕适配，全像素
        private void picBox_MouseDoubleClick(object sender, MouseEventArgs e)
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
        private void picBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (picBox.Image == null) return;
            float zo = e.Delta > 0 ? 1f / 0.8f : 0.8f;
            int nW = (int)(drawRec.Size.Width * zo);
            if (nW < 100) return;
            int nH = nW * picBox.Image.Size.Height / picBox.Image.Size.Width;
            Size si = new Size(nW, nH);
            int nX = (int)((e.X - drawRec.X) * zo);
            int nY = (int)((e.Y - drawRec.Y) * zo);
            Point newLoc = new Point(e.X - nX, e.Y - nY);
            drawRec = new Rectangle(newLoc, si);
            picBox.Refresh();
        }
        //重绘
        private void picBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            if (picBox.Image != null)
            {
                e.Graphics.Clear(Color.White);
                e.Graphics.DrawImage(picBox.Image, drawRec, imgRec, GraphicsUnit.Pixel);
            }
        }
        //适配窗口
        private void fixToWindow()
        {
            if (picBox.Image == null) return;
            drawRec = new Rectangle();
            {
                if (picBox.Width * 1.0 / picBox.Height > picBox.Image.Width * 1.0 / picBox.Image.Height)
                {
                    ///picBox的宽高比大于图像，则使用高计算缩放
                    ///      _____________________
                    ///     |     |        |     |
                    ///     |     |        |     |
                    ///     |     |        |     |
                    ///     ``````````````````````
                    ///     绘宽 = 图宽 x (picBox高 / 图高) , 绘高 = picBox高
                    ///     loc.x = picBox中心 - 绘宽/2 , loc.y = 0
                    drawRec.Size = new Size((int)(picBox.Image.Width * picBox.Height / picBox.Image.Height * 1.0), picBox.Height);
                    drawRec.Location = new Point(picBox.Width / 2 - drawRec.Size.Width / 2, 0);
                }
                else
                {
                    ///picBox的宽高比小于等于图像，则使用宽计算缩放
                    ///     ___________________
                    ///     |__________________|
                    ///     |                  |
                    ///     |                  |
                    ///     |__________________|
                    ///     |                  |
                    ///     ````````````````````
                    ///     绘宽 = picBox宽 ，绘高 = 图高 x （picBoc宽 / 图宽）
                    ///     loc.x = 0 , loc.y = picbox.h中心 - 绘高/2
                    drawRec.Size = new Size(picBox.Width, (int)(picBox.Image.Height * picBox.Width / picBox.Image.Width * 1.0));
                    drawRec.Location = new Point(0, picBox.Height / 2 - drawRec.Size.Height / 2);
                }
            }
            picBox.Refresh();
        }
        //全像素显示
        private void fullPixce(Point p)
        {
            if (picBox.Image == null) return;
            //点击位置转为图像坐标
            double zo = 1.0 * picBox.Image.Width / drawRec.Size.Width;
            int imgX = (int)(p.X * zo - drawRec.X * zo);
            int imgY = (int)(p.Y * zo - drawRec.Y * zo);

            //把点击位置移到窗口中心
            //Point loc = new Point(picBox.Size.Width / 2 - imgX, picBox.Size.Height / 2 - imgY);
            //以点击位置为中心放大
            Point loc1 = new Point(p.X - imgX, p.Y - imgY);

            drawRec = new Rectangle(loc1, picBox.Image.Size);
            picBox.Refresh();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            setImage(@"E:\Automesh\Automesh v1.0\工程\测试数据\images\DSC00164.JPG");
        }
    }
}
