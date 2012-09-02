using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Linq;
namespace getGradesForms
{
    public partial class MainForm : Form
    {
        class Result
        {
            internal String label;
        };

        public MainForm()
        {
            InitializeComponent();
        }

        Result r;

        void go()
        {
            this.myDatabaseDataSet.init();
            r = new Result();
            r.label = "Connecting"; backgroundWorker1.ReportProgress(1);
            using (Connection conn = new Connection())
            {
                conn.tick += delegate { backgroundWorker1.ReportProgress(20); };
                r.label = "Authenticating";  backgroundWorker1.ReportProgress(1);
                
                TextReader reader = conn.retrieveHTML(useridTextbox.Text, passwordBox.Text);
            
                r.label = "Processing"; backgroundWorker1.ReportProgress(1);

                var pr = new Processor(reader.ReadLine);
                pr.sessionFound += this.myDatabaseDataSet.addSessionToSQL;
                pr.semesterFound += this.myDatabaseDataSet.addSemesterToSQL;
                pr.personalDetailsFound += this.myDatabaseDataSet.addPersonalDetails;
                pr.processText();

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
           // File.WriteAllText(saveFileDialog1.FileName, textBox2.Text, Connection.hebrewEncoding);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = r;
            go();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Increment(e.ProgressPercentage);
            statusLabel.Text = r.label;

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Result res = r;
            statusLabel.Text = r.label;
            goButton.Enabled = true;
            saveAsButton.Enabled = true;
            
            this.Refresh();
        }
        
        
        AboutBox ab = new AboutBox();
        private void button1_Click(object sender, EventArgs e)
        {
            ab.Activate();
            ab.Show();
        }

    }
}
