using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace getGradesForms
{
    class Grades
    {

        ConnectionControl conn;

        public string raw_html;
        public GradesDocument emptyDocument;
        public GradesDocument rawDocument;
        public GradesDocument document;

        public Grades()
        {
            conn = new ConnectionControl();
            emptyDocument = new GradesDocument();
        }

        public String csv
        {
            get
            {
                return SString.Join("\r\n",  document.dataSet.cleanView.Select(
                       row => SString.Join(" , ", new string[] {
                           row.courseId, row.courseName, row.grade.ToString()
                       })));
            }
        }

        public enum State {
            READY,
            CONNECTING,
            CONNECTED,
            AUTHENTICATING,
            AUTHENTICATED,
            PROCESSING,
            FAILED,
            DONE
        }

        public delegate void Tick(State state);
        public event Tick tick = delegate { };

        private State innerState; 
        public State state {
            get { return innerState; }
            private set {
                innerState = value;
                tick(innerState);
            }
        }

        internal void connect()
        {
            if (!conn.isConnected) {
                state = State.CONNECTING;
                conn.tick += delegate { tick(state); };
                conn.connect();
                state = State.CONNECTED;
            }
        }

        public void retrieve(string userid, string password) {
            state = State.AUTHENTICATING;
            raw_html = conn.retrieveHTML(userid, password);
            state = State.AUTHENTICATED;
        }

        internal void process()
        {
            state = State.PROCESSING;
            rawDocument = new GradesDocument(raw_html);
            restore();
            state = State.DONE;
        }

        internal void restore()
        {
            document = (GradesDocument)rawDocument.Clone();
        }

        public string getRawHtml()
        {
            if (document != null)
                return raw_html;
            return "";
        }


        public void saveCsvFile(string fileName)
        {
            File.WriteAllText(fileName, csv, ConnectionControl.hebrewEncoding);
        }

        internal void logOut()
        {
            raw_html = "";
            if (document != null)
                   document.clear();
            this.state = State.READY;
        }
    }
}
