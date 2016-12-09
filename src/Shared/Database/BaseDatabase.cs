using System;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Shared.Database
{
    public class BaseDatabase
    {
        private string _connectionString;

        private Regex _nameCheckRegex = new Regex(@"^[a-zA-Z][a-z0-9]{2,15}$", RegexOptions.Compiled);

        /// <summary>
        /// Returns a valid connection.
        /// </summary>
        public MySqlConnection Connection
        {
            get
            {
                if (_connectionString == null)
                    throw new Exception("Database has not been initialized.");

                var result = new MySqlConnection(_connectionString);
                result.Open();
                return result;
            }
        }

        /// <summary>
        /// Sets connection string and calls TestConnection.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="db"></param>
        public void Init(string host, int port, string user, string pass, string db)
        {
            _connectionString =
                $"server={host}; port={port}; database={db}; uid={user}; password={pass}; pooling=true; min pool size=0; max pool size=100; ConvertZeroDateTime=true";
            this.TestConnection();
        }

        /// <summary>
        /// Tests connection, throws on error.
        /// </summary>
        public void TestConnection()
        {
            MySqlConnection conn = null;
            try
            {
                conn = this.Connection;
            }
            finally
            {
                conn?.Close();
            }
        }
    }
}
