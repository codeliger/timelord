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
            }

            prepareQueries();

        }

        /// <summary>
        /// Calls the methods that create the database file, connection, and schema
        /// </summary>
        private void createDatabase()
        {
            SQLiteConnection.CreateFile(filePath);

            openDatabase();

            createSchema();
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
                MessageBox.Show(e.Message + e.StackTrace);
            }
        }

        /// <summary>
        /// Creates the schema for all tables nessicary
        /// </summary>
        private void createSchema()
        {
            string query = "CREATE TABLE timesheet (id int primary key, taskname text, timeinseconds int, date text, paid integer default 0)";

            SQLiteCommand cmd = new SQLiteCommand(query, sqlite);

            cmd.ExecuteNonQuery();

        }

        /// <summary>
        /// Prepares the queries used to manipulate the timesheet
        /// </summary>
        private void prepareQueries()
        {
            adapter = new SQLiteDataAdapter("select id,taskname,timeinseconds,date,paid from timesheet", sqlite);

            builder = new SQLiteCommandBuilder(adapter);

            adapter.UpdateCommand = builder.GetUpdateCommand();
            adapter.DeleteCommand = builder.GetDeleteCommand();
            adapter.InsertCommand = builder.GetInsertCommand();
        }

        /// <summary>
        /// Fills and returns a dataset with the data from the database
        /// </summary>
        /// <returns>A new dataset with the same data as the database</returns>
        public DataSet toDataSet()
        {
            DataSet data = new DataSet();

            this.adapter.Fill(data);

            return data;
        }

        /// <summary>
        /// Updates the dataset
        /// </summary>
        /// <param name="dataset"></param>
        public void Update(DataSet dataset)
        {
            this.adapter.Update(dataset);
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
