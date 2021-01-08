using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models.Contexts
{
    /// <summary>
    /// BaseContext that provides the connectionstring
    /// </summary>
    public abstract class BaseContext
    {
        public string ConnectionString { get; set; }

        public BaseContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// Gets the connection string to database
        /// </summary>
        /// <returns>The connection string</returns>
        protected MySqlConnection GetConnection()
        {
            try
            {
                return new MySqlConnection(ConnectionString);
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
    }
}
