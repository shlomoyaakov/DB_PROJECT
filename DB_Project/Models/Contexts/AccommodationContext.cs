﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models.Contexts
{
    public class AccommodationContext
    {
        public string ConnectionString { get; set; }

        public AccommodationContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Accommodation> GetAllAccommodation()
        {
            List<Accommodation> list = new List<Accommodation>();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = 
new MySqlCommand("select distinct name,places.lat,places.lon,phone,internet,city,country from accommodation join places on accommodation.lat = places.lat and accommodation.lon = places.lon; ", conn);
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
                return list;
            }
            return list;
        }
    }
}
