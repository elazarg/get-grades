using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace getGradesForms
{
    class UGDatabase
    {
        internal void init()
        {
      //    this.Clear();
            sessions = new BindingList<CourseSession>();
            semesters = new BindingList<Semester>();
            cleanView = new BindingList<CleanViewRow>();
            this.semesters.Add( new Semester {
                ID = 1,
                hebrewYear = "",
                year = "זיכויים",
                season = null,
                sum = new Summary()
            });
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
        
        internal void addPersonalDetails(string date, string id, string name, string program, string faculty)
        {
            string[] fullName = name.Split(new char[] { ' ' });
            personalDetails = new PersonalDetails {
                    date = DateTime.Parse(date),
                    id = id,
                    firstName = fullName[0],
                    lastName = fullName[1],
                    program = program, 
                    faculty = faculty
                };
        }

        public PersonalDetails personalDetails;

        public BindingList<CourseSession> sessions { get; set; }
        public BindingList<Semester> semesters { get; set; }
        public BindingList<Course> courses { get { return new BindingList<Course>(idToCourse.Values.ToList()); } }
        public BindingList<CleanViewRow> cleanView { get; set; }

        internal Dictionary<string, Course> idToCourse = new Dictionary<string, Course>();

        internal void addSessionToSQL(string course_ID, string course_Name, string points, string grade)
        {
            idToCourse[course_ID] = new Course
            {
                id = course_ID,
                name = course_Name,
                points = decimal.Parse(points)
            };

            CourseSession cs = new CourseSession
            {
                course = idToCourse[course_ID],
                semester = semesters.Last()
            };

            cs.Comments = grade;
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
                    cs.grade = decimal.Parse(grade.Replace("*", ""));
                    cs.Passed = cs.grade >= 55;
                    cs.inAverage = true;
                    break;                    
            }

            if (cs.inFinal && idToFaculty[course_ID.Remove(2)] != "ספורט")
            {
                foreach (CourseSession last in sessions.Where(row => row.course.name == course_Name))
                    last.inFinal = false;
            }

            sessions.Add(cs);
        }

        private decimal sumPoints(IEnumerable<CourseSession> rows)
        {
            return rows.Sum(x => x.course.points);
        }

        private decimal round(decimal from, int d)
        {
            return decimal.Round(from, d, MidpointRounding.AwayFromZero);
        }

        Summary computeSemester(Func<int, bool> pred)
        {
            Summary s = new Summary();
            IEnumerable<CourseSession> taken = sessions.Where(x => pred(x.semester.ID));

            var inAverage = taken.Where(x => x.inFinal && x.inAverage);
            if (inAverage.Any())
                s.Average = round(inAverage.Sum(x => x.course.points * x.grade) / sumPoints(inAverage), 1);

            var attendeds = taken.Where(x => x.Attended);
            if (attendeds.Any()) {
                s.Points = sumPoints(attendeds.Where(x => x.inFinal && x.Passed));
                s.SuccessRate = round(sumPoints(attendeds.Where(x => x.Passed)) * 100 / sumPoints(attendeds), 0);
            }

            return s;
        }

        internal void addSemesterToSQL(string year, string hebrewYear, string season)
        {
            this.semesters.Add( new Semester {
                year = year,
                season = season,
                hebrewYear = hebrewYear,
                sum = new Summary()
            });
        }

        internal void endSemesterSQL(string successRate, string points, string average)
        {
            Semester last = this.semesters.Last();
            Summary s = computeSemester(semid => semid == last.ID);

            // validation
            decimal p;
            if (decimal.TryParse(points, out p)){
                Summary actual = new Summary {
                                 Points = p,
                                 Average = decimal.Parse(average),
                                 SuccessRate = successRate == "" ? 0 : decimal.Parse(successRate),
                             };
                if (s.Points != actual.Points || s.Average != actual.Average || s.SuccessRate != actual.SuccessRate)
                {
                 //   MessageBox.Show("(" + points + " : " + s.Points + ")" + "(" + successRate + " : " + s.SuccessRate + ")" + "(" + average + " : " + s.Average + ")");
                }
            }
            else if (decimal.TryParse(successRate, out p))
            {
                s.Points = sumPoints(sessions.Where(x => x.semester.ID == last.ID));
            }
       //     else MessageBox.Show("cannot parse: (" + points + " " + successRate + "  " + average + ")");

            last.sum = s;
        }

        internal Summary total;

        internal void updateCleanSlate(bool show_empty = false)
        {
            total = computeSemester(semid => true);

            cleanView.Clear();
            foreach (var row in sessions.Where(x => x.inFinal).Reverse())
                if (row.inFinal && row.Passed)
                    cleanView.Add(new CleanViewRow {
                        course = row.course,
                        grade = row.inAverage ? row.grade : 1 
                    });

        }

    }
}
