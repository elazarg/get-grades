namespace getGradesForms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.goButton = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveAsButton = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxAuthentication = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabCleanSlate = new System.Windows.Forms.TabPage();
            this.dataGridViewCleanSlate = new System.Windows.Forms.DataGridView();
            this.courseIDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.courseNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Points = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gradeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.myDatabaseDataSet = new getGradesForms.MyDatabaseDataSet();
            this.tabSessions = new System.Windows.Forms.TabPage();
            this.dataGridViewSessions = new System.Windows.Forms.DataGridView();
            this.courseIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gradeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.semesterIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabCourses = new System.Windows.Forms.TabPage();
            this.dataGridViewCourseList = new System.Windows.Forms.DataGridView();
            this.iDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pointsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabSemesters = new System.Windows.Forms.TabPage();
            this.dataGridViewSemesters = new System.Windows.Forms.DataGridView();
            this.iDDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yearDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seasonDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hebrewYearDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.averageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.successRateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pointsDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabBrowser = new System.Windows.Forms.TabPage();
            this.browser = new System.Windows.Forms.WebBrowser();
            this.tabHtml = new System.Windows.Forms.TabPage();
            this.richTextBoxHtml = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.personalDetailsGroup = new System.Windows.Forms.GroupBox();
            this.passwordCheckBox = new System.Windows.Forms.CheckBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelUserId = new System.Windows.Forms.Label();
            this.useridTextbox = new System.Windows.Forms.TextBox();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.detailsBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelName = new System.Windows.Forms.Label();
            this.labelProgram = new System.Windows.Forms.Label();
            this.labelFaculty = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxAuthentication.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabCleanSlate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCleanSlate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myDatabaseDataSet)).BeginInit();
            this.tabSessions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSessions)).BeginInit();
            this.tabCourses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCourseList)).BeginInit();
            this.tabSemesters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSemesters)).BeginInit();
            this.tabBrowser.SuspendLayout();
            this.tabHtml.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.personalDetailsGroup.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            this.detailsBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // goButton
            // 
            resources.ApplyResources(this.goButton, "goButton");
            this.goButton.Name = "goButton";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // saveFileDialog
            // 
            resources.ApplyResources(this.saveFileDialog, "saveFileDialog");
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // saveAsButton
            // 
            resources.ApplyResources(this.saveAsButton, "saveAsButton");
            this.saveAsButton.Name = "saveAsButton";
            this.saveAsButton.UseVisualStyleBackColor = true;
            this.saveAsButton.Click += new System.EventHandler(this.saveAs_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.groupBoxAuthentication, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.personalDetailsGroup, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.detailsBox, 2, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // groupBoxAuthentication
            // 
            this.groupBoxAuthentication.Controls.Add(this.saveAsButton);
            this.groupBoxAuthentication.Controls.Add(this.goButton);
            resources.ApplyResources(this.groupBoxAuthentication, "groupBoxAuthentication");
            this.groupBoxAuthentication.Name = "groupBoxAuthentication";
            this.groupBoxAuthentication.TabStop = false;
            // 
            // tabControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 3);
            this.tabControl1.Controls.Add(this.tabCleanSlate);
            this.tabControl1.Controls.Add(this.tabSessions);
            this.tabControl1.Controls.Add(this.tabCourses);
            this.tabControl1.Controls.Add(this.tabSemesters);
            this.tabControl1.Controls.Add(this.tabBrowser);
            this.tabControl1.Controls.Add(this.tabHtml);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabCleanSlate
            // 
            this.tabCleanSlate.Controls.Add(this.dataGridViewCleanSlate);
            resources.ApplyResources(this.tabCleanSlate, "tabCleanSlate");
            this.tabCleanSlate.Name = "tabCleanSlate";
            this.tabCleanSlate.UseVisualStyleBackColor = true;
            // 
            // dataGridViewCleanSlate
            // 
            this.dataGridViewCleanSlate.AllowUserToAddRows = false;
            this.dataGridViewCleanSlate.AllowUserToDeleteRows = false;
            this.dataGridViewCleanSlate.AutoGenerateColumns = false;
            this.dataGridViewCleanSlate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCleanSlate.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.courseIDDataGridViewTextBoxColumn1,
            this.courseNameDataGridViewTextBoxColumn,
            this.Points,
            this.gradeDataGridViewTextBoxColumn1});
            this.dataGridViewCleanSlate.DataMember = "ViewTable";
            this.dataGridViewCleanSlate.DataSource = this.myDatabaseDataSet;
            resources.ApplyResources(this.dataGridViewCleanSlate, "dataGridViewCleanSlate");
            this.dataGridViewCleanSlate.Name = "dataGridViewCleanSlate";
            this.dataGridViewCleanSlate.ReadOnly = true;
            // 
            // courseIDDataGridViewTextBoxColumn1
            // 
            this.courseIDDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.courseIDDataGridViewTextBoxColumn1.DataPropertyName = "Course ID";
            resources.ApplyResources(this.courseIDDataGridViewTextBoxColumn1, "courseIDDataGridViewTextBoxColumn1");
            this.courseIDDataGridViewTextBoxColumn1.MaxInputLength = 6;
            this.courseIDDataGridViewTextBoxColumn1.Name = "courseIDDataGridViewTextBoxColumn1";
            this.courseIDDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // courseNameDataGridViewTextBoxColumn
            // 
            this.courseNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.courseNameDataGridViewTextBoxColumn.DataPropertyName = "Course Name";
            resources.ApplyResources(this.courseNameDataGridViewTextBoxColumn, "courseNameDataGridViewTextBoxColumn");
            this.courseNameDataGridViewTextBoxColumn.Name = "courseNameDataGridViewTextBoxColumn";
            this.courseNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Points
            // 
            this.Points.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Points.DataPropertyName = "Points";
            resources.ApplyResources(this.Points, "Points");
            this.Points.Name = "Points";
            this.Points.ReadOnly = true;
            // 
            // gradeDataGridViewTextBoxColumn1
            // 
            this.gradeDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.gradeDataGridViewTextBoxColumn1.DataPropertyName = "Grade";
            resources.ApplyResources(this.gradeDataGridViewTextBoxColumn1, "gradeDataGridViewTextBoxColumn1");
            this.gradeDataGridViewTextBoxColumn1.Name = "gradeDataGridViewTextBoxColumn1";
            this.gradeDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // myDatabaseDataSet
            // 
            this.myDatabaseDataSet.DataSetName = "MyDatabaseDataSet";
            this.myDatabaseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tabSessions
            // 
            this.tabSessions.Controls.Add(this.dataGridViewSessions);
            resources.ApplyResources(this.tabSessions, "tabSessions");
            this.tabSessions.Name = "tabSessions";
            this.tabSessions.UseVisualStyleBackColor = true;
            // 
            // dataGridViewSessions
            // 
            this.dataGridViewSessions.AllowUserToAddRows = false;
            this.dataGridViewSessions.AllowUserToDeleteRows = false;
            this.dataGridViewSessions.AutoGenerateColumns = false;
            this.dataGridViewSessions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSessions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.courseIDDataGridViewTextBoxColumn,
            this.gradeDataGridViewTextBoxColumn,
            this.semesterIDDataGridViewTextBoxColumn,
            this.commentsDataGridViewTextBoxColumn});
            this.dataGridViewSessions.DataMember = "CourseSessions";
            this.dataGridViewSessions.DataSource = this.myDatabaseDataSet;
            resources.ApplyResources(this.dataGridViewSessions, "dataGridViewSessions");
            this.dataGridViewSessions.Name = "dataGridViewSessions";
            this.dataGridViewSessions.ReadOnly = true;
            // 
            // courseIDDataGridViewTextBoxColumn
            // 
            this.courseIDDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.courseIDDataGridViewTextBoxColumn.DataPropertyName = "Course ID";
            resources.ApplyResources(this.courseIDDataGridViewTextBoxColumn, "courseIDDataGridViewTextBoxColumn");
            this.courseIDDataGridViewTextBoxColumn.MaxInputLength = 6;
            this.courseIDDataGridViewTextBoxColumn.Name = "courseIDDataGridViewTextBoxColumn";
            this.courseIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // gradeDataGridViewTextBoxColumn
            // 
            this.gradeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.gradeDataGridViewTextBoxColumn.DataPropertyName = "Grade";
            resources.ApplyResources(this.gradeDataGridViewTextBoxColumn, "gradeDataGridViewTextBoxColumn");
            this.gradeDataGridViewTextBoxColumn.MaxInputLength = 5;
            this.gradeDataGridViewTextBoxColumn.Name = "gradeDataGridViewTextBoxColumn";
            this.gradeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // semesterIDDataGridViewTextBoxColumn
            // 
            this.semesterIDDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.semesterIDDataGridViewTextBoxColumn.DataPropertyName = "Semester ID";
            resources.ApplyResources(this.semesterIDDataGridViewTextBoxColumn, "semesterIDDataGridViewTextBoxColumn");
            this.semesterIDDataGridViewTextBoxColumn.MaxInputLength = 2;
            this.semesterIDDataGridViewTextBoxColumn.Name = "semesterIDDataGridViewTextBoxColumn";
            this.semesterIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // commentsDataGridViewTextBoxColumn
            // 
            this.commentsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.commentsDataGridViewTextBoxColumn.DataPropertyName = "Comments";
            resources.ApplyResources(this.commentsDataGridViewTextBoxColumn, "commentsDataGridViewTextBoxColumn");
            this.commentsDataGridViewTextBoxColumn.MaxInputLength = 15;
            this.commentsDataGridViewTextBoxColumn.Name = "commentsDataGridViewTextBoxColumn";
            this.commentsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tabCourses
            // 
            this.tabCourses.Controls.Add(this.dataGridViewCourseList);
            resources.ApplyResources(this.tabCourses, "tabCourses");
            this.tabCourses.Name = "tabCourses";
            this.tabCourses.UseVisualStyleBackColor = true;
            // 
            // dataGridViewCourseList
            // 
            this.dataGridViewCourseList.AllowUserToAddRows = false;
            this.dataGridViewCourseList.AllowUserToDeleteRows = false;
            this.dataGridViewCourseList.AutoGenerateColumns = false;
            this.dataGridViewCourseList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCourseList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn1,
            this.nameDataGridViewTextBoxColumn,
            this.pointsDataGridViewTextBoxColumn});
            this.dataGridViewCourseList.DataMember = "CourseList";
            this.dataGridViewCourseList.DataSource = this.myDatabaseDataSet;
            resources.ApplyResources(this.dataGridViewCourseList, "dataGridViewCourseList");
            this.dataGridViewCourseList.Name = "dataGridViewCourseList";
            this.dataGridViewCourseList.ReadOnly = true;
            // 
            // iDDataGridViewTextBoxColumn1
            // 
            this.iDDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.iDDataGridViewTextBoxColumn1.DataPropertyName = "ID";
            resources.ApplyResources(this.iDDataGridViewTextBoxColumn1, "iDDataGridViewTextBoxColumn1");
            this.iDDataGridViewTextBoxColumn1.Name = "iDDataGridViewTextBoxColumn1";
            this.iDDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            resources.ApplyResources(this.nameDataGridViewTextBoxColumn, "nameDataGridViewTextBoxColumn");
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pointsDataGridViewTextBoxColumn
            // 
            this.pointsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.pointsDataGridViewTextBoxColumn.DataPropertyName = "Points";
            resources.ApplyResources(this.pointsDataGridViewTextBoxColumn, "pointsDataGridViewTextBoxColumn");
            this.pointsDataGridViewTextBoxColumn.Name = "pointsDataGridViewTextBoxColumn";
            this.pointsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tabSemesters
            // 
            this.tabSemesters.Controls.Add(this.dataGridViewSemesters);
            resources.ApplyResources(this.tabSemesters, "tabSemesters");
            this.tabSemesters.Name = "tabSemesters";
            this.tabSemesters.UseVisualStyleBackColor = true;
            // 
            // dataGridViewSemesters
            // 
            this.dataGridViewSemesters.AllowUserToAddRows = false;
            this.dataGridViewSemesters.AllowUserToDeleteRows = false;
            this.dataGridViewSemesters.AutoGenerateColumns = false;
            this.dataGridViewSemesters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSemesters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn2,
            this.yearDataGridViewTextBoxColumn,
            this.seasonDataGridViewTextBoxColumn,
            this.hebrewYearDataGridViewTextBoxColumn,
            this.averageDataGridViewTextBoxColumn,
            this.successRateDataGridViewTextBoxColumn,
            this.pointsDataGridViewTextBoxColumn1});
            this.dataGridViewSemesters.DataMember = "Semester";
            this.dataGridViewSemesters.DataSource = this.myDatabaseDataSet;
            resources.ApplyResources(this.dataGridViewSemesters, "dataGridViewSemesters");
            this.dataGridViewSemesters.Name = "dataGridViewSemesters";
            this.dataGridViewSemesters.ReadOnly = true;
            // 
            // iDDataGridViewTextBoxColumn2
            // 
            this.iDDataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.iDDataGridViewTextBoxColumn2.DataPropertyName = "ID";
            resources.ApplyResources(this.iDDataGridViewTextBoxColumn2, "iDDataGridViewTextBoxColumn2");
            this.iDDataGridViewTextBoxColumn2.MaxInputLength = 3;
            this.iDDataGridViewTextBoxColumn2.Name = "iDDataGridViewTextBoxColumn2";
            this.iDDataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // yearDataGridViewTextBoxColumn
            // 
            this.yearDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.yearDataGridViewTextBoxColumn.DataPropertyName = "Year";
            resources.ApplyResources(this.yearDataGridViewTextBoxColumn, "yearDataGridViewTextBoxColumn");
            this.yearDataGridViewTextBoxColumn.MaxInputLength = 10;
            this.yearDataGridViewTextBoxColumn.Name = "yearDataGridViewTextBoxColumn";
            this.yearDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // seasonDataGridViewTextBoxColumn
            // 
            this.seasonDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.seasonDataGridViewTextBoxColumn.DataPropertyName = "Season";
            resources.ApplyResources(this.seasonDataGridViewTextBoxColumn, "seasonDataGridViewTextBoxColumn");
            this.seasonDataGridViewTextBoxColumn.MaxInputLength = 4;
            this.seasonDataGridViewTextBoxColumn.Name = "seasonDataGridViewTextBoxColumn";
            this.seasonDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // hebrewYearDataGridViewTextBoxColumn
            // 
            this.hebrewYearDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.hebrewYearDataGridViewTextBoxColumn.DataPropertyName = "Hebrew Year";
            resources.ApplyResources(this.hebrewYearDataGridViewTextBoxColumn, "hebrewYearDataGridViewTextBoxColumn");
            this.hebrewYearDataGridViewTextBoxColumn.MaxInputLength = 5;
            this.hebrewYearDataGridViewTextBoxColumn.Name = "hebrewYearDataGridViewTextBoxColumn";
            this.hebrewYearDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // averageDataGridViewTextBoxColumn
            // 
            this.averageDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.averageDataGridViewTextBoxColumn.DataPropertyName = "Average";
            resources.ApplyResources(this.averageDataGridViewTextBoxColumn, "averageDataGridViewTextBoxColumn");
            this.averageDataGridViewTextBoxColumn.MaxInputLength = 6;
            this.averageDataGridViewTextBoxColumn.Name = "averageDataGridViewTextBoxColumn";
            this.averageDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // successRateDataGridViewTextBoxColumn
            // 
            this.successRateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.successRateDataGridViewTextBoxColumn.DataPropertyName = "Success Rate";
            resources.ApplyResources(this.successRateDataGridViewTextBoxColumn, "successRateDataGridViewTextBoxColumn");
            this.successRateDataGridViewTextBoxColumn.MaxInputLength = 6;
            this.successRateDataGridViewTextBoxColumn.Name = "successRateDataGridViewTextBoxColumn";
            this.successRateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pointsDataGridViewTextBoxColumn1
            // 
            this.pointsDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.pointsDataGridViewTextBoxColumn1.DataPropertyName = "Points";
            resources.ApplyResources(this.pointsDataGridViewTextBoxColumn1, "pointsDataGridViewTextBoxColumn1");
            this.pointsDataGridViewTextBoxColumn1.Name = "pointsDataGridViewTextBoxColumn1";
            this.pointsDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // tabBrowser
            // 
            this.tabBrowser.Controls.Add(this.browser);
            resources.ApplyResources(this.tabBrowser, "tabBrowser");
            this.tabBrowser.Name = "tabBrowser";
            this.tabBrowser.UseVisualStyleBackColor = true;
            // 
            // browser
            // 
            resources.ApplyResources(this.browser, "browser");
            this.browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.browser.Name = "browser";
            // 
            // tabHtml
            // 
            this.tabHtml.Controls.Add(this.richTextBoxHtml);
            resources.ApplyResources(this.tabHtml, "tabHtml");
            this.tabHtml.Name = "tabHtml";
            this.tabHtml.UseVisualStyleBackColor = true;
            // 
            // richTextBoxHtml
            // 
            resources.ApplyResources(this.richTextBoxHtml, "richTextBoxHtml");
            this.richTextBoxHtml.Name = "richTextBoxHtml";
            // 
            // statusStrip1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip1, 2);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.SizingGrip = false;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Maximum = 65;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            resources.ApplyResources(this.toolStripProgressBar, "toolStripProgressBar");
            this.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // personalDetailsGroup
            // 
            this.personalDetailsGroup.Controls.Add(this.passwordCheckBox);
            this.personalDetailsGroup.Controls.Add(this.labelPassword);
            this.personalDetailsGroup.Controls.Add(this.labelUserId);
            this.personalDetailsGroup.Controls.Add(this.useridTextbox);
            this.personalDetailsGroup.Controls.Add(this.passwordBox);
            resources.ApplyResources(this.personalDetailsGroup, "personalDetailsGroup");
            this.personalDetailsGroup.Name = "personalDetailsGroup";
            this.personalDetailsGroup.TabStop = false;
            // 
            // passwordCheckBox
            // 
            resources.ApplyResources(this.passwordCheckBox, "passwordCheckBox");
            this.passwordCheckBox.Checked = true;
            this.passwordCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.passwordCheckBox.Name = "passwordCheckBox";
            this.passwordCheckBox.UseVisualStyleBackColor = true;
            this.passwordCheckBox.CheckStateChanged += new System.EventHandler(this.passwordCheckBox_CheckStateChanged);
            // 
            // labelPassword
            // 
            resources.ApplyResources(this.labelPassword, "labelPassword");
            this.labelPassword.Name = "labelPassword";
            // 
            // labelUserId
            // 
            resources.ApplyResources(this.labelUserId, "labelUserId");
            this.labelUserId.Name = "labelUserId";
            // 
            // useridTextbox
            // 
            resources.ApplyResources(this.useridTextbox, "useridTextbox");
            this.useridTextbox.Name = "useridTextbox";
            this.useridTextbox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // passwordBox
            // 
            resources.ApplyResources(this.passwordBox, "passwordBox");
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // statusStrip2
            // 
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            resources.ApplyResources(this.statusStrip2, "statusStrip2");
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.SizingGrip = false;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            resources.ApplyResources(this.statusLabel, "statusLabel");
            // 
            // detailsBox
            // 
            this.detailsBox.Controls.Add(this.tableLayoutPanel2);
            resources.ApplyResources(this.detailsBox, "detailsBox");
            this.detailsBox.Name = "detailsBox";
            this.detailsBox.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.labelName, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelProgram, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.labelFaculty, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // labelName
            // 
            resources.ApplyResources(this.labelName, "labelName");
            this.labelName.Name = "labelName";
            this.labelName.TextChanged += new System.EventHandler(this.labelPD_TextChanged);
            // 
            // labelProgram
            // 
            resources.ApplyResources(this.labelProgram, "labelProgram");
            this.labelProgram.Name = "labelProgram";
            this.labelProgram.TextChanged += new System.EventHandler(this.labelPD_TextChanged);
            // 
            // labelFaculty
            // 
            resources.ApplyResources(this.labelFaculty, "labelFaculty");
            this.labelFaculty.Name = "labelFaculty";
            this.labelFaculty.TextChanged += new System.EventHandler(this.labelPD_TextChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkRate = 500;
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.errorProvider1.ContainerControl = this;
            resources.ApplyResources(this.errorProvider1, "errorProvider1");
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBoxAuthentication.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabCleanSlate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCleanSlate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myDatabaseDataSet)).EndInit();
            this.tabSessions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSessions)).EndInit();
            this.tabCourses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCourseList)).EndInit();
            this.tabSemesters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSemesters)).EndInit();
            this.tabBrowser.ResumeLayout(false);
            this.tabHtml.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.personalDetailsGroup.ResumeLayout(false);
            this.personalDetailsGroup.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.detailsBox.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button saveAsButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBoxAuthentication;
        private MyDatabaseDataSet myDatabaseDataSet;
        private System.Windows.Forms.GroupBox personalDetailsGroup;
        private System.Windows.Forms.CheckBox passwordCheckBox;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelUserId;
        private System.Windows.Forms.TextBox useridTextbox;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.GroupBox detailsBox;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelProgram;
        private System.Windows.Forms.Label labelFaculty;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabCleanSlate;
        private System.Windows.Forms.DataGridView dataGridViewCleanSlate;
        private System.Windows.Forms.DataGridViewTextBoxColumn courseIDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn courseNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Points;
        private System.Windows.Forms.DataGridViewTextBoxColumn gradeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.TabPage tabSessions;
        private System.Windows.Forms.DataGridView dataGridViewSessions;
        private System.Windows.Forms.DataGridViewTextBoxColumn courseIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gradeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn semesterIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentsDataGridViewTextBoxColumn;
        private System.Windows.Forms.TabPage tabCourses;
        private System.Windows.Forms.DataGridView dataGridViewCourseList;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pointsDataGridViewTextBoxColumn;
        private System.Windows.Forms.TabPage tabSemesters;
        private System.Windows.Forms.DataGridView dataGridViewSemesters;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn yearDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn seasonDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hebrewYearDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn averageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn successRateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pointsDataGridViewTextBoxColumn1;
        private System.Windows.Forms.TabPage tabBrowser;
        private System.Windows.Forms.WebBrowser browser;
        private System.Windows.Forms.TabPage tabHtml;
        private System.Windows.Forms.RichTextBox richTextBoxHtml;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}

