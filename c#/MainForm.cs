using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace getGradesForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            browser.DocumentCompleted += delegate { browser.Document.Encoding = "iso-8859-8-i"; };
            this.Text += String.Format(" (גרסה {0})", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        void browser_Navigated(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           // textBoxHtml.Text = browser.DocumentText;
        }

        void browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.ToString().Contains("gai"))
                return;

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
            buttonGo.Enabled = false;
            buttonClear.Enabled = false;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

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
                case ".htm" :
                case ".html":
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
                e.Result = "שגיאת חיבור";
            }
            catch (SocketException) {
                e.Result = "שגיאת חיבור";
            }
            catch (BadHtmlFormat) {
                e.Result = "שגיאת הזדהות";
            }
            backgroundWorker.CancelAsync();
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
         //   textBoxHtml.Text = grades.html;
            
            File.WriteAllText(htmlfilename, grades.html, Connection.hebrewEncoding);
            browser.Navigate(htmlfilename);
            

            var details = ugDatabase.personalDetails;
            labelName.Text = details.firstName + " " + details.lastName + " ," + details.id;
            labelFaculty.Text = details.faculty;
            labelProgram.Text = details.program;

            textBoxAvGrade.Text = ugDatabase.total.Average.ToString();
            textBoxPoints.Text = ugDatabase.total.Points.ToString();
            textBoxSuccessRate.Text = ugDatabase.total.SuccessRate.ToString();

          //  textBoxPtsClean.Text = ugDatabase.totalClean.Points.ToString();
            textBoxAvrgClean.Text = ugDatabase.totalClean.Average.ToString();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try {
                string error = e.Result as String;
                if (error != null) {
                    statusLabel.Text = error;
                    return;
                }

                ugDatabase = grades.dataSet;
                foreach (var i in new DataGridView[] { dataGridViewSessions, dataGridViewCourseList, dataGridViewSemesters, dataGridViewCleanSlate }) {
                    uGDatabaseBindingSource.DataSource = this.ugDatabase;
                }
                refresh();
                saveAsButton.Enabled = true;

                contextMenuStrip1.Enabled = true;
                foreach (ToolStripMenuItem i in contextMenuStrip1.Items)
                    i.Enabled = true;
                contextMenuStrip1.Tag = null;

                contextMenuStripSemesters.Enabled = true;
                foreach (ToolStripMenuItem i in contextMenuStripSemesters.Items)
                    i.Enabled = true;
                contextMenuStripSemesters.Tag = null;

                panel1.Visible = true;
            }
            finally {
                buttonGo.Enabled = true;
                buttonClear.Enabled = true;

                this.Cursor = System.Windows.Forms.Cursors.Default;
                this.Refresh();
                this.Focus();
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (grades != null)
                grades.logOut();
            grades = null;

            browser.Navigate("about:blank");
            //      textBoxHtml.ResetText();

            File.Delete(htmlfilename);
            foreach (Control i in new Control[] {
                    passwordBox,  useridTextbox,
                    labelName,  labelFaculty, labelProgram,
                    textBoxAvGrade, textBoxPoints, textBoxSuccessRate,
                    textBoxAvrgClean
                })
                i.ResetText();
            foreach (var i in new DataGridView[] { dataGridViewSessions, dataGridViewCourseList, dataGridViewSemesters, dataGridViewCleanSlate }) {
                uGDatabaseBindingSource.Clear();// DataSource = this.ugDatabase;
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;

            this.buttonGo.Enabled = false;
            this.saveAsButton.Enabled = false;
            panel1.Visible = false;
            contextMenuStrip1.Enabled = false;
            contextMenuStripSemesters.Enabled = false;

            this.Refresh();
            this.Focus();
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
            if ( !e.Modifiers.HasFlag(Keys.Shift | Keys.Control | Keys.Alt) &&
                        (e.KeyData >= Keys.D0 && e.KeyData <= Keys.D9 || e.KeyData >= Keys.NumPad0   && e.KeyData <= Keys.NumPad9)
                        && (Sender.TextLength < Sender.MaxLength || Sender.SelectionLength > 0))
                    return;

            if (e.KeyData == Keys.Enter && buttonGo.Enabled)
            {
                buttonGo.PerformClick();
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
            buttonGo.Enabled = idValid && passValid;
        }

        private void dataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (saveAsButton.Enabled) {
                grades.dataSet.updateCleanSlate();
                refresh();
                this.Update();
            }
        }

        private void dataGridViewCleanSlate_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("www.technion.ac.il/~gai/cm/");
            Process.Start("http://www.undergraduate.technion.ac.il/Tadpis.html");
        }

        #region Context Menu

        struct ContextData
        {
            internal DataGridView grid;
            internal Object data;

            static internal ContextData fromSender(object sender)
            {
                ToolStripItem Sender = (ToolStripItem)sender;
                ToolStrip ts = Sender.GetCurrentParent();
                return (ContextData)ts.Tag;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataGridView dgv = ContextData.fromSender(sender).grid;
            dgv.SuspendLayout();
            dgv.RightToLeft = RightToLeft.No;
            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dgv.SelectAll();
            Clipboard.SetDataObject(dgv.GetClipboardContent());

            dgv.ClearSelection();
            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText;
            dgv.RightToLeft = RightToLeft.Yes;
            dgv.ResumeLayout();
        }

        private void dataGridViewCleanSlate_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            DataGridView Sender = (DataGridView)sender;
            string courseId = Sender.Rows[e.RowIndex].Cells[0].Value.ToString();

            Sender.ContextMenuStrip.Tag = new ContextData {
                grid = Sender,
                data = courseId
            };
            Sender.ContextMenuStrip.Items[1].Enabled = Info.isFacultyCS(courseId);
        }

        private void SurfToCourseToolStripMenuIt_Click(object sender, EventArgs e)
        {
            Process.Start("http://webcourse.cs.technion.ac.il/"
                + ContextData.fromSender(sender).data);
        }

        private void SurfToUGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem Sender = (ToolStripItem)sender;
            ToolStrip ts = Sender.GetCurrentParent();
            Process.Start("http://ug.technion.ac.il/rishum/mikdet.php?MK="
                + ((ContextData)ts.Tag).data + "&SEM=201201");
        }



        private void dataGridViewSemesters_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            DataGridView Sender = (DataGridView)sender;
            Sender.ContextMenuStrip.Tag = new ContextData {
                grid = Sender,
                data = new Tuple<int, int>(e.RowIndex, e.ColumnIndex)
            };
        }

        private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dgv = ContextData.fromSender(sender).grid;
            foreach (DataGridViewRow row in dgv.SelectedRows)
                dgv.Rows.Remove(row);
        }

        #endregion

        ListSortDirection dr = ListSortDirection.Ascending;
        private void dataGridViewSessions_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            dgv.Sort(dgv.Columns[e.ColumnIndex], dr);
            if (dr == ListSortDirection.Ascending)
                dr = ListSortDirection.Descending;
            else
                dr = ListSortDirection.Ascending;
        }



    }
}
