using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// 参考资料：  http://www.cnblogs.com/cncc/p/7170951.html
namespace AutomeshNodeManager
{
    public partial class Form1 : Form
    {
        //服务程序
        string serviceFilePath = $"{Application.StartupPath}\\AutoMeshNodeServerInstaller.exe";

        //服务名称，对应服务程序中的服务名
        string serviceName = "am_node_server";

        #region 窗体相关
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化,检测状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //贴左上角
            //int SW = Screen.PrimaryScreen.Bounds.Width;
            //int SH = Screen.PrimaryScreen.Bounds.Height;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 300, 0);


            //当前IP
            IPAddress[] IP = Dns.GetHostAddresses(Dns.GetHostName());
            for (int i = 0; i < IP.Length; i++)
            {
                if (IP[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    this.Text += " " + IP[i].ToString();
                    break;
                }
            }


            //启动后最小化
            //WindowState = FormWindowState.Minimized;

            timer1.Start();
        }
        
        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)  //判断是否最小化
            {
                this.ShowInTaskbar = false;  //不显示在系统任务栏
                notifyIcon1.Visible = true;  //托盘图标可见
            }
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = true;  //显示在系统任务栏
                this.WindowState = FormWindowState.Normal;  //还原窗体
                notifyIcon1.Visible = false;  //托盘图标隐藏
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认退出程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // 关闭所有的线程
                this.Dispose();
                this.Close();
            }
        }
        #endregion

        #region 交互事件

        //事件：安装服务
        private void install_btn_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted())
                this.UninstallService();
            this.InstallService();
        }

        //事件：启动服务
        private void start_btn_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted()) this.ServiceStart();
        }

        //事件：停止服务
        private void stop_btn_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted()) this.ServiceStop();
        }

        //事件：卸载服务
        private void Uninstall_btn_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted())
            {
                this.ServiceStop();
                this.UninstallService();
            }
        }

        private void set_btn_Click(object sender, EventArgs e)
        {

        }

        private void help_btn_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region 相关方法

        /// <summary>
        /// 判断服务是否存在
        /// </summary>
        /// <returns></returns>
        private bool IsServiceExisted()
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController sc in services)
            {
                if (sc.ServiceName.ToLower() == serviceName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 安装服务
        /// </summary>
        private void InstallService()
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = serviceFilePath;
                IDictionary savedState = new Hashtable();
                installer.Install(savedState);
                installer.Commit(savedState);
                MessageBox.Show("安装成功！");
            }
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        private void UninstallService()
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = serviceFilePath;
                installer.Uninstall(null);
                MessageBox.Show("服务程序已卸载！");
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        private void ServiceStart()
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Stopped)
                {
                    control.Start();
                   // MessageBox.Show("启动成功！");
                }
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        private void ServiceStop()
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Running)
                {
                    control.Stop();
                   // MessageBox.Show("服务已停止！");
                }
            }
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (IsServiceExisted())
            {
                install_btn.Enabled = false;
                Uninstall_btn.Enabled = true;
                set_btn.Enabled = true;

                //已安装服务
                using (ServiceController control = new ServiceController(serviceName))
                {
                    if (control.Status == ServiceControllerStatus.Running)
                    {
                        //已启动服务
                        start_btn.Enabled = false;
                        stop_btn.Enabled = true;
                        label1.Text = ">>> 服务已启动...";
                    }
                    else
                    {
                        //未启动服务
                        start_btn.Enabled = true;
                        stop_btn.Enabled = false;
                        label1.Text = ">>> 服务已停止...";
                    }
                }
            }
            else
            {
                //未安装服务
                install_btn.Enabled = true;
                start_btn.Enabled = false;
                stop_btn.Enabled = false;
                Uninstall_btn.Enabled = false;
                set_btn.Enabled = false;
                label1.Text = ">>> 未安装服务";
            }
        }
        #endregion
    }
}
