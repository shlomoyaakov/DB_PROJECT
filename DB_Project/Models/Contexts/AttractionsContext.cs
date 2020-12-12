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

        public List<Attraction> GetAllAttractions()
        {
            List<Attraction> list = new List<Attraction>();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    string req = "select distinct name,places.lat,places.lon,phone" +
                        ",city,country from attractions join places on attractions.lat = " +
                        "places.lat and attractions.lon = places.lon;";
                    MySqlCommand cmd = new MySqlCommand(req, conn);
                    ReaderConversion convert = new ReaderConversion();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Attraction()
                            {
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
    }
}
