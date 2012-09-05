using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace getGradesForms
{
    class Grades
    {
        public readonly MyDatabaseDataSet dataSet= new getGradesForms.MyDatabaseDataSet();
        public String html { get; private set; }

        private BackgroundWorker bw;

        public enum State {
            READY,
            CONNECTING,
            AUTHENTICATING,
            PROCESSING,
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
                conn.tick += delegate { bw.ReportProgress(20); };
                state = State.AUTHENTICATING;
                html = conn.retrieveHTML(userid, password).ReadToEnd();
            }
        }

        private void process()
        {
            state = State.PROCESSING;
            var pr = new Processor(new StringReader(html).ReadLine);
            pr.sessionFound         += this.dataSet.addSessionToSQL;
            pr.semesterFound        += this.dataSet.addSemesterToSQL;
            pr.personalDetailsFound += this.dataSet.addPersonalDetails;
            pr.processText();

            html = pr.flipHtml(html);

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
    }
}
