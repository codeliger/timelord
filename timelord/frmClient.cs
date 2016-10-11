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
    public partial class frmClient : Form
    {
        private DataRow Client;
        private DataTable Clients;
        private SQLiteInstance Db;
        private int ClientId;
        private bool NewClient = false;
        private string SelectQueryString;

        public frmClient(SQLiteInstance db, DataTable clients, string selectQueryString)
        {
            InitializeComponent();
            Db = db;
            Clients = clients;
            NewClient = true;
            SelectQueryString = selectQueryString;
        }

        public frmClient(SQLiteInstance db, DataTable clients, DataRowView row, string selectQueryString)
        {
            InitializeComponent();
            Db = db;
            Clients = clients;

            // For some reason its created as an Int64 so it needs to be converted to an Int32
            ClientId = Convert.ToInt32(row["id"]);
            txtName.Text = row["Name"].ToString();
            txtAddress.Text = row["Address"].ToString();
            txtPhone.Text = row["Phone"].ToString();
            txtEmail.Text = row["Email"].ToString();

            SelectQueryString = selectQueryString;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (NewClient)
            {
                // clone a new row schema from client table
                Client = Clients.NewRow();

                Client["Name"] = txtName.Text;
                Client["Address"] = txtAddress.Text;
                Client["Phone"] = txtPhone.Text;
                Client["Email"] = txtEmail.Text;

                Clients.Rows.Add(Client);
                Db.Commit(Clients, SelectQueryString);
            }else
            {
                // find existing client in clients table
                Client = Clients.Rows.Find(ClientId);

                Client["Name"] = txtName.Text;
                Client["Address"] = txtAddress.Text;
                Client["Phone"] = txtPhone.Text;
                Client["Email"] = txtEmail.Text;

                Db.Commit(Clients, SelectQueryString);
            }
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
