using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;

namespace getGradesForms
{
    public partial class UpdateForm : Form
    {

        WebClient webClient;
        public UpdateForm()
        {
            InitializeComponent();
            webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringAsync(new Uri("http://get-grades.googlecode.com/svn/trunk/c%23/AssemblyInfo.cs"));
            textBox1.Text = System.Reflection.Assembly.GetEntryAssembly().Location;
            //Dialog1.InitialDirectory = System.Reflection.Assembly.GetEntryAssembly().Location;
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try {
                string info = e.Result;

                Match m = Regex.Match(info, @"[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+");
                if (m.Success && m.Value != Assembly.GetExecutingAssembly().GetName().Version.ToString()) {
                    buttonDownload.Enabled = true;
                    buttonDirectory.Enabled = true;
                    label1.Text = "נמצאה גרסה " + m.Value + ".\r\nלהוריד?\r\n";
                }
                else {
                    label1.Text = "לא נמצאו עדכונים.";
                }
            }
            catch (WebException) {
                label1.Text = "אירעה שגיאה בעת החיבור לאינטרנט.";
            }
        }
        
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            label1.Text = "ההורדה הושלמה.\r\nלהשלמת הפעולה הפעל מחדש את התוכנית.";
            buttonDownload.Enabled = false;
            buttonDirectory.Enabled = false;
            buttonCancel.Text = "אישור";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonDownload.Enabled = false;
            try
            {
                string targetFile = textBox1.Text;
                Uri webAddress = new Uri("http://get-grades.googlecode.com/svn/trunk/c%23/download/getGrades.exe");
                if (File.Exists(targetFile))
                    File.Delete(targetFile);
                webClient.DownloadFileAsync(webAddress, targetFile);
            }
            catch (WebException) {
                label1.Text = "אירעה שגיאה בעת החיבור לאינטרנט.";
            }
            catch (InvalidOperationException) {
                label1.Text = "אירעה שגיאה בעת החיבור לאינטרנט.";
            }
            catch (IOException) {
                label1.Text = "לא ניתן לשמור את הקובץ.";
                buttonDownload.Enabled = true;
            }
            catch (UnauthorizedAccessException) {
                label1.Text = "לא ניתן לשמור את הקובץ.";
                buttonDownload.Enabled = true;
            }

        }

        private void buttonDirectory_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog(this);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox1.Text = saveFileDialog1.FileName;
        }

    }
}
