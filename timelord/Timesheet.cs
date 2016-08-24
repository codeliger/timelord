using System.Data;
using System.Data.SQLite;
using System;

namespace timelord
{
    /// <summary>
    /// Manages the tasks database
    /// </summary>
    class Timesheet
    {
        private readonly string _filePath;
        private const string createTaskTable = "CREATE TABLE IF NOT EXISTS task (id INTEGER PRIMARY KEY AUTOINCREMENT, description TEXT, begindate TEXT, enddate TEXT, status TEXT)";
        private const string selectTaskQuery = "SELECT id,description,begindate,enddate,status FROM task";


        public Timesheet(string filePath)
        {
            this._filePath = filePath;

            ExecuteAction(c => CreateSchema(c));
        }

        private SQLiteConnection GetConnection()
        {
            return new SQLiteConnection("Data Source=" + this._filePath + ";Version=3;");
        }

        private void ExecuteAction(Action<SQLiteConnection> action)
        {
            using (SQLiteConnection c = GetConnection())
            {
                c.Open();

                action(c);
            }
        }

        private T ExecuteFunction<T>(Func<SQLiteConnection,T> function)
        {
            using (SQLiteConnection c = GetConnection())
            {
                c.Open();

                return function(c);
            }
        }

        private T ExecuteCommand<T>(Func<SQLiteDataAdapter, T> function)
        {
            return ExecuteFunction(c =>
            {
                using (SQLiteDataAdapter a = new SQLiteDataAdapter(selectTaskQuery, c))
                {
                    using (SQLiteCommandBuilder b = new SQLiteCommandBuilder(a))
                    {
                        a.UpdateCommand = b.GetUpdateCommand();
                        a.DeleteCommand = b.GetDeleteCommand();
                        a.InsertCommand = b.GetInsertCommand();

                        return function(a);
                    }
                }
            });
        }

        private void CreateSchema(SQLiteConnection c)
        {
            using (SQLiteCommand cmd = c.CreateCommand())
            {
                cmd.CommandText = createTaskTable;
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable Tasks()
        {
            return ExecuteCommand(a =>
            {
                DataTable d = new DataTable("task");
                a.Fill(d);
                return d;
            });
        }

        public int Commit(DataTable d)
        {
            return ExecuteCommand(a =>{
                return a.Update(d);
            });
        }

    }
}
