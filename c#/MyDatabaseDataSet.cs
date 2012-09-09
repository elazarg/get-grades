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
            this.Semester.AddSemesterRow("זיכויים", null, null, 0, 0, 0);
        }

        private CourseListRow addCourse(string id, string name, string points)
        {
            CourseListRow crow = this.CourseList.NewCourseListRow();
            crow.ID = id;
            crow.Name = name;

            crow.Points = decimal.Parse(points);
            if (!this.CourseList.Rows.Contains(id))
                this.CourseList.AddCourseListRow(crow);
            this.CourseList.AcceptChanges();
            return crow;
        }

        internal void addPersonalDetails(string date, string id, string name, string program, string faculty)
        {
            string[] fullName = name.Split(new char[] { ' ' });
            this.PersonalDetails.AddPersonalDetailsRow(date, id, fullName[0], fullName[1], program, faculty);
        }

        internal void addSessionToSQL(string course_ID, string course_Name, string points, string grade)
        {
            CourseSessionsRow cs = this.CourseSessions.NewCourseSessionsRow();
            cs.CourseListRow = addCourse(course_ID, course_Name, points);
            cs.SemesterRow = this.Semester.Last();

            if (grade.Contains("-") || grade.Contains("*")) {
                cs.Comments = (grade.Contains("-")) ? "טרם ניגש" : "לא נחשב";
                cs.isLast = false;
                cs.Attended = false;
            }
            else {
                CourseSessionsRow last = this.CourseSessions.LastOrDefault(row => row.CourseListRow.Name == course_Name);
                if (last != null)
                    last.isLast = false;

                cs.isLast = cs.Attended = (cs.Semester_ID != 0);
                cs.Comments = grade; //could be nothing
                cs.Grade = (cs.Semester_ID == 0) ? 100 : -1; //לא השלים

                decimal decimalGrade = 100;
                if (decimal.TryParse(grade, out decimalGrade))
                    cs.Grade = decimalGrade;
            }
            this.CourseSessions.AddCourseSessionsRow(cs);
            this.CourseSessions.AcceptChanges();
        }

        struct Sum
        {
            internal decimal Average;
            internal decimal SuccessRate;
            internal decimal Points;
        }


        Sum computeSemester(Func<int, bool> pred)
        {
            //BAD - does not recompute when theres another semester!!
            var templist = from ses in CourseSessions
                           where ses.Attended && pred(ses.Semester_ID)
                           select ses;

            var relevant = from ses in templist
                           where ses.Grade > -1 && ses.isLast
                           select new
                           {
                               points = ses.CourseListRow.Points,
                               sum = ses.CourseListRow.Points * ses.Grade
                           };

            if (relevant.Any()) {
                decimal totalPoints = relevant.Sum(x => x.points);
                return new Sum {
                    Points = totalPoints,
                    Average = decimal.Round(relevant.Sum(x => x.sum) / totalPoints, 1, MidpointRounding.AwayFromZero),
                    SuccessRate = decimal.Round(
                                    templist.Where(x => x.Grade >= 55).Sum(x => x.CourseListRow.Points) * 100
                                  / templist.Where(x => x.Attended).Sum(x => x.CourseListRow.Points)
                                  , 0, MidpointRounding.AwayFromZero)
                };
            }
            return new Sum();
        }

        internal void addSemesterToSQL(string year, string hebrewYear, string season)
        {
            var last = this.Semester.Last();

            Sum s = computeSemester(semid => semid == last.ID);
            last.Points = s.Points;
            last.Average = s.Average;
            last.Success_Rate = s.SuccessRate;

            this.Semester.AddSemesterRow(year, season, hebrewYear, 0, 0, 0);
        }

        internal void updateCleanSlate(bool show_empty = false)
        {
            Sum s = computeSemester(semid => true);
            this.Semester.AddSemesterRow("סה\"כ", "", "", s.Average, s.SuccessRate, s.Points);

            var view = from session in CourseSessions
                       where session.isLast
                       join course in CourseList on session.Course_ID equals course.ID
                       select new Tuple<string, string, decimal, decimal>(course.ID, course.Name, course.Points, session.Grade);

            foreach (var row in view.Reverse())
                if ((from c in ViewTable where c.Course_Name == row.Item2 select 1).Count() == 0
                    && (show_empty || row.Item4 > 55))
                    ViewTable.AddViewTableRow(row.Item1, row.Item2, row.Item3, row.Item4);
        }
    }
}
