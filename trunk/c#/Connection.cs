using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.ComponentModel;
using System.Windows.Forms;

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
            try {
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
            catch (IOException) {
                throw new ConnectionError();
            }
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
            try {
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
            }
            catch (SocketException) {
                throw new ConnectionError();
            }
        }


        internal String retrieveHTML(string userid, string password)
        {
            if (!s.Connected)
                connect();

            try {
                authenticate(userid, password);


                String html = validateFormat(readInput());

                if (html == "")
                    throw new BadHtmlFormat();
                return html;
            }
            catch (SocketException) {
                throw new ConnectionError();
            }
        }

        private string readInput()
        {
            var reader = new StreamReader(ns, hebrewEncoding);
            s.Disconnect(false);
            return reader.ReadToEnd();
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
            if (output != null) output.Dispose();
            if (input != null) input.Dispose();
            if (ns != null) ns.Dispose();
        }

    }

}
