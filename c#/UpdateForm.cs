﻿using System;
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
        string thisFilename = System.Reflection.Assembly.GetEntryAssembly().Location;
        WebClient webClient;
        public UpdateForm()
        {
            InitializeComponent();
            webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringAsync(new Uri("http://get-grades.googlecode.com/svn/trunk/c%23/AssemblyInfo.cs"));
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try {
                string info = e.Result;

                Match m = Regex.Match(info, @"[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+");
                if (m.Success && m.Value != Assembly.GetExecutingAssembly().GetName().Version.ToString()) {
                    buttonDownload.Enabled = true;
                    linkLabel1.Enabled = true;
                    label1.Text = "נמצאה גרסה חדשה.";
                }
                else {
                    label1.Text = "לא נמצאו עדכונים.";
                }
            }
            catch (WebException) {
                label1.Text = "אירעה שגיאה בעת החיבור לאינטרנט.";
            }
            catch (TargetInvocationException) {
                //We're closing the window. continue normally.
            }
        }
        
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            label1.Text = "ההורדה הושלמה.\r\nלהשלמת הפעולה הפעל מחדש את התוכנית.";
            buttonDownload.Enabled = false;
            linkLabel1.Enabled = false;
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
                string targetFile = thisFilename;
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

        protected override void OnClosed(EventArgs e)
        {
            webClient.CancelAsync();
            base.OnClosed(e);
        }
    }
}