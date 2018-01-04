using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace getComputerInfos
{
    class WMIC
    {
        /**************************

       比CMD更强大的命令行WMIC
       cmd的API
       https://www.cnblogs.com/top5/p/3143837.html
       先决条件：
               a. 启动Windows Management Instrumentation服务，开放TCP135端口。
               b. 本地安全策略的“网络访问: 本地帐户的共享和安全模式”应设为“经典-本地用户以自己的身份验证”。

        **************************/
        static void Main()
        {
            string cmd;
            ///
            /// 1.联接远程主机
            /// wmic /node:"192.168.1.20" /user:"domain\administrator" /password:"123456"\
            /// 

            ///
            /// 2.【硬件管理】：
            /// 

            // 获取磁盘资料：
            cmd = "wmic DISKDRIVE get deviceid, Caption, size, InterfaceType";
            //获取分区资料：
            cmd = "wmic LOGICALDISK get name, Description, filesystem, size, freespace";
            //获取CPU资料:
            cmd = "wmic cpu get name, addresswidth, processorid";
            //获取主板资料:
            cmd = "wmic BaseBoard get Manufacturer, Product, Version, SerialNumber";
            //获取内存数:
            cmd = "wmic memlogical get totalphysicalmemory";
            //获得品牌机的序列号:
            cmd = "wmic csproduct get IdentifyingNumber";
            //获取声卡资料:
            cmd = "wmic SOUNDDEV get ProductName";
            //获取屏幕分辨率
            cmd = "wmic DESKTOPMONITOR where Status = 'ok' get ScreenHeight, ScreenWidth";

            ///
            /// 3. PROCESS【进程管理】：
            /// 

            // 列出进程
            cmd = "wmic process list brief";
            //(Full显示所有、Brief显示摘要、Instance显示实例、Status显示状态)

            //wmic 获取进程路径: 
            cmd = "wmic process where name = \"jqs.exe\" get executablepath";

            //wmic 创建新进程
            cmd = "wmic process call create notepad";
            cmd = "wmic process call create \"C:\\Program Files\\Tencent\\QQ\\QQ.exe\"";
            cmd = "wmic process call create \"shutdown.exe -r -f -t 20\"";

            //wmic 删除指定进程: 
            cmd = "wmic process where name = \"qq.exe\" call terminate";
            cmd = "wmic process where processid = \"2345\" delete";
            cmd = "wmic process 2345 call terminate";

            //wmic 删除可疑进程
            cmd = "wmic process where \"name ='explorer.exe' and executablepath<>'%SystemDrive%\\windows\\explorer.exe'\" delete";
            cmd = "wmic process where \"name ='svchost.exe' and ExecutablePath<>'C:\\WINDOWS\\system32\\svchost.exe'\" call Terminate";

            ///
            /// 3. USERACCOUNT【账号管理】：
            /// 

            //更改当前用户名
            cmd = "WMIC USERACCOUNT where \"name ='%UserName%'\" call rename newUserName";
            cmd = "WMIC USERACCOUNT create /?";

            ///
            /// 4. SHARE【共享管理】：
            /// 

            /*
                建立共享
                WMIC SHARE CALL Create "","test","3","TestShareName","","c:\test",0
                (可使用 WMIC SHARE CALL Create /? 查看create后的参数类型)

                删除共享
                WMIC SHARE where name="C$" call delete
                WMIC SHARE where path='c:\\test' delete
             */

            ///
            /// 5. SERVICE【服务管理】：
            /// 

            /*
            更改telnet服务启动类型[Auto|Disabled|Manual]
            wmic SERVICE where name="tlntsvr" set startmode="Auto"

            运行telnet服务
            wmic SERVICE where name="tlntsvr" call startservice

            停止ICS服务
            wmic SERVICE where name="ShardAccess" call stopservice

            删除test服务
            wmic SERVICE where name="test" call delete    
         */

            ///
            /// 6. FSDIR【目录管理】
            /// 

            /*
                列出c盘下名为test的目录
                wmic FSDIR where "drive='c:' and filename='test'" list
                删除c:\good文件夹
                wmic fsdir "c:\\test" call delete
                重命名c:\test文件夹为abc
                wmic fsdir "c:\\test" rename "c:\abc"
                wmic fsdir where (name='c:\\test') rename "c:\abc"
                复制文件夹
                wmic fsdir where name='d:\\test' call copy "c:\\test"


                7.datafile【文件管理】

                重命名
                wmic datafile "c:\\test.txt" call rename c:\abc.txt

                8.【任务计划】：
                wmic job call create "notepad.exe",0,0,true,false,********154800.000000+480
                wmic job call create "explorer.exe",0,0,1,0,********154600.000000+480
             */

        }
        static void run(string myCmd)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.CreateNoWindow = true;//不创建新窗口
            cmd.StartInfo.UseShellExecute = false;//不使用系统shell
            cmd.StartInfo.RedirectStandardInput = true;//从当前实例（cmd）读取输入内容
            cmd.StartInfo.RedirectStandardOutput = true;//输出内容传给cmd
            cmd.StartInfo.RedirectStandardError = true;
            cmd.Start();
            cmd.StandardInput.WriteLine("");//输出一条命令
            cmd.StandardInput.WriteLine("exit");//退出
            cmd.WaitForExit(1000);//等待完成1秒后退出 }
        }
    }
}
