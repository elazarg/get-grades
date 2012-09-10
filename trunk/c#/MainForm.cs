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
using System.Net.Sockets;
using System.Text;
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
            errorProvider1.Clear();

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            goButton.Enabled = false;
            toolStripProgressBar.Value = toolStripProgressBar.Minimum;
            backgroundWorker.RunWorkerAsync();
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
            try {
                grades = new Grades(useridTextbox.Text, passwordBox.Text, backgroundWorker);
                e.Result = true;
                return;
            }
            catch (ConnectionError) {
                statusLabel.Text = "שגיאת חיבור";
            }
            catch (SocketException) {
                statusLabel.Text = "שגיאת חיבור";
            }
            catch (BadHtmlFormat) {
                statusLabel.Text = "שגיאת הזדהות";
       //        MessageBox.Show(ex.Message, "שגיאה לא ידועה");
            }
            backgroundWorker.CancelAsync();
            e.Result = false;
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
                    case Grades.State.FAILED: statusLabel.Text = "נכשל. בדוק שם משתמש וסיסמא"; break;
                } 
        }

        private void refresh()
        {
            richTextBoxHtml.Text = grades.html;

            string htmlfilename = "Z:\\grades.html";
            File.WriteAllText(htmlfilename, grades.html, Connection.hebrewEncoding);
            browser.Navigate(htmlfilename);
            browser.Document.Encoding = "iso-8859-8-i";

            var details = myDatabaseDataSet.PersonalDetails.Last();
            labelName.Text = details.First_Name + " " + details.Last_Name;
            labelFaculty.Text = details.Faculty;
            labelProgram.Text = details.Program;

            var total =  myDatabaseDataSet.Semester.Last();
            textBoxAvGrade.Text = total.Average.ToString();
            textBoxPoints.Text = total.Points.ToString();
            textBoxSuccessRate.Text = total.Success_Rate.ToString();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try {
                if (!(bool)e.Result)
                    return;

                myDatabaseDataSet = grades.dataSet;
                foreach (var i in new DataGridView[] { dataGridViewSessions, dataGridViewCourseList, dataGridViewSemesters, dataGridViewCleanSlate }) {
                    i.DataSource = this.myDatabaseDataSet;
                }
                refresh();
                saveAsButton.Enabled = true;
            }
            finally {
                goButton.Enabled = true;
                this.Cursor = System.Windows.Forms.Cursors.Default;
                this.Refresh();
                this.Focus();
            }
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

        private void numericTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            Func<Keys, bool> isNumeral = delegate (Keys k) {
                return k >= Keys.D0 && k <= Keys.D9
                    || k >= Keys.NumPad0 && k <= Keys.NumPad9;
            };

            Keys[] goodKeys = new Keys[] { Keys.Delete, Keys.Back,
                    Keys.Right, Keys.Left, Keys.Home, Keys.End };

            TextBox Sender = (TextBox)sender;
            
            if (!e.Modifiers.HasFlag(Keys.Shift | Keys.Control | Keys.Alt))
                if (isNumeral(e.KeyData) && (Sender.TextLength < Sender.MaxLength || Sender.SelectionLength > 0)
                    || goodKeys.Contains(e.KeyData))
                    return;

            if (e.KeyData == Keys.Enter && goButton.Enabled)
            {
                goButton.PerformClick();
                return;
            }
            e.SuppressKeyPress = true;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox Sender = (TextBox)sender;
            goButton.Enabled =
                useridTextbox.TextLength /2  == useridTextbox.MaxLength /2 //8 or 9
                && passwordBox.TextLength == passwordBox.MaxLength;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (grades != null)
                grades.logOut();
            grades = null;

            browser.Navigate("about:blank");
            richTextBoxHtml.ResetText();
            
            foreach (var i in new Control[] {
                passwordBox,  useridTextbox,
                labelName,  labelFaculty, labelProgram,
                textBoxAvGrade, textBoxPoints, textBoxSuccessRate})
                i.ResetText();

            goButton.Enabled = false;
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Refresh();
            this.Focus();
        }


    }
}
