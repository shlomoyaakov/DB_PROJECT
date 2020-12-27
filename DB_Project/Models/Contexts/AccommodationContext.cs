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

        private List<Accommodation> Get_Accommodation_By_Req(string request)
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
            string req = "select distinct name,places.lat,places.lon,phone,internet,type" +
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
            string req = "select distinct name,places.lat,places.lon,phone,internet,type" +
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
                        myCommand.CommandText = $"Insert into attractions (name, lat, lon, phone, internet, type) VALUES (\"{accommodation.Name}\", {lat}, {lon}, \"{accommodation.Phone}\", \"{accommodation.Internet}\", \"{accommodation.Type}\");";
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
