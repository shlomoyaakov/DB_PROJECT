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

        public List<Accommodation> Get_Accommodation_By_Region_And_User(string country, string city, string user_name)
        {
            string request = "select distinct t4.id,t4.name, t4.lat, t4.lon, country, city, t4.phone, t4.internet, t4.type " +
                "from Accommodation as t4 join places as t5 " +
                "on t4.lat = t5.lat and t4.lon = t5.lon " +
                $"where country=\"{country}\" and city=\"{city}\" and NOT EXISTS (select distinct t1.id,t1.name, t1.lat, t1.lon, country, city, t1.phone, t1.internet, t4.type " +
                "from users_trips as t3 join trip_Accommodation as t2 " +
                "on t2.trip_id = t3.trip_id " +
                "join Accommodation as t1 " +
                "on t2.Accommodation_id = t1.id join places as t0 " +
                "on t0.lat = t1.lat and t0.lon = t1.lon " +
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
                        myCommand.CommandText = $"Insert into accomodation (name, lat, lon, phone, internet, type) VALUES (\"{accommodation.Name}\", {lat}, {lon}, \"{accommodation.Phone}\", \"{accommodation.Internet}\", \"{accommodation.Type}\");";
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
