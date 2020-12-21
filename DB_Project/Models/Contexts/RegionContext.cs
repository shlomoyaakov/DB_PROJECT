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

  

        private List<Region> Get_Region_By_Req(string req,Boolean country=true,Boolean city=true)
        {
            List<Region> list = new List<Region>();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(req, conn);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Region reg = new Region();
                        if (city)
                            reg.City = reader["city"].ToString();
                        if (country)
                            reg.Country = reader["country"].ToString();
                        list.Add(reg);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
        public List<Region> GetAllRegions()
        {
            string req = "select DISTINCT * from region;";
            try
            {
                return Get_Region_By_Req(req);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public List<Region> Get_All_Countries()
        {
            string req = "select DISTINCT country from region;";
            try
            {
                return Get_Region_By_Req(req,true,false);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Region> Get_All_Cities_In_Country(string country)
        {
            string req = "select DISTINCT country,city from region " +
                         $"where country=\"{country}\";";
            try
            {
                return Get_Region_By_Req(req);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
