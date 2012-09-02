using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
namespace getGradesForms {
    
    
    public partial class _MyDatabase_2DataSet {
        int semester = 0;
        HashSet<string> s = new HashSet<string>();

        internal void init()
        {
            this.Clear();
            semester = 0;
            this.tableSemester.AddSemesterRow("זיכויים", null, null);
        }

        internal void addSessionToSQL(string course_ID, string name, string points, string grade)
        {
            CourseListRow crow = this.tableCourseList.NewCourseListRow();
            crow.ID = course_ID;
            crow.Name = name;
            crow.Points = points;
            if (!s.Contains(course_ID))
                this.tableCourseList.AddCourseListRow(crow);
            s.Add(course_ID);
            decimal d;

            CourseSessionsRow cs = this.tableCourseSessions.NewCourseSessionsRow();
            cs.CourseListRow = crow;
            cs.SemesterRow = this.tableSemester.Last();

            if (decimal.TryParse(grade, out d)) {
                cs.Grade = d;
            }
            if (grade.Contains("*"))
                cs.RD = "RD";
            this.tableCourseSessions.AddCourseSessionsRow(cs);

            
        }

        internal void addSemesterToSQL(string year, string hebrewYear, string season)
        {
            semester++;
            this.tableSemester.AddSemesterRow(year, season, hebrewYear);
        }
    }
}
