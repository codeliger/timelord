using System;
using System.Windows.Forms;

namespace timelord
{
    public partial class EditTime : EditMaster
    {
        int h;
        int m;
        int s;

        public EditTime(TimeSpan time)
        {
            InitializeComponent();

            h = time.Hours;
            m = time.Minutes;
            s = time.Seconds;

            txtHours.Text = h.ToString();
            txtMinutes.Text = m.ToString();
            txtSeconds.Text = s.ToString();
        }

        public TimeSpan getValue()
        {
            return new TimeSpan(h, m, s);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int i;

            if (int.TryParse(txtHours.Text,out i))
                h = int.Parse(txtHours.Text);

            if (int.TryParse(txtMinutes.Text, out i))
                m = int.Parse(txtMinutes.Text);

            if (int.TryParse(txtSeconds.Text, out i))
                s = int.Parse(txtSeconds.Text);

            this.Close();
        }
    }
}
