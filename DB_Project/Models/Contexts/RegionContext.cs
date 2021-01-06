using System;
using System.Collections.Generic;
using DB_Project.Models.Contexts;
using DB_Project.Models.Data_Class;
using MySql.Data.MySqlClient;

namespace DB_Project.Models
{
    public class RegionContext : BaseContext
    {

        public RegionContext(string connectionString) : base(connectionString) {}

  

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

        private void Get_Data_Per_Region_By_Req(string request, Dictionary<string, Stats> stats_map,
            MySqlConnection conn, string type)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(request, conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string city = reader["city"].ToString();
                    if (!stats_map.ContainsKey(city))
                    {
                        Region reg = new Region()
                        {
                            City = city
                        };
                        stats_map[city] = new Stats()
                        {
                            General_Location = reg
                        };
                    }
                    stats_map[city].Data[type] = (Int64)reader["amount"];
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        private List<Stats> Get_Activities_By_Req(string att_req, string rest_req, string acc_req, string trips_req)
        {
            List<Stats> list = new List<Stats>();
            Dictionary<string, Stats> stats_map = new Dictionary<string, Stats>();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    Get_Data_Per_Region_By_Req(rest_req, stats_map, conn,"Restaurants");
                    Get_Data_Per_Region_By_Req(att_req, stats_map, conn, "Attractions");
                    Get_Data_Per_Region_By_Req(acc_req, stats_map, conn, "Accommodation");
                    Get_Data_Per_Region_By_Req(trips_req, stats_map, conn, "Trips");
                }
                foreach (KeyValuePair<string, Stats> entry in stats_map)
                {
                    list.Add(entry.Value);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }

        public List<Stats> Get_Stats_Per_Region(string country)
        {
            List<Stats> ret_list;
            string att_req = "select city, count(city) as amount from attractions join places on attractions.lat = " +
                        "places.lat and attractions.lon = places.lon " +
                        $"where country = \"{country}\" " +
                        "group by city " +
                        $"ORDER  by amount DESC;";

            string rest_req = "select city, count(city) as amount from restaurants join places on restaurants.lat = " +
                         "places.lat and restaurants.lon = places.lon " +
                         $"where country = \"{country}\" " +
                         "group by city " +
                         $"ORDER  by amount DESC;";

            string acc_req = "select city, count(city) as amount from accommodation join places on accommodation.lat = " +
                         "places.lat and accommodation.lon = places.lon " +
                         $"where country = \"{country}\" " +
                         "group by city " +
                         $"ORDER  by amount DESC;";

            string trips_req = "select city, count(city) as amount from trip_region " +
                        $"where country = \"{country}\"" +
                        "group by city " +
                        $"ORDER  by amount DESC;";

            try
            {
                ret_list = Get_Activities_By_Req(att_req, rest_req, acc_req, trips_req);
                foreach(Stats s in ret_list)
                {
                    s.General_Location.Country = country;
                }
                return ret_list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
