using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace DB_Project.Models.Contexts
{
    /// <summary>
    /// RestaurantsContext responsible to the communication with
    /// the restaurants table in the database.
    /// </summary>
    public class RestaurantsContext : BaseContext
    {

        public RestaurantsContext(string connectionString) : base(connectionString)
        {

        }

        /// <summary>
        /// Gets a list of Restaurant according to the  sql request
        /// </summary>
        /// <param name="req">sql query</param>
        /// <returns>list of Restaurant according to request</returns>
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
                        //Here we add the accommodation that mysql gave us
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

        /// <summary>
        /// Gets the restaurants from a certain city and country
        /// </summary>
        /// <param name="country">The country name</param>
        /// <param name="city">The city name</param>
        /// <returns>A list of Restaurant from city,country</returns>
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
            catch (Exception)
            {
                throw new Exception("There was a problem while trying to recieve restaurants" +
                    $"from {city} ,{country}");
            }
        }

        /// <summary>
        /// Gets all the restaurants from certain city and country that 
        /// the user hasent been yet.
        /// </summary>
        /// <param name="country">The country name</param>
        /// <param name="city">The city name</param>
        /// <param name="user_name">The user name</param>
        /// <returns>A list of Restaurant from the city,region that user hasn't been yet</returns>
        public List<Restaurant> Get_Restaurants_By_Region_And_User(string country, string city, string user_name)
        {
            string request = "select distinct t4.id,t4.name, t4.lat, t4.lon, country, city, t4.phone, t4.cuisine " +
                "from Restaurants as t4 join places as t5 " +
                "on t4.lat = t5.lat and t4.lon = t5.lon " +
                $"where country=\"{country}\" and city=\"{city}\" and t4.id NOT in " +
               "(select distinct t2.restaurant_id " +
               "from users_trips as t3 join trip_restaurants as t2 " +
                $"where user_name=\"{user_name}\");";
            try
            {
                return Get_Restaurants_By_Req(request);
            }
            catch (Exception)
            {
                throw new Exception("There was a problem while trying to recieve accommodation" +
                    $"from {city} ,{country}");
            }
        }

        /// <summary>
        /// Add new restaurant to Restaurant table
        /// </summary>
        /// <param name="restaurant">The new restaurant that we want to add</param>
        public void Add_Restaurant(Restaurant restaurant)
        {
            string country = restaurant.Location.General_Location.Country;
            string city = restaurant.Location.General_Location.City;
            double lat = restaurant.Location.Coordinates.Latitude;
            double lon = restaurant.Location.Coordinates.Longitude;
            try
            {
                //We use transaction because we also have to insert values to region and places,
                // the restaurants parents
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
                        // first we insert values to region in case of new city/country
                        myCommand.CommandText = $"Insert into region (country, city) VALUES (\"{country}\", \"{city}\")" +
                            $" ON DUPLICAT" +
                            $"E KEY UPDATE city=city,country=country;";
                        myCommand.ExecuteNonQuery();
                        //then we try to insert values to places in case of new coordinates
                        myCommand.CommandText = $"Insert into places (lat, lon, country, city) VALUES ({lat}, {lon}, \"{country}\", \"{city}\")" +
                            $"ON DUPLICATE KEY UPDATE city=city,country=country;";
                        myCommand.ExecuteNonQuery();
                        //finally we insert the new values to restaurants table
                        myCommand.CommandText = $"Insert into restaurants (name, lat, lon, phone, cuisine)" +
                            $" VALUES (\"{restaurant.Name}\", {lat}, {lon}, \"{restaurant.Phone}\", \"{restaurant.Cuisine}\");";
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
            catch (Exception)
            {
                throw new Exception("There was a problem while to add new restaurants");
            }
        }


        /// <summary>
        /// Gets the amount of trips that each restaurant was involved
        /// </summary>
        /// <param name="country">The country name</param>
        /// <param name="city">The city name</param>
        /// <returns>keyvaluepair when the key is the restaurant id
        /// and the values is the amount of trips that this restaurant was involved </returns>
        public List<KeyValuePair<int, Int64>> Get_Amount_By_Region(string country, string city)
        {
            List<KeyValuePair<int, Int64>> list = new List<KeyValuePair<int, Int64>>();
            string req = "select restaurant_id, count(restaurant_id) as amount from trip_region " +
                        "join trip_restaurants " +
                        "on trip_restaurants.trip_id = trip_region.trip_id " +
                        $"where country = \"{country}\" and city = \"{city}\" " +
                        "group by restaurant_id " +
                        "ORDER by amount DESC; ";
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(req, conn);
                    using var reader = cmd.ExecuteReader();
                    // here we genereate the keyvalue pair for each restaurant
                    while (reader.Read())
                    {
                        KeyValuePair<int, Int64> kv = new KeyValuePair<int, Int64>((int)reader["restaurant_id"]
                            , (Int64)reader["amount"]);
                        list.Add(kv);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("There was problem while trying to get the amount of trips that" +
                    "each restaurant was involved");
            }
            return list;
        }

        /// <summary>
        /// Delte a certain restaurant from Restaurant table
        /// </summary>
        /// <param name="ret"> The restaurant that we want to delete</param>
        public void Delete(Restaurant ret)
        {
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
                        //deleting the restaurants by its id
                        myCommand.CommandText = $"delete from Restaurants where id={ret.ID};";
                        myCommand.ExecuteNonQuery();
                        // in case we updated the location we try to remove the previous location
                        // if there is no use of the previous location it will be deleted.
                        myCommand.CommandText = "delete ignore from places where " +
                                                $"lat={ret.Location.Coordinates.Latitude} and " +
                                                $"lon = {ret.Location.Coordinates.Longitude};";
                        myCommand.ExecuteNonQuery();
                        myCommand.CommandText = "delete ignore from region where exists (select country,city " +
                                    $"from Restaurants as t1 join places as t2 " +
                                    $"on t1.lat = t2.lat and t2.lon=t1.lon " +
                                    $"where id={ret.ID});";
                        myCommand.ExecuteNonQuery();
                        myTrans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        myTrans.Rollback();
                        throw e;
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("There was a problem while trying to delete the restaurant");
            }
        }

        /// <summary>
        /// updates the values of a certain restaurant
        /// </summary>
        /// <param name="prev_ret">The previous restaurant</param>
        /// <param name="new_ret">The new restaurant with new values</param>
        public void Update(Restaurant prev_ret, Restaurant new_ret)
        {
            int id = prev_ret.ID;
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
                        // in case the update includes new city and country
                        myCommand.CommandText = $"Insert into region (country, city) VALUES " +
                            $"(\"{new_ret.Location.General_Location.Country}\", " +
                            $"\"{new_ret.Location.General_Location.City}\") " +
                            $" ON DUPLICATE " +
                            $" KEY UPDATE city=city,country=country;";
                        myCommand.ExecuteNonQuery();
                        // in case the update include new latitude and longiutde
                        myCommand.CommandText = "delete ignore from places where " +
                                                $"lat={prev_ret.Location.Coordinates.Latitude} and " +
                                                $"lon = {prev_ret.Location.Coordinates.Longitude};";
                        myCommand.ExecuteNonQuery();
                        //updating the values
                        myCommand.CommandText = $"UPDATE restaurants SET name = \"{new_ret.Name}\"," +
                                     $"lat = {new_ret.Location.Coordinates.Latitude}, lon = {new_ret.Location.Coordinates.Longitude}," +
                                     $"phone = \"{new_ret.Phone}\", cuisine =\"{new_ret.Cuisine}\" " +
                                     $"WHERE id = {id};";
                        myCommand.ExecuteNonQuery();
                        // in case we updated the location we try to remove the previous location
                        // if there is no use of the previous location it will be deleted.
                        myCommand.CommandText = "delete ignore from places where " +
                                                $"lat={prev_ret.Location.Coordinates.Latitude} and " +
                                                $"lon = {prev_ret.Location.Coordinates.Longitude};";
                        myCommand.ExecuteNonQuery();
                        myCommand.CommandText = "delete ignore from region where exists (select country,city " +
                                    $"from Restaurants as t1 join places as t2 " +
                                    $"on t1.lat = t2.lat and t2.lon=t1.lon " +
                                    $"where id={id});";
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
            catch (Exception)
            {
                throw new Exception("There was a problem while trying to update the accommodation");
            }
        }
    }
}
