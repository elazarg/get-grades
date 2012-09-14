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

        private void process()
        {
            state = State.PROCESSING;
            var pr = new Processor(html);
            pr.sessionFound         += this.dataSet.addSessionToSQL;
            pr.semesterFound        += this.dataSet.addSemester;
            pr.personalDetailsFound += this.dataSet.addPersonalDetails;
            pr.semesterFinished     += this.dataSet.addEndSemester;
            pr.processText();
            html = pr.fixedHtml;

            dataSet.updateCleanSlate();
        }

        public Grades(string userid, string password, BackgroundWorker bw)
        {
            dataSet.init();
            this.bw = bw;

            this.state = State.READY;

            connectAndDownload(userid, password);
            process();
            state = State.DONE;
        }

        public void saveCsvFile(string fileName)
        {
            var txt = SString.Join("\r\n", dataSet.cleanView.Select(row => SString.Join(" , ", new string[] { row.course.ToString(), row.grade.ToString() })));
            File.WriteAllText(fileName, txt, Connection.hebrewEncoding);
        }

        internal void logOut()
        {
            html = "";
            dataSet.Clear();
        }
    }
}
