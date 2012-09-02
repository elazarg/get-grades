using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;


namespace getGradesForms
{
    public partial class MyDatabaseDataSet
    {
        HashSet<string> s = new HashSet<string>();

        internal void init()
        {
            this.Clear();
            this.tableSemester.AddSemesterRow("זיכויים", null, null);
        }
        
        private CourseListRow addCourse(string id, string name, string points)
        {
            CourseListRow crow = this.tableCourseList.NewCourseListRow();
            crow.ID = id;
            crow.Name = name;
            crow.Points = points;
            if (!s.Contains(id))
                this.tableCourseList.AddCourseListRow(crow);
            s.Add(id);
            this.tableCourseList.AcceptChanges();
            return crow;
        }

        internal void addPersonalDetails(string id, string name, string program, string faculty)
        {
            string[] fullName = name.Split(new char[] {' '});
            
            this.tablePersonalDetails.AddPersonalDetailsRow(id, fullName[0], fullName[1], program, faculty);
        }
        
        internal void addSessionToSQL(string course_ID, string course_Name, string points, string grade)
        {
            CourseSessionsRow cs = this.tableCourseSessions.NewCourseSessionsRow();
            cs.CourseListRow = addCourse(course_ID, course_Name, points);
            cs.SemesterRow = this.tableSemester.Last();

            decimal d;
            if (decimal.TryParse(grade, out d))
            {
                cs.Grade = d;
            }
            if (grade.Contains("*"))
                cs.RD = "RD";
            this.tableCourseSessions.AddCourseSessionsRow(cs);
            this.tableCourseSessions.AcceptChanges();
        }

        internal void addSemesterToSQL(string year, string hebrewYear, string season)
        {
            this.tableSemester.AddSemesterRow(year, season, hebrewYear);
        }
    }
}
