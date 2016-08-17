using System;
using System.Data;
using System.Data.SQLite;
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
        BindingSource source;

        /// <summary>
        /// sets up the form
        /// </summary>
        public frmMain()
        {
            InitializeComponent();

            Clipboard.SetText(DateTime.Now.ToString());

            this.MaximizeBox = false;
            this.MinimizeBox = true;
            dgvTimesheet.AutoGenerateColumns = false;

            txtTaskName.GotFocus += TxtTaskName_GotFocus;
            dgvTimesheet.SelectionChanged += DgvTimesheet_SelectionChanged;
            dgvTimesheet.CellMouseDown += DgvTimesheet_CellMouseDown;
            dgvTimesheet.RowsAdded += DgvTimesheet_RowsAdded;

            // initialize a timer for counting seconds
            timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 1000;

            AddDataGridViewColumns();

            dgvTimesheet.CellDoubleClick += DgvTimesheet_CellDoubleClick;
            dgvTimesheet.CellValueChanged += DgvTimesheet_CellValueChanged;
        }

        #region Events
        
        private void DgvTimesheet_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // ensure cell index is correct index
            // change cell background color to match
            if (dgvTimesheet.Columns[e.ColumnIndex].Name == "status")
            {
                UpdateRowBackgroundColor(dgvTimesheet.Rows[e.RowIndex]);
            }
        }

        
        private void DgvTimesheet_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = dgvTimesheet.Rows[e.RowIndex];

            // create context menu for each row
            ContextMenuStrip taskContextMenu = new ContextMenuStrip();

            taskContextMenu.Items.Add("Delete").Click += taskContextMenuDelete_Click;

            // conditional context menu entries
            switch ( (TaskStatus) Enum.Parse(typeof(TaskStatus), dgvTimesheet.Rows[e.RowIndex].Cells["status"].Value.ToString() ) )
            {
                case TaskStatus.UNINVOICED:
                    taskContextMenu.Items.Add("Mark as Invoiced").Click += markAsInvoiced;
                    taskContextMenu.Items.Add("Mark as Paid").Click += markAsPaid;
                    break;
                case TaskStatus.INVOICED:
                    taskContextMenu.Items.Add("Mark as Not Invoiced").Click += markAsNotInvoiced;
                    taskContextMenu.Items.Add("Mark as Paid").Click += markAsPaid;
                    break;
                case TaskStatus.PAID:
                    taskContextMenu.Items.Add("Mark as Not Invoiced").Click += markAsNotInvoiced;
                    taskContextMenu.Items.Add("Mark as Invoiced").Click += markAsInvoiced;
                    break;
            }

            dgvTimesheet.Rows[e.RowIndex].ContextMenuStrip = taskContextMenu;

            row.Cells["duration"].Value = DateTime.Parse(row.Cells["enddate"].Value.ToString()).Subtract(DateTime.Parse(row.Cells["begindate"].Value.ToString())).ToString();

            DataGridViewCellStyle defaultStyle = new DataGridViewCellStyle();

            // Changes color of cells based on if it has been invoiced or paid

            defaultStyle.ForeColor = Color.Black;
            defaultStyle.SelectionForeColor = Color.Black;
            dgvTimesheet.Rows[e.RowIndex].DefaultCellStyle = defaultStyle;

            UpdateRowBackgroundColor(dgvTimesheet.Rows[e.RowIndex]);
        }

        private void UpdateRowBackgroundColor(DataGridViewRow dataGridViewRow)
        {
            dataGridViewRow.DefaultCellStyle.SelectionBackColor  = dataGridViewRow.DefaultCellStyle.BackColor = getTaskColor((TaskStatus)Enum.Parse(typeof(TaskStatus), dataGridViewRow.Cells["status"].Value.ToString()));
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
                    case 1:
                        editor = new EditTitle(dgvTimesheet.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                        break;
                    case 2:
                        editor = new EditTime(dgvTimesheet.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                        break;
                    // The date column is not currently editable
                }

                if (editor != null && editor.ShowDialog() == DialogResult.OK)
                {
                    newValue = editor.getValue();

                    timesheet.Tasks().Rows[e.RowIndex][e.ColumnIndex] = newValue;

                    timesheet.Update();
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
            DisableTimesheet();
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
            OpenFileDialog fileBrowser = new OpenFileDialog()
            {
                CheckFileExists = false,
                CheckPathExists = false,
                DefaultExt = "sqlite",
                Multiselect = false
            };

            DialogResult result = fileBrowser.ShowDialog();

            if (result.Equals(DialogResult.OK) && isSQLiteDatabase(fileBrowser.FileName))
            {
                DisableTimesheet();
                timesheet = new Timesheet(fileBrowser.FileName);

                EnableTimesheet();
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
                EnableTimesheet();
            }
        }

        /// <summary>
        /// Closes the timesheet 
        /// </summary>
        private void mnuTimesheetClose_Click(object sender, EventArgs e)
        {
            DisableTimesheet();
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
                    timesheet.Tasks().Rows[selectedRow.Index].Delete();
                }
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
                ActiveTask = new Task();
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
            DataRow row = timesheet.Tasks().NewRow();

            ActiveTask.Description = lblTaskName.Text;

            row["description"] = ActiveTask.Description;
            row["begindate"] = ActiveTask.BeginDate;
            row["enddate"] = ActiveTask.EndDate;
            row["status"] = ActiveTask.Status;

            timesheet.Tasks().Rows.Add(row);

            timesheet.Update();

            ActiveTask = new Task();
            setTimerText(ActiveTask.Duration);
            btnTaskSave.Enabled = false;
            txtTaskName.Text = string.Empty;
            txtTaskName.Focus();
        }

        #endregion

        #region Methods

        private void AddDataGridViewColumns()
        {
            dgvTimesheet.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "id",
                Visible = false,
                DataPropertyName = "id"
            });

            dgvTimesheet.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "description",
                HeaderText = "Description",
                Visible = true,
                DataPropertyName = "description"
            });

            dgvTimesheet.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "duration",
                HeaderText = "Duration",
                Visible = true,

            });

            dgvTimesheet.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "begindate",
                HeaderText = "Date Started",
                Visible = false,
                DataPropertyName = "begindate"
            });

            dgvTimesheet.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "enddate",
                HeaderText = "Date Completed",
                Visible = true,
                DataPropertyName = "enddate"
            });

            dgvTimesheet.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "status",
                Visible = false,
                DataPropertyName = "status"
            });
        }


        private void EnableTimesheet()
        {
            dgvTimesheet.Enabled = true;
            mnuTimesheetClose.Enabled = true;
            btnTaskStart.Enabled = true;
            txtTaskName.Enabled = true;
            lblTaskName.Enabled = true;
            btnTaskSave.Enabled = false;

            ActiveTask = new Task();
            source = new BindingSource();
            source.DataSource = timesheet.Tasks();

            dgvTimesheet.DataSource = source;

            this.FormClosed += FrmMain_FormClosed;
        }

        /// <summary>
        /// Close a timesheet to open a new one
        /// </summary>
        private void DisableTimesheet()
        {
            if (timesheet != null)
            {
                timesheet.Tasks().Clear();
                timesheet.close();
                timesheet = null;
            }


            mnuTimesheetClose.Enabled = false;
            btnTaskStart.Enabled = false;
            txtTaskName.Enabled = false;
            lblTaskName.Enabled = false;
            dgvTimesheet.Enabled = false;
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
                timesheet.Tasks().Rows[selectedRow.Index]["status"] = (int) state;
            }

            timesheet.Update();
        }


        /// <summary>
        /// Set the text of the timer
        /// </summary>
        /// <param name="time">The time in seconds to set it to</param>
        private void setTimerText(TimeSpan duration)
        {
            lblTaskDuration.Text = duration.ToString(@"hh\:mm\:ss");
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
