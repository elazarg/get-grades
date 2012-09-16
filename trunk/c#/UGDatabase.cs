using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Be.Timvw.Framework;

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
            return cs.Sum( x => x.mult() );
        }

        public static decimal Sum(this IEnumerable<CourseSession> cs, SessionStatus ss)
        {
            return cs.Where(ss).Sum();
        }
    }
    class UGDatabase
    {
        public PersonalDetails personalDetails;

        public BindingList<CourseSession> sessions { get; set; }
        public BindingList<Semester> semesters { get; private set; }
        public BindingList<Course> courses { get; private set; }
        public BindingList<CourseSession> cleanView { get; private set; }

        internal UGDatabase()
        {
            sessions = new SortableBindingList<CourseSession>();
            semesters = new SortableBindingList<Semester>();
            courses = new SortableBindingList<Course>();
            cleanView = new SortableBindingList<CourseSession>();
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
                    date = DateTime.Today,
                    id = id,
                    firstName = fullName[0],
                    lastName = fullName[1],
                    program = program, 
                    faculty = faculty
                };
            DateTime dt;
            if (DateTime.TryParse(date, out dt))
                personalDetails.date = dt;
        }

        internal void addSession(string courseId, string courseName, string points, string grade)
        {
            Course course = new Course
            {
                id = courseId,
                name = courseName.Replace("'", ""),
                points = decimal.Parse(points)
            };
            if (!courses.Any(x => x.id == courseId))
                courses.Add(course);

            CourseSession courseSession = new CourseSession {
                course = course,
                semester = semesters.Last(),
                grade = grade.Trim()
            };
            sessions.Add(courseSession);
        }

        private Summary computeSemester(IEnumerable<CourseSession> taken)
        {
            Summary s = new Summary();

            List<CourseSession> cleanSessions = new List<CourseSession>();

            foreach (var row in taken.Reverse()) {
                if (!row.course.onceOnly)
                    cleanSessions.Add(row);
                else if (cleanSessions.All(x => x.courseId != row.courseId))
                    cleanSessions.Add(row);                
            }

            var inAverage = cleanSessions.Where(SessionStatus.Grade);
            decimal avSum = inAverage.Sum();
            if (avSum > 0)
                s.Average = inAverage.Sum( (p, g) => p * g ) / avSum;

            s.Points = cleanSessions.Sum(SessionStatus.inPoints);

            decimal sum = taken.Sum(SessionStatus.Attended);
            if (sum > 0) {
                s.SuccessRate = taken.Sum(SessionStatus.inSuccess) * 100  / sum;
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
            IEnumerable<CourseSession> taken = sessions.Where(session => session.semester.id == last.id);
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
                    MessageBox.Show(string.Format("{0} : {3}, {1} : {4}, {2} : {5}",
                        actual.Points,  actual.SuccessRate,  actual.Average,
                        summary.Points, summary.SuccessRate, summary.Average));
            }
            else if (decimal.TryParse(successRate, out p))
                summary.Points = taken.Sum();
            else MessageBox.Show("cannot parse: (" + points + " " + successRate + "  " + average + ")");

            last.summary = summary;
        }

        internal Summary total;
        internal Summary totalClean;
        internal void updateCleanSlate(bool show_empty = false)
        {
            total = computeSemester(sessions);
            

            cleanView.Clear();
            HashSet<string> ignoreList = new HashSet<string>();
            foreach (var row in sessions.Reverse()) {
                if (!row.status.HasFlag(SessionStatus.Passed)) {
                    ignoreList.Add(row.courseId);
                    continue;
                }
                if (!row.status.HasFlag(SessionStatus.inPoints))
                    continue;
                if (!ignoreList.Contains(row.courseId) && cleanView.All(x => x.courseId != row.courseId)
                    || !row.course.onceOnly)
                    cleanView.Add(row);
            }

            totalClean = computeSemester(cleanView);
        }

    }
}
