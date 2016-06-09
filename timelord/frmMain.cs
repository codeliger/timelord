﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timelord
{
    /// <summary>
    /// the main time tracking form
    /// </summary>
    public partial class frmMain : Form
    {
        Timesheet timesheet;
        DataSet dataset;
        Timer timer;
        int time;
        bool timerState = false;

        /// <summary>
        /// sets up the form
        /// </summary>
        public frmMain()
        {
            InitializeComponent();

            txtTaskName.GotFocus += TxtTaskName_GotFocus;

            // initialize a timer for counting seconds
            timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 1000;

            // reset duration
            lblTaskDuration.Text = TimeSpan.FromSeconds(0).ToString(@"hh\:mm\:ss");

            dgvTimesheet.Columns.Add("taskname", "Task");
            dgvTimesheet.Columns.Add("timeinseconds", "Time");
            dgvTimesheet.Columns.Add("date", "Date");
            //dgvTimesheet.Columns.Add("paid", "Paid");

        }

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

            if (result == DialogResult.OK)
            {
                timesheet = new Timesheet(fileBrowser.FileName);
                // Enable form controls
                TimesheetOpen();
            }
            else if (result != DialogResult.Cancel)
            {
                MessageBox.Show("Could not open the file specified: " + result.ToString());
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

        private void TimesheetOpen()
        {
            mnuTimesheetClose.Enabled = true;
            btnTaskStart.Enabled = true;
            txtTaskName.Enabled = true;
            lblTaskName.Enabled = true;
            btnTaskSave.Enabled = false;

            this.dataset = timesheet.toDataSet();
        }

        /// <summary>
        /// clear and redraw the timesheet
        /// </summary>
        private void updateDgvTimesheet()
        {
            // The DataGridView preserves rows from old datasets when we update-- we need to remove them.
            dgvTimesheet.Rows.Clear();

            foreach (DataRow r in dataset.Tables[0].Rows)
            {
                // Instead of a list, create a new row for the DataGridView.
                DataGridViewRow row = new DataGridViewRow();

                // Populate the row with cells.
                row.CreateCells(dgvTimesheet);

                row.Cells[0].Value = r["taskname"].ToString();
                row.Cells[1].Value = TimeSpan.FromSeconds(double.Parse(r["timeinseconds"].ToString()));
                row.Cells[2].Value = r["date"].ToString();
                //row.Cells[3].Value = r["paid"].ToString();

                dgvTimesheet.Rows.Add(row);
            }
        }

        /// <summary>
        /// Close a timesheet to open a new one
        /// </summary>
        private void TimesheetClose()
        {
            timesheet.close();
            mnuTimesheetClose.Enabled = false;
            btnTaskStart.Enabled = false;
            txtTaskName.Enabled = false;
            lblTaskName.Enabled = false;
            dgvTimesheet.Enabled = false;
            // TODO: Clear DataGridView
            dgvTimesheet.Rows.Clear();
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
            }
        }

        /// <summary>
        /// Add a row to the database when the user clicks save
        /// </summary>
        private void btnTaskSave_Click(object sender, EventArgs e)
        {
            DataRow row = dataset.Tables[0].NewRow();

            row.BeginEdit();

            row["taskname"] = txtTaskName.Text;
            row["timeinseconds"] = time;
            row["date"] = DateTime.Now.ToString();
            row["paid"] = 0;

            row.EndEdit();

            dataset.Tables[0].Rows.Add(row);

            // this isnt working
            timesheet.Update(dataset);

            updateDgvTimesheet();

            // clear
            time = 0;
            setTimerText(0);
            btnTaskSave.Enabled = false;
        }

        /// <summary>
        /// Set the text of the timer
        /// </summary>
        /// <param name="time">The time in seconds to set it to</param>
        private void setTimerText(int time)
        {
            lblTaskDuration.Text = TimeSpan.FromSeconds(time).ToString();
        }

        

    }

}
