using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace getGradesForms
{
    struct PersonalDetails
    {
        public DateTime date { get; set; }
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string program { get; set; }
        public string faculty { get; set; }
    }

    static class Info
    {
        internal static Dictionary<string, string> idToFaculty = new Dictionary<string, string>
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
    }

    internal struct Course
    {
        public string id { get; set; }
        public string name { get; set; }
        public decimal points { get; set; }
        public string faculty { get { return Info.idToFaculty[id.Remove(2)]; } }

        internal bool onceOnly { get { return Info.idToFaculty[id.Remove(2)] != "ספורט"; } }
    }

    class Semester
    {
        internal static int gen = 0;
        //int id;
        public int ID { get; private set;  }
        string _year;
        public string year { get { return _year; } set { ID = gen; gen++; _year = value; } }
        public string season { get; set; }
        public string hebrewYear { get; set; }
        
        internal Summary sum { get; set; }
        public decimal Average { get { return sum.Average; } }
        public decimal SuccessRate { get { return sum.SuccessRate; } }
        public decimal Points { get { return sum.Points; } }
    }

    public struct Summary
    {
        public decimal Average { get; set; }
        public decimal SuccessRate { get; set; }
        public decimal Points { get; set; }
    }

    internal class CourseSession
    {
        [Flags]
        internal enum Status
        {
            Attended = 1,
            inAverage = 2,
            inFinal = 4,
            Passed = 8,

            DidNotHappen = 0, //"-" "לא השלים ש" "לא השלים ש*"
            Grade = inAverage | inFinal | Attended,
            NoFinal = Attended, //"לא השלים*"
            Ptor = Attended | Passed | inFinal, //"פטור ללא ניקוד" "פטור עם ניקוד" "עבר"
            Failed = Attended | inFinal //"לא השלים" "נכשל"
        }

        internal Course course;
        internal Semester semester;

        internal Status status;

        public string courseId { get { return course.id.ToString(); } set { course.id = value; } }
        public string courseName { get { return course.name; } set { course.name = value; } }
        public decimal grade { get; set; }

        public string Comments { get; set; }
        internal bool inAverage { get { return status.HasFlag(Status.inAverage); } }
        internal bool inFinal { get { return status.HasFlag(Status.inFinal); } }
        internal bool Attended { get { return status.HasFlag(Status.Attended); } }
        internal bool Passed { get { return status.HasFlag(Status.Passed) || grade >= 55; } }

        public CourseSession()
        {
            course = new Course();
            semester = new Semester();
            grade = 0;
            Comments = "";
            status = Status.DidNotHappen;
        }
    }

    struct CleanViewRow
    {
        internal Course course { get; set; }

        public string courseId { get { return course.id; } }
        public string courseName { get { return course.name; } }
        public decimal grade { get; set; }
    }

}
