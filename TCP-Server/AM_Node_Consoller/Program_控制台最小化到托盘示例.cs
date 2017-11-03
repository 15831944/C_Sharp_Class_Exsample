using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
namespace Test2
{
    /// <summary>
    /// 控制台最小化到托盘示例
    /// </summary>
    class Program
    {
        static bool _IsExit = false;
        static void Main(string[] args)
        {
            Console.Title = "ffffff";
            ConsoleWin32Helper.ShowNotifyIcon();
            ConsoleWin32Helper.DisableCloseButton(Console.Title);



            Thread threadMonitorInput = new Thread(new ThreadStart(MonitorInput));
            threadMonitorInput.Start();
            while (true)
            {
                Application.DoEvents();
                if (_IsExit)
                {
                    break;
                }
            }
        }
        static void MonitorInput()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "exit")
                {
                    _IsExit = true;
                    Thread.CurrentThread.Abort();
                }
            }
        }
    }


    class ConsoleWin32Helper
    {
        static NotifyIcon _NotifyIcon = new NotifyIcon();
        static ConsoleWin32Helper()
        {
            _NotifyIcon.Icon = new Icon("103.ico");
            _NotifyIcon.Visible = false;
            _NotifyIcon.Text = "tray";
            ContextMenu menu = new ContextMenu();
            MenuItem item = new MenuItem();
            item.Text = "右键菜单，还没有添加事件";
            item.Index = 0;
            menu.MenuItems.Add(item);
            _NotifyIcon.ContextMenu = menu;
            _NotifyIcon.MouseDoubleClick += new MouseEventHandler(_NotifyIcon_MouseDoubleClick);

        }

        public static void ShowNotifyIcon()
        {
            _NotifyIcon.Visible = true;
            _NotifyIcon.ShowBalloonTip(3000, "", "Automesh 节点管理器已启动。", ToolTipIcon.None);
        }

        /// <summary>
        /// 右键事件：退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void _NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ConsoleWin32Helper.Show(Console.Title);
            MessageBox.Show("fdsafdsa");
        }


        #region 禁用关闭按钮

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        static extern IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("User32.dll", EntryPoint = "ShowWindow")]
        public static extern bool ShowWindow(IntPtr hwind, int cmdShow);

        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hwind);

        ///<summary>
        /// 禁用关闭按钮
        ///</summary>
        ///<param name="consoleName">控制台名字</param>
        public static void DisableCloseButton(string title)//线程睡眠，确保closebtn中能够正常FindWindow，否则有时会Find失败。。 
        {
            Thread.Sleep(100);
            IntPtr windowHandle = FindWindow(null, title);
            IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
            uint SC_CLOSE = 0xF060;
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
        }

        public static bool IsExistsConsole(string title)
        {
            IntPtr windowHandle = FindWindow(null, title);
            if (windowHandle.Equals(IntPtr.Zero))
            {
                return false;
            }
            return true;
        }

        public static void Hidden(string title)
        {
            IntPtr ParenthWnd = new IntPtr(0);
            IntPtr et = new IntPtr(0);
            ParenthWnd = FindWindow(null, title);
            int normalState = 0;//窗口状态(隐藏)
            ShowWindow(ParenthWnd, normalState);
        }

        public static void Show(string title)
        {
            IntPtr ParenthWnd = new IntPtr(0);
            IntPtr et = new IntPtr(0);
            ParenthWnd = FindWindow(null, title);
            int normalState = 9;//窗口状态(隐藏)
            ShowWindow(ParenthWnd, normalState);
        }
        #endregion

    }


}