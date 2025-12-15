using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCAIS
{
    public sealed class DatabaseConnection
    {
        private static readonly DatabaseConnection _instance = new DatabaseConnection();

        private SqlConnection _connection;

        private DatabaseConnection()
        {
            string connString = Properties.Settings.Default.SCAISDBConnectionString;
            _connection = new SqlConnection(connString);
        }

        public static DatabaseConnection Instance
        {
            get { return _instance; }
        }

        public SqlConnection GetConnection()
        {
            return _connection;
        }

        public void Open()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public void Close()
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
            {
                _connection.Close();
            }
        }
    }
}
