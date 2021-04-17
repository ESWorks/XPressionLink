
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XPressionHost
{
    public class Webhost
    {
        private string[] _indexFiles = {
            "index.html",
            "index.htm",
            "default.html",
            "default.htm"
        };

        public bool DataCommand;

        private static readonly IDictionary<string, string> MimeTypeMappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
        #region extension to MIME type list
        {".asf", "video/x-ms-asf"},
        {".asx", "video/x-ms-asf"},
        {".avi", "video/x-msvideo"},
        {".bin", "application/octet-stream"},
        {".cco", "application/x-cocoa"},
        {".crt", "application/x-x509-ca-cert"},
        {".css", "text/css"},
        {".deb", "application/octet-stream"},
        {".der", "application/x-x509-ca-cert"},
        {".dll", "application/octet-stream"},
        {".dmg", "application/octet-stream"},
        {".ear", "application/java-archive"},
        {".eot", "application/octet-stream"},
        {".exe", "application/octet-stream"},
        {".flv", "video/x-flv"},
        {".gif", "image/gif"},
        {".hqx", "application/mac-binhex40"},
        {".htc", "text/x-component"},
        {".htm", "text/html"},
        {".html", "text/html"},
        {".ico", "image/x-icon"},
        {".img", "application/octet-stream"},
        {".iso", "application/octet-stream"},
        {".jar", "application/java-archive"},
        {".jardiff", "application/x-java-archive-diff"},
        {".jng", "image/x-jng"},
        {".jnlp", "application/x-java-jnlp-file"},
        {".jpeg", "image/jpeg"},
        {".jpg", "image/jpeg"},
        {".js", "application/x-javascript"},
        {".mml", "text/mathml"},
        {".mng", "video/x-mng"},
        {".mov", "video/quicktime"},
        {".mp3", "audio/mpeg"},
        {".mpeg", "video/mpeg"},
        {".mpg", "video/mpeg"},
        {".msi", "application/octet-stream"},
        {".msm", "application/octet-stream"},
        {".msp", "application/octet-stream"},
        {".pdb", "application/x-pilot"},
        {".pdf", "application/pdf"},
        {".pem", "application/x-x509-ca-cert"},
        {".pl", "application/x-perl"},
        {".pm", "application/x-perl"},
        {".png", "image/png"},
        {".prc", "application/x-pilot"},
        {".ra", "audio/x-realaudio"},
        {".rar", "application/x-rar-compressed"},
        {".rpm", "application/x-redhat-package-manager"},
        {".rss", "text/xml"},
        {".run", "application/x-makeself"},
        {".sea", "application/x-sea"},
        {".shtml", "text/html"},
        {".sit", "application/x-stuffit"},
        {".swf", "application/x-shockwave-flash"},
        {".tcl", "application/x-tcl"},
        {".tk", "application/x-tcl"},
        {".txt", "text/plain"},
        {".war", "application/java-archive"},
        {".wbmp", "image/vnd.wap.wbmp"},
        {".wmv", "video/x-ms-wmv"},
        {".xml", "text/xml"},
        {".xpi", "application/x-xpinstall"},
        {".zip", "application/zip"},
        #endregion
    };

        private Thread _serverThread;
        private string _rootDirectory;
        private HttpListener _listener;
        private int _port;
        private readonly Dictionary<string, PageEvent> _pagesDictionary = new Dictionary<string, PageEvent>();

        public delegate object PageEvent(string filename);

        public delegate void ConsoleEntry(string entry);

        public ConsoleEntry Console;

        public void AddPage(string pagename, PageEvent eventname)
        {
            if (eventname == null) throw new ArgumentNullException(nameof(eventname));
            _pagesDictionary[pagename.ToLower()] = eventname;
        }

        public void RemovePage(string pagename)
        {
            if (_pagesDictionary.ContainsKey(pagename.ToLower()))
            {
                _pagesDictionary.Remove(pagename.ToLower());
            }
        }
        public int Port
        {
            get { return _port; }
            private set { }
        }

        /// <summary>
        /// Construct server with given port.
        /// </summary>
        /// <param name="path">Directory path to serve.</param>
        /// <param name="port">Port of the server.</param>
        /// <param name="serverForm"></param>
        public Webhost(string path, int port)
        {
            this.Initialize(path, port);
        }

        public void SetIndexFiles(string[] index)
        {
            _indexFiles = index;
        }

        /// <summary>
        /// Construct server with suitable port.
        /// </summary>
        /// <param name="path">Directory path to serve.</param>
        public Webhost(string path)
        {
            //get an empty port
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();

            this.Initialize(path, port);
        }

        /// <summary>
        /// Stop server and dispose all functions.
        /// </summary>
        public void Stop()
        {
            _serverThread.Abort();
            _listener.Stop();
        }

        private void Listen()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://127.0.0.1:" + _port.ToString() + "/");
            _listener.Start();
            while (true)
            {
                try
                {
                    HttpListenerContext context = _listener.GetContext();
                    Process(context);
                }
                catch (Exception ex)
                {
                    Console?.Invoke(ex.Message);
                }
            }
        }

        private string GetFileNameFromUrl(string url)
        {
            var uri = new Uri(url);
            return uri.Segments.Last();
        }

        private void Process(HttpListenerContext context)
        {
            string pagename = context.Request.Url.AbsolutePath;

            System.Console.WriteLine(pagename);

            pagename = pagename.Substring(1);

            System.Console.WriteLine(pagename);

            Console?.Invoke("Checking for action" + pagename);

            string pagekey = FindPage(pagename);
            if (!string.IsNullOrEmpty(pagekey))
            {
                Console?.Invoke("Action found. " + pagename.ToLower());
                Stream input = ConvertObjectToStream(_pagesDictionary[pagekey].Invoke(pagename));

                //Adding permanent http response headers
                string mime;
                context.Response.ContentType = MimeTypeMappings.TryGetValue(Path.GetExtension(pagename), out mime) ? mime : "application/octet-stream";
                context.Response.ContentLength64 = input.Length;
                context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                context.Response.AddHeader("Last-Modified", System.IO.File.GetLastWriteTime(pagename).ToString("r"));

                byte[] buffer = new byte[1024 * 16];
                int nbytes;
                while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    context.Response.OutputStream.Write(buffer, 0, nbytes);
                input.Close();

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.OutputStream.Flush();
            }
            else
            {
                Console?.Invoke("No page found. " + pagename.ToLower());
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            context.Response.OutputStream.Close();
        }

        private string FindPage(string pagename)
        {
            foreach (string key in _pagesDictionary.Keys)
            {
                if (pagename.Contains(key))
                {
                    return key;
                }
            }
            return string.Empty;
        }

        public Stream ConvertObjectToStream(object value)
        {
            Console?.Invoke("Creating Stream Data.");
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(value);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private void Initialize(string path, int port)
        {
            Console?.Invoke("Initializing Web Server.");
            this._rootDirectory = path;
            this._port = port;
            _serverThread = new Thread(Listen);
            _serverThread.IsBackground = true;
            _serverThread.Start();
        }


    }
}



