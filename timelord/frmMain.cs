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
        Task ActiveTask;
        BindingSource source;
        DataTable _table;

        /// <summary>
        /// sets up the form
        /// </summary>
        public frmMain()
        {
            InitializeComponent();

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
            time = 0;

            AddDataGridViewColumns();

            dgvTimesheet.CellDoubleClick += DgvTimesheet_CellDoubleClick;
            dgvTimesheet.CellValueChanged += DgvTimesheet_CellValueChanged;
        }



        #region Events

        private void DgvTimesheet_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dgvTimesheet.Columns[e.ColumnIndex].Name;

            if (columnName == "status")
            {
                UpdateRowBackgroundColor(dgvTimesheet.Rows[e.RowIndex]);
            }
            else if(columnName == "enddate")
            {
                DataGridViewRow row = dgvTimesheet.Rows[e.RowIndex];
                Task AddedTask = new Task();
                AddedTask.BeginDate = DateTime.Parse(row.Cells["begindate"].Value.ToString());
                AddedTask.EndDate = DateTime.Parse(row.Cells["enddate"].Value.ToString());
                row.Cells["duration"].Value = AddedTask.Duration;
            }
        }

        /// <summary>
        /// Every time a row is added determine the context menu buttons needed based on its state and change the background color of the task.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvTimesheet_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = dgvTimesheet.Rows[e.RowIndex];

            Task AddedTask = new Task();
            AddedTask.Description = row.Cells["description"].Value.ToString();
            AddedTask.BeginDate = DateTime.Parse(row.Cells["begindate"].Value.ToString());
            AddedTask.EndDate = DateTime.Parse(row.Cells["enddate"].Value.ToString());
            AddedTask.Status = (TaskStatus)(Enum.Parse(typeof(TaskStatus), row.Cells["status"].Value.ToString()));

            // create context menu for each row
            ContextMenuStrip taskContextMenu = new ContextMenuStrip();

            taskContextMenu.Items.Add("Delete").Click += taskContextMenuDelete_Click;

            switch (AddedTask.Status)
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

            row.ContextMenuStrip = taskContextMenu;

            row.Cells["duration"].Value = AddedTask.Duration;

            DataGridViewCellStyle defaultStyle = new DataGridViewCellStyle();

            // Changes color of cells based on if it has been invoiced or paid

            defaultStyle.ForeColor = Color.Black;
            defaultStyle.SelectionForeColor = Color.Black;
            dgvTimesheet.Rows[e.RowIndex].DefaultCellStyle = defaultStyle;

            UpdateRowBackgroundColor(dgvTimesheet.Rows[e.RowIndex]);
        }

        private void UpdateRowBackgroundColor(DataGridViewRow dataGridViewRow)
        {
            dataGridViewRow.DefaultCellStyle.SelectionBackColor = dataGridViewRow.DefaultCellStyle.BackColor = getTaskColor((TaskStatus)Enum.Parse(typeof(TaskStatus), dataGridViewRow.Cells["status"].Value.ToString()));
        }

        /// <summary>
        /// Edit the cell value on doubleclick
        /// </summary>
        private void DgvTimesheet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                EditMaster editor = null;

                string columnName = dgvTimesheet.Columns[e.ColumnIndex].Name;

                if (columnName == "description")
                {
                    editor = new EditDescription(dgvTimesheet.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                    if (editor.ShowDialog() == DialogResult.OK)
                    {
                        _table.Rows[e.RowIndex]["description"] = ((EditDescription)editor).getValue();
                    }

                    timesheet.Commit(_table);
                }
                else if (columnName == "duration")
                {
                    editor = new EditTime(DateTime.Parse(_table.Rows[e.RowIndex]["enddate"].ToString()).Subtract(DateTime.Parse(_table.Rows[e.RowIndex]["begindate"].ToString())));

                    if (editor.ShowDialog() == DialogResult.OK)
                    {
                        _table.Rows[e.RowIndex]["enddate"] = DateTime.Parse(_table.Rows[e.RowIndex]["begindate"].ToString()).Add(TimeSpan.Parse(((EditTime)editor).getValue().ToString()));
                    }

                    timesheet.Commit(_table);


                    Task EditedTask = new Task();
                    EditedTask.BeginDate = DateTime.Parse(dgvTimesheet.Rows[e.RowIndex].Cells["begindate"].Value.ToString());
                    EditedTask.EndDate = DateTime.Parse(dgvTimesheet.Rows[e.RowIndex].Cells["enddate"].Value.ToString());
                    dgvTimesheet.Rows[e.RowIndex].Cells["duration"].Value = EditedTask.Duration;

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
        private void OpenTimesheet_Click(object sender, EventArgs e)
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
        private void NewTimesheet_Click(object sender, EventArgs e)
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
        private void CloseTimesheet_Click(object sender, EventArgs e)
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
                    _table.Rows[selectedRow.Index].Delete();
                }
            }
        }

        /// <summary>
        /// Starts the task timer
        /// </summary>
        private void ToggleTask_Click(object sender, EventArgs e)
        {
            if (!timerState)
            {
                timer.Start();
                btnTaskToggle.Text = "Stop";
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
                btnTaskToggle.Text = "Start";
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
        private void ClearTask_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Are you sure you want to clear the task?", "Clear Time", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
            {
                ActiveTask = new Task();
                time = 0;
                setTimerText(time);
                btnTaskSave.Enabled = false;
                btnTaskClear.Enabled = false;
                txtTaskName.Text = string.Empty;
            }
        }

        /// <summary>
        /// Add a row to the database when the user clicks save
        /// </summary>
        private void SaveTask_Click(object sender, EventArgs e)
        {
            ActiveTask.Description = txtTaskName.Text;
            ActiveTask.EndDate = ActiveTask.BeginDate.Add(TimeSpan.FromSeconds(time));

            DataRow row = _table.NewRow();

            row["description"] = ActiveTask.Description;
            row["begindate"] = ActiveTask.BeginDate.ToString();
            row["enddate"] = ActiveTask.EndDate.ToString();
            row["status"] = ActiveTask.Status.ToString();

            _table.Rows.Add(row);

            timesheet.Commit(_table);

            _table = timesheet.Tasks();

            ActiveTask = new Task();
            time = 0;
            setTimerText(time);
            btnTaskSave.Enabled = false;
            btnTaskClear.Enabled = false;
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
                DataPropertyName = "id",
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
                Visible = true            
            });

            dgvTimesheet.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "begindate",
                HeaderText = "Date Started",
                Visible = false,
                DataPropertyName = "begindate",
                ValueType = typeof(DateTime)
            });

            dgvTimesheet.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "enddate",
                HeaderText = "Date Completed",
                Visible = true,
                DataPropertyName = "enddate",
                ValueType = typeof(DateTime)
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
            btnTaskToggle.Enabled = true;
            txtTaskName.Enabled = true;
            lblTaskName.Enabled = true;
            btnTaskSave.Enabled = false;

            ActiveTask = new Task();

            source = new BindingSource();

            _table = timesheet.Tasks();

            source.DataSource = _table;

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
                dgvTimesheet.DataSource = null;
                source = null;
                timesheet = null;
            }

            mnuTimesheetClose.Enabled = false;
            btnTaskToggle.Enabled = false;
            txtTaskName.Enabled = false;
            lblTaskName.Enabled = false;
            dgvTimesheet.Enabled = false;
            btnTaskSave.Enabled = false;
            btnTaskClear.Enabled = false;
            txtTaskName.Text = string.Empty;
            setTimerText(0);
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
                _table.Rows[selectedRow.Index]["status"] = state.ToString();
            }

            timesheet.Commit(_table);

            _table = timesheet.Tasks();
        }


        /// <summary>
        /// Set the text of the timer
        /// </summary>
        /// <param name="time">The time in seconds to set it to</param>
        private void setTimerText(int timeInSeconds)
        {
            lblTaskDuration.Text = TimeSpan.FromSeconds(timeInSeconds).ToString();
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
