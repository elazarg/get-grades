using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace getGradesForms
{
    static class Selector
    {
        public static IEnumerable<CourseSession> Where(this IEnumerable<CourseSession> cs, SessionStatus flags)
        {
            return cs.Where( x => x.status.HasFlag(flags));
        }

        public static decimal Sum(this IEnumerable<CourseSession> cs, Func<decimal, decimal, decimal> func = null)
        {
            if (func == null)
                return cs.Sum(x => x.course.points);
            return cs.Sum( x => func(x.course.points, x.grade) );
        }
    }
    class UGDatabase
    {
        public PersonalDetails personalDetails;

        public BindingList<CourseSession> sessions { get; set; }
        public BindingList<Semester> semesters { get; private set; }
        public BindingList<Course> courses { get; private set; }
        public BindingList<CleanViewRow> cleanView { get; private set; }

        internal UGDatabase()
        {
            sessions = new BindingList<CourseSession>();
            semesters = new BindingList<Semester>();
            courses = new BindingList<Course>(); 
            cleanView = new BindingList<CleanViewRow>();
        }

        internal void Clear()
        {
            sessions.Clear();
            semesters.Clear();
            cleanView.Clear();
            courses.Clear();
            personalDetails = new PersonalDetails();
        }

        internal void init()
        {
            Semester.gen = 0;
            this.semesters.Add( new Semester {  year = "זיכויים"  });
        }

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

        internal void addSessionToSQL(string courseId, string courseName, string points, string grade)
        {
            Course course = new Course
            {
                id = courseId,
                name = courseName,
                points = decimal.Parse(points)
            };
            if (!courses.Any(x => x.id == courseId))
                courses.Add(course);

            CourseSession courseSession = new CourseSession(course, semesters.Last(), grade.Trim());

            if (courseSession.inFinal && courseSession.course.onceOnly)
            {
                foreach (CourseSession last in sessions.Where(row => row.course.name == courseName))
                    last.status &= ~SessionStatus.inFinal;
            }

            sessions.Add(courseSession);
        }

        private static Func<CourseSession, decimal> selectPoints = cs => cs.course.points;


        private Summary computeSemester(IEnumerable<CourseSession> taken)
        {
            Summary s = new Summary();
            var inAverage = taken.Where(SessionStatus.Grade);
            if (inAverage.Any())
                s.Average = inAverage.Sum( (p, g) => p * g ) / inAverage.Sum();

            if (taken.Where(x => x.Attended).Any()) {
                s.Points = taken.Where(SessionStatus.inPoints).Sum();
                s.SuccessRate = taken.Where(SessionStatus.inSuccess).Sum() * 100
                              / taken.Where(SessionStatus.Attended).Sum();
            }

            return s;
        }

        internal void addSemester(string year, string hebrewYear, string season)
        {
            this.semesters.Add( new Semester {
                year = year,
                season = season,
                hebrewYear = hebrewYear,
            });
        }

        internal void addEndSemester(string successRate, string points, string average)
        {
            Semester last = this.semesters.Last();
            IEnumerable<CourseSession> taken = sessions.Where(session => session.semester.ID == last.ID);
            Summary summary = computeSemester(taken);

            // validation
            decimal p;
            if (decimal.TryParse(points, out p)) {
                Summary actual = new Summary {
                                 Points = p,
                                 Average = decimal.Parse(average),
                                 SuccessRate = successRate == "" ? 0 : decimal.Parse(successRate),
                             };
                if (!summary.Equals(actual))
                    MessageBox.Show(string.Format("({0} : {3}, {1} : {4}, {2} : {5}",
                        actual.Points,  actual.SuccessRate,  actual.Average,
                        summary.Points, summary.SuccessRate, summary.Average));
            }
            else if (decimal.TryParse(successRate, out p))
                summary.Points = taken.Sum(selectPoints);
            else MessageBox.Show("cannot parse: (" + points + " " + successRate + "  " + average + ")");

            last.summary = summary;
        }

        internal Summary total;
        internal Summary totalClean;
        internal void updateCleanSlate(bool show_empty = false)
        {
            total = computeSemester(sessions);
            totalClean = computeSemester(sessions.Where(SessionStatus.inClean));

            cleanView.Clear();
            foreach (var row in sessions.Where(SessionStatus.inPoints).Reverse())
                cleanView.Add(new CleanViewRow {
                    course = row.course,
                    grade = row.inAverage ? row.grade.ToString() : row.Comments 
                });

        }

    }
}
