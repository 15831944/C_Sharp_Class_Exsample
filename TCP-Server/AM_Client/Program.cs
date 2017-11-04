using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AM_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient();
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 22215;
            client.Connect(ip, port);

            NetworkStream clientStream = client.GetStream();
            byte[] requestBuffer = Encoding.ASCII.GetBytes("msg");
            clientStream.Write(requestBuffer, 0, requestBuffer.Length);

            waitBack(client);

            clientStream.Close();
            client.Close();
        }
        static string waitBack(TcpClient client)
        {

            NetworkStream clientStream = client.GetStream();
            //read response  
            byte[] responseBuffer = new byte[1024];
            MemoryStream memStream = new MemoryStream();
            int bytesRead = 0;
            do
            {
                bytesRead = clientStream.Read(responseBuffer, 0, 1024);
                memStream.Write(responseBuffer, 0, bytesRead);

            } while (bytesRead > 0);

            byte[] buffer = memStream.GetBuffer();
            return Encoding.ASCII.GetString(buffer);
        }
    }
}
