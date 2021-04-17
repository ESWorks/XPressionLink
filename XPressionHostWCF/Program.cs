using System;
using System.ServiceModel;
using XPressionService;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Windows.Forms;

namespace XPressionHostWCF
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Immutable.Create();
                //CreateRemoteService();
                XPressionWCFServer service = new XPressionWCFServer();
                service.Create("9000");
                service.Open();

                Console.WriteLine("Service Started - " + DateTime.Now.ToString());

                string result;
                do
                {
                    result = Console.ReadLine();
                    if (result == "R")
                    {
                        Process.Start("server.exe");
                        Environment.Exit(0);

                    }
                }
                while (result != "Q");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new ServerForm());
        //}

        private static void CreateRemoteService()
        {
            XPressionRemote remoteService = new XPressionRemote();


            // We create our own provider because we need to set the TYpeFilterLevel to full

            // Without this, we can not pass objects to remote procedure calls.

            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            IDictionary props = new Hashtable();
            props["port"] = 9000;
            props["name"] = "tcp://localhost:9000/XPressionService/";

            // Register as an available service with the name 
            TcpChannel channel = new TcpChannel(props, null, provider);

            ChannelServices.RegisterChannel(channel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(typeof(XPressionRemote), "XPressionService", WellKnownObjectMode.Singleton);

            Console.WriteLine("Service Started - " + DateTime.Now.ToString());
            remoteService.CreateEngine();
            remoteService.CreateTimer("Score Clock Timer Widget");
        }
    }
}
