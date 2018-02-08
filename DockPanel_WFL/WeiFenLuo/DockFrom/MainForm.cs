using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DockFrom
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Buttom
            DockForm buttom = new DockForm { BackColor = Color.Orange, Text = "bottom"};
            buttom.Show(dockPanel1, DockState.DockBottom);
            // 停靠窗占比：buttom,Hight / dockPanel.Hight
            dockPanel1.DockBottomPortion = 0.3;

            //Left
            DockForm left = new DockForm() { BackColor = Color.Gray, Text = "left" };
            left.Show(dockPanel1, DockState.DockLeft);
            DockForm left_D = new DockForm() { BackColor = Color.Gold, Text = "left_D" };
            // 子窗口各部占比 left_D / left
            left_D.Show(left.Pane,DockAlignment.Bottom, 0.4);

            //Right
            DockForm right1 = new DockForm() { BackColor = Color.BurlyWood, Text = "right1" };
            right1.Show(dockPanel1, DockState.DockRightAutoHide);
            DockForm right2 = new DockForm() { BackColor = Color.DarkCyan, Text = "right2" };
            //手动设置
            right2.Show(dockPanel1);
            right2.DockTo(dockPanel1, DockStyle.Right);
            right2.DockState = DockState.DockRightAutoHide;

            //Document
            DockForm document = new DockForm() { BackColor = Color.DeepSkyBlue, Text = "Document" };
            document.Show(dockPanel1, DockState.Document);

            //SubDocker
            DockForm subDocker_1 = new DockForm() { BackColor = Color.Green, Text = "Sub_Left" };
            subDocker_1.Show(document.Pane, DockAlignment.Right, 0.5);
            DockForm subDocker_2 = new DockForm() { BackColor = Color.PaleGreen, Text = "Sub_LeftDonw" };
            subDocker_2.Show(document.Pane, DockAlignment.Bottom, 0.5);
            DockForm subDocker_3 = new DockForm() { BackColor = Color.IndianRed, Text = "Sub_RightDonw" };
            subDocker_3.Show(subDocker_1.Pane, DockAlignment.Bottom, 0.5);
            
            this.Text = dockPanel1.DockWindows.Count.ToString();
        }
    }
}
