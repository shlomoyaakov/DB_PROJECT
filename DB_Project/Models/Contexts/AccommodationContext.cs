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

        public List<Accommodation> GetAllAccommodation()
        {
            string req = "select distinct name,places.lat,places.lon,phone,internet" +
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
            string req = "select distinct name,places.lat,places.lon,phone,internet" +
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
    }
}
