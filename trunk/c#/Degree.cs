using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getGradesForms
{
    class Degree
    {
        List<CourseSession> chrono = new List<CourseSession>(200);

        List<SemesterDetails> semesters = new List<SemesterDetails>(20);

        internal void AddSession(CourseSession cs)
        {
            chrono.Add(cs);
            tick();
        }

        internal void AddSemester(SemesterDetails sd)
        {
            semesters.Add(sd);
        }

        public override string ToString()
        {
            string str1 = string.Join("\r\n", chrono);
            string str2 = string.Join("\r\n", semesters);
            return str1 + "\r\n{}\r\n" + str2;
        }
        
        public delegate void Tick();
        public event Tick tick = delegate { };
    }
}
