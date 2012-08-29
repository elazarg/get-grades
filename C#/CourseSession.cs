using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace getGradesForms
{
    enum Faculty
    {
        CS,
        MATH
    }
    
    struct Course
    {
        internal int id;
        internal string name;
        internal decimal points;
        internal Faculty faculty { get { return Faculty.CS; } }

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
        internal decimal TotalPoints;
        internal decimal Average;
        internal decimal successRate;
        internal SemesterDetails(string _name)
        {
            this.year = _name;
        }
        public override string ToString()
        {
            return string.Join(" , ", new object[] { year, season, hebrewYear, numberOfCourses, TotalPoints, Average, successRate}) ;
        }
    }

    public class CourseSession
    {
        Course course = new Course();
        int semester;

        string[] grades = new string[] {"", "", ""};

        public CourseSession(string[] details, int _semester)
        {
            this.course = new Course(details[0], details[1], details[2]);
            this.grades[0] = details[3];
            this.semester = _semester;
        }

        public static CourseSession Create(string[] details, int _semester)
        {
            return new CourseSession(details, _semester);
        }

        public override string ToString()
        {
            return string.Join(" , ", new object[] { course, String.Join("/", grades) , semester} );
        }
    }
}
