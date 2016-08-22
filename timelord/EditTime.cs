using System;
using System.Windows.Forms;

namespace timelord
{
    public partial class EditTime : EditMaster
    {
        public EditTime(TimeSpan time)
        {
            InitializeComponent();

            txtHours.Text = time.Hours.ToString();
            txtMinutes.Text = time.Minutes.ToString();
            txtSeconds.Text = time.Seconds.ToString();
        }

        public string getValue()
        {
            return new TimeSpan(int.Parse(this.txtHours.Text), int.Parse(this.txtMinutes.Text), int.Parse(this.txtSeconds.Text)).ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
