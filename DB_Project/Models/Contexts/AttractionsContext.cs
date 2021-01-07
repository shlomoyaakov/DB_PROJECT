using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models.Contexts
{
    public class AttractionsContext : BaseContext
    {

        public AttractionsContext(string connectionString) : base(connectionString)
        {
        }

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

        public List<Attraction> GetAllAttractions()
        {
            string request = "select distinct id,name,places.lat,places.lon,phone" +
            ",city,country from attractions join places on attractions.lat = " +
             "places.lat and attractions.lon = places.lon;";
            try
            {
                return Get_Attractions_By_Req(request);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Attraction> Get_Attractions_By_Region(string country,string city)
        {
            string request = "select distinct id,name,places.lat,places.lon,phone" +
                        ",city,country from attractions join places on attractions.lat = " +
                        "places.lat and attractions.lon = places.lon " +
                        $"where country=\"{country}\" and city=\"{city}\";";
            try
            {
                return Get_Attractions_By_Req(request);
            }catch(Exception e)
            {
                throw e;
            }
            
        }

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
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Add_Attraction(Attraction attraction)
        {
            string country = attraction.Location.General_Location.Country;
            string city = attraction.Location.General_Location.City;
            double lat = attraction.Location.Coordinates.Latitude;
            double lon = attraction.Location.Coordinates.Longitude;
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
            catch (Exception e)
            {
                throw e;
            }
        }

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
                    while (reader.Read())
                    {
                        KeyValuePair<int, Int64> kv = new KeyValuePair<int, Int64>((int)reader["attraction_id"]
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
                        myCommand.CommandText = $"delete from attractions where attraction_id={att.ID};";
                        myCommand.ExecuteNonQuery();
                        // in case we updated the location we try to remove the previous location
                        // if there is no use of the previous location it will be deleted.
                        myCommand.CommandText = "delete ignore from places where exists (select lat,lon,country,city " +
                                $"from attractions as t1 join places as t2" +
                                $"on t1.lat = t2.lat and t2.lon=t1.lon " +
                                $"where id={att.ID};";
                        myCommand.ExecuteNonQuery();
                        myCommand.CommandText = "delete ignore from region where exists (select country,city " +
                                    $"from attractions as t1 join places as t2" +
                                    $"on t1.lat = t2.lat and t2.lon=t1.lon " +
                                    $"where id={att.ID};";
                        myCommand.ExecuteNonQuery();

                    }
                    catch (MySqlException e)
                    {
                        myTrans.Rollback();
                        throw e;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

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
                        myCommand.CommandText = $"UPDATE users_trips SET name = \"{new_att.Name}\"," +
                                     $"lat = {new_att.Location.Coordinates.Latitude}, lon = {new_att.Location.Coordinates.Longitude}," +
                                     $"phone = \"{new_att.Phone}\" " +
                                     $"WHERE attraction_id = {id};";
                        myCommand.ExecuteNonQuery();
                        // in case we updated the location we try to remove the previous location
                        // if there is no use of the previous location it will be deleted.
                        myCommand.CommandText = "delete ignore from places where exists (select lat,lon,country,city " +
                                $"from attractions as t1 join places as t2" +
                                $"on t1.lat = t2.lat and t2.lon=t1.lon " +
                                $"where id={id};";
                        myCommand.ExecuteNonQuery();
                        myCommand.CommandText = "delete ignore from region where exists (select country,city " +
                                    $"from attractions as t1 join places as t2" +
                                    $"on t1.lat = t2.lat and t2.lon=t1.lon " +
                                    $"where id={id};";
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
