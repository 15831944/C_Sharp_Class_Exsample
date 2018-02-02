using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Net;

namespace SOCKET实现TCP_IP协议的通讯
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    /// <summary>
    /// 客户端
    /// </summary>
    public class Test
    {
        protected TcpClient tcpclient { set; get; } //全局客户端对象  
        protected NetworkStream networkstream { set; get; }//全局数据流传输对象  

        /// <summary>  
        /// 进行远程服务器的连接  
        /// </summary>  
        /// <param name="ip">ip地址</param>  
        /// <param name="port">端口</param>  
        public Test(string ip, int port)
        {
            tcpclient = new TcpClient();  //对象转换成实体  

            AsyncCallback cback = new AsyncCallback(Connected);

            //开始进行尝试连接  
            IAsyncResult res = tcpclient.BeginConnect(IPAddress.Parse(ip), port, cback, tcpclient);  
        }
        /// <summary>
        /// 发送数据  
        /// </summary>
        /// <param name="data"></param>
        public void SendData(byte[] data)
        {
            if (networkstream != null)
                networkstream.Write(data, 0, data.Length);  //向服务器发送数据  
        }
        /// <summary>  
        /// 关闭  
        /// </summary>  
        public void Close()
        {
            networkstream.Dispose(); //释放数据流传输对象  
            tcpclient.Close(); //关闭连接  
        }
        /// <summary>  
        /// 连接回调
        /// </summary>  
        /// <param name="result">传入参数</param>  
        protected void Connected(IAsyncResult result)
        {
            TcpClient tcpclt = (TcpClient)result.AsyncState;  //将传递的参数强制转换成TcpClient  
            networkstream = tcpclt.GetStream();  //获取数据流传输对象  
            byte[] data = new byte[1000];  //新建传输的缓冲  
            networkstream.BeginRead(data, 0, 1000, new AsyncCallback(DataRec), data); //挂起数据的接收等待  
        }
        /// <summary>  
        /// 数据接收委托函数  
        /// </summary>  
        /// <param name="result">传入参数</param>  
        protected void DataRec(IAsyncResult result)
        {
            int length = networkstream.EndRead(result);  //获取接收数据的长度  
            List<byte> data = new List<byte>(); //新建byte数组  
            data.AddRange((byte[])result.AsyncState); //获取数据  
            data.RemoveRange(length, data.Count - length); //根据长度移除无效的数据  
            byte[] data2 = new byte[1000]; //重新定义接收缓冲  
            networkstream.BeginRead(data2, 0, 1000, new AsyncCallback(DataRec), data2);  //重新挂起数据的接收等待  
                                                                                         //自定义代码区域，处理数据data  
            if (length == 0)
            {
                //连接已经关闭  
            }
        }
    }

    /// <summary>
    /// 服务器监听
    /// </summary>
    public class TSever
    {
        public List<TClient> Clients = new List<TClient>();  //客户端列表  
        private TcpListener tcplistener = null;  //侦听对象  
        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="port">侦听端口</param>  
        public TSever(int port)
        {
            tcplistener = new TcpListener(System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName())[0], port);  //启动侦听  
            tcplistener.Start(); //启动侦听  
            tcplistener.BeginAcceptTcpClient(new AsyncCallback(ClientAccept), tcplistener); //开始尝试客户端的连接  
        }
        private void ClientAccept(IAsyncResult result)
        {
            TcpListener tcplst = (TcpListener)result.AsyncState;
            TcpClient bak_tcpclient = tcplst.EndAcceptTcpClient(result);
            TClient bak_client = new TClient(bak_tcpclient, this);
            Clients.Add(bak_client);
            tcplst.BeginAcceptTcpClient(new AsyncCallback(ClientAccept), tcplst);
        }
    }

    /// <summary>
    /// 服务器回应
    /// </summary>
    public class TClient
    {
        private TcpClient tcpclient = null;  //客户端对象  
        private NetworkStream networkstream = null;  //数据发送对象  
        private TSever m_Parent = null;  //父级类  
        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="tcpclt">客户端对象</param>  
        /// <param name="parent">父级</param>  
        public TClient(TcpClient tcpclt, TSever parent)
        {
            this.tcpclient = tcpclt;
            this.m_Parent = parent;
            string ip = ((IPEndPoint)tcpclient.Client.RemoteEndPoint).Address.ToString(); //获取客户端IP  
            string port = ((IPEndPoint)tcpclient.Client.RemoteEndPoint).Port.ToString();  //获取客户端端口  
            this.networkstream = tcpclt.GetStream();  //获取数据传输对象  
            byte[] data = new byte[1024];
            this.networkstream.BeginRead(data, 0, 1024, new AsyncCallback(DataRec), data);//启动数据侦听  
        }
        /// <summary>  
        /// 数据接收  
        /// </summary>  
        /// <param name="result"></param>  
        private void DataRec(IAsyncResult result)
        {
            int length = networkstream.EndRead(result);
            List<byte> data = new List<byte>();
            data.AddRange((byte[])result.AsyncState);
            byte[] data2 = new byte[1024];
          //  networkstream.BeginRead(data2, 0, MaxRec, new AsyncCallback(DataRec), data2);
            if (length == 0)
            {
                m_Parent.Clients.Remove(this);  //告知父类删除此客户端  
            }
            else
            {
                data.RemoveRange(length, data.Count - length);
                //数据处理代码data  
            }
        }
        /// <summary>  
        /// 发送数据  
        /// </summary>  
        /// <param name="data">数据</param>  
        /// <returns></returns>  
        public bool SendData(byte[] data)
        {
            networkstream.Write(data, 0, data.Length);
            return (true);
        }
    }





    //http://blog.csdn.net/a497785609/article/details/12871301#

    /*
        1.客户端：
        
      第一步，要创建一个客户端对象TcpClient(命名空间在System.Net.Sockets)，接着，调用对象下的方法BeginConnect进
      行尝试连接，入口参数有4个，address(目标IP地址)，port(目标端口号)，requestCallback(连接成功后的返调函数)，
      state(传递参数，是一个对象，随便什么都行，我建议是将TcpClient自己传递过去)，调用完毕这个函数，系统将进行
      尝试连接服务器。

      第二步，在第一步讲过一个入口参数requestCallback(连接成功后的返调函数)，比如我们定义一个函数void Connected(IAsyncResult result),
      在连接服务器成功后，系统会调用此函数，在函数里，我们要获取到系统分配的数据流传输对象(NetworkStream)，这
      个对象是用来处理客户端与服务器端数据传输的，此对象由TcpClient获得，在第一步讲过入口参数state，如果我们传
      递了TcpClient进去，那么，在函数里我们可以根据入口参数state获得，将其进行强制转换TcpClient tcpclt = (TcpClient)result.AsyncState，
      接着获取数据流传输对象NetworkStream ns = tcpclt.GetStream()，此对象我建议弄成全局变量，以便于其他函数调用，
      接着我们将挂起数据接收等待，调用ns下的方法BeginRead，入口参数有5个，buff(数据缓冲)，offset(缓冲起始序号)，
      size(缓冲长度)，callback(接收到数据后的返调函数)，state(传递参数，一样，随便什么都可以，建议将buff传递过去)，
      调用完毕函数后，就可以进行数据接收等待了，在这里因为已经创建了NetworkStream对象，所以也可以进行向服务器发送
      数据的操作了，调用ns下的方法Write就可以向服务器发送数据了，入口参数3个，buff(数据缓冲)，offset(缓冲起始序号)，
      size(缓冲长度)。

      第三步，在第二步讲过调用了BeginRead函数时的一个入口参数callback(接收到数据后的返调函数)，比如我们定义了一个
      函数void DataRec(IAsyncResult result)，在服务器向客户端发送数据后，系统会调用此函数，在函数里我们要获得数据
      流(byte数组)，在上一步讲解BeginRead函数的时候还有一个入口参数state，如果我们传递了buff进去，那么，在这里我
      们要强制转换成byte[]类型byte[] data= (byte[])result.AsyncState，转换完毕后，我们还要获取缓冲区的大
      小int length = ns.EndRead(result)，ns为上一步创建的NetworkStream全局对象，接着我们就可以对数据进行处理了，
      如果获取的length为0表示客户端已经断开连接。


      2.服务器端： 
      
    相对于客户端的实现，服务器端的实现稍复杂一点，因为前面讲过，一个服务器端可以接受N个客户端的连接，因此，在服务器端，
    有必要对每个连接上来的客户端进行登记，因此服务器端的程序结构包括了2个程序结构，第一个程序结构主要负责启动服务器、
    对来访的客户端进行登记和撤销，因此我们需要建立2个类。
    第一个程序结构负责服务器的启动与客户端连接的登记，首先建立TcpListener网络侦听类，建立的时候构造函数分别包括localaddr
    和port2个参数，localaddr指的是本地地址，也就是服务器的IP地址，有人会问为什么它自己不去自动获得本机的地址？关于这个举
    个很简单的例子，服务器安装了2个网卡，也就有了2个IP地址，那建立服务器的时候就可以选择侦听的使用的是哪个网络端口了，不
    过一般的电脑只有一个网络端口，你可以懒点直接写个固定的函数直接获取IP地
    址System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName())[0]，GetHostAddresses函数就是获取本机的IP地址，默认选
    择第一个端口于是后面加个[0]，第2个参数port是真侦听的端口，这个简单，自己决定，如果出现端口冲突，函数自己会提醒错误的。
    第二步，启动服务器，TcpListener.Start()。
    第三步，启动客户端的尝试连接，TcpListener.BeginAcceptTcpClient，入口2个参数，callback(客户端连接上后的返调函数)，
    state(传递参数，跟第二节介绍的一样，随便什么都可以，建立把TcpListener自身传递过去)，
    第四步，建立客户端连接上来后的返调函数，比如我们建立个名为void ClientAccept(IAsyncResult result)的函数，函数里，我们
    要获取客户端的对象，第三步里讲过我们传递TcpListener参数进去，在这里，我们通过入口参数获取它TcpListener tcplst = 
    (TcpListener)result.AsyncState，获取客户端对象TcpClient bak_tcpclient = tcplst.EndAcceptTcpClient(result)，这个bak_tcpclient
    我建议在类里面建立个列表，然后把它加进去，因为下一个客户端连接上来后此对象就会被冲刷掉了，客户端处理完毕后，接下来我们
    要启动下一个客户端的连接tcplst.BeginAcceptTcpClient(new AsyncCallback(sub_ClientAccept), tcplst)，这个和第三步是一样的，
    我就不重复了。

     第二个程序结构主要负责单个客户端与服务器端的处理程序，主要负责数据的通讯，方法很类似客户端的代码，基本大同，除了不需要
     启动连接的函数，因此这个程序结构主要启动下数据的侦听的功能、判断断开的功能、数据发送的功能即可，在第一个程序第四步我们
     获取了客户端的对象bak_tcpclient，在这里，我们首先启动数据侦听功能NetworkStream ns= bak_tcpclient.GetStream();
     ns.BeginRead(data, 0, 1024, new AsyncCallback(DataRec), data);这个跟我在第二节里介绍的是一模一样的(第二节第10行),还有数
     据的处理函数，数据发送函数，判断连接已断开的代码与第二节也是一模一样的，不过在这里我们需要额外的添加一段代码，当判断出
     连接已断开的时候，我们要将客户端告知第一个程序结构进行删除客户端操作，这个方法我的实现方法是在建立第二个程序结构的时候，
     将第一个程序结构当参数传递进来，判断连接断开后，调用第一个程序结构的公开方法去删除，即从客户端列表下删除此对象。
     */

}
