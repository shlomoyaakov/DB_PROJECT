using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models.Contexts
{
    /// <summary>
    /// AttractionsContext responsible to the communication with
    /// the attraction table in the database.
    /// </summary>
    public class AttractionsContext : BaseContext
    {

        public AttractionsContext(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Gets a list of Attraction according to the  sql request
        /// </summary>
        /// <param name="request">sql query</param>
        /// <returns>list of Attraction according to request</returns>
        public List<Attraction> Get_Attractions_By_Req(string request)
        {
            List<Attraction> list = new List<Attraction>();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(request, conn);
                    ReaderConversion convert = new ReaderConversion();

                    //Here we add the accommodation that mysql gave us
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Attraction()
                            {
                                ID = (int)reader["id"],
                                Name = reader["name"].ToString(),
                                Phone = reader["phone"].ToString(),
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
        /// Gets the Attraction from a certain city and country
        /// </summary>
        /// <param name="country">The country name</param>
        /// <param name="city">The city name</param>
        /// <returns>A list of Attraction from city,country</returns>
        public List<Attraction> Get_Attractions_By_Region(string country,string city)
        {
            string request = "select distinct id,name,places.lat,places.lon,phone" +
                        ",city,country from attractions join places on attractions.lat = " +
                        "places.lat and attractions.lon = places.lon " +
                        $"where country=\"{country}\" and city=\"{city}\";";
            try
            {
                return Get_Attractions_By_Req(request);
            }
            catch (Exception)
            {
                throw new Exception("There was a problem while trying to recieve attraction" +
                    $"from {city} ,{country}");
            }

        }

        /// <summary>
        /// Gets all the Attraction from certain city and country that 
        /// the user hasent been yet.
        /// </summary>
        /// <param name="country">The country name</param>
        /// <param name="city">The city name</param>
        /// <param name="user_name">The user name</param>
        /// <returns>A list of Attraction from the city,region that user hasn't been yet</returns>
        public List<Attraction> Get_Attractions_By_Region_And_User(string country, string city, string user_name)
        {
            string request = "select distinct t4.id,t4.name, t4.lat, t4.lon, country, city, t4.phone " +
                "from attractions as t4 join places as t5 " +
                "on t4.lat = t5.lat and t4.lon = t5.lon " +
               $"where country=\"{country}\" and city=\"{city}\" and t4.id NOT in " +
               "(select distinct t2.attraction_id " +
               "from users_trips as t3 join trip_attractions as t2 " +
                $"where user_name=\"{user_name}\");";
            try
            {
                return Get_Attractions_By_Req(request);
            }
            catch (Exception)
            {
                throw new Exception("There was a problem while trying to recieve accommodation" +
                    $"from {city} ,{country}");
            }
        }

        /// <summary>
        /// Add new Attraction to Attraction table
        /// </summary>
        /// <param name="attraction">The new Attraction that we want to add</param>
        public void Add_Attraction(Attraction attraction)
        {
            string country = attraction.Location.General_Location.Country;
            string city = attraction.Location.General_Location.City;
            double lat = attraction.Location.Coordinates.Latitude;
            double lon = attraction.Location.Coordinates.Longitude;
            try
            {
                //We use transaction because we also have to insert values to region and places,
                // the attractions parents
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
                        //finally we insert the new values to attractions table
                        myCommand.CommandText = $"Insert into attractions (name, lat, lon, phone) VALUES (\"{attraction.Name}\", {lat}, {lon}, \"{attraction.Phone}\");";
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
                throw new Exception("There was a problem while to add new attraction");
            }
        }

        /// <summary>
        /// Gets the amount of trips that each attraction was involved
        /// </summary>
        /// <param name="country">The country name</param>
        /// <param name="city">The city name</param>
        /// <returns>keyvaluepair when the key is the attraction id
        /// and the values is the amount of trips that this attraction was involved </returns>
        public List<KeyValuePair<int, Int64>> Get_Amount_By_Region(string country, string city)
        {
            List<KeyValuePair<int, Int64>> list = new List<KeyValuePair<int, Int64>>();
            string req = "select attraction_id, count(attraction_id) as amount from trip_region "+
                        "join trip_attractions " +
                        "on trip_attractions.trip_id = trip_region.trip_id " +
                        $"where country = \"{country}\" and city = \"{city}\" "+
                        "group by attraction_id " +
                        "ORDER by amount DESC; ";
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(req, conn);
                    using var reader = cmd.ExecuteReader();
                    // here we genereate the keyvalue pair for each attraction
                    while (reader.Read())
                    {
                        KeyValuePair<int, Int64> kv = new KeyValuePair<int, Int64>((int)reader["attraction_id"]
                            , (Int64)reader["amount"]);
                        list.Add(kv);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("There was problem while trying to get the amount of trips that" +
                    "each attraction was involved");
            }
            return list;
        }


        /// <summary>
        /// Delte a certain attraction from Attraction table
        /// </summary>
        /// <param name="att"> The attraction that we want to delete</param>
        public void Delete(Attraction att)
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
                        //deleting the attraction by its id
                        myCommand.CommandText = $"delete from attractions where id={att.ID};";
                        myCommand.ExecuteNonQuery();
                        // in case we updated the location we try to remove the previous location
                        // if there is no use of the previous location it will be deleted.
                        myCommand.CommandText = "delete ignore from places where " +
                                                $"lat={att.Location.Coordinates.Latitude} and " +
                                                $"lon = {att.Location.Coordinates.Longitude};";
                        myCommand.ExecuteNonQuery();
                        myCommand.CommandText = "delete ignore from region where exists (select country,city " +
                                    $"from attractions as t1 join places as t2 " +
                                    $"on t1.lat = t2.lat and t2.lon=t1.lon " +
                                    $"where id={att.ID});";
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
                throw new Exception("There was a problem while trying to delete the attraction");
            }
        }

        /// <summary>
        /// updates the values of a certain attraction
        /// </summary>
        /// <param name="prev_att">The previous attraction</param>
        /// <param name="new_att">The new attraction with new values</param>
        public void Update(Attraction prev_att, Attraction new_att)
        {
            int id = prev_att.ID;
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
                            $"(\"{new_att.Location.General_Location.Country}\", " +
                            $"\"{new_att.Location.General_Location.City}\")" +
                            $" ON DUPLICATE " +
                            $" KEY UPDATE city=city,country=country;";
                        myCommand.ExecuteNonQuery();
                        // in case the update include new latitude and longiutde
                        myCommand.CommandText = $"Insert into places (lat, lon, country, city) VALUES" +
                                    $" ({new_att.Location.Coordinates.Latitude}, {new_att.Location.Coordinates.Longitude}" +
                                    $", \"{new_att.Location.General_Location.Country}\"," +
                                    $" \"{new_att.Location.General_Location.City}\")" +
                                    $"ON DUPLICATE KEY UPDATE city=city,country=country;";
                        myCommand.ExecuteNonQuery();
                        //updating the values
                        myCommand.CommandText = $"UPDATE attractions SET name = \"{new_att.Name}\"," +
                                     $"lat = {new_att.Location.Coordinates.Latitude}, lon = {new_att.Location.Coordinates.Longitude}," +
                                     $"phone = \"{new_att.Phone}\" " +
                                     $"WHERE id = {id};";
                        myCommand.ExecuteNonQuery();
                        // in case we updated the location we try to remove the previous location
                        // if there is no use of the previous location it will be deleted.
                        myCommand.CommandText = "delete ignore from places where " +
                                                $"lat={prev_att.Location.Coordinates.Latitude} and " +
                                                $"lon = {prev_att.Location.Coordinates.Longitude};";
                        myCommand.ExecuteNonQuery();
                        myCommand.CommandText = "delete ignore from region where exists (select country,city " +
                                    $"from attractions as t1 join places as t2 " +
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
                throw new Exception("There was a problem while trying to update the attraction");
            }
        }

    }
}
