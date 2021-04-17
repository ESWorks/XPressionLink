using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace XPressionService
{
    public class XPressionWCFClient
    {
        IXPressionWCF srv;
        DuplexChannelFactory<IXPressionWCF> cf;
        bool manual_close = false;
        public void Create(Action<long> handle, string host, string port)
        {

            EndpointAddress end = new EndpointAddress("net.tcp://" + host + ":" + port + "/XPressionService");
            NetTcpBinding tcp = new NetTcpBinding();
            CallbackHandler handler = new CallbackHandler(handle);
            ServiceEndpoint svend = new ServiceEndpoint(new ContractDescription("XPressionService", "XPressionService.IXPressionWCF"),tcp, end);

            cf = new DuplexChannelFactory<IXPressionWCF>(handler, tcp, end);
            srv = cf.CreateChannel();
            manual_close = false;
            cf.Closed += Cf_Closed;
            cf.Faulted += Cf_Faulted;
            
        }

        private void Cf_Faulted(object sender, EventArgs e)
        {
            srv = cf.CreateChannel();
        }

        private void Cf_Closed(object sender, EventArgs e)
        {
            if(!manual_close)
            {
                srv = cf.CreateChannel();
            }
        }

        public Image GetSceneImage(string scene, int start, int height, int width)
        {
            return StringToImage(Service.GetSceneImage(scene, start, height, width));
        }

        public Bitmap StringToImage(string imageString)
        {
            if (imageString == null) throw new ArgumentNullException("imageString");
            byte[] array = Convert.FromBase64String(imageString);
            Bitmap image = (Bitmap)Bitmap.FromStream(new MemoryStream(array));
            return image;
        }
        public void Destroy()
        {
            manual_close = true;
            cf.Close();
            cf = null;
            srv = null;
        }

        public IXPressionWCF Service => srv;

        public bool Registered()
        {
            return cf?.State == CommunicationState.Opened;
        }

        public void Reconnect()
        {
            
            srv = cf.CreateChannel();
        }
    }
}
