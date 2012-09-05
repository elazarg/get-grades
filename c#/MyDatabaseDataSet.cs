using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System;


namespace getGradesForms
{
    public partial class MyDatabaseDataSet
    {
        internal void init()
        {
            this.Clear();
            this.tableSemester.AddSemesterRow("זיכויים", null, null, 0 ,0, 0);
        }

        Dictionary<string, decimal> coursesDic= new Dictionary<string,decimal>();
        Dictionary<string, int> sessionsDic = new Dictionary<string,int>();

        private CourseListRow addCourse(string id, string name, string points)
        {
            CourseListRow crow = this.tableCourseList.NewCourseListRow();
            crow.ID = id;
            crow.Name = name;

            crow.Points = decimal.Parse(points);
            if (!this.tableCourseList.Rows.Contains(id))// !s.Contains(id))
                this.tableCourseList.AddCourseListRow(crow);
            coursesDic[name] = crow.Points;
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

            decimal decimalGrade = 0;
            if (decimal.TryParse(grade, out decimalGrade))
            {
                cs.Grade = decimalGrade;
                sessionsDic[course_Name] = cs.ID;
            }
            else
            {
                sessionsDic.Remove(course_Name);
                cs.Comments = grade;
            }

            
            this.tableCourseSessions.AddCourseSessionsRow(cs);
            this.tableCourseSessions.AcceptChanges();
        }

        internal void addSemesterToSQL(string year, string hebrewYear, string season)
        {
            var last = this.tableSemester.Last();

            var relevant = from ses in sessionsDic// where ses.Grade > 0
                           where CourseSessions.FindByID(ses.Value).Semester_ID == last.ID
                           select new {
                               points = coursesDic[ses.Key],
                               sum = coursesDic[ses.Key] * CourseSessions.FindByID(ses.Value).Grade 
                           };
            if (relevant.Any())
            {
                last.Points = relevant.Sum(x => x.points);
                last.Average = decimal.Round(relevant.Sum(x => x.sum) / last.Points, 1, MidpointRounding.AwayFromZero);
                var took = from s in CourseSessions
                           where s.Semester_ID == last.ID
                           join course in CourseList on s.Course_ID equals course.ID
                           select new { /* Grade = s.RD.Length > 0 ? 100 : */s.Grade, course.Points, s.Comments };
                last.Success_Rate = decimal.Round(
                                    took.Where(s => s.Grade >= 55).Sum(s => s.Points) * 100
                                  / took.Where(s => !s.Comments.Contains('*') && !s.Comments.Contains('-')).Sum(s => s.Points)
                                  , 0, MidpointRounding.AwayFromZero);
            }
            this.tableSemester.AddSemesterRow(year, season, hebrewYear, 0, 0, 0);
        }

        internal void updateCleanSlate(bool show_empty = false)
        {
            var view = from session in CourseSessions
                       join course in CourseList on session.Course_ID equals course.ID
                       select new Tuple<string, string, decimal, decimal>(course.ID, course.Name, course.Points, session.Grade);

            foreach (var row in view.Reverse())
                if ((from c in ViewTable where c.Course_Name == row.Item2 select 1).Count() == 0
                    && (show_empty || row.Item4 > -1) )
                    ViewTable.AddViewTableRow(row.Item1, row.Item2, row.Item3, row.Item4);
        }
    }
}
