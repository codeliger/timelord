using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
namespace timelord
{
    /// <summary>
    /// the main time tracking form
    /// </summary>
    public partial class frmMain : Form
    {
        Timesheet timesheet;
        Timer timer;
        int time;
        bool timerState = false;

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

            // initialize a timer for counting seconds
            timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 1000;

            // reset duration
            lblTaskDuration.Text = TimeSpan.FromSeconds(0).ToString(@"hh\:mm\:ss");

            dgvTimesheet.Columns.Add("taskname", "Task");
            dgvTimesheet.Columns.Add("timeinseconds", "Time");
            dgvTimesheet.Columns.Add("date", "Date");
            dgvTimesheet.Columns.Add("paid", "Paid");
            dgvTimesheet.Columns[3].Visible = false;

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
            string message;
            string title;

            if (dgvTimesheet.SelectedRows.Count > 1)
            {
                message = "these tasks?";
                title = "Delete Tasks";
            }
            else
            {
                message = "this task?";
                title = "Delete Task";
            }

            if (DialogResult.Yes == MessageBox.Show("Are you sure you want to delete " + message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
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
            }
            else
            {
                timer.Stop();
                btnTaskClear.Enabled = true;
                btnTaskSave.Enabled = true;
                btnTaskStart.Text = "Start";
                timerState = false;
            }
        }


        /// <summary>
        /// Adds one second to the task timer
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            time++;
            setTimerText(time);
        }


        /// <summary>
        /// Clears the current task if one exists
        /// </summary>
        private void btnTaskClear_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Are you sure you want to clear the task?", "Clear Time", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
            {
                time = 0;
                setTimerText(0);
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

            row["taskname"] = txtTaskName.Text;
            row["timeinseconds"] = time;
            row["date"] = DateTime.Now.ToString();
            row["paid"] = 0;

            timesheet.dataset.Tables[0].Rows.Add(row);

            timesheet.synchronizeDatasetWithDatabase();

            fillDataGridView();

            // clear
            time = 0;
            setTimerText(0);
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
            // Empty the datagrid view so we can get updated rows from the database
            dgvTimesheet.Rows.Clear();

            foreach (DataRow r in timesheet.dataset.Tables[0].Rows)
            {
                // create context menu for each row
                ContextMenuStrip taskContextMenu = new ContextMenuStrip();

                taskContextMenu.Items.Add("Delete");
                taskContextMenu.Items[0].Click += taskContextMenuDelete_Click;

                // conditional context menu entries
                switch ( int.Parse( r["paid"].ToString() ) )
                {
                    case 0:
                        taskContextMenu.Items.Add("Mark as Invoiced");
                        taskContextMenu.Items[taskContextMenu.Items.Count - 1].Click += markAsInvoiced;
                        taskContextMenu.Items.Add("Mark as Paid");
                        taskContextMenu.Items[taskContextMenu.Items.Count - 1].Click += markAsPaid;
                        break;
                    case 1:
                        taskContextMenu.Items.Add("Mark as Not Invoiced");
                        taskContextMenu.Items[taskContextMenu.Items.Count - 1].Click += markAsNotInvoiced;
                        taskContextMenu.Items.Add("Mark as Paid");
                        taskContextMenu.Items[taskContextMenu.Items.Count - 1].Click += markAsPaid;
                        break;
                    case 2:
                        taskContextMenu.Items.Add("Mark as Not Invoiced");
                        taskContextMenu.Items[taskContextMenu.Items.Count - 1].Click += markAsNotInvoiced;
                        taskContextMenu.Items.Add("Mark as Invoiced");
                        taskContextMenu.Items[taskContextMenu.Items.Count - 1].Click += markAsInvoiced;
                        break;
                }

                // Instead of a list, create a new row for the DataGridView.
                DataGridViewRow row = new DataGridViewRow();
                row.ContextMenuStrip = taskContextMenu;

                // Populate the row with cells.
                row.CreateCells(dgvTimesheet);

                row.Cells[0].Value = r["taskname"].ToString();
                row.Cells[1].Value = TimeSpan.FromSeconds(double.Parse(r["timeinseconds"].ToString()));
                row.Cells[2].Value = r["date"].ToString();
                row.Cells[3].Value = r["paid"].ToString();                           

                DataGridViewCellStyle defaultStyle = new DataGridViewCellStyle();

                // Changes color of cells based on if it has been invoiced or paid

                defaultStyle.ForeColor = Color.Black;
                defaultStyle.SelectionForeColor = Color.Black;

                defaultStyle.BackColor = getTaskStatus(int.Parse(r["paid"].ToString()));
                defaultStyle.SelectionBackColor = defaultStyle.BackColor;

                row.DefaultCellStyle = defaultStyle;

                dgvTimesheet.Rows.Add(row);

            }

            dgvTimesheet.ClearSelection();
        }


        /// <summary>
        /// Changes the item to uninvoiced in the database
        /// </summary>
        private void markAsNotInvoiced(object sender, EventArgs e)
        {
            foreach(DataGridViewRow selectedRow in dgvTimesheet.SelectedRows)
            {
                timesheet.dataset.Tables[0].Rows[selectedRow.Index]["paid"] = 0;
            }

            timesheet.synchronizeDatasetWithDatabase();

            fillDataGridView();
        }


        /// <summary>
        /// Changes the item to invoiced in the database
        /// </summary>
        private void markAsInvoiced(object sender, EventArgs e)
        {
            foreach (DataGridViewRow selectedRow in dgvTimesheet.SelectedRows)
            {
                timesheet.dataset.Tables[0].Rows[selectedRow.Index]["paid"] = 1;
            }

            timesheet.synchronizeDatasetWithDatabase();

            fillDataGridView();
        }

        /// <summary>
        /// Changes the item to paid in the database
        /// </summary>
        private void markAsPaid(object sender, EventArgs e)
        {
            foreach (DataGridViewRow selectedRow in dgvTimesheet.SelectedRows)
            {
                timesheet.dataset.Tables[0].Rows[selectedRow.Index]["paid"] = 2;
            }

            timesheet.synchronizeDatasetWithDatabase();

            fillDataGridView();
        }


        /// <summary>
        /// Set the text of the timer
        /// </summary>
        /// <param name="time">The time in seconds to set it to</param>
        private void setTimerText(int time)
        {
            lblTaskDuration.Text = TimeSpan.FromSeconds(time).ToString();
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
        public Color getTaskStatus(int status)
        {
            Color backgroundColor = new Color();

            switch (status)
            {
                case 0:
                    backgroundColor = Color.FromArgb(255, 171, 171); // Red
                    break;
                case 1:
                    backgroundColor = Color.FromArgb(255, 252, 171); // Green
                    break;
                case 2:
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
