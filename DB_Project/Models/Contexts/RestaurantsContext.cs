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
                         " places.lat and restaurants.lon = places.lon " +
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

        public void Add_Restaurant(Restaurant restaurant)
        {
            string country = restaurant.Location.General_Location.Country;
            string city = restaurant.Location.General_Location.City;
            double lat = restaurant.Location.Coordinates.Latitude;
            double lon = restaurant.Location.Coordinates.Longitude;
            try
            {
                using (MySqlConnection myConnection = GetConnection())
                {
                    myConnection.Open();
                    MySqlCommand myCommand = myConnection.CreateCommand();
                    MySqlTransaction myTrans;
                    myTrans = myConnection.BeginTransaction();
                    myCommand.Connection = myConnection;
                    myCommand.Transaction = myTrans;
                    try
                    {
                        myCommand.CommandText = $"Insert into region (country, city) VALUES (\"{country}\", \"{city}\")" +
                            $" ON DUPLICAT" +
                            $"E KEY UPDATE city=city,country=country;";
                        myCommand.ExecuteNonQuery();
                        myCommand.CommandText = $"Insert into places (lat, lon, country, city) VALUES ({lat}, {lon}, \"{country}\", \"{city}\")" +
                            $"ON DUPLICATE KEY UPDATE city=city,country=country;";
                        myCommand.ExecuteNonQuery();
                        myCommand.CommandText = $"Insert into attractions (name, lat, lon, phone, cuisine) VALUES (\"{restaurant.Name}\", {lat}, {lon}, \"{restaurant.Phone}\", \"{restaurant.Cuisine}\");";
                        myCommand.ExecuteNonQuery();
                        myTrans.Commit();
                    }
                    catch (MySqlException ex)
                    {
                        myTrans.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
