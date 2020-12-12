using System;
using System.Collections.Generic;
using DB_Project.Models.Contexts;
using MySql.Data.MySqlClient;

namespace DB_Project.Models
{
    public class RegionContext : BaseContext
    {

        public RegionContext(string connectionString) : base(connectionString)
        {
        }


        public List<Region> GetAllRegions()
        {
            List<Region> list = new List<Region>();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("select DISTINCT * from region", conn);
                    using var reader = cmd.ExecuteReader();
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
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
    }
}
