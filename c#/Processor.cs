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
        internal Processor(ReadLine inputMethod)
        {
            readLine = inputMethod;
        }

	    public delegate string ReadLine();
        private ReadLine readLine;

        public delegate void PersonalDetailsFound(string id, string name, string program, string faculty);
        public event PersonalDetailsFound personalDetailsFound = delegate { };

        public delegate void SessionFound(string Course_ID,string name, string points, string grade);
        public event SessionFound sessionFound = delegate { };

        public delegate void SemesterFound(string year, string hebrewYear, string season);
        public event SemesterFound semesterFound = delegate { };

        internal void processText()
        {
            string[][] tables = getTables().ToArray();

            parseDetails(tables[0]);
            parseSummary(tables[1]);
            parseZikui(tables[2]);

            for (int i = 3; i < tables.Length; i++)
                parseSemester(tables[i].ToArray(), i, i == tables.Length);
        }

        private IEnumerable<String[]> getTables()
        {
            while (true)
            {
                for (string temp = readLine(); !temp.StartsWith("<TABLE"); temp = readLine()) 
                {
                    if (temp == null || temp.Contains("</HTML>"))
                         yield break;
                }
                
                var table = new List<string>();
                string line = "";
                for (string temp = readLine(); !temp.StartsWith("</TABLE"); temp = readLine())
                {
                    if (temp == null)
                        yield break;
                    line += temp;
                    if (temp.EndsWith("</tr>", StringComparison.CurrentCultureIgnoreCase))
                    {
                        table.Add(line);
                        line = "";
                    }
                }
                yield return table.ToArray();
            }
        }

        string specialsep = "~";
        private string cleanLine(string line)
        {
            return removeXML(line.Replace("</td><td>", specialsep).Replace("&nbsp;", "   "));
        }



        private void parseDetails(string[] table)
        {
            for (int i = 0; i < table.Length; i++)
            {
                table[i] = table[i].Substring(20);
                table[i] = putSpace(table[i].Remove(table[i].IndexOf("</TD")));
                if (i > 1)
                    table[i] = reverse(table[i]);
            }
            personalDetailsFound(table[0], table[1], table[2], table[3]);
        }

        private void parseSummary(string[] table)
        {
           // return new Summary(removeXML(table[1].Replace("</TD><TD>", ",")).Split(','));
        }

        private void parseZikui(string[] table)
        {
            /*
            SemesterDetails sem = new SemesterDetails("זיכויים");
            sem.summary = new Summary(new string[] { "100", "100", "100" });
            sem.hebrewYear = sem.season = "";
            sem.numberOfCourses = table.Length - 3;
            string points = table[table.Length - 2].Split(new char[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries)[5];
            sem.summary.totalPoints = decimal.Parse(points);
            */
            foreach (string line in table)
                if (line.StartsWith("<TR ALIGN"))
                    parseLine(line);
        }

        private void parseSemester(string[] table, int semester, bool islast)
        {
            string[] args = strip(removeXML(table[0])).Split("() ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            args[0] = reverse(args[0]);
            args[2] = reverse(args[2]);

            semesterFound(args[1], args[0], args[2]);
            /*
            SemesterDetails sem = new SemesterDetails(args); ;
            if (!islast)
            {
                string[] results = table[table.Length - 1].Split(new string[] {
                    "<TR BGCOLOR=#FFFFCC ALIGN=RIGHT VALIGN=TOP><TD>",
                    "&nbsp;עצוממ<BR>",
                    "%&nbsp;תוחלצה</TD><TD>",
                    "</TD><TD ALIGN=LEFT>"
                }, 6, StringSplitOptions.RemoveEmptyEntries);

                sem.summary = new Summary(results);
            }
            sem.numberOfCourses = table.Length - 2;

            */

            foreach (string line in table)
                if (line.StartsWith("<TR ALIGN"))
                    parseLine(line);
        }

        private void parseLine(string line)
        {
            string[] x = line.Split(new string[] { "<TR ALIGN=RIGHT><td>", "</td><td>", "</td></TR>" }, 5, StringSplitOptions.RemoveEmptyEntries);
            string grade = x[0].Contains('י') ? reverse(x[0]) : x[0];
            string points = x[1];

            string[] nameAndId = x[2].Split(new string[] { "&nbsp;" }, 10, StringSplitOptions.RemoveEmptyEntries);
            
            string courseId = nameAndId.Last();
            string name = reverse(string.Join(" ", nameAndId.TakeWhile( str => str!=courseId)));

            sessionFound(courseId, name, points, grade);
        }

        static string seperator = ",";
        static string spaceseperator = " " + seperator + " ";
        static string reverse(string str)
        {
            return new string(str.Reverse().ToArray());
        }

        static string putSpace(string str)
        {
            return str.Replace("&nbsp;", " ");
        }     

        static string strip(string str)
        {
            return str.Replace("<td>", "").Replace("</td>", "").Replace("&nbsp;", " ");
        }

        static private string removeXML(string str)
        {
            var sb = new StringBuilder(str.Length);
            bool copy = true;
            foreach (char i in str)
            {
                if (i == '<')
                    copy = false;
                if (copy)
                    sb.Append(i);
                if (i == '>')
                    copy = true;
            }
            return sb.ToString();
        }
    }

}
