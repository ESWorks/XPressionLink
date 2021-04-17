using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPressionHost
{
    class Program
    {
        static SuperSocket.SocketBase.AppServer _server;

        static void Main(string[] args)
        {
            _server = new SuperSocket.SocketBase.AppServer();
            _server.Setup("127.0.0.1", 5000);

            _server.Start();

            Console.ReadLine();
            _server.Stop();
        }

    }
}
