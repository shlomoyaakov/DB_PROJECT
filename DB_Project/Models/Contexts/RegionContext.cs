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

        public List<KeyValuePair<string, Int64>> Get_Amount_By_Country()
        {
            List<KeyValuePair<string, Int64>> list = new List<KeyValuePair<string, Int64>>();
            string req = "select country, count(country) as amount from trip_region " +
                        "group by country " +
                        $"ORDER  by amount DESC;";
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(req, conn);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        KeyValuePair<string, Int64> kv = new KeyValuePair<string, Int64>(reader["country"].ToString()
                            , (Int64)reader["amount"]);
                        list.Add(kv);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }

        public List<KeyValuePair<string, int>> Get_Amount_By_City(string country)
        {
            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
            string req = "select city, count(city) as amount from trip_region " +
                        $"where country = \"{country}\"" +
                        "group by city " +
                        $"ORDER  by amount DESC;";
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(req, conn);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        KeyValuePair<string, int> kv = new KeyValuePair<string, int>(reader["city"].ToString()
                            , (int)reader["amount"]);
                        list.Add(kv);
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
