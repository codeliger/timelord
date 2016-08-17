using System;
using System.Windows.Forms;

namespace timelord
{
    public partial class EditTime : EditMaster
    {
        public EditTime(string initialValue)
        {
            InitializeComponent();

            TimeSpan time = TimeSpan.Parse(initialValue);

            txtHours.Text = time.Seconds.ToString();
            txtMinutes.Text = time.Minutes.ToString();
            txtSeconds.Text = time.Hours.ToString();
        }

        public override string getValue()
        {
            return new TimeSpan(int.Parse(this.txtHours.Text), int.Parse(this.txtMinutes.Text), int.Parse(this.txtSeconds.Text)).ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
