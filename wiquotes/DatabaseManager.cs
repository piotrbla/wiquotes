using System;
using System.Data.SQLite;

namespace wiquotes
{
    class DatabaseManager
    {
        private const string DatabaseName = "wiquotes.sqlite";
        private readonly SQLiteConnection connection;

        public static void InitFile()
        {
            SQLiteConnection.CreateFile(DatabaseName);
        }

        public void CreatTables()
        {
            string sql = "CREATE TABLE preferences (name VARCHAR(40), code varchar(20), value varchar(500))";
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
        }
        public DatabaseManager()
        {

            connection = new SQLiteConnection("Data Source=" + DatabaseName + ";Version=3;");
            connection.Open();
        }

        private void Insert(string table, string fields, string values)
        {
            string sql = "insert into " + table + " (" + fields + ") values (" + values + ")";
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        private SQLiteDataReader Select()
        {
            string sql = "select * from highscores order by score desc";
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            return reader;
            return reader;
        }
    }
}
