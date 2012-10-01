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
using System.Data;
using System.Drawing;

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
            grades.tick += grades_tick;
        }
        
        Grades grades = new Grades();
        string htmlfilename = Path.GetTempPath() + "getgrades.html";

        #region TextBox

        private bool passValid = false, idValid = false;

        private void checkEnabled()
        {
            toolStripGoButton.Enabled = idValid && passValid;
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

        private void userIdBox_TextChanged(object sender, EventArgs e)
        {
            TextBox Sender = (TextBox)sender;
            int[,] arr = new int[2, 10] {
                {0,1,2,3,4,5,6,7,8,9},
                {0,2,4,6,8,1,3,5,7,9}
            };
            Func<string, bool> validateId = delegate(string userid) {
                var usernums = userid.Select(c => int.Parse(char.ToString(c)));
                return usernums.Last() == (10-usernums.Take(8).Select((d, i) => arr[i%2, d]).Sum()%10) % 10;
            };
            idValid = Sender.TextLength / 2 == Sender.MaxLength / 2 //8 or 9
                // && validateId(useridTextbox.Text)
                ;
            if (idValid) {
                string id = useridTextbox.Text;
                if (id.Length < 9)
                    id = "0" + id;
                if (validateId(id)) {
                    DateTime rtime;
                    switch (UG_API.getRtime(id, out rtime)) {
                        case UG_API.StudentStatus.Valid:
                            zimunLabel.Text = "תאריך רישום: " + rtime;
                            Sender.ForeColor = Color.Green;
                            break;
                        case UG_API.StudentStatus.NoRishumTime:
                            zimunLabel.Text = "";
                            Sender.ForeColor = Color.Black;
                            break;
                        case UG_API.StudentStatus.DoesNotExist:
                            zimunLabel.Text = "סטודנט לא קיים";
                            Sender.ForeColor = Color.Red;
                            break;
                    }
                }
                else {
                    Sender.ForeColor = Color.Blue;
                    zimunLabel.Text = "מספר סטודנט לא תקין";
                }
            }
            else {
                Sender.ForeColor = Color.Black;
            }
            checkEnabled();
        }

        private void passwordBox_TextChanged(object sender, EventArgs e)
        {
            TextBox Sender = (TextBox)sender;
            passValid = Sender.TextLength == Sender.MaxLength;
            checkEnabled();
        }

        private void passwordCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            passwordBox.UseSystemPasswordChar = !passwordCheckBox.Checked;
            toolTip1.SetToolTip(passwordCheckBox,
                passwordCheckBox.Checked ? "לחץ להסתיר תווים" : "לחץ להציג תווים");
        }

        #endregion
       
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

        #region BGworker

        BackgroundWorker backgroundWorker;

        private BackgroundWorker createBackgroundWorker()
        {
            grades.tick -= grades_tick;
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;

            bw.DoWork += this.backgroundWorker1_DoWork;
            bw.ProgressChanged += this.backgroundWorker1_ProgressChanged;
            bw.RunWorkerCompleted += this.backgroundWorker1_RunWorkerCompleted;
            grades.tick += grades_tick;
            return bw;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try {
                grades = new Grades();
                grades.connect();
                grades.retrieve(useridTextbox.Text, passwordBox.Text);
                grades.process();
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
            catch (DimaErorr) {
                e.Result = "שגיאה בקריאה מהשרת";
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
                    case Grades.State.CONNECTED: statusLabel.Text = "מחובר"; break;
                    case Grades.State.AUTHENTICATING: statusLabel.Text = "מבצע הזדהות"; break;
                    case Grades.State.AUTHENTICATED: statusLabel.Text = "הזדהות הושלמה"; break;
                    case Grades.State.FAILED: statusLabel.Text = "נכשל. בדוק שם משתמש וסיסמא"; break;
                }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try {
                toolStripProgressBar.Value = toolStripProgressBar.Maximum;
                string error = e.Result as String;
                if (error != null) {
                    statusLabel.Text = error;
                    return;
                }

                RefreshGrades(grades.document.dataSet);
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
                statusLabel.Text = "סיים";
            }
            finally {
                toolStripGoButton.Enabled = true;
                newToolStripButton.Enabled = true;
                this.Cursor = System.Windows.Forms.Cursors.Default;
                this.Refresh();
                this.Focus();
            }

        }

        void grades_tick(Grades.State state)
        {
            if (backgroundWorker != null)
                backgroundWorker.ReportProgress(10, state);
        }
        #endregion

        private void goButton_Click(object sender, EventArgs e)
        {
            toolStripGoButton.Enabled = false;
            newToolStripButton.Enabled = false;
            toolStripRefresButton.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            toolStripProgressBar.Value = toolStripProgressBar.Minimum;

            backgroundWorker = createBackgroundWorker();

            backgroundWorker.RunWorkerAsync();
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
                    File.WriteAllText(saveFileDialog.FileName, grades.document.html, ConnectionControl.hebrewEncoding);
                    break;
            }
        }
        
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            if (grades == null)
                return;
            grades.restore();
            this.RefreshGrades(grades.document.dataSet);
            this.Update();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (grades != null)
                grades.logOut();

            browser.Navigate("about:blank");
            if (File.Exists(htmlfilename))
                File.Delete(htmlfilename);

            foreach (Control i in new Control[] { passwordBox,  useridTextbox})
                i.ResetText();
            uGDatabaseBindingSource.Clear();

            this.Cursor = System.Windows.Forms.Cursors.Default;

            foreach (var i in new [] { toolStripGoButton, saveToolStripButton,
                                        copyToolStripButton, toolStripRefresButton })
                i.Enabled = false;
            foreach (var i in new [] {  contextMenuStrip1,  contextMenuStripSemesters } )
                i.Enabled = false;
            this.activeContext = null;


            this.Refresh();
            this.Focus();
        }

        private void updatetoolStripButton_Click(object sender, EventArgs e)
        {
            new UpdateForm().ShowDialog(this);
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
        
        #region DataGrid

        private void RefreshGrades(UGDatabase ds)
        {
            uGDatabaseBindingSource.DataSource = ds;
            personalDetailsBindingSource.DataSource = ds.personalDetails;
            summaryBindingSource.DataSource = ds.total;
            summaryBindingSourceClean.DataSource = ds.totalClean;
            summaryBindingSourceFaculty.DataSource = ds.totalFaculty;

            File.WriteAllText(htmlfilename, grades.document.html, ConnectionControl.hebrewEncoding);
            browser.Navigate(htmlfilename);
        }

        private void dataGrid_SomethingChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (saveToolStripButton.Enabled) {
                if (sender != dataGridViewCleanSlate)
                    grades.document.dataSet.updateCleanSlate();
                RefreshGrades(grades.document.dataSet);
                this.Update();
            }
        }

        private void dataGridViewSessions_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            dataGrid_SomethingChanged(sender, null);
        }

        private void dataGridViewCleanSlate_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            dataGrid_SomethingChanged(sender, null);
        }
        
        private void dataGridView_MouseEnter(object sender, EventArgs e)
        {
            DataGridView Sender = (DataGridView)sender;
            if (Sender.RowCount > 3)
                Sender.Focus();
        }

        ListSortDirection dr = ListSortDirection.Ascending;
        private void dataGridViewSessions_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;
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

            if (dgv.GetClipboardContent() == null)
                return;

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
            if (e.RowIndex < 0) {
                Sender.ContextMenuStrip.Enabled = false;
                return;
            }
            Sender.ContextMenuStrip.Enabled = true;
            string courseId = Sender.Rows[e.RowIndex].Cells[0].Value.ToString();

            activeContext = new ContextData {
                grid = Sender,
                data = courseId
            };
            Sender.ContextMenuStrip.Items[1].Enabled = Info.isFacultyCS(courseId);
            contextMenuStrip1.Items[3].Enabled = false;
            contextMenuStrip1.Items[4].Enabled = false;

            if (Sender == dataGridViewCourseList)
                return;

            foreach (DataGridViewRow row in Sender.SelectedRows) {
                if (row.DataBoundItem == null)
                    break;
                if (((CourseSession)row.DataBoundItem).inList)
                    contextMenuStrip1.Items[3].Enabled = true;
                else
                    contextMenuStrip1.Items[4].Enabled = true;
            }

            Sender.ContextMenuStrip.Items[3].Enabled &= Sender.SelectedCells.Count > 0;
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

        private void EditLines(DataGridView dgv, bool inList, Color backColor)
        {
            foreach (DataGridViewRow row in dgv.SelectedRows) {
                ((CourseSession)row.DataBoundItem).inList = inList;
                row.DefaultCellStyle.BackColor = backColor;
                row.Selected = false;
            }
            dataGrid_SomethingChanged(dgv, null);
        }

        private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditLines(activeContext.grid, false, ProfessionalColors.SeparatorDark);
        }
        
        private void RestoreLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditLines(activeContext.grid, true, Color.White);
        }
        
        #endregion

        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            buttonClear_Click(sender, e);
        }
        
    }
}
