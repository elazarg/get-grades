using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;


namespace getGradesForms
{
    public partial class MyDatabaseDataSet
    {
        internal void init()
        {
            this.Clear();
            this.tableSemester.AddSemesterRow("זיכויים", null, null);
        }

        HashSet<string> s = new HashSet<string>();
        private CourseListRow addCourse(string id, string name, string points)
        {
            CourseListRow crow = this.tableCourseList.NewCourseListRow();
            crow.ID = id;
            crow.Name = name;
            crow.Points = points;
            if (!this.tableCourseList.Rows.Contains(id))// !s.Contains(id))
                this.tableCourseList.AddCourseListRow(crow);
            s.Add(id);
            this.tableCourseList.AcceptChanges();
            return crow;
        }

        internal void addPersonalDetails(string date, string id, string name, string program, string faculty)
        {
            string[] fullName = name.Split(new char[] {' '});
            this.tablePersonalDetails.AddPersonalDetailsRow(date, id, fullName[0], fullName[1], program, faculty);
        }
        
        internal void addSessionToSQL(string course_ID, string course_Name, string points, string grade)
        {
            CourseSessionsRow cs = this.tableCourseSessions.NewCourseSessionsRow();
            cs.CourseListRow = addCourse(course_ID, course_Name, points);
            cs.SemesterRow = this.tableSemester.Last();

            decimal decimalGrade;
            if (decimal.TryParse(grade, out decimalGrade))
                cs.Grade = decimalGrade;
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
