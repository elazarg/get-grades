using System;
using System.Collections.Generic;

namespace getGradesForms
{
    enum Faculty
    {
        CS,
        MATH
    }


    struct Summary
    {
        internal decimal totalPoints;
        internal decimal average;
        internal decimal successRate;

        public Summary(string[] det)
        {
            decimal.TryParse(det[0], out totalPoints);
            average     = decimal.Parse(det[1]);
            successRate = decimal.Parse(det[2]);
        }

        public override string ToString()
        {
            return string.Join(" , ", totalPoints, average, successRate);
        }
    }

    internal struct Course
    {
        internal int id;
        internal string name;
        internal decimal points;
        internal Faculty faculty { get { return id/10000==23 ? Faculty.CS : Faculty.MATH; } }

        internal Course(string _id, string _name, string _points)
        {
            id = int.Parse(_id);
            name = _name;
            points = Decimal.Parse(_points);

        }

        public override string ToString()
        {
            return string.Join(" , ", new object[] { id, name, points , faculty});
        }
    }

    class SemesterDetails
    {
        internal string year;
        internal string season;
        internal string hebrewYear;

        internal decimal numberOfCourses;
        internal Summary summary;
        internal SemesterDetails(string _name)
        {
            this.year = _name;
        }

        //hebrewYear, year, season
        internal SemesterDetails(string[] args)
        {
            this.year = args[1];
            this.hebrewYear = args[0];
            this.season = args[2];
        }

        public override string ToString()
        {
            return string.Join(" , ", year, season, hebrewYear, numberOfCourses, summary);
        }
    }

    internal class CourseSession
    {
        internal Course course;
        internal int semester;

        internal string[] grades = new string[] {"", "", ""};

        public CourseSession(Course c, string grade, int _semester)
        {
            this.course = c;
            this.grades[0] = grade;
            this.semester = _semester;
        }

        public override string ToString()
        {
            return string.Join(" , ", new object[] { course, String.Join("/", grades) , semester} );
        }
    }
    
}
