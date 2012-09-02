using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;

namespace getGradesForms
{
    class Connection : IDisposable
    {
        internal bool isConnected { get { return s.Connected && session != null; } }
        public String session { get; private set; }


        static internal Encoding hebrewEncoding = Encoding.GetEncoding("iso-8859-8-i");
        
        public delegate void Tick();
        public event Tick tick = delegate { };

        const string host = "techmvs.technion.ac.il";
        const string path = "/cics/wmn/wmngrad";

        private Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        NetworkStream ns;
        private StreamWriter output;
        private StreamReader input;
        private StreamReader input_hebrew;

        private void send(string op, string postdata = "", string suff = "ORD=1")
        {
            tick();
            string dest = path + "?" + session + "&" + suff;
            output.WriteLine(op + " " + dest + " HTTP/1.1");
            output.WriteLine("Host: " + host);
            if (postdata != null)
            {
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
            do
            {
                tick();
                temp = input_hebrew.ReadLine();
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
            string res = input.ReadLine();
            if (res.Contains("302"))
            {
                input.ReadLine();
                res = input.ReadLine();
                session = res.Split(sep, 3, StringSplitOptions.None)[1].Substring(1, 8);
                skipBlock();
                return -1;
            }
            else if (res.Contains("200"))
            {
                String len;
                do
                {
                    len = input.ReadLine();
                } while (!len.Contains("Content-Length:"));
                return System.Int32.Parse(len.Substring(16));
            }
            return 0;
        }

        private void skipBlock()
        {
            while (input.ReadLine() != "") ;
        }


        private void skipLines(int num)
        {
            for (; num > 0; num-- )
                input_hebrew.ReadLine();
        }

        internal Connection()
        {
            this.s.Connect(host, 80);

            ns = new NetworkStream(s);
            this.output = new StreamWriter(ns);
            this.input = new StreamReader(ns);
            this.input_hebrew = new StreamReader(ns, hebrewEncoding);
            tick();            
            
            while (redirect() < 0) ;
        }


        internal TextReader retrieveHTML(string userid, string password)
        {
            send("POST", "function=signon&userid=" + userid + "&password=" + password);

            redirect();
            int len = redirect();
            tick();
            skipBlock();
            skipBlock();
            send("GET");
            skipBlock();

            s.Disconnect(false);
            return input_hebrew;
        }

        public void Dispose()
        {
            output.Dispose();
            input.Dispose();
            ns.Dispose();
        }
    }

}
