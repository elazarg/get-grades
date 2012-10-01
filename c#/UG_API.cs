using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace getGradesForms
{
    static class UG_API
    {
        static WebClient wc = new WebClient();

        public enum StudentStatus
        {
            DoesNotExist,
            NoRishumTime,
            Valid
        }

        public static StudentStatus getRtime(string userid, out DateTime rtime)
        {
            rtime = DateTime.Today;
            string s = wc.DownloadString(
                string.Format("http://ug.technion.ac.il/rishum/zimun.php?ID={0}", userid));

            if (Regex.IsMatch(s, "םייק אל "+ userid +" טנדוטס"))
                return StudentStatus.DoesNotExist;

            Match m = Regex.Match(s, @"(\d\d?(?:\:\d\d)?)</TD>\s*<TD>(\d\d\.\d\d\.\d{4})", RegexOptions.Singleline);
            if (DateTime.TryParse(m.Groups[2].Value + " " + m.Groups[1].Value + ":00", out rtime))
                return StudentStatus.Valid;
            return StudentStatus.NoRishumTime;
        }
    }
}
