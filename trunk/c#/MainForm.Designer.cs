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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabCleanSlate = new System.Windows.Forms.TabPage();
            this.dataGridViewCleanSlate = new System.Windows.Forms.DataGridView();
            this.courseIdDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.courseNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gradeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uGDatabaseBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabSessions = new System.Windows.Forms.TabPage();
            this.dataGridViewSessions = new System.Windows.Forms.DataGridView();
            this.courseIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.courseNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gradeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inAverageDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.inFinalDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.attendedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.passedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tabCourses = new System.Windows.Forms.TabPage();
            this.dataGridViewCourseList = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pointsDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabSemesters = new System.Windows.Forms.TabPage();
            this.dataGridViewSemesters = new System.Windows.Forms.DataGridView();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yearDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seasonDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hebrewYearDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.averageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.successRateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pointsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSuccessRate = new System.Windows.Forms.TextBox();
            this.textBoxPoints = new System.Windows.Forms.TextBox();
            this.textBoxAvGrade = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.aboutButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.courseIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabCleanSlate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCleanSlate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uGDatabaseBindingSource)).BeginInit();
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
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
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
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tabControl, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.personalDetailsGroup, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.detailsBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tabControl
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl, 4);
            this.tabControl.Controls.Add(this.tabCleanSlate);
            this.tabControl.Controls.Add(this.tabSessions);
            this.tabControl.Controls.Add(this.tabCourses);
            this.tabControl.Controls.Add(this.tabSemesters);
            this.tabControl.Controls.Add(this.tabBrowser);
            this.tabControl.Controls.Add(this.tabHtml);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.TabStop = false;
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
            this.dataGridViewCleanSlate.AutoGenerateColumns = false;
            this.dataGridViewCleanSlate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCleanSlate.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.courseIdDataGridViewTextBoxColumn2,
            this.courseNameDataGridViewTextBoxColumn1,
            this.gradeDataGridViewTextBoxColumn1});
            this.dataGridViewCleanSlate.DataMember = "cleanView";
            this.dataGridViewCleanSlate.DataSource = this.uGDatabaseBindingSource;
            resources.ApplyResources(this.dataGridViewCleanSlate, "dataGridViewCleanSlate");
            this.dataGridViewCleanSlate.Name = "dataGridViewCleanSlate";
            // 
            // courseIdDataGridViewTextBoxColumn2
            // 
            this.courseIdDataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.courseIdDataGridViewTextBoxColumn2.DataPropertyName = "courseId";
            resources.ApplyResources(this.courseIdDataGridViewTextBoxColumn2, "courseIdDataGridViewTextBoxColumn2");
            this.courseIdDataGridViewTextBoxColumn2.Name = "courseIdDataGridViewTextBoxColumn2";
            this.courseIdDataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // courseNameDataGridViewTextBoxColumn1
            // 
            this.courseNameDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.courseNameDataGridViewTextBoxColumn1.DataPropertyName = "courseName";
            resources.ApplyResources(this.courseNameDataGridViewTextBoxColumn1, "courseNameDataGridViewTextBoxColumn1");
            this.courseNameDataGridViewTextBoxColumn1.Name = "courseNameDataGridViewTextBoxColumn1";
            this.courseNameDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // gradeDataGridViewTextBoxColumn1
            // 
            this.gradeDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.gradeDataGridViewTextBoxColumn1.DataPropertyName = "grade";
            resources.ApplyResources(this.gradeDataGridViewTextBoxColumn1, "gradeDataGridViewTextBoxColumn1");
            this.gradeDataGridViewTextBoxColumn1.Name = "gradeDataGridViewTextBoxColumn1";
            // 
            // uGDatabaseBindingSource
            // 
            this.uGDatabaseBindingSource.DataSource = typeof(getGradesForms.UGDatabase);
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
            this.dataGridViewSessions.AutoGenerateColumns = false;
            this.dataGridViewSessions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSessions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.courseIdDataGridViewTextBoxColumn1,
            this.courseNameDataGridViewTextBoxColumn,
            this.gradeDataGridViewTextBoxColumn,
            this.commentsDataGridViewTextBoxColumn,
            this.inAverageDataGridViewCheckBoxColumn,
            this.inFinalDataGridViewCheckBoxColumn,
            this.attendedDataGridViewCheckBoxColumn,
            this.passedDataGridViewCheckBoxColumn});
            this.dataGridViewSessions.DataMember = "sessions";
            this.dataGridViewSessions.DataSource = this.uGDatabaseBindingSource;
            resources.ApplyResources(this.dataGridViewSessions, "dataGridViewSessions");
            this.dataGridViewSessions.Name = "dataGridViewSessions";
            // 
            // courseIdDataGridViewTextBoxColumn1
            // 
            this.courseIdDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.courseIdDataGridViewTextBoxColumn1.DataPropertyName = "courseId";
            resources.ApplyResources(this.courseIdDataGridViewTextBoxColumn1, "courseIdDataGridViewTextBoxColumn1");
            this.courseIdDataGridViewTextBoxColumn1.MaxInputLength = 6;
            this.courseIdDataGridViewTextBoxColumn1.Name = "courseIdDataGridViewTextBoxColumn1";
            // 
            // courseNameDataGridViewTextBoxColumn
            // 
            this.courseNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.courseNameDataGridViewTextBoxColumn.DataPropertyName = "courseName";
            resources.ApplyResources(this.courseNameDataGridViewTextBoxColumn, "courseNameDataGridViewTextBoxColumn");
            this.courseNameDataGridViewTextBoxColumn.Name = "courseNameDataGridViewTextBoxColumn";
            // 
            // gradeDataGridViewTextBoxColumn
            // 
            this.gradeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.gradeDataGridViewTextBoxColumn.DataPropertyName = "grade";
            resources.ApplyResources(this.gradeDataGridViewTextBoxColumn, "gradeDataGridViewTextBoxColumn");
            this.gradeDataGridViewTextBoxColumn.Name = "gradeDataGridViewTextBoxColumn";
            // 
            // commentsDataGridViewTextBoxColumn
            // 
            this.commentsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.commentsDataGridViewTextBoxColumn.DataPropertyName = "Comments";
            resources.ApplyResources(this.commentsDataGridViewTextBoxColumn, "commentsDataGridViewTextBoxColumn");
            this.commentsDataGridViewTextBoxColumn.Name = "commentsDataGridViewTextBoxColumn";
            // 
            // inAverageDataGridViewCheckBoxColumn
            // 
            this.inAverageDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.inAverageDataGridViewCheckBoxColumn.DataPropertyName = "inAverage";
            resources.ApplyResources(this.inAverageDataGridViewCheckBoxColumn, "inAverageDataGridViewCheckBoxColumn");
            this.inAverageDataGridViewCheckBoxColumn.Name = "inAverageDataGridViewCheckBoxColumn";
            // 
            // inFinalDataGridViewCheckBoxColumn
            // 
            this.inFinalDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.inFinalDataGridViewCheckBoxColumn.DataPropertyName = "inFinal";
            resources.ApplyResources(this.inFinalDataGridViewCheckBoxColumn, "inFinalDataGridViewCheckBoxColumn");
            this.inFinalDataGridViewCheckBoxColumn.Name = "inFinalDataGridViewCheckBoxColumn";
            // 
            // attendedDataGridViewCheckBoxColumn
            // 
            this.attendedDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.attendedDataGridViewCheckBoxColumn.DataPropertyName = "Attended";
            resources.ApplyResources(this.attendedDataGridViewCheckBoxColumn, "attendedDataGridViewCheckBoxColumn");
            this.attendedDataGridViewCheckBoxColumn.Name = "attendedDataGridViewCheckBoxColumn";
            // 
            // passedDataGridViewCheckBoxColumn
            // 
            this.passedDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.passedDataGridViewCheckBoxColumn.DataPropertyName = "Passed";
            resources.ApplyResources(this.passedDataGridViewCheckBoxColumn, "passedDataGridViewCheckBoxColumn");
            this.passedDataGridViewCheckBoxColumn.Name = "passedDataGridViewCheckBoxColumn";
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
            this.dataGridViewCourseList.AutoGenerateColumns = false;
            this.dataGridViewCourseList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCourseList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn1,
            this.nameDataGridViewTextBoxColumn,
            this.pointsDataGridViewTextBoxColumn1});
            this.dataGridViewCourseList.DataMember = "courses";
            this.dataGridViewCourseList.DataSource = this.uGDatabaseBindingSource;
            resources.ApplyResources(this.dataGridViewCourseList, "dataGridViewCourseList");
            this.dataGridViewCourseList.Name = "dataGridViewCourseList";
            // 
            // idDataGridViewTextBoxColumn1
            // 
            this.idDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.idDataGridViewTextBoxColumn1.DataPropertyName = "id";
            resources.ApplyResources(this.idDataGridViewTextBoxColumn1, "idDataGridViewTextBoxColumn1");
            this.idDataGridViewTextBoxColumn1.MaxInputLength = 6;
            this.idDataGridViewTextBoxColumn1.Name = "idDataGridViewTextBoxColumn1";
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "name";
            resources.ApplyResources(this.nameDataGridViewTextBoxColumn, "nameDataGridViewTextBoxColumn");
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // pointsDataGridViewTextBoxColumn1
            // 
            this.pointsDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.pointsDataGridViewTextBoxColumn1.DataPropertyName = "points";
            resources.ApplyResources(this.pointsDataGridViewTextBoxColumn1, "pointsDataGridViewTextBoxColumn1");
            this.pointsDataGridViewTextBoxColumn1.Name = "pointsDataGridViewTextBoxColumn1";
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
            this.dataGridViewSemesters.AutoGenerateColumns = false;
            this.dataGridViewSemesters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSemesters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.yearDataGridViewTextBoxColumn,
            this.seasonDataGridViewTextBoxColumn,
            this.hebrewYearDataGridViewTextBoxColumn,
            this.averageDataGridViewTextBoxColumn,
            this.successRateDataGridViewTextBoxColumn,
            this.pointsDataGridViewTextBoxColumn});
            this.dataGridViewSemesters.DataMember = "semesters";
            this.dataGridViewSemesters.DataSource = this.uGDatabaseBindingSource;
            resources.ApplyResources(this.dataGridViewSemesters, "dataGridViewSemesters");
            this.dataGridViewSemesters.Name = "dataGridViewSemesters";
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            resources.ApplyResources(this.iDDataGridViewTextBoxColumn, "iDDataGridViewTextBoxColumn");
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            // 
            // yearDataGridViewTextBoxColumn
            // 
            this.yearDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.yearDataGridViewTextBoxColumn.DataPropertyName = "year";
            resources.ApplyResources(this.yearDataGridViewTextBoxColumn, "yearDataGridViewTextBoxColumn");
            this.yearDataGridViewTextBoxColumn.Name = "yearDataGridViewTextBoxColumn";
            // 
            // seasonDataGridViewTextBoxColumn
            // 
            this.seasonDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.seasonDataGridViewTextBoxColumn.DataPropertyName = "season";
            resources.ApplyResources(this.seasonDataGridViewTextBoxColumn, "seasonDataGridViewTextBoxColumn");
            this.seasonDataGridViewTextBoxColumn.Name = "seasonDataGridViewTextBoxColumn";
            // 
            // hebrewYearDataGridViewTextBoxColumn
            // 
            this.hebrewYearDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.hebrewYearDataGridViewTextBoxColumn.DataPropertyName = "hebrewYear";
            resources.ApplyResources(this.hebrewYearDataGridViewTextBoxColumn, "hebrewYearDataGridViewTextBoxColumn");
            this.hebrewYearDataGridViewTextBoxColumn.Name = "hebrewYearDataGridViewTextBoxColumn";
            // 
            // averageDataGridViewTextBoxColumn
            // 
            this.averageDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.averageDataGridViewTextBoxColumn.DataPropertyName = "Average";
            resources.ApplyResources(this.averageDataGridViewTextBoxColumn, "averageDataGridViewTextBoxColumn");
            this.averageDataGridViewTextBoxColumn.Name = "averageDataGridViewTextBoxColumn";
            this.averageDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // successRateDataGridViewTextBoxColumn
            // 
            this.successRateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.successRateDataGridViewTextBoxColumn.DataPropertyName = "SuccessRate";
            resources.ApplyResources(this.successRateDataGridViewTextBoxColumn, "successRateDataGridViewTextBoxColumn");
            this.successRateDataGridViewTextBoxColumn.Name = "successRateDataGridViewTextBoxColumn";
            this.successRateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pointsDataGridViewTextBoxColumn
            // 
            this.pointsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.pointsDataGridViewTextBoxColumn.DataPropertyName = "Points";
            resources.ApplyResources(this.pointsDataGridViewTextBoxColumn, "pointsDataGridViewTextBoxColumn");
            this.pointsDataGridViewTextBoxColumn.Name = "pointsDataGridViewTextBoxColumn";
            this.pointsDataGridViewTextBoxColumn.ReadOnly = true;
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
            this.toolStripProgressBar.Maximum = 75;
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
            this.passwordCheckBox.Name = "passwordCheckBox";
            this.passwordCheckBox.TabStop = false;
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
            this.useridTextbox.ShortcutsEnabled = false;
            this.useridTextbox.TextChanged += new System.EventHandler(this.userIdBox_TextChanged);
            this.useridTextbox.Enter += new System.EventHandler(this.textBox_Enter);
            this.useridTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericTextbox_KeyDown);
            // 
            // passwordBox
            // 
            resources.ApplyResources(this.passwordBox, "passwordBox");
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.ShortcutsEnabled = false;
            this.passwordBox.UseSystemPasswordChar = true;
            this.passwordBox.TextChanged += new System.EventHandler(this.passwordBox_TextChanged);
            this.passwordBox.Enter += new System.EventHandler(this.textBox_Enter);
            this.passwordBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericTextbox_KeyDown);
            // 
            // statusStrip2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip2, 2);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxSuccessRate);
            this.groupBox1.Controls.Add(this.textBoxPoints);
            this.groupBox1.Controls.Add(this.textBoxAvGrade);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.label3.TextChanged += new System.EventHandler(this.labelPD_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.label2.TextChanged += new System.EventHandler(this.labelPD_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.label1.TextChanged += new System.EventHandler(this.labelPD_TextChanged);
            // 
            // textBoxSuccessRate
            // 
            resources.ApplyResources(this.textBoxSuccessRate, "textBoxSuccessRate");
            this.textBoxSuccessRate.Name = "textBoxSuccessRate";
            this.textBoxSuccessRate.ReadOnly = true;
            // 
            // textBoxPoints
            // 
            resources.ApplyResources(this.textBoxPoints, "textBoxPoints");
            this.textBoxPoints.Name = "textBoxPoints";
            this.textBoxPoints.ReadOnly = true;
            // 
            // textBoxAvGrade
            // 
            resources.ApplyResources(this.textBoxAvGrade, "textBoxAvGrade");
            this.textBoxAvGrade.Name = "textBoxAvGrade";
            this.textBoxAvGrade.ReadOnly = true;
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.saveAsButton);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.goButton);
            this.flowLayoutPanel1.Controls.Add(this.aboutButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // aboutButton
            // 
            resources.ApplyResources(this.aboutButton, "aboutButton");
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.UseVisualStyleBackColor = true;
            this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkRate = 500;
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.errorProvider1.ContainerControl = this;
            resources.ApplyResources(this.errorProvider1, "errorProvider1");
            // 
            // courseIDDataGridViewTextBoxColumn
            // 
            this.courseIDDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.courseIDDataGridViewTextBoxColumn.DataPropertyName = "Course ID";
            resources.ApplyResources(this.courseIDDataGridViewTextBoxColumn, "courseIDDataGridViewTextBoxColumn");
            this.courseIDDataGridViewTextBoxColumn.MaxInputLength = 6;
            this.courseIDDataGridViewTextBoxColumn.Name = "courseIDDataGridViewTextBoxColumn";
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ID.DataPropertyName = "ID";
            resources.ApplyResources(this.ID, "ID");
            this.ID.Name = "ID";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Course ID";
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.MaxInputLength = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabCleanSlate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCleanSlate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uGDatabaseBindingSource)).EndInit();
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button saveAsButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
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
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSuccessRate;
        private System.Windows.Forms.TextBox textBoxPoints;
        private System.Windows.Forms.TextBox textBoxAvGrade;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn courseIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.Button aboutButton;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabCleanSlate;
        private System.Windows.Forms.TabPage tabSessions;
        private System.Windows.Forms.TabPage tabCourses;
        private System.Windows.Forms.TabPage tabSemesters;
        private System.Windows.Forms.TabPage tabBrowser;
        private System.Windows.Forms.WebBrowser browser;
        private System.Windows.Forms.TabPage tabHtml;
        private System.Windows.Forms.RichTextBox richTextBoxHtml;
        private System.Windows.Forms.BindingSource uGDatabaseBindingSource;
        private System.Windows.Forms.DataGridView dataGridViewSemesters;
        private System.Windows.Forms.DataGridView dataGridViewSessions;
        private System.Windows.Forms.DataGridView dataGridViewCleanSlate;
        private System.Windows.Forms.DataGridView dataGridViewCourseList;
        private System.Windows.Forms.DataGridViewTextBoxColumn courseIdDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn courseNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn gradeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn courseIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn courseNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gradeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn inAverageDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn inFinalDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn attendedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn passedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pointsDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn yearDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn seasonDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hebrewYearDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn averageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn successRateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pointsDataGridViewTextBoxColumn;
    }
}

