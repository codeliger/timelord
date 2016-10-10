using System.Data;
using System.Data.SQLite;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

namespace timelord
{
    /// <summary>
    /// Manages the tasks database
    /// </summary>
    class SQLiteInstance
    {
        private readonly string PathToDatabase;

        public SQLiteInstance(string pathToDatabase, List<string> createQueries)
        {
            this.PathToDatabase = pathToDatabase;

            // Execute each create query
            ExecuteAction(c =>
            {
                foreach(string createQuery in createQueries)
                {
                    ExecuteVoidStatement(c, createQuery);
                }
            });
    }

        private SQLiteConnection GetConnection()
        {
            return new SQLiteConnection("Data Source=" + PathToDatabase + ";Version=3;");
        }

        /// <summary>
        /// This method executes and passes a <c>SQLiteConnection</c> to a void method.
        /// </summary>
        /// <param name="action">The function to execute.</param>
        private void ExecuteAction(Action<SQLiteConnection> action)
        {
            using (SQLiteConnection c = GetConnection())
            {
                c.Open();
                action(c);
            }
        }

        /// <summary>
        /// This method executes and passes a <c>SQLiteConnection</c> to a generic method.
        /// </summary>
        /// <typeparam name="T">The generic DataType to return from the function.</typeparam>
        /// <param name="function">The function to execute.</param>
        /// <returns></returns>
        private T ExecuteFunction<T>(Func<SQLiteConnection,T> function)
        {
            using (SQLiteConnection c = GetConnection())
            {
                c.Open();
                return function(c);
            }
        }

        private void ExecuteVoidStatement(SQLiteConnection c, string databaseCreateQuery)
        {
            using (SQLiteCommand cmd = c.CreateCommand())
            {
                cmd.CommandText = databaseCreateQuery;
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Gets a standard SQLite table assuming it has a primary key of id 
        /// </summary>
        /// <returns>A datatable containing the table from the SQLite database.</returns>
        public DataTable GetTable(string tableName, string selectTaskQuery)
        {
            return ExecuteFunction(c =>
            {
                using (SQLiteDataAdapter a = new SQLiteDataAdapter(selectTaskQuery, c))
                {
                    DataTable d = new DataTable(tableName);
                    a.Fill(d);
                    //d.Columns["id"].AllowDBNull = true;
                    d.PrimaryKey = new DataColumn[] { d.Columns["id"] };
                    return d;
                }

            });
        }

        /// <summary>
        /// Commits a datatable to the sqlite database
        /// </summary>
        /// <param name="d">The datatable to commit</param>
        public void Commit(DataTable d, string selectTaskQuery)
        {
            // Execute a void action that needs a database connection
            ExecuteAction(c =>{

                using (SQLiteDataAdapter a = new SQLiteDataAdapter(selectTaskQuery, c))
                {
                    using (SQLiteCommandBuilder b = new SQLiteCommandBuilder(a))
                    {
                        a.UpdateCommand = b.GetUpdateCommand();
                        a.DeleteCommand = b.GetDeleteCommand();
                        a.InsertCommand = b.GetInsertCommand();

                        a.RowUpdated += DataAdapter_RowUpdated;

                        a.Update(d);
                    }
                }
            });
        }

        private void DataAdapter_RowUpdated(object sender, System.Data.Common.RowUpdatedEventArgs e)
        {
            if (e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert)
            {
                e.Row["id"] = ( (SQLiteConnection) e.Command.Connection).LastInsertRowId;
                e.Row.AcceptChanges();
            }
        }

    }
}
