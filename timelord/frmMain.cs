using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace timelord
{
    /// <summary>
    /// the main time tracking form
    /// </summary>
    public partial class frmMain : Form
    {
        Timesheet timesheet;
        Timer timer;
        bool timerState = false;
        Task ActiveTask;
        List<Task> Tasks;

        /// <summary>
        /// sets up the form
        /// </summary>
        public frmMain()
        {
            InitializeComponent();

            this.MaximizeBox = false;
            this.MinimizeBox = true;

            txtTaskName.GotFocus += TxtTaskName_GotFocus;
            dgvTimesheet.SelectionChanged += DgvTimesheet_SelectionChanged;
            dgvTimesheet.CellMouseDown += DgvTimesheet_CellMouseDown;
            dgvTimesheet.RowsAdded += DgvTimesheet_RowsAdded;

            // initialize a timer for counting seconds
            timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 1000;

            Tasks = new List<Task>();

            // reset duration (the @ symbol makes tostring ignore escape characters)
            lblTaskDuration.Text = TimeSpan.FromSeconds(0).ToString(@"hh\:mm\:ss");

            dgvTimesheet.CellDoubleClick += DgvTimesheet_CellDoubleClick;

            dgvTimesheet.DataSource = Tasks;
        }

        private void DgvTimesheet_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = dgvTimesheet.Rows[e.RowIndex];

            // create context menu for each row
            ContextMenuStrip taskContextMenu = new ContextMenuStrip();

            taskContextMenu.Items.Add("Delete").Click += taskContextMenuDelete_Click;

            // conditional context menu entries
            switch ( int.Parse( row.Cells["status"].ToString() ) )
            {
                case 0:
                    taskContextMenu.Items.Add("Mark as Invoiced").Click += markAsInvoiced;
                    taskContextMenu.Items.Add("Mark as Paid").Click += markAsPaid;
                    break;
                case 1:
                    taskContextMenu.Items.Add("Mark as Not Invoiced").Click += markAsNotInvoiced;
                    taskContextMenu.Items.Add("Mark as Paid").Click += markAsPaid;
                    break;
                case 2:
                    taskContextMenu.Items.Add("Mark as Not Invoiced").Click += markAsNotInvoiced;
                    taskContextMenu.Items.Add("Mark as Invoiced").Click += markAsInvoiced;
                    break;
            }

            dgvTimesheet.Rows[e.RowIndex].ContextMenuStrip = taskContextMenu;
        }

        /// <summary>
        /// Edit the cell value on doubleclick
        /// </summary>
        private void DgvTimesheet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                string newValue;
                EditMaster editor = null;

                switch (e.ColumnIndex)
                {
                    case 0:
                        editor = new EditTitle(timesheet.dataset.Tables[0].Rows[e.RowIndex][e.ColumnIndex + 1].ToString());
                        break;
                    case 1:
                        editor = new EditTime(timesheet.dataset.Tables[0].Rows[e.RowIndex][e.ColumnIndex + 1].ToString());
                        break;
                    // The date column is not currently editable
                }

                if (editor != null && editor.ShowDialog() == DialogResult.OK)
                {
                    newValue = editor.getValue();

                    timesheet.dataset.Tables[0].Rows[e.RowIndex][e.ColumnIndex + 1] = newValue;

                    timesheet.synchronizeDatasetWithDatabase();

                    fillDataGridView();
                }

            }
        }

        /// <summary>
        /// Selects the cell you right click on
        /// </summary>
        private void DgvTimesheet_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                if(e.Button == MouseButtons.Right)
                {
                    dgvTimesheet.Rows[e.RowIndex].Selected = true;
                }
            }
        }

        #region Events

        /// <summary>
        /// Toggle the font weight of the selected row
        /// </summary>
        private void DgvTimesheet_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTimesheet.SelectedRows.Count > 0)
            {
                mnuInvoice.Enabled = true;
            }
            else
            {
                mnuInvoice.Enabled = false;
            }

            foreach (DataGridViewRow row in dgvTimesheet.Rows)
            {
                if (row.Selected)
                {
                    row.DefaultCellStyle.Font = new Font(dgvTimesheet.Font, FontStyle.Bold);
                }else
                {
                    row.DefaultCellStyle.Font = new Font(dgvTimesheet.Font, FontStyle.Regular);
                }
            }
        }

        /// <summary>
        /// When the form is closed, close the database connection
        /// </summary>
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            TimesheetClose();
        }

        /// <summary>
        /// Select all of the text of the task name box when focused
        /// </summary>
        private void TxtTaskName_GotFocus(object sender, EventArgs e)
        {
            txtTaskName.SelectAll();
        }

        /// <summary>
        /// Opens a FileDialog to open an existing timesheet
        /// </summary>
        private void mnuTimesheetOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.CheckFileExists = false;
            fileBrowser.CheckPathExists = false;
            fileBrowser.DefaultExt = "sqlite";
            fileBrowser.Multiselect = false;

            DialogResult result = fileBrowser.ShowDialog();

            if (result.Equals(DialogResult.OK) && isSQLiteDatabase(fileBrowser.FileName))
            {
                TimesheetClose();
                timesheet = new Timesheet(fileBrowser.FileName);
                this.FormClosed += FrmMain_FormClosed;
                TimesheetOpen();
            }
            else if (result != DialogResult.Cancel)
            {
                MessageBox.Show("The file you tried to open was not a SQLite Database.");
            }
        }

        /// <summary>
        /// Opens a create timesheet dialog and creates a new timesheet
        /// </summary>
        private void mnuTimesheetNew_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileBrowser = new SaveFileDialog();
            fileBrowser.CheckFileExists = false;
            fileBrowser.CheckPathExists = false;
            fileBrowser.DefaultExt = "sqlite";

            if (fileBrowser.ShowDialog() == DialogResult.OK)
            {
                timesheet = new Timesheet(fileBrowser.FileName);
                TimesheetOpen();
            }
        }

        /// <summary>
        /// Closes the timesheet 
        /// </summary>
        private void mnuTimesheetClose_Click(object sender, EventArgs e)
        {
            TimesheetClose();
        }


        /// <summary>
        /// A right click context menu event for each task in the dgv
        /// </summary>
        private void taskContextMenuDelete_Click(object sender, EventArgs e)
        {
            string message = string.Format("Are you sure you want to delete {0} task(s)?", dgvTimesheet.SelectedRows.Count);
            string title = string.Format("Delete {0} task(s)?", dgvTimesheet.SelectedRows.Count);

            if (DialogResult.Yes == MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            {
                foreach (DataGridViewRow selectedRow in dgvTimesheet.SelectedRows)
                {
                    timesheet.dataset.Tables[0].Rows[selectedRow.Index].Delete();
                }

                timesheet.synchronizeDatasetWithDatabase();

                fillDataGridView();
            }
        }

        /// <summary>
        /// Starts the task timer
        /// </summary>
        private void btnTaskStart_Click(object sender, EventArgs e)
        {
            if (!timerState)
            {
                timer.Start();
                btnTaskStart.Text = "Stop";
                btnTaskSave.Enabled = false;
                btnTaskClear.Enabled = false;
                timerState = true;
                if(ActiveTask.BeginDate == DateTime.MinValue)
                {
                    ActiveTask.BeginDate = DateTime.Now;
                }
            }
            else
            {
                timer.Stop();
                btnTaskClear.Enabled = true;
                btnTaskSave.Enabled = true;
                btnTaskStart.Text = "Start";
                timerState = false;
                ActiveTask.EndDate = DateTime.Now;
            }
        }


        /// <summary>
        /// Adds one second to the task timer
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            setTimerText(ActiveTask.Duration);
        }


        /// <summary>
        /// Clears the current task if one exists
        /// </summary>
        private void btnTaskClear_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Are you sure you want to clear the task?", "Clear Time", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
            {
                setTimerText(ActiveTask.Duration);
                btnTaskSave.Enabled = false;
                btnTaskClear.Enabled = false;
                txtTaskName.Text = string.Empty;
            }
        }

        /// <summary>
        /// Add a row to the database when the user clicks save
        /// </summary>
        private void btnTaskSave_Click(object sender, EventArgs e)
        {
            DataRow row = timesheet.dataset.Tables[0].NewRow();

            row["name"] = txtTaskName.Text;
            row["begindate"] = ActiveTask.BeginDate.ToUniversalTime();
            row["enddate"] = ActiveTask.EndDate.ToUniversalTime();
            row["status"] = ActiveTask.Status.ToString();

            timesheet.dataset.Tables[0].Rows.Add(row);

            timesheet.synchronizeDatasetWithDatabase();

            fillDataGridView();

            ActiveTask = new Task();

            // clear
            setTimerText(ActiveTask.Duration);
            btnTaskSave.Enabled = false;
            txtTaskName.Text = string.Empty;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Open the timesheet and enable the form elements
        /// </summary>
        private void TimesheetOpen()
        {
            dgvTimesheet.Enabled = true;
            mnuTimesheetClose.Enabled = true;
            btnTaskStart.Enabled = true;
            txtTaskName.Enabled = true;
            lblTaskName.Enabled = true;
            btnTaskSave.Enabled = false;
            fillDataGridView();
            ActiveTask = new Task();
        }

        /// <summary>
        /// Close a timesheet to open a new one
        /// </summary>
        private void TimesheetClose()
        {
            if (timesheet != null)
            {
                timesheet.close();
                timesheet = null;
            }

            mnuTimesheetClose.Enabled = false;
            btnTaskStart.Enabled = false;
            txtTaskName.Enabled = false;
            lblTaskName.Enabled = false;
            dgvTimesheet.Enabled = false;
            dgvTimesheet.Rows.Clear();
        }

        /// <summary>
        /// clear and redraw the timesheet
        /// </summary>
        private void fillDataGridView()
        {

            Tasks.Clear();

            foreach (DataRow tableRow in timesheet.dataset.Tables[0].Rows)
            {
                Task task = new Task()
                {
                    Description = txtTaskName.Text,
                    BeginDate = DateTime.Parse(tableRow["begindate"].ToString()),
                    EndDate = DateTime.Parse(tableRow["enddate"].ToString()),
                    Status = (TaskStatus) int.Parse(tableRow["status"].ToString())
                };

                Tasks.Add(task);
            }
        }


        /// <summary>
        /// Changes the item to uninvoiced in the database
        /// </summary>
        private void markAsNotInvoiced(object sender, EventArgs e)
        {
            changeSelectedTaskStatus(TaskStatus.UNINVOICED);
        }


        /// <summary>
        /// Changes the item to invoiced in the database
        /// </summary>
        private void markAsInvoiced(object sender, EventArgs e)
        {
            changeSelectedTaskStatus(TaskStatus.INVOICED);
        }

        /// <summary>
        /// Changes the item to paid in the database
        /// </summary>
        private void markAsPaid(object sender, EventArgs e)
        {
            changeSelectedTaskStatus(TaskStatus.PAID);
        }


        /// <summary>
        /// Changes the states of the selected tasks
        /// </summary>
        /// <param name="state">The state</param>
        private void changeSelectedTaskStatus(TaskStatus state)
        {
            foreach (DataGridViewRow selectedRow in dgvTimesheet.SelectedRows)
            {
                timesheet.dataset.Tables[0].Rows[selectedRow.Index]["status"] = (int) state;
            }

            timesheet.synchronizeDatasetWithDatabase();

            fillDataGridView();
        }


        /// <summary>
        /// Set the text of the timer
        /// </summary>
        /// <param name="time">The time in seconds to set it to</param>
        private void setTimerText(TimeSpan duration)
        {
            lblTaskDuration.Text = duration.TotalSeconds.ToString();
        }

        /// <summary>
        /// Determines if the file path is a Format 3 SQLite Database
        /// </summary>
        /// <param name="pathToFile">A string leading to a file</param>
        /// <returns></returns>
        public static bool isSQLiteDatabase(string pathToFile)
        {
            bool result = false;

            if (File.Exists(pathToFile)) {

                using (FileStream stream = new FileStream(pathToFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    byte[] header = new byte[16];

                    for (int i = 0; i < 16; i++)
                    {
                        header[i] = (byte)stream.ReadByte();
                    }

                    result = System.Text.Encoding.UTF8.GetString(header).Contains("SQLite format 3");

                    stream.Close();
                }

            }

            return result;
        }


        /// <summary>
        /// Get the color the task should be set to
        /// </summary>
        /// <param name="status">The status code 0 for unbilled, 1 for billed, 2 for paid</param>
        /// <returns>The color associated with the status</returns>
        public Color getTaskColor(TaskStatus status)
        {
            Color backgroundColor = new Color();

            switch (status)
            {
                case TaskStatus.UNINVOICED:
                    backgroundColor = Color.FromArgb(255, 171, 171); // Red
                    break;
                case TaskStatus.INVOICED:
                    backgroundColor = Color.FromArgb(255, 252, 171); // Green
                    break;
                case TaskStatus.PAID:
                    backgroundColor = Color.FromArgb(171, 255, 172); // Blue
                    break;
                default:
                    backgroundColor = dgvTimesheet.DefaultCellStyle.BackColor;
                    break;
            }

            return backgroundColor;
        }

        #endregion
    }
}
