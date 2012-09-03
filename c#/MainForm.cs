using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Data.SqlTypes;
namespace getGradesForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        class Result
        {
            internal String label;
        };

        Result r;

        void go()
        {
            r = new Result();
            r.label = "מתחבר";
            using (Connection conn = new Connection())
            {
                conn.tick += delegate { backgroundWorker1.ReportProgress(20); };
                r.label = "מבצע הזדהות";
                
                TextReader reader = conn.retrieveHTML(useridTextbox.Text, passwordBox.Text);
            
                r.label = "מעבד";

                var pr = new Processor(reader.ReadLine);
                pr.sessionFound += this.myDatabaseDataSet.addSessionToSQL;
                pr.semesterFound += this.myDatabaseDataSet.addSemesterToSQL;
                pr.personalDetailsFound += this.myDatabaseDataSet.addPersonalDetails;
                pr.processText();

                r.label = "סיים";
            }
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            tabControl1.Enabled = false;
            foreach (var i in new DataGridView[] { dataGridView1, dataGridView2, dataGridView3, dataGridView4, dataGridView5 })
            {
                if (i.Rows.Count > 0)
                    i.Rows.RemoveAt(0);
            }
            tabControl1.Enabled = true;


            myDatabaseDataSet.Clear();

            this.myDatabaseDataSet.init();

            goButton.Enabled = false;
            toolStripProgressBar1.Value = toolStripProgressBar1.Minimum;
            backgroundWorker1.RunWorkerAsync();
            browser.Navigate("www.undergraduate.technion.ac.il/Tadpis.html");
        }

        private void passwordBox_TextChanged(object sender, EventArgs e)
        {
            //Dooes not work for some reason
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
            var txt = string.Join("\r\n", from row in myDatabaseDataSet.ViewTable
                    select string.Join(" , ", row.ItemArray));
            File.WriteAllText(saveFileDialog1.FileName, txt , Connection.hebrewEncoding);
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
            statusLabel.Text = r.label;
            goButton.Enabled = true;
            saveAsButton.Enabled = true;

            myDatabaseDataSet.updateCleanSlate();
            this.Refresh();
            this.Focus();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        
        
        AboutBox ab = new AboutBox();
        private void button1_Click(object sender, EventArgs e)
        {
            ab.Activate();
            ab.Show();
        }

    }
}
