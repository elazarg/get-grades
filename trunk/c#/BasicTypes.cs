using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

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

        internal static bool isFacultyCS(string id)
        {
            return id.Remove(2) == "23";
        }
    }

    internal struct Course
    {
        public string id { get; set; }
        public string name { get; set; }
        public decimal points { get; set; }
        public string faculty { get { return Info.idToFaculty[id.Remove(2)]; } }

        internal bool onceOnly { get { return faculty!= "ספורט"; } }
    }

    class Semester
    {
        //int id;
        public int id { get; internal set;  }
        public string year { get; set; }
        public string season { get; set; }
        public string hebrewYear { get; set; }
        
        internal Summary summary { get; set; }

        public decimal Average { get { return summary.Average; } }
        public decimal SuccessRate { get { return summary.SuccessRate; } }
        public decimal Points { get { return summary.Points; } }
    }
           

    public struct Summary
    {
        private static decimal round(decimal from, int d)
        {
            return decimal.Round(from, d, MidpointRounding.AwayFromZero);
        }

        private decimal _average;
        private decimal _successRate;

        public decimal Average { get { return _average; } set { _average = round(value, 1);  } }
        public decimal SuccessRate { get { return _successRate; } set { _successRate = round(value, 0); } }
        public decimal Points { get; set; }
    }


    [Flags]
    internal enum SessionStatus
    {
        Attended = 1,
        inAverage = 2,
        inFinal = 4,
        Passed = 8,

        DidNotHappen = 0,                                            //"-" "לא השלים ש" "לא השלים ש*"
        Grade =     Attended  /* Passed if > 55 */  | inFinal   | inAverage ,
        NoFinal =   Attended,                                        //"לא השלים*"
        inPoints =  Attended | Passed   | inFinal,                  //"פטור ללא ניקוד" "פטור עם ניקוד" "עבר"
        Failed =    Attended            | inFinal,                  //"לא השלים" "נכשל"

        inSuccess = Attended |  Passed,
        inClean =               Passed  | inFinal   | inAverage,
        All     =   Attended | Passed  | inFinal   | inAverage,
    }


    internal class CourseSession
    {
        static List<string> commentsOrder = new List<string> {
               "NOTHING",
               "-", "לא השלים ש", "לא השלים ש*",
               "לא השלים*",
               "לא השלים", "נכשל",
               "נכשל*", "פטור ללא ניקוד","פטור עם ניקוד", "עבר", "" };

        static Dictionary<string, SessionStatus> commentToStatus = new Dictionary<string,SessionStatus> {
               {"-",                 SessionStatus.DidNotHappen },
               {"לא השלים ש",       SessionStatus.DidNotHappen },
               {"לא השלים ש*",      SessionStatus.DidNotHappen },

               {"לא השלים*",        SessionStatus.NoFinal },


               {"לא השלים",         SessionStatus.Failed },
               {"נכשל",              SessionStatus.Failed },

               {"נכשל*",            SessionStatus.inPoints },
               {"פטור ללא ניקוד",   SessionStatus.inPoints },
               {"פטור עם ניקוד",    SessionStatus.inPoints },
               {"עבר",              SessionStatus.inPoints },
               {"",                 SessionStatus.inPoints }, // ? not sure
               {"<BR>",                 SessionStatus.inPoints }, // ? not sure
        };

        internal Course course;
        internal Semester semester;

        internal SessionStatus status;

        internal int index;

        public string courseId { get { return course.id.ToString(); } set { course.id = value; } }
        public string courseName { get { return course.name; } set { course.name = value; } }
        public decimal points { get { return course.points; } }

        public decimal _grade { get; private set; }
        private string _comments;
        public string grade {
            get {
                return _comments == "" ? _grade.ToString() : _comments;
            }
            set
            {
                if (value == null)
                    return;
                if (commentToStatus.ContainsKey(value)) {
                    this.status = commentToStatus[value];
                    this._grade = - commentsOrder.IndexOf(value);
                    this._comments = "";
                }
                else {
                    try {
                        decimal temp = decimal.Parse(value.Replace("*", ""));
                        if (0 <= temp && temp <= 100)
                            this._grade = temp;
                        else
                            return;
                        this.status = SessionStatus.Grade;
                        if (_grade >= 55)
                            this.status |= SessionStatus.Passed;
                        _comments = "";
                        return;
                    }
                    catch (FormatException) {
                        MessageBox.Show("Cannot Parse: " + value, "ParseError");
                        return;
                    }
                }
                _comments = value;
            }
        }

        internal bool inAverage { get { return status.HasFlag(SessionStatus.inAverage); } }
        internal bool inFinal { get { return status.HasFlag(SessionStatus.inFinal); } }
        internal bool Attended { get { return status.HasFlag(SessionStatus.Attended); } }
        internal bool Passed { get { return status.HasFlag(SessionStatus.Passed); } }

        internal decimal mult()
        {
            return _grade * points;
        }
    }

}
