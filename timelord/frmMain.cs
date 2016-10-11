using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace timelord
{
    public partial class frmMain : Form
    {
        SQLiteInstance timesheet;
        Timer timer;
        int time;
        bool timerState = false;
        Task ActiveTask;
        BindingSource source;
        DataTable _table;
        ContextMenuStrip cmUninvoiced;
        ContextMenuStrip cmInvoiced;
        ContextMenuStrip cmPaid;
        DataGridViewCellStyle defaultStyle;
        List<string> createQueries;

        public frmMain()
        {
            InitializeComponent();

            this.MaximizeBox = false;
            this.MinimizeBox = true;
            dgvTimesheet.AutoGenerateColumns = false;

            defaultStyle = new DataGridViewCellStyle();
            defaultStyle.ForeColor = Color.Black;
            defaultStyle.SelectionForeColor = Color.Black;

            txtTaskName.GotFocus += TxtTaskName_GotFocus;
            dgvTimesheet.SelectionChanged += DgvTimesheet_SelectionChanged;
            dgvTimesheet.CellMouseDown += DgvTimesheet_CellMouseDown;
            dgvTimesheet.RowsAdded += DgvTimesheet_RowsAdded;

            timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 1000;
            time = 0;

            AddDataGridViewColumns();

            CreateContextMenuStrips();

            dgvTimesheet.CellDoubleClick += DgvTimesheet_CellDoubleClick;

            createQueries = new List<string>();
            createQueries.Add(QueryString.Task.Create);
            createQueries.Add(QueryString.Client.Create);
            createQueries.Add(QueryString.Invoice.Create);
            createQueries.Add(QueryString.InvoiceTask.Create);
            createQueries.Add(QueryString.Identity.Create);
        }

        private void CreateContextMenuStrips()
        {
            cmUninvoiced = new ContextMenuStrip();
            cmInvoiced = new ContextMenuStrip();
            cmPaid = new ContextMenuStrip();

            cmUninvoiced.Items.Add("Delete").Click += taskContextMenuDelete_Click;
            cmInvoiced.Items.Add("Delete").Click += taskContextMenuDelete_Click;
            cmPaid.Items.Add("Delete").Click += taskContextMenuDelete_Click;

            cmUninvoiced.Items.Add("Mark as Invoiced").Click += markAsInvoiced;
            cmUninvoiced.Items.Add("Mark as Paid").Click += markAsPaid;

            cmInvoiced.Items.Add("Mark as Not Invoiced").Click += markAsNotInvoiced;
            cmInvoiced.Items.Add("Mark as Paid").Click += markAsPaid;

            cmPaid.Items.Add("Mark as Not Invoiced").Click += markAsNotInvoiced;
            cmPaid.Items.Add("Mark as Invoiced").Click += markAsInvoiced;
        }


        #region Events

        private void DgvTimesheet_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for(int i = e.RowIndex; i < dgvTimesheet.Rows.Count; i++)
            {
                DataGridViewRow row = dgvTimesheet.Rows[i];
                row.DefaultCellStyle = defaultStyle.Clone();
                refreshTaskValues(row);
            }
        }

        private void refreshTaskValues(DataGridViewRow row)
        {
            Task AddedTask = new Task()
            {
                BeginDate = DateTime.Parse(row.Cells["begindate"].Value.ToString()),
                EndDate = DateTime.Parse(row.Cells["enddate"].Value.ToString())
            };

            row.Cells["duration"].Value = AddedTask.Duration.ToString();

            UpdateRowBackgroundColor(row);

            setTaskContextMenu((TaskStatus)Enum.Parse(typeof(TaskStatus), row.Cells["status"].Value.ToString()), row);
        }

        private void setTaskContextMenu(TaskStatus status, DataGridViewRow row)
        {
            switch (status)
            {
                case TaskStatus.UNINVOICED:
                    row.ContextMenuStrip = cmUninvoiced;
                    break;
                case TaskStatus.INVOICED:
                    row.ContextMenuStrip = cmInvoiced;
                    break;
                case TaskStatus.PAID:
                    row.ContextMenuStrip = cmPaid;
                    break;
            }
        }

        private void UpdateRowBackgroundColor(DataGridViewRow row)
        {
            row.DefaultCellStyle.SelectionBackColor =
                row.DefaultCellStyle.BackColor =
                    getTaskColor((TaskStatus)Enum.Parse(typeof(TaskStatus), row.Cells["status"].Value.ToString()));
        }

        private void DgvTimesheet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                EditMaster editor = null;

                string columnName = dgvTimesheet.Columns[e.ColumnIndex].Name;

                DataGridViewRow row = dgvTimesheet.Rows[e.RowIndex];

                if (columnName == "description")
                {
                    editor = new EditDescription(row.Cells[e.ColumnIndex].Value.ToString());

                    if (editor.ShowDialog() == DialogResult.OK)
                    {
                        _table.Rows.Find(row.Cells["id"].Value)["description"] = ((EditDescription)editor).getValue();

                        timesheet.Commit(_table, QueryString.Task.Select);
                    }

                }
                else if (columnName == "duration")
                {
                    editor = new EditTime(DateTime.Parse(_table.Rows.Find(row.Cells["id"].Value)["enddate"].ToString()).Subtract(DateTime.Parse(_table.Rows.Find(row.Cells["id"].Value)["begindate"].ToString())));

                    if (editor.ShowDialog() == DialogResult.OK)
                    {
                        _table.Rows[e.RowIndex]["enddate"] = DateTime.Parse(_table.Rows.Find(row.Cells["id"].Value)["begindate"].ToString()).Add(TimeSpan.Parse(((EditTime)editor).getValue().ToString()));

                        timesheet.Commit(_table, QueryString.Task.Select);

                        refreshTaskValues(dgvTimesheet.Rows[e.RowIndex]);
                    }
                }
            }
        }

        private void DgvTimesheet_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    dgvTimesheet.Rows[e.RowIndex].Selected = true;
                }
            }
        }

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
                }
                else
                {
                    row.DefaultCellStyle.Font = new Font(dgvTimesheet.Font, FontStyle.Regular);
                }
            }
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            DisableTimesheet();
        }

        private void TxtTaskName_GotFocus(object sender, EventArgs e)
        {
            txtTaskName.SelectAll();
        }

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
                timesheet = new SQLiteInstance(fileBrowser.FileName, createQueries);

                EnableTimesheet();
            }
            else if (result != DialogResult.Cancel)
            {
                MessageBox.Show("The file you tried to open was not a SQLite Database.");
            }
        }

        private void NewTimesheet_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileBrowser = new SaveFileDialog();
            fileBrowser.CheckFileExists = false;
            fileBrowser.CheckPathExists = false;
            fileBrowser.DefaultExt = "sqlite";

            if (fileBrowser.ShowDialog() == DialogResult.OK)
            {
                timesheet = new SQLiteInstance(fileBrowser.FileName, createQueries);
                EnableTimesheet();
            }
        }

        private void CloseTimesheet_Click(object sender, EventArgs e)
        {
            DisableTimesheet();
        }

        private void taskContextMenuDelete_Click(object sender, EventArgs e)
        {
            string message = string.Format("Are you sure you want to delete {0} task(s)?", dgvTimesheet.SelectedRows.Count);
            string title = string.Format("Delete {0} task(s)?", dgvTimesheet.SelectedRows.Count);

            if (DialogResult.Yes == MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            {
                foreach (DataGridViewRow selectedRow in dgvTimesheet.SelectedRows)
                {
                    _table.Rows.Find(selectedRow.Cells["id"].Value).Delete();
                }

                timesheet.Commit(_table, QueryString.Task.Select);
            }
        }

        private void ToggleTask_Click(object sender, EventArgs e)
        {
            if (!timerState)
            {
                timer.Start();
                btnTaskToggle.Text = "Stop";
                btnTaskSave.Enabled = false;
                btnTaskClear.Enabled = false;
                timerState = true;

                if (ActiveTask.BeginDate == DateTime.MinValue)
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            time++;
            setTimerText(time);
        }

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

            timesheet.Commit(_table, QueryString.Task.Select);

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

            _table = timesheet.GetTable("tasks", QueryString.Task.Select);

            source.DataSource = _table;

            dgvTimesheet.DataSource = source;

            this.FormClosed += FrmMain_FormClosed;
        }

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

        private void markAsNotInvoiced(object sender, EventArgs e)
        {
            changeSelectedTaskStatus(TaskStatus.UNINVOICED);
        }

        private void markAsInvoiced(object sender, EventArgs e)
        {
            changeSelectedTaskStatus(TaskStatus.INVOICED);
        }

        private void markAsPaid(object sender, EventArgs e)
        {
            changeSelectedTaskStatus(TaskStatus.PAID);
        }

        private void changeSelectedTaskStatus(TaskStatus state)
        {
            foreach (DataGridViewRow selectedRow in dgvTimesheet.SelectedRows)
            {
                _table.Rows.Find(selectedRow.Cells["id"].Value)["status"] = state.ToString();

                refreshTaskValues(dgvTimesheet.Rows[selectedRow.Index]);
            }

            timesheet.Commit(_table, QueryString.Task.Select);
        }

        private void setTimerText(int timeInSeconds)
        {
            lblTaskDuration.Text = TimeSpan.FromSeconds(timeInSeconds).ToString();
        }

        public static bool isSQLiteDatabase(string pathToFile)
        {
            bool result = false;

            if (File.Exists(pathToFile))
            {

                using (FileStream stream = new FileStream(pathToFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    byte[] header = new byte[16];

                    for (int i = 0; i < 16; i++)
                    {
                        header[i] = (byte)stream.ReadByte();
                    }

                    result = System.Text.Encoding.UTF8.GetString(header).Contains("SQLite format 3");
                }
            }

            return result;
        }

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

        private void mnuInvoiceCreate_Click(object sender, EventArgs e)
        {
            new frmInvoice(timesheet, dgvTimesheet.SelectedRows).ShowDialog();
        }
    }
}
