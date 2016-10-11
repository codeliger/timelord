using System;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

namespace timelord
{
    public partial class frmInvoice : Form
    {
        private SQLiteInstance Db;
        private DataGridViewSelectedRowCollection Tasks;
        private DataTable Identities;
        private DataTable Clients;
        private frmClient Client;

        public frmInvoice(SQLiteInstance db, DataGridViewSelectedRowCollection tasks)
        {
            InitializeComponent();

            this.Db = db;
            this.Tasks = tasks;

            cbClient.SelectedIndexChanged += CbClient_SelectedIndexChanged;
            cbIdentity.SelectedIndexChanged += CbIdentity_SelectedIndexChanged;

            GetIdentities();
            GetClients();

            cbIdentity.DataSource = Identities;
            cbIdentity.DisplayMember = "Name";

            cbClient.DataSource = Clients;
            cbClient.DisplayMember = "Name";

            btnClientEdit.Enabled = false;
            btnClientDelete.Enabled = false;
            btnClientNew.Enabled = true;

            btnIdentityEdit.Enabled = false;
            btnIdentityDelete.Enabled = false;
            btnIdentityNew.Enabled = true;

            /// bind task stuff

        }

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

        public void GetIdentities()
        {
            Identities = Db.GetTable("Identity", QueryString.Identity.Select);
        }

        public void GetClients()
        {
            Clients = Db.GetTable("Client", QueryString.Client.Select);

        }

        private void btnIdentityEdit_Click(object sender, EventArgs e)
        {
            if (cbClient.SelectedIndex > -1)
            {
                Client = new frmClient(Db, Identities, (DataRowView)cbIdentity.SelectedItem, QueryString.Identity.Select);

                Client.ShowDialog();
                GetIdentities();
            }
        }

        private void btnClientEdit_Click(object sender, EventArgs e)
        {
            if (cbClient.SelectedIndex > -1)
            {
                Client = new frmClient(Db, Clients, (DataRowView)cbClient.SelectedItem, QueryString.Client.Select);
                Client.FormClosing += Client_FormClosing;
                Client.ShowDialog();
            }
        }

        private void btnIdentityNew_Click(object sender, EventArgs e)
        {
            Client = new frmClient(Db, Identities, QueryString.Identity.Select);
            Client.FormClosing += Identity_FormClosing;
            Client.ShowDialog();
            GetIdentities();
        }

        private void btnClientNew_Click(object sender, EventArgs e)
        {
            Client = new frmClient(Db, Clients, QueryString.Client.Select);
            Client.FormClosing += Client_FormClosing;
            Client.ShowDialog();
            GetClients();
        }

        private void Identity_FormClosing(object sender, FormClosingEventArgs e)
        {
            GetIdentities();
            cbClient.Refresh();
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            GetClients();
            cbClient.Refresh();
        }

        private void btnIdentityDelete_Click(object sender, EventArgs e)
        {
            if(cbIdentity.SelectedIndex > -1)
            {
                Identities.Rows.Find(((DataRowView)cbIdentity.SelectedValue)["id"]).Delete();
                Db.Commit(Identities, QueryString.Identity.Select);
                GetIdentities();
                cbIdentity.Refresh();
            }
        }

        private void btnClientDelete_Click(object sender, EventArgs e)
        {
            if(cbClient.SelectedIndex > -1)
            {
                Clients.Rows.Find(((DataRowView)cbClient.SelectedValue)["id"]).Delete();
                Db.Commit(Clients, QueryString.Client.Select);
                GetClients();
                cbClient.Refresh();
            }
        }
    }
}
