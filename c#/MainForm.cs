using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Net;
using System.Text.RegularExpressions;

namespace getGradesForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            browser.DocumentCompleted += delegate { browser.Document.Encoding = "iso-8859-8-i"; };
            this.Text += String.Format(" (גרסה {0})", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            activeContext = new ContextData { data = null, grid = dataGridViewCleanSlate };
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
            else {
                MessageBox.Show(this, "שגיאה לא צפויה. התוכנית תיסגר");
                this.Close();
            }
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            toolStripGoButton.Enabled = false;
            newToolStripButton.Enabled = false;
            toolStripRefresButton.Enabled = false;
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
            switch (Path.GetExtension(saveFileDialog.FileName)) {
                case ".csv":
                    grades.saveCsvFile(saveFileDialog.FileName);
                    break;
                case ".htm":
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

        string htmlfilename = Path.GetTempPath() + "getgrades.html";
        private void RefreshGrades()
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
            textBoxAvrgFaculty.Text = ugDatabase.totalFaculty.Average.ToString();
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
                RefreshGrades();
                saveToolStripButton.Enabled = true;

                contextMenuStrip1.Enabled = true;
                foreach (ToolStripMenuItem i in contextMenuStrip1.Items)
                    i.Enabled = true;
                contextMenuStrip1.Tag = null;

                contextMenuStripSemesters.Enabled = true;
                foreach (ToolStripMenuItem i in contextMenuStripSemesters.Items)
                    i.Enabled = true;
                contextMenuStripSemesters.Tag = null;

                //     panel1.Visible = true;
                this.toolStripRefresButton.Enabled = true;
                this.copyToolStripButton.Enabled = true;
            }
            finally {
                toolStripGoButton.Enabled = true;
                newToolStripButton.Enabled = true;
                toolStripProgressBar.Value = toolStripProgressBar.Maximum;
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


            if (File.Exists(htmlfilename))
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

            this.toolStripGoButton.Enabled = false;
            this.saveToolStripButton.Enabled = false;
            this.copyToolStripButton.Enabled = false;
            this.toolStripRefresButton.Enabled = false;
            //   panel1.Visible = false;
            this.contextMenuStrip1.Enabled = false;
            this.contextMenuStripSemesters.Enabled = false;
            this.activeContext = null;


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
            if (new Keys[] { Keys.Delete, Keys.Back, Keys.Right, Keys.Left, Keys.Home, Keys.End }
                .Contains(e.KeyData & ~Keys.Shift))
                return;

            TextBox Sender = (TextBox)sender;
            if (!e.Modifiers.HasFlag(Keys.Shift | Keys.Control | Keys.Alt) &&
                        (e.KeyData >= Keys.D0 && e.KeyData <= Keys.D9 || e.KeyData >= Keys.NumPad0 && e.KeyData <= Keys.NumPad9)
                        && (Sender.TextLength < Sender.MaxLength || Sender.SelectionLength > 0))
                return;

            if (e.KeyData == Keys.Enter && toolStripGoButton.Enabled) {
                toolStripGoButton.PerformClick();
                return;
            }
            e.SuppressKeyPress = true;
        }

        private bool passValid = false, idValid = false;
        private void userIdBox_TextChanged(object sender, EventArgs e)
        {
            TextBox Sender = (TextBox)sender;
            Func<int, int, int> tr = (int d, int i) => i % 2 == 1 ? d : (2 * d / 10) + (2 * d % 10);
            Func<string, bool> validateId = delegate(string userid) {
                var usernums = (from c in userid.Reverse() select int.Parse(char.ToString(c))).ToArray();
                return usernums[0] == (10 - usernums.Skip(1).Select(tr).Sum() % 10);
            };
            idValid = Sender.TextLength / 2 == Sender.MaxLength / 2 //8 or 9
                // && validateId(useridTextbox.Text)
                ;
            checkEnabled();
        }

        private void passwordBox_TextChanged(object sender, EventArgs e)
        {
            TextBox Sender = (TextBox)sender;
            passValid = Sender.TextLength == Sender.MaxLength;
            checkEnabled();
        }

        private void checkEnabled()
        {
            toolStripGoButton.Enabled = idValid && passValid;
        }

        private void dataGrid_SomethingChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (saveToolStripButton.Enabled) {
                grades.dataSet.updateCleanSlate();
                RefreshGrades();
                this.Update();
            }
        }

        private void dataGridViewSessions_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            dataGrid_SomethingChanged(sender, null);
        }

        private void dataGridViewCleanSlate_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (saveToolStripButton.Enabled) {
                textBoxAvrgClean.Text = ugDatabase.totalClean.Average.ToString();
                textBoxAvrgClean.Update();
            }
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        #region Context Menu

        class ContextData
        {
            internal DataGridView grid;
            internal Object data;
        }

        ContextData activeContext = null;
        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            var seq = e.TabPage.Controls.OfType<DataGridView>();

            if (seq.Count() > 0) {
                activeContext = new ContextData { grid = seq.First(), data = null };
                if (seq.First().RowCount > 0) {
                    copyToolStripButton.Enabled = true;
                    return;
                }
            }
            copyToolStripButton.Enabled = false;
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            toolStripMenuItemCopy_Click(sender, e);
        }

        private void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            DataGridView dgv = activeContext.grid;
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

            activeContext = new ContextData {
                grid = Sender,
                data = courseId
            };
            Sender.ContextMenuStrip.Items[1].Enabled = Info.isFacultyCS(courseId);
            Sender.ContextMenuStrip.Items[3].Enabled =
                Sender.AllowUserToDeleteRows
                && Sender.SelectedCells.Count > 0;
        }

        private void updatetoolStripButton_Click(object sender, EventArgs e)
        {
            new UpdateForm().ShowDialog(this);
        }


        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            Process.Start("www.technion.ac.il/~gai/cm/");
            Process.Start("http://www.undergraduate.technion.ac.il/Tadpis.html");
        }

        private void SurfToCourseToolStripMenuIt_Click(object sender, EventArgs e)
        {
            Process.Start("http://webcourse.cs.technion.ac.il/"
                + activeContext.data);
        }

        private void SurfToUGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem Sender = (ToolStripItem)sender;
            ToolStrip ts = Sender.GetCurrentParent();
            Process.Start("http://ug.technion.ac.il/rishum/mikdet.php?MK="
                + activeContext.data + "&SEM=201201");
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
            DataGridView dgv = activeContext.grid;
            foreach (DataGridViewRow row in dgv.SelectedRows)
                dgv.Rows.Remove(row);
        }

        #endregion

        ListSortDirection dr = ListSortDirection.Ascending;
        private void dataGridViewSessions_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.RowCount == 0)
                return;
            if (dgv.SortedColumn == dgv.Columns[e.ColumnIndex]) {
                if (dr == ListSortDirection.Ascending)
                    dr = ListSortDirection.Descending;
                else
                    dr = ListSortDirection.Ascending;
            }
            int index = e.ColumnIndex;
            if (dgv.Columns[e.ColumnIndex].HeaderText == "ציון")
                index++;
            dgv.Sort(dgv.Columns[index], dr);
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            if (grades == null)
                return;
            grades.bw = null;
            grades.process();
            this.RefreshGrades();
            this.Update();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            buttonClear_Click(sender, e);
        }



    }
}
