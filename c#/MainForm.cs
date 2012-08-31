using System;
using System.IO;
using System.Net.Sockets;
using System.Windows;
using System.Text;

using System.Linq;
using System.Linq.Expressions;

using System.Collections.Generic;

using System.Text.RegularExpressions;

using System.Drawing;
using System.Windows.Forms;

using System.Threading;
using System.ComponentModel;
using System.Net;

namespace getGradesForms
{
    public partial class MainForm : Form
    {
        class Result
        {
            internal String html;
            internal String csv;
            internal String label;
        };


        public MainForm()
        {
            InitializeComponent();
        }

        Result r;
        Degree degree;

        void go()
        {
            r = new Result();
            r.label = "Connecting"; backgroundWorker1.ReportProgress(1);
            using (Connection conn = new Connection())
            {
                conn.tick += delegate { backgroundWorker1.ReportProgress(20); };
                r.label = "Authenticating";  backgroundWorker1.ReportProgress(1);
                
                TextReader reader = conn.retrieveHTML(useridTextbox.Text, passwordBox.Text);
            
                r.label = "Processing"; backgroundWorker1.ReportProgress(1);
                degree = new Degree();
                degree.tick += delegate { backgroundWorker1.ReportProgress(1); };
                
                new Processor(reader.ReadLine).processText(degree);
                
                r.label = "Done";
            }
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            goButton.Enabled = false;
            toolStripProgressBar1.Value = toolStripProgressBar1.Minimum;
            backgroundWorker1.RunWorkerAsync();
            browser.Navigate("www.undergraduate.technion.ac.il/Tadpis.html");
        }

        private void passwordBox_TextChanged(object sender, EventArgs e)
        {
            if (passwordBox.Text.Length == 0)
                return;
            if (! Char.IsDigit(passwordBox.Text[passwordBox.Text.Length-1]))
                passwordBox.Text.Remove(passwordBox.Text.Length-1);
            goButton.Enabled = passwordBox.TextLength == passwordBox.MaxLength && (useridTextbox.TextLength/4==2);
        }

        private void passwordCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            passwordBox.UseSystemPasswordChar = !passwordCheckBox.Checked;
        }

        private void saveAs_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.WriteAllText(saveFileDialog1.FileName, textBox1.Text, Connection.hebrewEncoding);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = r;
            go();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Increment(e.ProgressPercentage);
            textBox1.Text = r.csv;
            htmlTextBox.Text = r.html;
            statusLabel.Text = r.label;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Result res = r;
            textBox1.Text = res.csv;
            htmlTextBox.Text = res.html;
            statusLabel.Text = r.label;

            textBox3.Text = degree.ToString();
            goButton.Enabled = true;
            saveAsButton.Enabled = true;
        }
        
        
        AboutBox ab = new AboutBox();
        private void button1_Click(object sender, EventArgs e)
        {
            ab.Activate();
            ab.Show();
        }


        private void degreeBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
