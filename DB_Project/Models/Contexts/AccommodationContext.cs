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

        public List<Accommodation> GetAllAccommodation()
        {
            List<Accommodation> list = new List<Accommodation>();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    string req = "select distinct name,places.lat,places.lon,phone,internet" +
                        ",city,country from accommodation join places on accommodation.lat = " +
                        "places.lat and accommodation.lon = places.lon;";
                    MySqlCommand cmd = new MySqlCommand(req, conn);
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
    }
}
