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

        /// <summary>
        /// Prepares an empty form
        /// </summary>
        /// <param name="clients">The clients table</param>
        /// <param name="selectQueryString">The select statement used for the command builder</param>
        public frmClient(SQLiteInstance db, DataTable clients, string selectQueryString, DataRowView identity=null)
        {
            InitializeComponent();
            Db = db;
            Clients = clients;
            NewClient = true;
            SelectQueryString = selectQueryString;
            if(identity != null)
            {
                txtRate.Text = identity.Row["hourly_rate"].ToString();
            }
        }

        /// <summary>
        /// Prepares a form with provided client information
        /// </summary>
        /// <param name="db">The clients table</param>
        /// <param name="clients"></param>
        /// <param name="row">The row from the databse</param>
        /// <param name="selectQueryString"></param>
        public frmClient(SQLiteInstance db, DataTable clients, DataRowView row, string selectQueryString)
        {
            InitializeComponent();
            Db = db;
            Clients = clients;

            // For some reason its created as an Int64 so it needs to be converted to an Int32
            ClientId = Convert.ToInt32(row["id"]);
            txtName.Text = row["name"].ToString();
            txtAddress.Text = row["address"].ToString();
            txtPhone.Text = row["phone"].ToString();
            txtEmail.Text = row["email"].ToString();
            txtRate.Text = row["hourly_rate"].ToString();

            SelectQueryString = selectQueryString;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (NewClient)
            {
                // clone a new row schema from client table
                Client = Clients.NewRow();

                Client["name"] = txtName.Text;
                Client["address"] = txtAddress.Text;
                Client["phone"] = txtPhone.Text;
                Client["email"] = txtEmail.Text;

                Clients.Rows.Add(Client);
                Db.Commit(Clients, SelectQueryString);
            }else
            {
                // find existing client in clients table
                Client = Clients.Rows.Find(ClientId);

                Client["name"] = txtName.Text;
                Client["address"] = txtAddress.Text;
                Client["phone"] = txtPhone.Text;
                Client["email"] = txtEmail.Text;

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
