using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timelord
{
    class TaskContextMenu : ContextMenuStrip
    {

        DataGridView dgv;

        public TaskContextMenu(DataGridView dgv)
        {
            this.dgv = dgv;

            this.Items.Add("Delete");

            this.Items[0].Click += TaskContextMenu_Click;

        }

        private void TaskContextMenu_Click(object sender, EventArgs e)
        {
            string message;
            string title;

            if (dgv.SelectedRows.Count > 1)
            {
                message = "these tasks?";
                title = "Delete Tasks";
            }
            else
            {
                message = "this task?";
                title = "Delete Task";
            }

            if (DialogResult.Yes == MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            {

            }
        }
    }
}
