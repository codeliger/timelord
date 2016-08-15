using System;
using System.Windows.Forms;

namespace timelord
{
    public partial class EditTime : EditMaster
    {
        const decimal h = 3600M;
        const decimal m = 60M;

        decimal hours;
        decimal minutes;
        decimal seconds;

        decimal time;

        public EditTime(string initialValue)
        {
            InitializeComponent();

            time = decimal.Parse(initialValue);

            hours = Math.Floor(time / h);

            minutes = Math.Floor((time - (hours * h)) / m);

            seconds = time - ((hours * h) + (minutes * m));

            txtHours.Text = hours.ToString();
            txtMinutes.Text = minutes.ToString();
            txtSeconds.Text = seconds.ToString();
        }

        public override string getValue()
        {
            return (
                (decimal.Parse(this.txtHours.Text) * 3600M)
                +
                (decimal.Parse(this.txtMinutes.Text) * 60M)
                +
                decimal.Parse(this.txtSeconds.Text)
            ).ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
