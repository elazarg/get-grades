using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace getGradesForms
{
    class GradesDocument : ICloneable
    {
        public string html = "";
        public UGDatabase dataSet;
        private Processor pr;

        public GradesDocument()
        {
            dataSet = new UGDatabase();
            dataSet.Clear();
        }

        public GradesDocument(string raw_html)
        {
            dataSet = new UGDatabase();

            pr = new Processor();
            pr.sessionFound += this.dataSet.addSession;
            pr.semesterFound += this.dataSet.addSemester;
            pr.personalDetailsFound += this.dataSet.addPersonalDetails;
            pr.semesterFinished += this.dataSet.addEndSemester;

            dataSet.init();
            pr.processText(raw_html);
            html = pr.fixedHtml;
            dataSet.updateCleanSlate();
        }


        public static string toCsv(IList<CourseSession> cslist)
        {
            return SString.Join("\r\n", cslist.Select(
                       row => SString.Join(" , ", new string[] {
                           row.courseId, row.courseName, row.grade
                       })));
        }


        internal void clear()
        {
            html = "";
            dataSet.Clear();
        }

        public object Clone()
        {
            GradesDocument res = new GradesDocument();
            res.dataSet = (UGDatabase)dataSet.Clone();
            return res;
        }
    }
}
