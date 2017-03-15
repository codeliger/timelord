using System;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;

namespace timelord
{

    /// <summary>
    /// Prepares tasks inside of a data grid view
    /// This data will then be used to generate an invoice
    /// </summary>
    public partial class frmInvoice : Form
    {
        private SQLiteInstance Db;
        private List<Task> Tasks = new List<Task>();
        private DataTable Identities;
        private DataTable Clients;
        private frmClient Client;

        public frmInvoice(SQLiteInstance db, DataGridViewSelectedRowCollection dgTasks)
        {
            InitializeComponent();

            this.Db = db;

            btnClientEdit.Enabled = false;
            btnClientDelete.Enabled = false;
            btnClientNew.Enabled = true;

            btnIdentityEdit.Enabled = false;
            btnIdentityDelete.Enabled = false;
            btnIdentityNew.Enabled = true;

            cbClient.SelectedIndexChanged += CbClient_SelectedIndexChanged;
            cbIdentity.SelectedIndexChanged += CbIdentity_SelectedIndexChanged;

            GetIdentities();
            GetClients();

            // Keep a synchronized list of tasks with extended properties between the datagrid view and database
            foreach (DataGridViewRow dgTask in dgTasks)
            {
                Task temporaryTask = new Task();

                temporaryTask.Id = (Int64) dgTask.Cells["id"].Value;
                temporaryTask.Description = dgTask.Cells["description"].Value.ToString();
                temporaryTask.BeginDate = DateTime.Parse(dgTask.Cells["begindate"].Value.ToString());
                temporaryTask.EndDate = DateTime.Parse(dgTask.Cells["enddate"].Value.ToString());

                // add to task list
                Tasks.Add(temporaryTask);
            }

            dgvInvoice.DataSource = Tasks;

        }

        /// <summary>
        /// Changes the state of comboboxes if there values or not
        /// </summary>
        private void CbIdentity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbIdentity.SelectedIndex > -1)
            {
                btnIdentityEdit.Enabled = true;
                btnIdentityDelete.Enabled = true;
            }else
            {
                btnIdentityEdit.Enabled = false;
                btnIdentityDelete.Enabled = false;
            }
        }

        private void CbClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbClient.SelectedIndex > -1)
            {
                btnClientEdit.Enabled = true;
                btnClientDelete.Enabled = true;
            }else
            {
                btnClientEdit.Enabled = false;
                btnClientDelete.Enabled = false;
            }
        }

        /// <summary>
        /// (Re)populates the identities table from the database.
        /// </summary>
        public void GetIdentities()
        {
            Identities = Db.GetTable("Identity", QueryString.Identity.Select);
            cbIdentity.DataSource = null;
            cbIdentity.DataSource = Identities;
            cbIdentity.DisplayMember = "Name";
        }

        /// <summary>
        /// (Re)populates the clients table from the database
        /// </summary>
        public void GetClients()
        {
            Clients = Db.GetTable("Client", QueryString.Client.Select);
            cbClient.DataSource = null;
            cbClient.DataSource = Clients;
            cbClient.DisplayMember = "Name";
        }

        /// <summary>
        /// 
        /// </summary>
        private void btnIdentityEdit_Click(object sender, EventArgs e)
        {
            if (cbIdentity.SelectedIndex > -1)
            {
                Client = new frmClient(Db, Identities, (DataRowView) cbIdentity.SelectedItem, QueryString.Identity.Select);
                Client.ShowDialog();
                GetIdentities();
            }
        }

        private void btnClientEdit_Click(object sender, EventArgs e)
        {
            if (cbClient.SelectedIndex > -1)
            {
                Client = new frmClient(Db, Clients, (DataRowView)cbClient.SelectedItem, QueryString.Client.Select);
                Client.ShowDialog();
                GetClients();
            }
        }

        private void btnIdentityNew_Click(object sender, EventArgs e)
        {
            Client = new frmClient(Db, Identities, QueryString.Identity.Select);
            Client.ShowDialog();
            GetIdentities();
        }

        private void btnClientNew_Click(object sender, EventArgs e)
        {
            Client = new frmClient(Db, Clients, QueryString.Client.Select, (DataRowView)cbIdentity.SelectedItem);
            Client.ShowDialog();
            GetClients();
        }

        /// <summary>
        /// When an identity is deleted from the form:
        ///     Delete row from table
        ///     Commit changes to database
        ///     Repopulate identities table
        /// </summary>
        private void btnIdentityDelete_Click(object sender, EventArgs e)
        {
            if(cbIdentity.SelectedIndex > -1)
            {
                Identities.Rows.Find(((DataRowView)cbIdentity.SelectedValue)["id"]).Delete();
                Db.Commit(Identities, QueryString.Identity.Select);
                GetIdentities();
            }
        }

        /// <summary>
        /// When an client is deleted from the form:
        ///     Delete row from table
        ///     Commit changes to database
        ///     Repopulate clients table
        /// </summary>
        private void btnClientDelete_Click(object sender, EventArgs e)
        {
            if(cbClient.SelectedIndex > -1)
            {
                Clients.Rows.Find(((DataRowView)cbClient.SelectedValue)["id"]).Delete();
                Db.Commit(Clients, QueryString.Client.Select);
                GetClients();
            }
        }
    }
}
