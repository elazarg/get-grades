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
        internal Processor(String html_in)
        {
            this.raw_html = "<HTML DIR=\"RTL\"><BODY><DIV ALIGN=RIGHT>"
                + Regex.Match(html_in, "(?<=<P>).*</HTML>", RegexOptions.Singleline).Value;
            fixHtml();
        }
        
        string raw_html;
        internal string fixedHtml;
        private string[][][] tables;
        
        public delegate void PersonalDetailsFound(string date, string id, string name, string program, string faculty);
        public event PersonalDetailsFound personalDetailsFound = delegate { };

        public delegate void SessionFound(string Course_ID, string name, string points, string grade);
        public event SessionFound sessionFound = delegate { };

        public delegate void SemesterFound(string year, string hebrewYear, string season);
        public event SemesterFound semesterFound = delegate { };

        internal void processText()
        {
            parseDetails(tables[0]);
            //TODO Summary - tables[1][1];

            for (int i = 2; i < tables.Length; i++)
                parseSemester(tables[i], i, i == tables.Length);
        }
        
        private void parseDetails(string[][] table)
        {
            personalDetailsFound(table[0][1], table[1][1], table[2][1], table[3][1], table[4][1]);
        }
        
        private void parseSemester(string[][] table, int semester, bool islast)
        {
            //TODO Summary
            foreach (string[] line in table)
            {
                if (Regex.IsMatch(line[0], "[0-9]{6}"))
                    parseLine(line);
                else if (line[0].StartsWith("תש"))   
                    semesterFound(line[3], line[0], line[1]);
            }
        }

        private void parseLine(string[] line)
        {
            sessionFound(line[0], line[1], line[2], line[3]);
        }

        static string reverse(string str)
        {
            return new string(str.Reverse().ToArray());
        }

        static private string reverseSession(Match s)
        {
            string[] nameAndId = s.Value.Split(new string[] { " " }, 10, StringSplitOptions.None);

            var courseId = Regex.Match(s.Value, "[0-9]{6}", RegexOptions.Compiled);
            string name = reverse(string.Join(" ", nameAndId.TakeWhile(str => str != courseId.Value)))
                            .Replace('(', '$').Replace(')', '(').Replace('$', ')');
            if (courseId.Success)
                name += "</td><td>" + courseId.Value;

            return name;
        }

        static private string fixSemesterHead(Match s)
        {
            string[] arr = s.Value.Remove(0, 10).Split("() ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return "<TR BGCOLOR=#D3D3D3 ALIGN=CENTER>\r\n<td>" + string.Join("</td><td>", new string[] { arr[4], "<BR>", arr[5], arr[3] });
        }

        internal void fixHtml()
        {
            //TODO split function

            string html =raw_html
                        .Replace("TD", "td")
                        .Replace("<TR ALIGN=RIGHT><td>", "<TR ALIGN=RIGHT>\r\n<td>")
                        .Replace("</TR>", "\r\n</TR>")
                        .Replace("</td>\r\n<td", "</td><td")
                        .Replace("&nbsp;", " ");

            //now html is ready to the actual work
            html = Regex.Replace(html, "<TR BGCOLOR=#FFCC00 ALIGN=CENTER><td COLSPAN=3>"
                    + "[(][א-ת\"]" + "{4,5}[)][ ]?[0-9]{4}/[01][0-9] " + "[א-ת]" + "{3,4}",
                    fixSemesterHead, RegexOptions.Compiled);

            string pattern = "[א-ת]" + "([0-9" + "א-ת \\-\"'\\./()])*";
            html = Regex.Replace(html, pattern, reverseSession, RegexOptions.Compiled);

          
            string[] lines = html.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("<td>"))
                {
                    lines[i] = "<td>" + string.Join("</td><td>", lines[i].Substring(4, lines[i].Length - 9).Split(new string[] { "</td><td>" }, StringSplitOptions.None).Reverse()) + "</td>";
                    switch (lines[i].Substring(4, 5))
                    {
                        case "ממוצע" : lines[i] = "<td>ממוצע</td><td>שיעור הצלחות</td><td>נקודות מצטברות</td>"; break;
                        case "סה\"כ ": lines[i] = lines[i].Replace("<td>סה\"כ", "<td COLSPAN=2>סה\"כ"); break;
                        case "סה\"כ<":
                            string[] temp = new string[3];
                            int j = 0;
                            foreach (Match match in Regex.Matches(lines[i],"[0-9]{1,3}(.[0-9]+)?")) 
                            {
                                temp[j] = match.Value;
                                j++;
                            }
                            lines[i] = "<td>" + string.Join("</td><td>", new string[] { "סה\"כ", temp[2]+ "%" + " הצלחות " , temp[0], temp[1] }) + "</td>";
                            break;
                        case "שם מק": lines[i] = lines[i].Replace("<td>שם מקצוע</td>", "<td>מספר מקצוע</td><td>שם מקצוע</td>"); break;
                    }
                }
            }

            // fix grades headers
            html = string.Join("\r\n", lines).Replace("<td COLSPAN=3>", "<td COLSPAN=4>")
                         .Replace("ע<BR>", "ע</td><td>");

            // Union grade tables
            string tabsep = "</TABLE>\r\n<BR>\r\n<TABLE BORDER=1 CELLPADDING=3 CELLSPACING=0>";
            string[] fulltables = html.Split(new string[] {tabsep}, StringSplitOptions.None);

            this.fixedHtml = fulltables[0] + tabsep + fulltables[1] + tabsep + string.Join("", fulltables.Skip(2));

            //the ultimate split - [table][line][cell]
            tables = (from t in fulltables
                      select (from x in t.Split(new string[] { "\r\n" }, StringSplitOptions.None)
                              where x.StartsWith("<td>")
                              select x.Split(new string[] { "<td>", "</td>" }, StringSplitOptions.RemoveEmptyEntries)
                              ).ToArray()
                             ).ToArray();
        }
    }
}