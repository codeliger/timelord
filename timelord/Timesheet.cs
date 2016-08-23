using System.Data;
using System.Data.SQLite;

namespace timelord
{
    /// <summary>
    /// Manages the tasks database
    /// </summary>
    class Timesheet
    {
        private readonly string _filePath;
        private SQLiteConnection _sqlite;
        private SQLiteDataAdapter _adapter;
        private SQLiteCommandBuilder _builder;
        private DataTable _dataTable;
        private const string _createTaskTable = "CREATE TABLE IF NOT EXISTS task (id INTEGER PRIMARY KEY AUTOINCREMENT, description TEXT, begindate TEXT, enddate TEXT, status TEXT)";
        private const string _selectTaskQuery = "SELECT id,description,begindate,enddate,status FROM task";


        public Timesheet(string filePath)
        {
            this._filePath = filePath;

            _dataTable = new DataTable();

            OpenDatabase();

            CreateSchema();

            PrepareQueries();

            Fill();
        }

        /// <summary>
        /// Creates the database connection
        /// </summary>
        private void OpenDatabase()
        {
            _sqlite = new SQLiteConnection("Data Source=" + this._filePath + ";Version=3;");
            _sqlite.Open();
        }

        private void CreateSchema()
        {
            SQLiteCommand cmd = new SQLiteCommand(_createTaskTable, _sqlite);
            cmd.ExecuteNonQuery();
        }

        private void PrepareQueries()
        {
            _adapter = new SQLiteDataAdapter(_selectTaskQuery, _sqlite);

            _builder = new SQLiteCommandBuilder(_adapter);

            _adapter.UpdateCommand = _builder.GetUpdateCommand();
            _adapter.DeleteCommand = _builder.GetDeleteCommand();
            _adapter.InsertCommand = _builder.GetInsertCommand();
        }

        public DataTable Tasks()
        {
            return _dataTable;
        }

        public void Update()
        {
            _adapter.Update(_dataTable);
        }

        public void Fill()
        {
            _dataTable.Clear();
            _adapter.Fill(_dataTable);
        }

        /// <summary>
        /// Closes the database connection
        /// </summary>
        public void close()
        {
            _sqlite.Close();
        }
    }
}
