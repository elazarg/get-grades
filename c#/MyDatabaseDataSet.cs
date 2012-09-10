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
            this.PersonalDetails.AddPersonalDetailsRow(date, id, fullName[0], fullName[1], program, faculty);
        }


        internal void addSessionToSQL(string course_ID, string course_Name, string points, string grade)
        {
            CourseSessionsRow cs = CourseSessions.NewCourseSessionsRow();
            cs.CourseListRow = addCourse(course_ID, course_Name, points);
            cs.SemesterRow = Semester.Last();
 
            switch (grade.Trim()) {
                case "-":   case "לא השלים*":   case "לא השלים ש":  case "לא השלים ש*":
                    cs.Comments = (grade.Contains("-")) ? "טרם ניגש" : "לא נחשב";
                    cs.Grade = 0;
                    cs.isLast = false;
                    cs.Attended = grade.Trim() == "לא השלים*";//
                    cs.inAverage = false;
                    break;

                case "לא השלים":    case "פטור ללא ניקוד":  case "פטור עם ניקוד":   case "עבר": case "נכשל":
                    cs.Grade = (grade == "נכשל" || grade == ("לא השלים")) ? 0 : 100;
                    cs.inAverage = false;
                    goto finish;

                default:
                    cs.Grade = decimal.Parse(grade.Replace("*", ""));
                    cs.inAverage = true;
                    goto finish;

                finish:
                    removeIsLast(course_ID, course_Name);
                    cs.isLast = cs.Attended = true;
                    cs.Comments = grade; //could be nothing 
                    break;
            }

            this.CourseSessions.AddCourseSessionsRow(cs);
            this.CourseSessions.AcceptChanges();
        }

        private void removeIsLast(string course_ID, string course_Name)
        {
            if (idToFaculty[course_ID.Remove(2)] != "ספורט") {
                CourseSessionsRow last = this.CourseSessions.LastOrDefault(row => row.CourseListRow.Name == course_Name);
                if (last != null)
                    last.isLast = false;
            }
        }

        struct Sum
        {
            internal decimal Average;
            internal decimal SuccessRate;
            internal decimal Points;
        }


        Sum computeSemester(Func<int, bool> pred)
        {
            var taken = CourseSessions.Where(x => pred(x.Semester_ID));
            var inAverage = taken.Where(x => x.isLast && x.Grade > -1 && x.inAverage);
            var inpoints = taken.Where(x => x.CourseListRow.Points > -1);
            var inSuccess = inpoints.Where(x => x.Attended);
            if (inSuccess.Any()) {
                return new Sum
                {
                    Points = inpoints.Where(x => x.isLast && x.Grade > 55).Sum(x => x.CourseListRow.Points),
                    Average = inAverage.Any() ? decimal.Round(inAverage.Sum(x => x.CourseListRow.Points * x.Grade)
                                            / inAverage.Sum(x => x.CourseListRow.Points), 1, MidpointRounding.AwayFromZero) : 0,
                    SuccessRate = decimal.Round(
                                    inSuccess.Where(x => x.Grade >= 55).Sum(x => x.CourseListRow.Points) * 100
                                  / inSuccess.Sum(x => x.CourseListRow.Points)
                                  , 0, MidpointRounding.AwayFromZero)
                };
            }
            return new Sum();
        }

        internal void addSemesterToSQL(string year, string hebrewYear, string season)
        {
            this.Semester.AddSemesterRow(year, season, hebrewYear, 0, 0, 0);
        }

        internal void endSemesterSQL(string successRate, string points, string average)
        {
            SemesterRow last = this.Semester.Last();
            Sum s = computeSemester(semid => semid == last.ID);
            last.Points = s.Points;
            last.Average = s.Average;
            last.Success_Rate = s.SuccessRate;

            // validation
            decimal p;
            if (decimal.TryParse(points, out p)){
                Sum actual = new Sum {
                                 Points = decimal.Parse(points),
                                 Average = decimal.Parse(average),
                                 SuccessRate = successRate == "" ? 0 : decimal.Parse(successRate),
                             };
                if (s.Points != actual.Points || s.Average != actual.Average || s.SuccessRate != actual.SuccessRate)
                        MessageBox.Show("(" + points + " : " + s.Points + ")" + "(" + successRate + " : " + s.SuccessRate + ")" + "(" + average + " : " + s.Average + ")");
            }
            else if (decimal.TryParse(successRate, out p)) {
                if (p != s.Points)
                    MessageBox.Show("(" + successRate + " : " + s.Points + ")");
            }
            else MessageBox.Show("Error: " + successRate + " is not parsable");
        }

        internal void updateCleanSlate(bool show_empty = false)
        {
            Sum s = computeSemester(semid => true);
            this.Semester.AddSemesterRow("סה\"כ", "", "", s.Average, s.SuccessRate, s.Points);

            ViewTable.Clear();
            foreach (var row in CourseSessions.Where(x => x.isLast).Reverse())
                if ((!ViewTable.Any(c => c.Course_Name == row.CourseListRow.Name)) && (show_empty || row.Grade > 55 || row.Comments == "עבר"))
                    ViewTable.AddViewTableRow(row.Course_ID, row.CourseListRow.Name, row.CourseListRow.Points, row.Grade);
        }
    }
}
