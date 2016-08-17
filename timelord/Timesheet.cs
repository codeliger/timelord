using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace timelord
{
    /// <summary>
    /// Manages the tasks database
    /// </summary>
    class Timesheet
    {
        private string filePath;
        private SQLiteConnection sqlite;
        private SQLiteDataAdapter adapter;
        private SQLiteCommandBuilder builder;
        private DataTable datatable;

        /// <summary>
        /// Creates a timesheet object that determines if the filepath exists.
        /// If the filepath exists then it opens a database.
        /// If it does not exist it creates a database.
        /// </summary>
        /// <param name="filePath">The path to open or create a database file at.</param>
        public Timesheet(string filePath)
        {
            this.filePath = filePath;

            if (File.Exists(filePath))
            {
                openDatabase();

            }else
            {
                createDatabase();

                createSchema();
            }

            prepareQueries();

            this.datatable = new DataTable();

            this.adapter.Fill(this.datatable);
        }

        /// <summary>
        /// Calls the methods that create the database file, connection, and schema
        /// </summary>
        private void createDatabase()
        {
            SQLiteConnection.CreateFile(filePath);

            openDatabase();
        }

        /// <summary>
        /// Creates the database connection
        /// </summary>
        private void openDatabase()
        {
            sqlite = new SQLiteConnection("Data Source=" + this.filePath + ";Version=3;");
            try
            {
                sqlite.Open();
            }
            catch(SQLiteException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Creates the schema for all tables nessicary
        /// </summary>
        private void createSchema()
        {
            string query = "CREATE TABLE task (id INTEGER PRIMARY KEY AUTOINCREMENT, description TEXT, begindate TEXT, enddate TEXT, status INTEGER default 0)";

            SQLiteCommand cmd = new SQLiteCommand(query, sqlite);

            cmd.ExecuteNonQuery();

        }

        /// <summary>
        /// Prepares the queries used to manipulate the timesheet
        /// </summary>
        private void prepareQueries()
        {
            adapter = new SQLiteDataAdapter("select id,description,begindate,enddate,status from task", sqlite);

            builder = new SQLiteCommandBuilder(adapter);

            adapter.UpdateCommand = builder.GetUpdateCommand();
            adapter.DeleteCommand = builder.GetDeleteCommand();
            adapter.InsertCommand = builder.GetInsertCommand();
        }

        /// <summary>
        /// Return the task table
        /// </summary>
        /// <returns></returns>
        public DataTable Tasks()
        {
            return this.datatable;
        }

        public void Update()
        {
            adapter.Update(datatable);
            
            datatable.Clear();

            adapter.Fill(datatable);
        }

        /// <summary>
        /// Closes the database connection
        /// </summary>
        public void close()
        {
            sqlite.Close();
        }
    }
}
