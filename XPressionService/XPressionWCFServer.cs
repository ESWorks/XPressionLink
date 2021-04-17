using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace XPressionService
{
    public class XPressionWCFServer
    {
        ServiceHost serviceHost;
        public void Create(string port)
        {
            serviceHost = new ServiceHost(typeof(XPressionWCF));
            //exp = new XPressionWCF();
            //serviceHost = new ServiceHost(exp);
            serviceHost.AddServiceEndpoint(typeof(IXPressionWCF), new NetTcpBinding(), "net.tcp://localhost:" + port + "/XPressionService");
             
            serviceHost.UnknownMessageReceived += ServiceHost_UnknownMessageReceived;
            serviceHost.Closed += ServiceHost_Closed;
            serviceHost.Closing += ServiceHost_Closing;
            serviceHost.Faulted += ServiceHost_Faulted;
        }

        private void ServiceHost_Faulted(object sender, EventArgs e)
        {
            Close();
            Open();
        }

        private void ServiceHost_Closing(object sender, EventArgs e)
        {
            
        }

        private void ServiceHost_Closed(object sender, EventArgs e)
        {
            
        }

        private void ServiceHost_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            
        }

        public bool Running => serviceHost != null && serviceHost.State == CommunicationState.Opened;

        public void Open()
        {
            Immutable.Create();
            try
            {
                Immutable.Engine = new Graphic();
            }
            catch
            {
                Console.WriteLine("Can't instantiate graphics engine.");
            }
            serviceHost.Open();
            //((XPressionWCF)serviceHost.SingletonInstance).CreateTimer("Score Clock Timer Widget");
            Console.WriteLine(String.Format("The WCF server is ready at localhost"));
            Console.WriteLine();
        }

        public void Close()
        {
            serviceHost.Close();
        }
    }
}
