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

        public SortableBindingList<CourseSession> sessions { get; private set; }
        public BindingList<Semester> semesters { get; private set; }
        public BindingList<Course> courses { get; set; }
        public BindingList<CourseSession> cleanView { get; private set; }

        internal UGDatabase()
        {
            sessions = new SortableBindingList<CourseSession>();
            cleanView = new SortableBindingList<CourseSession>();
            semesters = new SortableBindingList<Semester>();
            courses = new SortableBindingList<Course>();
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
            this.Clear();
            this.semesters.Add( new Semester {  year = "זיכויים"  });
        }

        internal void addPersonalDetails(string date, string id, string name, string faculty, string program)
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
                index = sessions.Count + 1,
                course = course,
                semester = semesters.Last(),
                grade = grade.Trim()
            };
            sessions.Add(courseSession);
        }

        private Summary computeSemester(IEnumerable<CourseSession> taken)
        {
            var cleanSessions = new List<CourseSession>();

            foreach (var row in taken.OrderByDescending(cs => cs.index)) {
                if (!row.course.onceOnly || cleanSessions.All(x => x.courseId != row.courseId))
                    cleanSessions.Add(row);                
            }

            Summary s = new Summary();
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

        private void computeSemester(int id)
        {
            semesters.Single(sem => sem.id == id).summary = computeSemester(sessions.Where(s => s.semester.id == id));
        }

        internal void addSemester(string year, string hebrewYear, string season)
        {
            this.semesters.Add( new Semester {
                                    id = semesters.Count(),
                                    year = year,
                                    season = season,
                                    hebrewYear = hebrewYear,
                                });
        }

        internal void addEndSemester(string successRate, string points, string average)
        {
            Semester last = this.semesters.Last();
            IEnumerable<CourseSession> taken = sessions.Where(s => s.semester.id == last.id);
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
            else
                MessageBox.Show(string.Format(
                    "Cannot parse: ({0} {1} {2})", points , successRate , average));

            last.summary = summary;
        }

        internal Summary total { get { return computeSemester(sessions); } }
        internal Summary totalClean { get { return computeSemester(cleanView); } }
        internal Summary totalFaculty {
            get { 
                return computeSemester(cleanView.Where(ses => ses.course.faculty == personalDetails.faculty));
            }
        }
        internal void updateCleanSlate()
        {
            int n = semesters.Count;
            for (int i = 0; i < n; i++)
                computeSemester(i);

            cleanView.Clear();
            HashSet<string> ignoreList = new HashSet<string>();
            foreach (var row in sessions.OrderByDescending(cs => cs.index)) {
                if (!row.status.HasFlag(SessionStatus.Passed)) {
                    ignoreList.Add(row.courseName);
                    continue;
                }
                if (!row.status.HasFlag(SessionStatus.inPoints))
                    continue;
                if (!ignoreList.Contains(row.courseName) && cleanView.All(x => x.courseName != row.courseName)
                    || !row.course.onceOnly)
                    cleanView.Add(row);
            }
        }
    }
}
