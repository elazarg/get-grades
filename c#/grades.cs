using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace getGradesForms
{
    class Grades : IDisposable
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
            pr.semesterFound        += this.dataSet.addSemesterToSQL;
            pr.personalDetailsFound += this.dataSet.addPersonalDetails;
            pr.semesterFinished     += this.dataSet.endSemesterSQL;
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
            var txt = string.Join("\r\n", dataSet.cleanView.Select(row => string.Join(" , ", row.course.ToString(), row.grade.ToString())));
            File.WriteAllText(fileName, txt, Connection.hebrewEncoding);
        }

        internal void logOut()
        {
            html = "";
            dataSet.init();
            /*
            dataSet.personalDetails.AddPersonalDetailsRow(new DateTime(), "", "", "", "", "");
            dataSet.Semester.Clear();
             * */
        }

        public void Dispose()
        {
            //dataSet.Dispose();
        }
    }
}
