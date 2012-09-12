using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace getGradesForms
{
    [Serializable]
    public class ConnectionException : System.ApplicationException { }
    
    [Serializable]
    public class ConnectionError : ConnectionException { }
    
    [Serializable]
    public class BadHtmlFormat : ConnectionException { }


    class Connection : IDisposable
    {
        #region NETWORK

        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        private extern static bool InternetGetConnectedState(ref InternetConnectionState_e lpdwFlags, int dwReserved);

        [Flags]
        enum InternetConnectionState_e : int
        {
            INTERNET_CONNECTION_MODEM = 0x1,
            INTERNET_CONNECTION_LAN = 0x2,
            INTERNET_CONNECTION_PROXY = 0x4,
            INTERNET_RAS_INSTALLED = 0x10,
            INTERNET_CONNECTION_OFFLINE = 0x20,
            INTERNET_CONNECTION_CONFIGURED = 0x40
        }

        internal static SocketError getNetworkConnectionStatus()
        {
            InternetConnectionState_e flags = 0;
            if (InternetGetConnectedState(ref flags, 0)) {
                using (var test1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP)) {
                    try {
                        test1.Connect("www.google.com", 80);
                        if (!test1.Connected)
                            return SocketError.HostUnreachable;
                        test1.Disconnect(true);
                    }
                    catch (SocketException ex) {
                        return ex.SocketErrorCode;
                    }
                }
                using (var test2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP)) {
                    try {
                        test2.Connect(host, 80);
                        if (!test2.Connected)
                            return SocketError.HostDown;
                    }
                    catch (SocketException ex) {
                        return ex.SocketErrorCode;
                    }
                    return SocketError.Success;
                }
            }
            return SocketError.NetworkDown;
        }
        #endregion


        internal bool isConnected { get { return s.Connected && session != null; } }
        public String session { get; private set; }


        static internal Encoding hebrewEncoding = Encoding.GetEncoding("iso-8859-8-i");

        public delegate void Tick();
        public event Tick tick = delegate { };

        const string host = "techmvs.technion.ac.il";
        const string path = "/cics/wmn/wmngrad";

        private Socket s;
        NetworkStream ns;
        private StreamWriter output;
        private StreamReader input;

        private void send(string op, string postdata = "", string suff = "ORD=1")
        {
            tick();
            string dest = path + "?" + session + "&" + suff;
            output.WriteLine(op + " " + dest + " HTTP/1.1");
            output.WriteLine("Host: " + host);
            if (postdata != null) {
                output.WriteLine("Content-Length: " + postdata.Length);
                output.WriteLine("Content-Type: application/x-www-form-urlencoded");
            }
            output.WriteLine();
            if (postdata != "")
                output.Write(postdata);
            output.Flush();
        }

        private string readLines(int from = 0, int count = 10000)
        {
            for (; from > 0; from--)
                input.ReadLine();
            string res = "", temp = "";
            do {
                tick();
                temp = input.ReadLine();
                if (count > 0)
                    res += temp + "\r\n";
                count--;
            } while (temp != "" && !input.EndOfStream && count != 0);
            return res;
        }

        private int redirect()
        {
            string[] sep = { "Location: http://techmvs.technion.ac.il:80/cics/wmn/wmngrad" };
            send("HEAD");
            tick();
            string temp = input.ReadLine();
            if (temp.Contains("302")) {
                input.ReadLine();
                temp = input.ReadLine();
                session = temp.Split(sep, 3, StringSplitOptions.None)[1].Substring(1, 8);
                while (input.ReadLine() != "") ;
                return -1;
            }
            else if (temp.Contains("200")) {
                String line;
                int res = -1;
                do {
                    line = input.ReadLine();
                    if (line.Contains("Content-Length:"))
                        return System.Int32.Parse(line.Substring(16));
                } while (!input.EndOfStream && line != null && line.Trim() != "");
                return res;
            }
            return 0;
        }

        internal void connect()
        {
            this.s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            this.s.Connect(host, 80);

            this.ns = new NetworkStream(s);
            this.output = new StreamWriter(ns);
            this.input = new StreamReader(ns);
            tick();
            int NUM_OF_REDIRECT = 60;
            for (int i = 0; i < NUM_OF_REDIRECT; i++) {
                if (redirect() > 0)
                    return;
            }
            throw new ConnectionError();
        }


        internal String retrieveHTML(string userid, string password)
        {
            if (!s.Connected)
                connect();

            authenticate(userid, password);

            String html = validateFormat(readInput());

            if (html == "")
                throw new BadHtmlFormat();

            return html;
        }

        private string readInput()
        {
            var reader = new StreamReader(ns, hebrewEncoding);
            StringBuilder htmlBuilder = new StringBuilder("");

            bool append = false;
            for (int i = 0; i < 700; i++) {
                string line = reader.ReadLine();

                append = append || line.ToUpper().Contains("<HTML>");

                if (append) {
                    htmlBuilder.AppendLine(line);

                    if (line.ToUpper().Contains("</HTML>"))
                        break;
                }
            }

            return htmlBuilder.ToString();
        }

        private void authenticate(string userid, string password)
        {
            send("POST", "function=signon&userid=" + userid + "&password=" + password);
            tick();
            redirect();
            redirect();
            send("GET");
        }

        private string validateFormat(string html)
        {
            return Regex.Match(html, "(?<=.*)<HTML>.*<P>(\\s*<TABLE.*</TABLE>\\s*<BR>)+(\\s*<TABLE.*</TABLE>\\s*)</DIV>\\s*</BODY>\\s*</HTML>", RegexOptions.Singleline).Value;
        }

        public void Dispose()
        {
            output.Dispose();
            input.Dispose();
            ns.Dispose();
          //  s.Dispose();
        }
    }

}
