using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DB_Project.Models
{
    public class ReaderConversion
    {
        public Location Location_from_reader(MySqlDataReader reader)
        {
            return new Location()
            {
                Coordinates = Coordinates_from_reader(reader),
                General_Location = Region_from_reader(reader)
            };
        }

        public Region Region_from_reader(MySqlDataReader reader)
        {
            return new Region()
            {
                City = reader["city"].ToString(),
                Country = reader["country"].ToString()
            };
        }

        public Coordinates Coordinates_from_reader(MySqlDataReader reader)
        {
            return new Coordinates()
            {
                Longitude = (double)reader["lon"],
                Latitude = (double)reader["lat"]
            };
             
        }
    }
}
