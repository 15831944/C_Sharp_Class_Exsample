using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCP_Client
{
    public partial class Form1 : Form
    {
        private const int bufferSize = 4096;
        public Form1()
        {
            InitializeComponent();
        }
   
        private void button1_Click(object sender, EventArgs e)
        {
            //send data  
            TcpClient client = ConnectToServer();
            NetworkStream clientStream = client.GetStream();
            byte[] requestBuffer = Encoding.ASCII.GetBytes(textBox1.Text);
            clientStream.Write(requestBuffer, 0, requestBuffer.Length);
            
            waitBack(client);

            clientStream.Close();
            client.Close();
        }
        private TcpClient ConnectToServer()
        {
            TcpClient client = new TcpClient();
            IPAddress ip = IPAddress.Parse("192.168.0.105");
            int port = 22215;
            client.Connect(ip, port);
            return client;
        }
        void waitBack(TcpClient client)
        {
           
            NetworkStream clientStream = client.GetStream();
            //read response  
            byte[] responseBuffer = new byte[bufferSize];
            MemoryStream memStream = new MemoryStream();
            int bytesRead = 0;
            do
            {
                bytesRead = clientStream.Read(responseBuffer, 0, bufferSize);
                memStream.Write(responseBuffer, 0, bytesRead);

            } while (bytesRead > 0);
            
            byte[] buffer = memStream.GetBuffer();
            textBox2.Text = Encoding.ASCII.GetString(buffer);
        }
    }
}
