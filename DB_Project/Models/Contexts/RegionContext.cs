using System;
using System.Collections.Generic;
using DB_Project.Models.Contexts;
using DB_Project.Models.Data_Class;
using MySql.Data.MySqlClient;

namespace DB_Project.Models
{
    /// <summary>
    /// RegionContext responsible to the communication with
    /// the region table in the database.
    /// </summary>
    public class RegionContext : BaseContext
    {

        public RegionContext(string connectionString) : base(connectionString) {}


        /// <summary>
        /// Gets a list of region according to the sql request
        /// </summary>
        /// <param name="req">the sql request</param>
        /// <param name="country"> wether to initallize country value
        /// in the region</param>
        /// <param name="city"> wether to initalize country value
        /// in the region</param>
        /// <returns>list of region according to request</returns>
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

        /// <summary>
        /// Gets all of the countries from the database
        /// </summary>
        /// <returns>A list that contains all of the countries in the database
        /// here the region only include countries</returns>
        public List<Region> Get_All_Countries()
        {
            string req = "select DISTINCT country from region;";
            try
            {
                return Get_Region_By_Req(req,true,false);
            }
            catch (Exception)
            {
                throw new Exception("There was a problem while trying to get all countries");
            }
        }


        /// <summary>
        /// Get all of the cities from a certain country
        /// </summary>
        /// <param name="country">The country name</param>
        /// <returns>A list of region from that country
        /// Here the region contains city and country</returns>
        public List<Region> Get_All_Cities_In_Country(string country)
        {
            string req = "select DISTINCT country,city from region " +
                         $"where country=\"{country}\";";
            try
            {
                return Get_Region_By_Req(req);
            }
            catch (Exception)
            {
                throw new Exception($"There was a problem while trying to get cities from {country}");
            }
        }

        /// <summary>
        /// Initialize the dictionary that maps between type to the amount of different types in the
        /// database.
        /// The values for type are trips/attraction/accommodation/restaurants.
        /// So if type is attraction it will initiallize "attractions" : X
        /// when x is the number of different attractions in the return table from the sql req
        /// </summary>
        /// <param name="request">The sql request</param>
        /// <param name="stats_map">The dictionary that maps between type to the amount of 
        /// different types</param>
        /// <param name="conn">Mysqlconnections</param>
        /// <param name="type">string that can have the values:
        /// trips/attraction/accommodation/restaurants</param>
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
                    //creating the map between type to the amount
                    stats_map[city].Data[type] = (Int64)reader["amount"];
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Get stats according to the requests
        /// </summary>
        /// <param name="att_req"> the sql request that involves the attractions table</param>
        /// <param name="rest_req"> the sql request that involves the restaurants table</param>
        /// <param name="acc_req">the sql request that involves the accommodation table</param>
        /// <param name="trips_req">the sql request that involves table that related to trips</param>
        /// <returns>A list of stats object</returns>
        private List<Stats> Get_Activities_By_Req(string att_req, string rest_req, string acc_req, string trips_req)
        {
            List<Stats> list = new List<Stats>();
            Dictionary<string, Stats> stats_map = new Dictionary<string, Stats>();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    //here we gets stats per each table
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

        /// <summary>
        /// Gets stats per each city in the country.
        /// that includes the number of trips, restaurants, accommodation, and attractions
        /// in that region.
        /// </summary>
        /// <param name="country">The country that we want to get the stats from</param>
        /// <returns>A list of stat on that region</returns>
        public List<Stats> Get_Stats_Per_Region(string country)
        {
            List<Stats> ret_list;
            // the requests
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
            catch (Exception)
            {
                throw new Exception($"There was a problem while trying to get stats per each city in {country}");
            }
        }
    }
}
