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
        public readonly MyDatabaseDataSet dataSet= new getGradesForms.MyDatabaseDataSet();
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

        public void saveFile(string fileName)
        {
            var txt = string.Join("\r\n", from row in dataSet.ViewTable
                                              select string.Join(" , ", row.ItemArray));
            File.WriteAllText(fileName, txt, Connection.hebrewEncoding);
        }

        internal void logOut()
        {
            html = "";
            dataSet.init();
            dataSet.PersonalDetails.AddPersonalDetailsRow("", "", "", "", "", "");
            dataSet.Semester.Clear();
        }

        public void Dispose()
        {
            dataSet.Dispose();
        }
    }
}
