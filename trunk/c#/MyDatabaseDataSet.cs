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
            this.Semester.AddSemesterRow("", "זיכויים", null, 0, 0, 0);
        }
        Dictionary<string, string> idToFaculty = new Dictionary<string, string>
        {
            { "23", "מדעי המחשב" }, 
            { "13", "ביולוגיה" },
            { "33", "הנדסה ביו-רפואית" },
            { "05", "הנדסה כימית" },
            { "06", "הנדסת ביוטכנולוגיה ומזון" },
            { "03", "הנדסת מכונות" },
            { "12", "כימיה" },
            { "31", "הנדסת חומרים" },
            { "11", "פיסיקה" },
            { "20", "ארכיטקטורה ובינוי ערים" },
            { "21", "הוראת המדעים" },
            { "01", "הנדסה אזרחית" },
            { "08", "הנדסת אווירונאוטיקה" },
            { "04", "הנדסת חשמל" },
            { "09", "הנדסת תעשיה וניהול" },
            { "32", "לימודים הומניסטיים ואמנות" },
            { "39", "ספורט" }, // can take many times
            { "10", "מתמטיקה" },
            { "19", "מתמטיקה" }, //advanced
            { "27", "רפואה" },
        };
        private CourseListRow addCourse(string id, string name, string points)
        {
            CourseListRow crow = this.CourseList.NewCourseListRow();
            crow.ID = id;
            crow.Name = name;
            crow.Faculty = idToFaculty[id.Remove(2)];

            crow.Points = decimal.Parse(points);
            if (!this.CourseList.Rows.Contains(id))
                this.CourseList.AddCourseListRow(crow);
            this.CourseList.AcceptChanges();
            return crow;
        }

        internal void addPersonalDetails(string date, string id, string name, string program, string faculty)
        {
            string[] fullName = name.Split(new char[] { ' ' });
            this.PersonalDetails.AddPersonalDetailsRow(DateTime.Parse(date), id, fullName[0], fullName[1], program, faculty);
        }


        internal void addSessionToSQL(string course_ID, string course_Name, string points, string grade)
        {
            CourseSessionsRow cs = CourseSessions.NewCourseSessionsRow();
            cs.CourseListRow = addCourse(course_ID, course_Name, points);
            cs.SemesterRow = Semester.Last();

            cs.Course_Name = course_Name;
            cs.Comments = grade;
            cs.Grade = 0;
            cs.inAverage = false;
            cs.inFinal = true;
            cs.Attended = true;
            cs.Passed = false;

            switch (grade.Trim()) {
                case "-":    case "לא השלים ש": case "לא השלים ש*":
                    cs.inFinal = false;
                    cs.Attended = false;
                    break;

                case "לא השלים*":
                    cs.inFinal = false;
                    break;

                case "לא השלים": case "נכשל":
                    break;

                case "פטור ללא ניקוד":  case "פטור עם ניקוד":   case "עבר":
                    cs.Passed = true;
                    break;

                default: // Real grade
                    cs.Grade = decimal.Parse(grade.Replace("*", ""));
                    cs.Passed = cs.Grade >= 55;
                    cs.inAverage = true;
                    break;                    
            }

            if (cs.inFinal && idToFaculty[course_ID.Remove(2)] != "ספורט")
            {
                foreach (CourseSessionsRow last in this.CourseSessions.Where(row => row.CourseListRow.Name == course_Name))
                    last.inFinal = false;
            }
            this.CourseSessions.AddCourseSessionsRow(cs);
            this.CourseSessions.AcceptChanges();
        }

        public struct Sum
        {
            internal decimal Average;
            internal decimal SuccessRate;
            internal decimal Points;
        }


        private decimal sumPoints(IEnumerable<CourseSessionsRow> rows)
        {
            return rows.Sum(x => x.CourseListRow.Points);
        }

        private decimal round(decimal from, int d)
        {
            return decimal.Round(from, d, MidpointRounding.AwayFromZero);
        }

        Sum computeSemester(Func<int, bool> pred)
        {
            Sum s = new Sum();
            IEnumerable<CourseSessionsRow> taken = CourseSessions.Where(x => pred(x.Semester_ID));

            var inAverage = taken.Where(x => x.inFinal && x.inAverage);
            if (inAverage.Any())
                s.Average = round(inAverage.Sum(x => x.CourseListRow.Points * x.Grade) / sumPoints(inAverage), 1);

            var attendeds = taken.Where(x => x.Attended);
            if (attendeds.Any()) {
                s.Points = sumPoints(attendeds.Where(x => x.inFinal && x.Passed));
                s.SuccessRate = round(sumPoints(attendeds.Where(x => x.Passed)) * 100 / sumPoints(attendeds), 0);
            }

            return s;
        }

        internal void addSemesterToSQL(string year, string hebrewYear, string season)
        {
            this.Semester.AddSemesterRow(year, season, hebrewYear, 0, 0, 0);
        }

        internal void endSemesterSQL(string successRate, string points, string average)
        {
            SemesterRow last = this.Semester.Last();
            Sum s = computeSemester(semid => semid == last.ID);

            // validation
            decimal p;
            if (decimal.TryParse(points, out p)){
                Sum actual = new Sum {
                                 Points = p,
                                 Average = decimal.Parse(average),
                                 SuccessRate = successRate == "" ? 0 : decimal.Parse(successRate),
                             };
                if (s.Points != actual.Points || s.Average != actual.Average || s.SuccessRate != actual.SuccessRate)
                    MessageBox.Show("(" + points + " : " + s.Points + ")" + "(" + successRate + " : " + s.SuccessRate + ")" + "(" + average + " : " + s.Average + ")");
            }
            else if (decimal.TryParse(successRate, out p))
            {
                s.Points = sumPoints(CourseSessions.Where(x => x.Semester_ID == last.ID));
            }
            else MessageBox.Show("cannot parse: (" + points + " " + successRate + "  " + average + ")");

            last.Points = s.Points;
            last.Average = s.Average;
            last.Success_Rate = s.SuccessRate;
        }

        internal Sum total;

        internal void updateCleanSlate(bool show_empty = false)
        {
            total = computeSemester(semid => true);

            ViewTable.Clear();
            foreach (var row in CourseSessions.Where(x => x.inFinal).Reverse())
                if (row.inFinal && row.Passed)
                    ViewTable.AddViewTableRow(row.Course_ID, row.CourseListRow.Name, row.CourseListRow.Points, row.inAverage ? row.Grade : 1);

        }
    }
}
