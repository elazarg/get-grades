using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using getGradesForms.Properties;
using System.Threading;
using System.Text.RegularExpressions;
namespace getGradesForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        void browser_Navigated(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            richTextBoxHtml.Text = browser.DocumentText;
        }

        void browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.ToString().Contains("ug"))
                e.Cancel = true;
            else
                MessageBox.Show(e.Url.ToString());

        }

        private void goButton_Click(object sender, EventArgs e)
        {
            if (Connection.getNetworkConnectionStatus() != System.Net.Sockets.SocketError.Success)
            {
                errorProvider1.SetError(goButton, "החיבור לשרת נכשל");
                return;
            }

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            goButton.Enabled = false;
            toolStripProgressBar.Value = toolStripProgressBar.Minimum;
            backgroundWorker.RunWorkerAsync();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            //Does not work for some reason
            TextBox textbox = (TextBox)sender;
            if (textbox.Text.Length == 0)
                return;
            string old = textbox.Text;
            textbox.Text = new string(textbox.Text.Where(char.IsDigit).ToArray());
            if (textbox.Text != old)
            {
                toolTip1.IsBalloon = true;
                toolTip1.Show("יש להזין ספרות בלבד", textbox, 1500);
            }
            goButton.Enabled = textbox.TextLength == textbox.MaxLength && (textbox.TextLength / 4 == 2);
        }

        private void passwordCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            passwordBox.UseSystemPasswordChar = !passwordCheckBox.Checked;
        }

        private void saveAs_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            grades.saveFile(saveFileDialog.FileName);
        }

        Grades grades;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            grades = new Grades(useridTextbox.Text, passwordBox.Text, backgroundWorker);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar.Increment(e.ProgressPercentage);
            if (e.UserState != null)
                switch ((Grades.State)e.UserState) {
                    case Grades.State.DONE: statusLabel.Text = "סיים"; break;
                    case Grades.State.PROCESSING: statusLabel.Text = "מעבד"; break;
                    case Grades.State.CONNECTING: statusLabel.Text = "מתחבר"; break;
                    case Grades.State.AUTHENTICATING: statusLabel.Text = "מבצע הזדהות"; break;
                } 
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            goButton.Enabled = true;
            saveAsButton.Enabled = true;

            myDatabaseDataSet = grades.dataSet;
            foreach (var i in new DataGridView[] { dataGridViewSessions, dataGridViewCourseList, dataGridViewSemesters, dataGridViewCleanSlate })
            {
                i.DataSource = this.myDatabaseDataSet;
            }
            File.WriteAllText("C:\\Temp\\grades.html", grades.html, Connection.hebrewEncoding);
            richTextBoxHtml.Text = grades.html;
            browser.Navigate("C:\\Temp\\grades.html");
            var details = myDatabaseDataSet.PersonalDetails.Last();
            labelName.Text = details.First_Name + " " + details.Last_Name;
            labelFaculty.Text = details.Faculty;
            labelProgram.Text = details.Program;
           // File.WriteAllText("Z:\\grades.html", grades.html);
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

        private void labelPD_TextChanged(object sender, EventArgs e)
        {
            ((Label)sender).Visible = true;
        }

    }
}
