using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace timelord
{
    class Timesheet
    {
        private string filePath;
        private SQLiteConnection sqlite;
        private SQLiteDataAdapter adapter;

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
        /// Creates the databse connection
        /// </summary>
        private void openDatabase()
        {
            sqlite = new SQLiteConnection("Data Source=" + this.filePath + ";Version=3;");

            try
            {
                sqlite.Open();

            }catch(SQLiteException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Creates the schema for all tables nessicary
        /// </summary>
        private void createSchema()
        {
            string query = "CREATE TABLE timesheet (id int primary key not null, taskname text, timeinseconds int, date text, paid integer default 0)";
            SQLiteCommand cmd = new SQLiteCommand(query, sqlite);
            cmd.ExecuteNonQuery();

        }

        /// <summary>
        /// Fills and returns a dataset with the data from the database
        /// </summary>
        /// <returns>A new dataset with the same data as the database</returns>
        public DataSet toDataSet()
        {
            DataSet data = new DataSet();
            adapter = new SQLiteDataAdapter("select id,taskname,timeinseconds,date,paid from timesheet", sqlite);
            adapter.Fill(data);
            return data;
        }

        public void Update(DataSet dataset)
        {
            this.adapter.Update(dataset);
        }

        public void close()
        {
            sqlite.Close();
        }
    }
}
