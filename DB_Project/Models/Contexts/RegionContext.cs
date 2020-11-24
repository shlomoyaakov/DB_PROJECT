using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DB_Project.Models
{
    public class RegionContext
    {
        public string ConnectionString { get; set; }

        public RegionContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Region> GetAllRegions()
        {
            List<Region> list = new List<Region>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select DISTINCT * from region", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Region()
                        {
                            City = reader["city"].ToString(),
                            Country = reader["country"].ToString()
                        });
                    }
                }
            }
            return list;
        }
    }
}
