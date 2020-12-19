using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models.Contexts
{
    public class RestaurantsContext : BaseContext
    {

        public RestaurantsContext(string connectionString) : base(connectionString)
        {

        }

        private List<Restaurant> Get_Restaurants_By_Req(string req)
        {
            List<Restaurant> list = new List<Restaurant>();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(req, conn);
                    ReaderConversion convert = new ReaderConversion();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Restaurant()
                            {
                                Name = reader["name"].ToString(),
                                Phone = reader["phone"].ToString(),
                                Cuisine = reader["cuisine"].ToString(),
                                Location = convert.Location_from_reader(reader)
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }
        public List<Restaurant> GetALLRestaurants()
        {
            string req = "select distinct name,places.lat,places.lon,phone,cuisine," +
                        "city,country from restaurants join places on restaurants.lat =" +
                         " places.lat and restaurants.lon = places.lon;";
            try
            {
                return Get_Restaurants_By_Req(req);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Restaurant> Get_Restaurants_By_Region(string country, string city)
        {
            string req = "select distinct name,places.lat,places.lon,phone,cuisine," +
                        "city,country from restaurants join places on restaurants.lat =" +
                         " places.lat and restaurants.lon = places.lon" +
                         $"where city=\"{city}\" and country=\"{country}\";";
            try
            {
                return Get_Restaurants_By_Req(req);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
