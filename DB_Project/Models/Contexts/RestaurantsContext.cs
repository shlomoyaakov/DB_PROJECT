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

        public List<Restaurant> Get_Restaurants_By_Req(string req)
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
                                ID = (int)reader["id"],
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
            string req = "select distinct id,name,places.lat,places.lon,phone,cuisine," +
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
            string req = "select distinct t1.id,t1.name,t1.lat,t1.lon, country, city, t1.phone, t1.cuisine" +
                        " from restaurants as t1 join places as t2 on t1.lat =" +
                         " t2.lat and t1.lon = t2.lon " +
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

        public List<Restaurant> Get_Restaurants_By_Region_And_User(string country, string city, string user_name)
        {
            string request = "select distinct t4.id,t4.name, t4.lat, t4.lon, country, city, t4.phone, t4.cuisine " +
                "from Restaurants as t4 join places as t5 " +
                "on t4.lat = t5.lat and t4.lon = t5.lon " +
                $"where country=\"{country}\" and city=\"{city}\" and NOT EXISTS (select distinct t1.id,t1.name, t1.lat, t1.lon, country, city, t1.phone, t1.cuisine " +
                "from users_trips as t3 join trip_Restaurants as t2 " +
                "on t2.trip_id = t3.trip_id " +
                "join Restaurants as t1 " +
                "on t2.restaurant_id = t1.id join places as t0 " +
                "on t0.lat = t1.lat and t0.lon = t1.lon " +
                $"where user_name=\"{user_name}\");";
            try
            {
                return Get_Restaurants_By_Req(request);
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
                        myCommand.CommandText = $"Insert into restaurants (name, lat, lon, phone, cuisine) VALUES (\"{restaurant.Name}\", {lat}, {lon}, \"{restaurant.Phone}\", \"{restaurant.Cuisine}\");";
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
