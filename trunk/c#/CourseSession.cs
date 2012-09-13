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

    internal struct Course
    {
        public string id { get; set; }
        public string name { get; set; }
        public decimal points { get; set; }
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
        internal Course course;
        internal Semester semester;

        public string courseId { get { return course.id.ToString(); } set { course.id = value; } }
        public string courseName { get { return course.name; } set { course.name = value; } }
        public decimal grade { get; set; }

        public string Comments { get; set; }
        public bool inAverage { get; set; }
        public bool inFinal { get; set; }
        public bool Attended { get; set; }
        public bool Passed { get; set; }
    }

    struct CleanViewRow
    {
        internal Course course { get; set; }

        public string courseId { get { return course.id; } }
        public string courseName { get { return course.name; } }
        public decimal grade { get; set; }
    }

}
