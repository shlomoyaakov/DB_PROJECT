using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DB_Project.Models
{

    /// <summary>
    /// a class that converts between reader to location
    /// </summary>
    public class ReaderConversion
    {

        /// <summary>
        /// convert between reader values to location
        /// </summary>
        /// <param name="reader">The reader that we get after excecuting sql query</param>
        /// <returns>Location that hold city country and coordinates</returns>
        public Location Location_from_reader(MySqlDataReader reader)
        {
            return new Location()
            {
                Coordinates = Coordinates_from_reader(reader),
                General_Location = Region_from_reader(reader)
            };
        }

        /// <summary>
        /// convert between reader values to region
        /// </summary>
        /// <param name="reader">The reader that we get after excecuting sql query</param>
        /// <returns>Region that hold city and country</returns>
        public Region Region_from_reader(MySqlDataReader reader)
        {
            return new Region()
            {
                City = reader["city"].ToString(),
                Country = reader["country"].ToString()
            };
        }


        /// <summary>
        /// convert between reader values to location
        /// </summary>
        /// <param name="reader">The reader that we get after excecuting sql query</param>
        /// <returns>Coordinates that holds latitude and longitude</returns>
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
