﻿using System.Data;
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
        
        public DataSet dataset
        {
            get;
            private set;
        }

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

            this.dataset = new DataSet();

            this.adapter.Fill(this.dataset);
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
            string query = "CREATE TABLE timesheet (id INTEGER PRIMARY KEY AUTOINCREMENT, taskname TEXT, timeinseconds INTEGER, date TEXT, paid INTEGER default 0)";

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
        /// Tells the data adapter to update the database to match the dataset
        /// </summary>
        /// <param name="dataset"></param>
        public void synchronizeDataSetWithDatabase()
        {
            this.adapter.Update(this.dataset);

            // Empty the dataset
            this.dataset.Clear();

            // Fill the dataset so it contains the same data as the database
            this.adapter.Fill(this.dataset);
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
