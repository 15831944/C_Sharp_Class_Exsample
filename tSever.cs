using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace PictureServer
{

    class Program
    {
        static void Main(string[] args)
        {
            string hostName = Dns.GetHostName();
            int port = 22215;
            IPAddress[] ips = Dns.GetHostAddresses(hostName);
            IPAddress ip = null;
            for (int i = 0; i < ips.Length; i++)
            {
                if (ips[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = ips[i];
                    break;
                }
            }
            if (ip != null)
                Console.WriteLine("hostAddress : {0}:{1}", ip, port);
            else
            {
                Console.WriteLine("未能获取本地IP地址");
                return;
            }
            TcpListener listener = new TcpListener(ip, port);
            listener.Start();
            Console.WriteLine("Server running...");

            while (true)
            {
                const int bufferSize = 8192;
                //如果没有用户连接，则在这一步挂起，进入等待状态！
                TcpClient client = listener.AcceptTcpClient();

                //如果有连接请求则进入这步，开始工作！
                NetworkStream clientStream = client.GetStream();

                byte[] buffer = new byte[bufferSize];
                int readBytes = 0;
                readBytes = clientStream.Read(buffer, 0, bufferSize);

                string request = Encoding.ASCII.GetString(buffer).Substring(0, readBytes);
                severDo sd = new severDo();
                if (request.IndexOf("11") > 0)
                {
                    byte[] responseBuffer = sd.getAnsers1();
                    clientStream.Write(responseBuffer, 0, responseBuffer.Length);
                    Console.WriteLine(request + "1111");
                }
                else if (request.IndexOf("22") > 0)
                {
                    byte[] data = sd.getAnsers2();
                    clientStream.Write(data, 0, data.Length);
                    Console.WriteLine(request + "2222");
                }
                else
                {
                    Console.WriteLine(request + "haha");
                }
                clientStream.Close();
            }
        }
    }
    class severDo
    {
        public severDo() { }

        public byte[] getAnsers1()
        {
            string as1 = "111111";
            byte[] ass1 = Encoding.ASCII.GetBytes(as1);
            return ass1;
        }
        public byte[] getAnsers2()
        {
            string as2 = "22222";
            byte[] ass2 = Encoding.ASCII.GetBytes(as2);
            return ass2;
        }
    }
}

