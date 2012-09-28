using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;

namespace getGradesForms
{
    public partial class ConnectionControl : UserControl
    {
        public ConnectionControl()
        {
            InitializeComponent();
            connect();
            timer1.Enabled = true;
        }
        
        internal bool isConnected { get { return s != null && s.Connected && session != null; } }
        private String session { get; set; }
        
        static internal Encoding hebrewEncoding = Encoding.GetEncoding("iso-8859-8-i");

        public delegate void Tick();
        public event Tick tick = delegate { };

        const string host = "techmvs.technion.ac.il";
        const string path = "/cics/wmn/wmngrad";

        private Socket s;
        NetworkStream ns;
        private StreamWriter output;
        private StreamReader input;

        public string status { get; private set; }

        private void send(string op, string postdata = "", string suff = "ORD=1")
        {
            if (output == null) {
                status = "שגיאה כללית";
                return;
            }
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
            }
            catch (IOException) {
                //throw new ConnectionError();
                status = "שגיאת חיבור";
            }
        }

        private int redirect()
        {
            if (input == null)
                return -1;
            try {
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
            }
            catch (WebException) {
                status = "שגיאת דימה";
                //throw new DimaErorr();
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
                output.AutoFlush = true;
                this.input = new StreamReader(ns);
                tick();
                int NUM_OF_REDIRECT = 60;
                for (int i = 0; i < NUM_OF_REDIRECT; i++) {
                    if (redirect() > 0) {
                        status = "מחובר לUG";
                        return;
                    }
                }
            }
            catch (SocketException) {
                
                //throw new ConnectionError();
            }
            status = "שגיאת חיבור";
        }


        internal String retrieveHTML(string userid, string password)
        {
            if (s == null || !s.Connected)
                connect();
            try {
                authenticate(userid, password);

                String html = validateFormat(readInput());
                if (html == "")
                    throw new BadHtmlFormat();
                status = "הורדת המסמך הושלמה";
                return html;
            }
            catch (SocketException) {
                status = "שגיאת חיבור";
               // throw new ConnectionError();
                return "";
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
            status = "הזדהות הושלמה";
        }

        private string validateFormat(string html)
        {
            string fmtcellcontent = @"(\p{IsHebrew}|\p{Nd}|\w|[\.=*\-'()/" + "\"" +@"]|&nbsp;|\s)*";
            string fmtcell = @"(\s*<td.*>" + fmtcellcontent + @"</td>\s*)";
            string fmtline = @"(\s*<tr.*>" + fmtcell + @"+</tr>\s*)";
            string fmttable = @"(\s*<TABLE.*>" + fmtline + @"+</TABLE>\s*)";
            string fmthtml = @"(?<=.*)<HTML>.*<P>(" + fmttable + "<BR>)+" + fmttable + @"</DIV>\s*</BODY>\s*</HTML>";
            return Regex.Match(html, "(?<=.*)<HTML>.*<P>(\\s*<TABLE.*</TABLE>\\s*<BR>)+(\\s*<TABLE.*</TABLE>\\s*)</DIV>\\s*</BODY>\\s*</HTML>", RegexOptions.Singleline |RegexOptions.IgnoreCase | RegexOptions.Compiled).Value;
            //return Regex.Match(html, fmthtml, RegexOptions.Singleline | RegexOptions.IgnoreCase).Value;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!s.Connected)
                connect();
            redirect();
        }
        /*
        public override void Dispose()
        {
            if (output != null) output.Dispose();
            if (input != null) input.Dispose();
            if (ns != null) ns.Dispose();
        }
        */
    }

    [Serializable]
    public class ConnectionException : System.ApplicationException { }

    [Serializable]
    public class ConnectionError : ConnectionException { }

    [Serializable]
    public class BadHtmlFormat : ConnectionException { }

    [Serializable]
    public class DimaErorr : ConnectionException { }

}
