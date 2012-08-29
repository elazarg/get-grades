using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace getGradesForms
{
    class Processor
    {
        static string seperator = ",";
        static string spaceseperator = " " + seperator + " ";
        static string reverse(string str)
        {
            return new string(str.ToCharArray().Reverse().ToArray());
        }

        static string strip(string str)
        {
            return str.Replace("<td>", "").Replace("</td>", "").Replace("&nbsp;", " ");
        }

        static private StringBuilder removeXML(string str)
        {
            StringBuilder sb = new StringBuilder(str.Length);
            bool copy = true;
            foreach (var i in str)
            {
                if (i == '<')
                    copy = false;
                if (copy)
                    sb.Append(i);
                if (i == '>')
                    copy = true;
            }
            return sb;
        }



        private string replaceAll(StringBuilder sb)
        {
            sb.Replace("</TD><TD>עוצקמ םש", "</TD><TD>עוצקמ םש</TD><TD>עוצקמ רפסמ");
            tick();
            sb.Replace("\n", "");
            tick();
            sb.Replace("\r", "");
            tick();
            sb.Replace("</TD><TD ALIGN=LEFT>", spaceseperator);
            tick();
            sb.Replace("</TD><TD>", spaceseperator);
            tick();
            sb.Replace("</TD>", "\r\n");
            tick();
            sb.Replace("</td></TR>", "\r\n");
            tick();
            sb.Replace("</td><td>", spaceseperator);
            tick();
            sb.Replace("&nbsp;", "   ");
            tick();
            sb.Replace("עצוממ", seperator + " עצוממ " + seperator);
            tick();
            sb.Replace("תוחלצה", seperator + " תוחלצה ");
            tick();
            sb.Replace("כה\"ס", ", כה\"ס ");
            tick();
            sb.Replace(")", ") ");
            tick();
            sb.Replace("(", " (");
            tick();
            return sb.ToString();
        }


        internal string processText(String html, Degree degree)
        {
            StringReader input_hebrew = new StringReader(html);

            while (input_hebrew.ReadLine() != "<TABLE BORDER=0 CELLSPACING=0 CELLPADDING=0>");
            string all = replaceAll(new StringBuilder(input_hebrew.ReadToEnd()));
            StringBuilder temp = removeXML(all);
            // comma after course ID
            foreach (Match m in Regex.Matches(temp.ToString(), "\\d{6}", RegexOptions.Compiled))
            {
                temp[m.Index - 2] = seperator[0];
                tick();
            }
            // fix year
            foreach (Match m in Regex.Matches(temp.ToString(), "\\d{4}.\\d{2} ", RegexOptions.Compiled))
            {
                temp[m.Index + m.Length] = seperator[0];
                tick();
            }
            foreach (Match m in Regex.Matches(temp.ToString(), "([\\.\\-א-ת\"0-9' ])*" + "[א-ת]", RegexOptions.Compiled))
            {
                string t = reverse(m.Value);
                for (int i = 0; i < m.Length; i++)
                    temp[m.Index + i] = t[i];
                tick();
            }


            int semester = 0;
            string[] coll = temp.ToString().Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            coll[5] = "נקודות מצטברות" + seperator + " שיעור הצלחות כללי" + seperator + " ממוצע כללי";
            StringBuilder res = new StringBuilder("");
            SemesterDetails thisSemester = new SemesterDetails("זיכויים");
            thisSemester.successRate = 100;
            thisSemester.Average = 100;
            foreach (string s in coll)
            {
                tick();
                string[] splitted = s.Split(seperator.ToCharArray()).Reverse().ToArray();

                if (Regex.IsMatch(splitted[0], "\\d{6}"))
                {
                    thisSemester.numberOfCourses++;
                    degree.AddSession(CourseSession.Create(splitted, semester));
                }

                if (splitted[0].Contains("סה\"כ"))
                {
                    thisSemester.TotalPoints = Decimal.Parse(splitted[1]);
                    if (splitted.Length > 5)
                    {
                        thisSemester.successRate = Decimal.Parse(splitted[3].Replace("%", ""));
                        thisSemester.Average = Decimal.Parse(splitted[5].Replace("%", "").Replace("-", "0"));
                    }
                    degree.AddSemester(thisSemester);
                    thisSemester = null;
                }
                string mended = String.Join(spaceseperator, splitted).Replace("  ", " ").Replace("  ", " ").Replace(seperator + " ", seperator).Replace(" " + seperator, seperator) + "\r\n";


                if (Regex.IsMatch(mended, "\\d{4}/\\d{2}")) {
                    string[] sar = mended.Split(new char[] { ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries); 
                    thisSemester = new SemesterDetails(sar[2].Trim());
                    thisSemester.hebrewYear = sar[1].Trim();
                    thisSemester.season = sar[0].Trim();
                    if (thisSemester.season[0] == 'א')
                        thisSemester.year = thisSemester.year.Remove(2, 3);
                    thisSemester.numberOfCourses = 0;
                    semester++;
                }

                res.Append(mended);
            }
            res.Append("\r\n");
            return res.ToString();
        }

        public delegate void Tick();
        public event Tick tick = delegate { };
    }

}
