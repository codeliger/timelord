using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timelord
{
    public partial class EditDescription : EditMaster
    {
        public EditDescription(string initialValue)
        {
            InitializeComponent();
            this.txtCell.Text = initialValue;
            Shown += EditDescription_Shown;
        }

        private void EditDescription_Shown(object sender, EventArgs e)
        {
            txtCell.Focus();
        }

        public string getValue()
        {
            return this.txtCell.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
