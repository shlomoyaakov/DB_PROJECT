using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models.Contexts
{
    public abstract class BaseContext
    {
        public string ConnectionString { get; set; }

        public BaseContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
