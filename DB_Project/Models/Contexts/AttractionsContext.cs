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

        private List<Attraction> Get_Attractions_By_Req(string request)
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
            string request = "select distinct name,places.lat,places.lon,phone" +
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
            string request = "select distinct name,places.lat,places.lon,phone" +
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

    }
}
