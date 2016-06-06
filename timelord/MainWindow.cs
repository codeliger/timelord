using System;
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
    public partial class MainWindow : Form
    {
        Timesheet timesheet;
        DataSet dataset;
        Timer timer;
        int time;
        bool timerState = false;

        public MainWindow()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 1000;
            lblTaskDuration.Text = TimeSpan.FromSeconds(0).ToString(@"hh\:mm\:ss");
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

            dataset = timesheet.toDataSet();
        }

        private void updateDgvTimesheet()
        {
            foreach (DataRow r in dataset.Tables[0].Rows)
            {
                List<object> row = new List<object>();

                row.Add(r["taskname"]);
                row.Add(r["timeinseconds"]);
                row.Add(r["date"]);
                row.Add(r["paid"]);

                dgvTimesheet.Rows.Add(row);
            }
        }

        private void TimesheetClose()
        {
            timesheet.close();
            mnuTimesheetClose.Enabled = false;
            btnTaskStart.Enabled = false;
            txtTaskName.Enabled = false;
            lblTaskName.Enabled = false;
            dgvTimesheet.Enabled = false;

            // TODO: Clear DataGridView
        }

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

        private void Timer_Tick(object sender, EventArgs e)
        {
            time++;
            setTimerText(time);
        }

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

            row["taskname"] = txtTaskName.Text;
            row["timeinseconds"] = time;
            row["date"] = DateTime.Now.ToString();
            row["paid"] = 0;
            dataset.Tables[0].Rows.Add(row);

            dataset.Tables[0].AcceptChanges();

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
            lblTaskDuration.Text = TimeSpan.FromSeconds(time).ToString(@"hh\:mm\:ss");
        }
    }

}
