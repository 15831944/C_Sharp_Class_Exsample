using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace TCP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 22215;

            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("Server running...");

            const int bufferSize = 4096;
            while (true)
            {
                //如果没有用户连接，则在这一步挂起，进入等待状态！
                TcpClient client = listener.AcceptTcpClient();
                string clientIP = client.Client.RemoteEndPoint.ToString();
                //如果有连接请求则进入这步，开始工作！
                NetworkStream clientStream = client.GetStream();

                byte[] buffer = new byte[bufferSize];
                int readBytes = clientStream.Read(buffer, 0, bufferSize);
                string request = Encoding.UTF8.GetString(buffer).Substring(0, readBytes);

                //执行操作
                Console.WriteLine(request);

                //回传消息
                //byte[] backData = Encoding.ASCII.GetBytes(request.ToUpper());
                //clientStream.Write(backData, 0, backData.Length);

                clientStream.Close();
            }
        }
    }
}

