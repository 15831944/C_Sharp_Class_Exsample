
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {

        //上下行资源(多网卡)
        static List<PerformanceCounter> downLoads;
        static List<PerformanceCounter> upLoads;
        //时长
        static int timeSpan = 3000;
        static string goBack = new string(' ', 20) + new string('\b', 1000);

        static void Main(string[] args)
        {
            init();

            // 监控全局资源
            lookAtSystem();

            // 监控指定进程
            //lookAtProcess("chrome");
        }

        static void init()
        {
            Console.WriteLine("init...");
            //初始化计数器，需要管理员权限
            {
                Process cmd = new Process();
                cmd.StartInfo.FileName = @"C:\Windows\System32\lodctr.exe";
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.RedirectStandardError = true;
                cmd.Start();
                cmd.WaitForExit();
                cmd.Close();
            }
            downLoads = new List<PerformanceCounter>();
            upLoads = new List<PerformanceCounter>();
            //把所有网卡的上下行分别加入集合
            string categoryName = "Network Interface";
            string downCounterName = "Bytes Received/sec";
            string upCounterName = "Bytes Sent/sec";
            try
            {
                PerformanceCounterCategory ff = new PerformanceCounterCategory(categoryName);
                string[] InstanceNames = ff.GetInstanceNames();
                for (int i = 0; i < InstanceNames.Length; i++)
                {
                    downLoads.Add(new PerformanceCounter(categoryName, downCounterName, InstanceNames[i]));
                    upLoads.Add(new PerformanceCounter(categoryName, upCounterName, InstanceNames[i]));
                }
            }
            catch { Console.WriteLine("上行、下行信息初始化失败"); }
        }
        static void lookAtSystem()
        {
            Console.WriteLine("Taget: System");
            //全局CPU
            PerformanceCounter sysCPU = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            //物理内存
            long physicalMemory = 0;
            ManagementObjectCollection moc = new ManagementClass("Win32_ComputerSystem").GetInstances();
            string TotalPhysicalMemory = "TotalPhysicalMemory";
            foreach (ManagementObject item in moc)
            {
                if (item[TotalPhysicalMemory] != null)
                    physicalMemory = long.Parse(item[TotalPhysicalMemory].ToString());
            }

            try
            {
                while (true)
                {
                    //可用内存
                    long availe = 0;
                    ManagementClass mos = new ManagementClass("WIn32_OperatingSystem");
                    string FreePhysicalMemory = "FreePhysicalMemory";
                    foreach (ManagementObject item in mos.GetInstances())
                    {
                        if (item[FreePhysicalMemory] != null)
                            availe = long.Parse(item[FreePhysicalMemory].ToString()) * 1024;
                    }

                    float CPU = sysCPU.NextValue();
                    float sysMemory = (physicalMemory - availe) / 1024 / 1024 / 1024;
                    string msg = $"{DateTime.Now.ToString()} CPU:{CPU:f1}% Memory:{sysMemory:N}GB Down:{getNet(downLoads):f3}MB Up:{getNet(upLoads):f3}MB   ";
                    Console.Write(goBack);
                    Console.Write(msg);
                    writeLog(msg);
                    Thread.Sleep(timeSpan);
                }
            }
            catch { }
        }
        static void lookAtProcess(string proName)
        {
            Process pro = getProcessByName(proName);
            if (pro != null)
            {
                Console.WriteLine($"Taget: {proName}");
                string categoryName = "Process";
                string memoryCounterName = "Working Set - Private";
                string cpuCounterName = "% Processor Time";
                string diskReadCounterName = "IO Read Bytes/sec";
                string diskWirteCounterName = "IO Write Bytes/sec";
                PerformanceCounter memory = new PerformanceCounter(categoryName, memoryCounterName, proName);
                PerformanceCounter cpu = new PerformanceCounter(categoryName, cpuCounterName, proName);
                PerformanceCounter diskRead = new PerformanceCounter(categoryName, diskReadCounterName, proName);
                PerformanceCounter diskWrite = new PerformanceCounter(categoryName, diskWirteCounterName, proName);

                try
                {
                    while (true)
                    {
                        string msg = $"{DateTime.Now.ToString()} CPU:{getCPU(cpu):f1}% Memory:{getMemory(memory):f1} Read:{getRW(diskRead):f1}KB Write:{getRW(diskWrite):f1}KB Down:{getNet(downLoads):f3}MB Up:{getNet(upLoads):f3}MB   ";
                        writeLog(msg);
                        Console.Write(goBack);
                        Console.Write(msg);
                        Thread.Sleep(timeSpan);
                    }
                }
                catch { }
            }
        }
        static float getNet(List<PerformanceCounter> netDU)
        {
            float res = 0;
            foreach (PerformanceCounter item in netDU)
            {
                res += item.NextValue();
            }
            return res / 1024 / 1024;//MB
        }
        static void writeLog(string msg)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter("aaa.txt", true);
            sw.WriteLine(msg);
            sw.Close();
        }
        static Process getProcessByName(string name)
        {
            Process res = null;
            Process[] all = Process.GetProcessesByName(name);
            if (all.Length > 0) res = all[0];
            return res;
        }
        static float getCPU(PerformanceCounter counter)
        {
            return counter.NextValue() / Environment.ProcessorCount;//%
        }
        static float getMemory(PerformanceCounter counter)
        {
            return counter.NextValue() / 1024 / 1024 / 1024;//GB
        }
        static float getRW(PerformanceCounter counter)
        {
            return counter.NextValue() / 1024;//KB
        }
    }
}
