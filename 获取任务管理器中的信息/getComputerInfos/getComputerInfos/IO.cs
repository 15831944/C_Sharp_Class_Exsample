using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace getComputerInfos
{
    class IO
    {
        //调winAPI
        [DllImport("kernel32.dll")]
        static extern bool GetProcessIoCounters(IntPtr ProcessHandle, out IO_COUNTERS IoCounters);
        //IO结构体
        [StructLayout(LayoutKind.Sequential)]
        struct IO_COUNTERS
        {
            public ulong ReadOperationCount;
            public ulong WriteOperationCount;
            public ulong OtherOperationCount;
            public ulong ReadTransferCount;
            public ulong WriteTransferCount;
            public ulong OtherTransferCount;
        }
        /// <summary>
        /// 获取进程IO信息
        /// </summary>
        /// <param name="pro">目标进程</param>
        /// <returns></returns>
        IO_COUNTERS getIO_info(Process pro)
        {
            //Process pro = Process.GetProcessesByName("QQ")[0];
            IO_COUNTERS counters;
            GetProcessIoCounters(pro.Handle, out counters);
            return counters;
        }
    }
}
