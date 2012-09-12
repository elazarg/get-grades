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

using System.Net;

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
            {
                MessageBox.Show(this, "שגיאה לא צפויה. התוכנית תיסגר");
                this.Close();
            }

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
            MessageBox.Show(Path.GetExtension(saveFileDialog.FileName));
            switch (Path.GetExtension(saveFileDialog.FileName))
            {
                case ".csv": 
                    grades.saveCsvFile(saveFileDialog.FileName);
                    break;
                case ".htm" :  case ".html":
                    File.WriteAllText(saveFileDialog.FileName, grades.html, Connection.hebrewEncoding);
                    break;
            }
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

        UGDatabase ugDatabase;

        string htmlfilename =  Path.GetTempPath() + "getgrades.html";
        private void refresh()
        {
            richTextBoxHtml.Text = grades.html;
            
            File.WriteAllText(htmlfilename, grades.html, Connection.hebrewEncoding);
            browser.Navigate(htmlfilename);
            browser.Document.Encoding = "iso-8859-8-i";

            var details = ugDatabase.personalDetails;
            labelName.Text = details.firstName + " " + details.lastName + " ," + details.id;
            labelFaculty.Text = details.faculty;
            labelProgram.Text = details.program;

            textBoxAvGrade.Text = ugDatabase.total.Average.ToString();
            textBoxPoints.Text = ugDatabase.total.Points.ToString();
            textBoxSuccessRate.Text = ugDatabase.total.SuccessRate.ToString();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try {
                if (!(bool)e.Result)
                    return;

                ugDatabase = grades.dataSet;
                foreach (var i in new DataGridView[] { dataGridViewSessions, /*dataGridViewCourseList,*/ dataGridViewSemesters, dataGridViewCleanSlate
                }) {
                    uGDatabaseBindingSource.DataSource = this.ugDatabase;
                    i.Refresh();
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

        AboutBox ab = null;
        private void aboutButton_Click(object sender, EventArgs e)
        {
            if (ab == null || ab.IsDisposed)
                ab = new AboutBox();
            ab.Activate();
            if (!ab.Visible)
                ab.Show(this);
        }

        private void labelPD_TextChanged(object sender, EventArgs e)
        {
            ((Label)sender).Visible = true;
        }

        private void numericTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (new Keys[] { Keys.Delete, Keys.Back,  Keys.Right, Keys.Left, Keys.Home, Keys.End }
                .Contains(e.KeyData & ~Keys.Shift))
                return;

            TextBox Sender = (TextBox)sender;
            if (!e.Modifiers.HasFlag(Keys.Shift | Keys.Control | Keys.Alt)
                        &&  (e.KeyData >= Keys.D0 && e.KeyData <= Keys.D9 || e.KeyData >= Keys.NumPad0   && e.KeyData <= Keys.NumPad9)
                        && (Sender.TextLength < Sender.MaxLength || Sender.SelectionLength > 0))
                    return;

            if (e.KeyData == Keys.Enter && goButton.Enabled)
            {
                goButton.PerformClick();
                return;
            }
            e.SuppressKeyPress = true;
        }

        private bool passValid = false, idValid = false;
        private void userIdBox_TextChanged(object sender, EventArgs e)
        {
            Func<int, int, int> tr = (int d, int i) => i % 2 == 1 ? d : (2 * d / 10) + (2 * d % 10);
            Func<string, bool> validateId = delegate(string userid)
            {
                var usernums = (from c in userid.Reverse() select int.Parse(char.ToString(c))).ToArray();
                return usernums[0] == (10 - usernums.Skip(1).Select(tr).Sum() % 10);
            };
            idValid = useridTextbox.TextLength / 2 == useridTextbox.MaxLength / 2 //8 or 9
                && validateId(useridTextbox.Text);
            checkEnabled();
        }

        private void passwordBox_TextChanged(object sender, EventArgs e)
        {
            passValid = passwordBox.TextLength == passwordBox.MaxLength;
            checkEnabled();
        }

        private void checkEnabled()
        {
            goButton.Enabled = idValid && passValid;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (grades != null)
                grades.logOut();
            grades = null;

            browser.Navigate("about:blank");
            richTextBoxHtml.ResetText();

            File.Delete(htmlfilename);
            foreach (Control i in new Control[] {
                    passwordBox,  useridTextbox,
                    labelName,  labelFaculty, labelProgram,
                    textBoxAvGrade, textBoxPoints, textBoxSuccessRate
                })
                i.ResetText();

 
            this.Cursor = System.Windows.Forms.Cursors.Default;

            this.goButton.Enabled = false;
            this.saveAsButton.Enabled = false;
            this.Refresh();
            this.Focus();
        }

        private void dataGridViewSessions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (saveAsButton.Enabled) {
                grades.dataSet.updateCleanSlate();
                refresh();
                this.Update();
            }
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
    }
}
