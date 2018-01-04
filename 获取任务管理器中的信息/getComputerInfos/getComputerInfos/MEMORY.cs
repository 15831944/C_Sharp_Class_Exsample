using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace getComputerInfos
{
    class MEMORY
    {
        //memory结构体
        [StructLayout(LayoutKind.Sequential, Size = 40)]
        struct PROCESS_MEMORY_COUNTERS
        {
            public uint cb;
            public uint PageFaultCount;
            public uint PeakWorkingSetSize;
            public uint WorkingSetSize;
            public uint QuotaPeakPagedPoolUsage;
            public uint QuotaPagedPoolUsage;
            public uint QuotaPeakNonPagedPoolUsage;
            public uint QuotaNonPagedPoolUsage;
            public uint PagefileUsage;
            public uint PeakPagefileUsage;
        }
        //winAPI
        [DllImport("psapi.dll", SetLastError = true)]
        static extern bool GetProcessMemoryInfo(IntPtr hProcess, out PROCESS_MEMORY_COUNTERS Memcounters, int size);

        PROCESS_MEMORY_COUNTERS getMemoryInfo(Process pro)
        {
            PROCESS_MEMORY_COUNTERS MemCounters;
            GetProcessMemoryInfo(pro.Handle, out MemCounters, Marshal.SizeOf(typeof(PROCESS_MEMORY_COUNTERS)));
            return MemCounters;
        }
    }
}
