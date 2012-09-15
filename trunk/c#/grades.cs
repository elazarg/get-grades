using System;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace getGradesForms
{
    class Grades
    {
        public readonly UGDatabase dataSet = new UGDatabase();
        public String html { get; private set; }
        public String csv
        {
            get
            {
                return SString.Join("\r\n",  dataSet.cleanView.Select(
                       row => SString.Join(" , ", new string[] {
                           row.courseId, row.courseName, row.grade.ToString()
                       })));
            }
        }

        private BackgroundWorker bw;

        public enum State {
            READY,
            CONNECTING,
            AUTHENTICATING,
            PROCESSING,
            FAILED,
            DONE
        }

        private State innerState; 
        public State state {
            get { return innerState; }
            private set {
                innerState = value;
                bw.ReportProgress(1, innerState);
            }
        }
        private void connectAndDownload(string userid, string password)
        {
            state = State.CONNECTING;
            using (Connection conn = new Connection())
            {
                conn.connect();
                conn.tick += delegate { bw.ReportProgress(10); };
                state = State.AUTHENTICATING;
                html = conn.retrieveHTML(userid, password);
            }
        }

        Processor pr = null;
        private void process()
        {
            state = State.PROCESSING;
            pr = new Processor(html);
            pr.sessionFound         += this.dataSet.addSession;
            pr.semesterFound        += this.dataSet.addSemester;
            pr.personalDetailsFound += this.dataSet.addPersonalDetails;
            pr.semesterFinished     += this.dataSet.addEndSemester;
            pr.processText();
            html = pr.fixedHtml;

            dataSet.updateCleanSlate();
        }

        public string getRawHtml()
        {
            if (pr != null)
                return pr.raw_html;
            return "";
        }

        public Grades(string userid, string password, BackgroundWorker bw)
        {
            dataSet.init();
            this.bw = bw;

            this.state = State.READY;

            connectAndDownload(userid, password);
            process();

            this.state = State.DONE;
        }

        public void saveCsvFile(string fileName)
        {
            File.WriteAllText(fileName, csv, Connection.hebrewEncoding);
        }

        internal void logOut()
        {
            html = "";
            pr = null;
            dataSet.Clear();
        }
    }
}
