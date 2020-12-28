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
               $"where country=\"{country}\" and city=\"{city}\" and NOT EXISTS (select distinct t1.id,t1.name, t1.lat, t1.lon, country, city, t1.phone " +
                "from users_trips as t3 join trip_attractions as t2 " +
                "on t2.trip_id = t3.trip_id " +
                "join attractions as t1 " +
                "on t2.attraction_id = t1.id join places as t0 " +
                "on t0.lat = t1.lat and t0.lon = t1.lon " +
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

        public List<KeyValuePair<int, int>> Get_Amount_By_Region(string country, string city)
        {
            List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
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
                        KeyValuePair<int, int> kv = new KeyValuePair<int, int>((int)reader["attraction_id"]
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
