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
        /*
        public static IEnumerable<CourseSession> Where(this IEnumerable<CourseSession> cs, SessionStatus flags)
        {
            return cs.Where( x => x.status.HasFlag(flags));
        }
        */
        public static decimal Sum(this IEnumerable<CourseSession> cs, Func<decimal, decimal, decimal> func = null)
        {
            if (func == null)
                return cs.Sum(x => x.course.points);
            return cs.Sum( x => x.mult() );
        }
        /*
        public static decimal Sum(this IEnumerable<CourseSession> cs, SessionStatus ss)
        {
            return cs.Where(ss).Sum();
        }*/
    }

    class UGDatabase : ICloneable
    {
        public PersonalDetails personalDetails;

        public SortableBindingList<CourseSession> sessions { get; private set; }
        public BindingList<Semester> semesters { get; private set; }
        public BindingList<Course> courses { get; set; }
        public SortableBindingList<CourseSession> cleanView { get; private set; }

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
            personalDetails = new PersonalDetails {
                    date = DateTime.Today,
                    id = id,
                    Name = name,
                    program = program, 
                    faculty = faculty
                };
            DateTime dt;
            if (DateTime.TryParse(date, out dt))
                personalDetails.date = dt;
        }

        internal void addSession(string courseId, string courseName, string points, string grade)
        {
            Course course = new Course {
                id = courseId,
                name = courseName.Replace("'", ""),
                points = decimal.Parse(points)
            };
            if (!courses.Any(x => x.id == courseId))
                courses.Add(course);

            CourseSession courseSession = new CourseSession {
                index = sessions.Count + 1,
                course = course,
                semester = semesters.Aggregate( (prev, cur) => prev.index > cur.index ? prev : cur),
                grade = grade.Trim(),
                inList = true,
            };
            sessions.Insert(0, courseSession);
        }

        private Summary computeSemester(IEnumerable<CourseSession> taken)
        {
            IEnumerable<CourseSession> lasts = filterLasts(taken);

            return new Summary {
                Average = getAverage(lasts),
                Points = getPoints(lasts),
                SuccessRate = getSuccessRate(taken)
            };
        }

        private static List<CourseSession> filterLasts(IEnumerable<CourseSession> taken)
        {
            var toFilter = new SessionStatus[] {
                SessionStatus.LoSh,  SessionStatus.Miluim, SessionStatus.MiluimStar,
                SessionStatus.Minus, SessionStatus.InCompleteStar
             };
            var temp = taken.Where(x =>  x.inList && !toFilter.Contains(x.status)).OrderBy(x => x.index);

            var lasts = new List<CourseSession>(temp.Where(x => !x.course.onceOnly));
            var courseSet = new HashSet<string>(temp.Where(x => x.course.onceOnly).Select(x => x.courseName));
            foreach (var name in courseSet) {
                lasts.Add(temp.Where(x => x.courseName == name).Aggregate((prev, cur) => prev.index > cur.index ? prev : cur));
            }
            return lasts;
        }

        private static decimal getSuccessRate(IEnumerable<CourseSession> taken)
        {
            var inSuccessPoints = new SessionStatus[] {
                SessionStatus.Failed, SessionStatus.Passed, SessionStatus.InComplete, 
                SessionStatus.InCompleteStar, SessionStatus.Grade
            };
            var successable = taken.Where(x => x.inList && inSuccessPoints.Contains(x.status));
            decimal sum = successable.Sum();
            if (sum > 0)
                return successable.Where(x => x.Passed).Sum() * 100 / sum;
            return 0;
        }

        private static decimal getAverage(IEnumerable<CourseSession> lasts)
        {
            var averageable = lasts.Where(x => x.status == SessionStatus.Grade);
            decimal sum = averageable.Sum();
            if (sum > 0)
                return averageable.Sum((p, g) => p * g) / sum;
            return 0;
        }

        private static decimal getPoints(IEnumerable<CourseSession> lasts)
        {
            return lasts.Where(x => x.Passed).Sum();
        }

        private void computeSemester(int id)
        {
            semesters.Single(sem => sem.index == id).summary =
                computeSemester(sessions.Where(s => s.semester.index == id));
        }

        internal void addSemester(string year, string hebrewYear, string season)
        {
            this.semesters.Insert(0,  new Semester {
                                    index = semesters.Count(),
                                    year = year,
                                    season = season,
                                    hebrewYear = hebrewYear,
                                });
        }

        internal void addEndSemester(string successRate, string points, string average)
        {
            Semester last = this.semesters.Aggregate((prev, cur) => prev.index > cur.index ? prev : cur);
            IEnumerable<CourseSession> taken = sessions.Where(s => s.semester.index == last.index);
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
                summary = computeSemester(taken);
            else
                MessageBox.Show(string.Format(
                    "Cannot parse: ({0} {1} {2})", points , successRate , average));

            last.summary = summary;
        }

        internal Summary total { get { return computeSemester(sessions); } }
        internal Summary totalClean { get { return computeSemester(cleanView); } }
        internal Summary totalFaculty {
            get { 
                return computeSemester(cleanView.Where(
                    ses => ses.course.faculty == 
                        personalDetails.faculty.Split("()".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0]
                    ));
            }
        }
        internal void updateCleanSlate()
        {
            for (int i = 0; i < semesters.Count; i++)
                computeSemester(i);

            cleanView = new SortableBindingList<CourseSession>(
                filterLasts(sessions.Where(x => x.Passed)
                            .Select(x => (CourseSession)x.Clone())).OrderBy(x => x._grade).Reverse());
        }

        private UGDatabase(UGDatabase other)
        {
            this.personalDetails = other.personalDetails;
            this.courses = new SortableBindingList<Course>(other.courses);
            this.semesters = new SortableBindingList<Semester>();
            foreach (var i in other.semesters)
                this.semesters.Add((Semester)i.Clone());
            this.sessions = new SortableBindingList<CourseSession>();
            foreach (var i in other.sessions)
                this.sessions.Add((CourseSession)i.Clone());
            this.cleanView = new SortableBindingList<CourseSession>();
            foreach (var i in other.cleanView)
                this.cleanView.Add((CourseSession)i.Clone());
        }

        public object Clone()
        {
            return new UGDatabase(this);
        }
    }
}
