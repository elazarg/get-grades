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
        public string Name { get; set; }
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

    class Semester : ICloneable
    {
        //int id;
        public int index { get; internal set;  }
        public string year { get; set; }
        public string season { get; set; }
        public string hebrewYear { get; set; }
        
        internal Summary summary { get; set; }

        public decimal Average { get { return summary.Average; } }
        public decimal SuccessRate { get { return summary.SuccessRate; } }
        public decimal Points { get { return summary.Points; } }



        public object Clone()
        {
            return MemberwiseClone();
        }
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

    internal enum SessionStatus
    {
        Ptor,
        LoShStar,
        LoSh,
        Passed,
        InCompleteStar,
        InComplete,
        Failed,
        FailedStar,
        Minus,
        Grade,
        MiluimStar,
        Miluim
    }


    internal class CourseSession : ICloneable
    {
        static Dictionary<string, SessionStatus> commentToStatus = new Dictionary<string,SessionStatus> {
               
               {"-",                 SessionStatus.Minus },

               {"לא השלים ש*",          SessionStatus.LoShStar },
               {"לא השלים ש",           SessionStatus.LoSh },

               {"לא השלים מילואים*",   SessionStatus.MiluimStar },
               {"לא השלים מילואים",    SessionStatus.Miluim },

               {"לא השלים*",        SessionStatus.InCompleteStar },


               {"לא השלים",        SessionStatus.InComplete },
               {"נכשל",             SessionStatus.Failed },

               {"נכשל*",            SessionStatus.FailedStar },
               {"פטור ללא ניקוד",  SessionStatus.Ptor },
               {"פטור עם ניקוד",   SessionStatus.Ptor },
               {"עבר",              SessionStatus.Passed },

               {"",                 SessionStatus.Ptor }, // ? not sure
               {"<BR>",             SessionStatus.Ptor }, // ? not sure
        };

        public string courseId { get { return course.id.ToString(); } set { course.id = value; } }
        public string courseName { get { return course.name; } set { course.name = value; } }
        public decimal points { get { return course.points; } }

        public decimal _grade { get; private set; }

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
                    this._grade = - (int)(this.status);
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
        internal bool Passed { 
            get { return status == SessionStatus.Passed
                        || status == SessionStatus.Ptor
                        || status==SessionStatus.Grade && _grade >= 55; } }

        public bool inList { get; set; }
        internal decimal mult()
        {
            return _grade * points;
        }

        internal Course course;
        internal Semester semester;
        internal SessionStatus status;
        public int index { get; internal set; }
        private string _comments;

        public object Clone()
        {/*
            CourseSession res = new CourseSession {
                course = course,
                semester = semester,
                status = status,
                _grade = _grade,
                index = index,
                inList = inList,
                _comments = _comments,
            };*/
            return this.MemberwiseClone();
        }
    }

}
