using System.Windows.Forms;

namespace timelord
{
    public class EditMaster : Form {

        public virtual string getValue()
        {
            return "";
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // EditMaster
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "EditMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }
    }
}
