﻿using System;
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
        Processor(ReadLine inputMethod)
        {
                readLine = inputMethod;
        }
	public delegate string ReadLine();
        private Readline readLine;	
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

        internal Degree processText(Degree degree)
        {
            string[][] tables = getTables().ToArray();

            parseDetails(tables[0]);
            parseSummary(tables[1]);
            parseZikui(tables[2], degree);
            int i;
            for (i = 3; i < tables.Length - 1; i++)
                parseSemester(tables[i].ToArray(), i, i == tables.Length, degree);
        }

        private PersonalDetails parseDetails(string[] table)
        {
            for (int i = 0; i < table.Length; i++)
            {
                table[i] = table[i].Substring(20);
                table[i] = putSpace(table[i].Remove(table[i].IndexOf("</TD")));
                if (i > 1)
                    table[i] = reverse(table[i]);
            }
            return new PersonalDetails(table);
        }

        private Summary parseSummary(string[] table)
        {
            return new Summary(removeXML(table[1].Replace("</TD><TD>", ",")).Split(','));
        }

        private void parseZikui(string[] table, Degree degree)
        {
            SemesterDetails sem = new SemesterDetails("זיכויים");
            sem.summary = new Summary(new string[] { "100", "100", "100" });
            sem.hebrewYear = sem.season = "";
            sem.numberOfCourses = table.Length - 3;
            string points = table[table.Length - 2].Split(new char[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries)[5];
            sem.summary.totalPoints = decimal.Parse(points);
            degree.AddSemester(sem);

            foreach (string line in table)
                if (line.StartsWith("<TR ALIGN"))
                    degree.AddSession(parseLine(line, 0));
        }

        private CourseSession parseLine(string line, int semester)
        {
            string[] x = line.Split(new string[] { "<TR ALIGN=RIGHT><td>", "</td><td>", "</td></TR>" }, 5, StringSplitOptions.RemoveEmptyEntries);
            string grade = x[0].Contains('י') ? reverse(x[0]) : x[0];
            string points = x[1];

            string[] nameAndId = x[2].Split(new string[] { "&nbsp;" }, 10, StringSplitOptions.RemoveEmptyEntries);
            string id = nameAndId.Last();
            string name = reverse(string.Join(" ", nameAndId.TakeWhile( str => str==id)));

            return new CourseSession(new Course(id, name, points), grade, semester);
        }

        private void parseSemester(string[] table, int semester, bool islast, Degree degree)
        {
            string[] args = strip(removeXML(table[0])).Split("() ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            args[0] = reverse(args[0]);
            args[2] = reverse(args[2]);
            SemesterDetails sem = new SemesterDetails(args);;
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

            degree.AddSemester(sem);

            foreach (string line in table)
                if (line.StartsWith("<TR ALIGN"))
                    degree.AddSession(parseLine(line, semester));
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
