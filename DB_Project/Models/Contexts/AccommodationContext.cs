using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models.Contexts
{
    public class AccommodationContext : BaseContext
    {

        public AccommodationContext(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<Accommodation> Get_Accommodation_By_Req(string request)
        {
            List<Accommodation> list = new List<Accommodation>();
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
                            list.Add(new Accommodation()
                            {
                                ID = (int)reader["id"],
                                Name = reader["name"].ToString(),
                                Phone = reader["phone"].ToString(),
                                Internet = reader["internet"].ToString(),
                                Location = convert.Location_from_reader(reader),
                                Type = reader["type"].ToString()
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
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Accommodation> GetAllAccommodation()
        {
            string req = "select distinct id,name,places.lat,places.lon,phone,internet,type" +
                        ",city,country from accommodation join places on accommodation.lat = " +
                        "places.lat and accommodation.lon = places.lon;";
            try
            {
                return Get_Accommodation_By_Req(req);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public List<Accommodation> Get_Accommodation_By_Region(string country, string city)
        {
            string req = "select distinct id,name,places.lat,places.lon,phone,internet,type" +
                        ",city,country from accommodation join places on accommodation.lat = " +
                         "places.lat and accommodation.lon = places.lon " +
                         $"where city=\"{city}\" and country = \"{country}\";";
            try
            {
                return Get_Accommodation_By_Req(req);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="city"></param>
        /// <param name="user_name"></param>
        /// <returns></returns>
        public List<Accommodation> Get_Accommodation_By_Region_And_User(string country, string city, string user_name)
        {
            string request = "select distinct t4.id,t4.name, t4.lat, t4.lon, country, city, t4.phone, t4.internet, t4.type " +
                "from Accommodation as t4 join places as t5 " +
                "on t4.lat = t5.lat and t4.lon = t5.lon " +
                $"where country=\"{country}\" and city=\"{city}\" and t4.id NOT in " +
               "(select distinct t2.accommodation_id " +
               "from users_trips as t3 join trip_accommodation as t2 " +
                $"where user_name=\"{user_name}\");";
            try
            {
                return Get_Accommodation_By_Req(request);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accommodation"></param>
        public void Add_Accomodation(Accommodation accommodation)
        {
            string country = accommodation.Location.General_Location.Country;
            string city = accommodation.Location.General_Location.City;
            double lat = accommodation.Location.Coordinates.Latitude;
            double lon = accommodation.Location.Coordinates.Longitude;
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
                        myCommand.CommandText = $"Insert into accommodation (name, lat, lon, phone, internet, type) VALUES (\"{accommodation.Name}\", {lat}, {lon}, \"{accommodation.Phone}\", \"{accommodation.Internet}\", \"{accommodation.Type}\");";
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, Int64>> Get_Amount_By_Region(string country, string city)
        {
            List<KeyValuePair<int, Int64>> list = new List<KeyValuePair<int, Int64>>();
            string req = "select accommodation_id, count(accommodation_id) as amount from trip_region " +
                        "join trip_accommodation " +
                        "on trip_accommodation.trip_id = trip_region.trip_id " +
                        $"where country = \"{country}\" and city = \"{city}\" " +
                        "group by accommodation_id " +
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
                        KeyValuePair<int, Int64> kv = new KeyValuePair<int, Int64>((int)reader["accommodation_id"]
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

        public void Delete(Accommodation acc)
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
                        myCommand.CommandText = $"delete from accommodation where id={acc.ID};";
                        myCommand.ExecuteNonQuery();
                        // in case we updated the location we try to remove the previous location
                        // if there is no use of the previous location it will be deleted.
                        myCommand.CommandText = "delete ignore from places where " +
                                            $"lat={acc.Location.Coordinates.Latitude} and " +
                                            $"lon = {acc.Location.Coordinates.Longitude};";
                        myCommand.ExecuteNonQuery();
                        myCommand.CommandText = "delete ignore from region where exists (select country,city " +
                                    $"from accommodation as t1 join places as t2 " +
                                    $"on t1.lat = t2.lat and t2.lon=t1.lon " +
                                    $"where id={acc.ID});";
                        myCommand.ExecuteNonQuery();
                        myTrans.Commit();
                    }
                    catch(MySqlException e)
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

        public void Update(Accommodation prev_acc,Accommodation new_acc)
        {
            int id = prev_acc.ID;
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
                            $"(\"{new_acc.Location.General_Location.Country}\", " +
                            $"\"{new_acc.Location.General_Location.City}\") " +
                            $" ON DUPLICATE " +
                            $" KEY UPDATE city=city,country=country;";
                        myCommand.ExecuteNonQuery();
                        // in case the update include new latitude and longiutde
                        myCommand.CommandText = $"Insert into places (lat, lon, country, city) VALUES" +
                                    $" ({new_acc.Location.Coordinates.Latitude}, {new_acc.Location.Coordinates.Longitude}" +
                                    $", \"{new_acc.Location.General_Location.Country}\"," +
                                    $" \"{new_acc.Location.General_Location.City}\") " +
                                    $"ON DUPLICATE KEY UPDATE city=city,country=country;";
                        myCommand.ExecuteNonQuery();
                        //updating the values
                        myCommand.CommandText = $"UPDATE accommodation SET name = \"{new_acc.Name}\"," +
                                     $"lat = {new_acc.Location.Coordinates.Latitude}, lon = {new_acc.Location.Coordinates.Longitude}," +
                                     $"phone = \"{new_acc.Phone}\", internet =\"{new_acc.Internet}\", type = \"{new_acc.Type}\" " +
                                     $"WHERE id = {id};";
                        myCommand.ExecuteNonQuery();
                        // in case we updated the location, we try to remove the previous location
                        // if there is no use of the previous location it will be deleted.
                        myCommand.CommandText = "delete ignore from places where " +
                                                $"lat={prev_acc.Location.Coordinates.Latitude} and " +
                                                $"lon = {prev_acc.Location.Coordinates.Longitude};";
                        myCommand.ExecuteNonQuery();
                        myCommand.CommandText = "delete ignore from region where exists (select country,city " +
                                    $"from accommodation as t1 join places as t2 " +
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
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
