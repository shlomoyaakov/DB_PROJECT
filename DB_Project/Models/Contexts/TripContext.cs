using DB_Project.Models.Data_Class;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models.Contexts
{
    public class TripContext: BaseContext
    {
        private string base_rest_req;
        private string base_att_req;
        private string base_acc_req;
        public TripContext(string connectionString) : base(connectionString) {
            Initialize_Base_Request();
        }

        private void Accommodation_In_Trip(string request, Dictionary<int, Trip> trip_map, MySqlConnection conn)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(request, conn);
                ReaderConversion convert = new ReaderConversion();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["trip_id"];
                    if (!trip_map.ContainsKey(id))
                    {
                        trip_map[id] = new Trip()
                        {
                            ID = id,
                            Time = (reader["date"].ToString()),
                            User_Name = reader["user_name"].ToString(),
                            Country = reader["country"].ToString(),
                            City = reader["city"].ToString()
                        };
                    }
                    trip_map[id].Accommodation.Add(new Accommodation()
                    {
                        Name = reader["name"].ToString(),
                        Phone = reader["phone"].ToString(),
                        Internet = reader["internet"].ToString(),
                        Location = convert.Location_from_reader(reader),
                        Type = reader["type"].ToString()
                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void Attractions_In_Trip(string request, Dictionary<int, Trip> trip_map, MySqlConnection conn)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(request, conn);
                ReaderConversion convert = new ReaderConversion();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["trip_id"];
                    if (!trip_map.ContainsKey(id))
                    {
                        trip_map[id] = new Trip()
                        {
                            ID = id,
                            Time = (reader["date"].ToString()),
                            User_Name = reader["user_name"].ToString(),
                            Country = reader["country"].ToString(),
                            City = reader["city"].ToString()
                        };
                    }
                    trip_map[id].Attractions.Add(new Attraction()
                    {
                        Name = reader["name"].ToString(),
                        Phone = reader["phone"].ToString(),
                        Location = convert.Location_from_reader(reader)
                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void Restaurants_In_Trip(string request, Dictionary<int, Trip> trip_map, MySqlConnection conn)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(request, conn);
                ReaderConversion convert = new ReaderConversion();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["trip_id"];
                    if (!trip_map.ContainsKey(id))
                    {
                        trip_map[id] = new Trip()
                        {   ID = id,
                            Time =(reader["date"].ToString()),
                            User_Name = reader["user_name"].ToString(),
                            Country = reader["country"].ToString(),
                            City = reader["city"].ToString()
                        };
                    }
                    trip_map[id].Restaurants.Add(new Restaurant()
                    {
                        Name = reader["name"].ToString(),
                        Phone = reader["phone"].ToString(),
                        Cuisine = reader["cuisine"].ToString(),
                        Location = convert.Location_from_reader(reader)
                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public List<Trip> Get_Trips_By_Requests(string rest_req, string acc_req, string att_req)
        {
            List<Trip> list = new List<Trip>();
            Dictionary<int, Trip> trip_map = new Dictionary<int, Trip>();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    Restaurants_In_Trip(rest_req, trip_map, conn);
                    Accommodation_In_Trip(acc_req, trip_map, conn);
                    Attractions_In_Trip(att_req, trip_map, conn);   
                }
                foreach (KeyValuePair<int, Trip> entry in trip_map)
                {
                    list.Add(entry.Value);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }

        public List<Trip> Get_Trips_By_User_Name(string user_name)
        {
            try
            {
                string rest_req = this.base_rest_req +
                  $"where t3.user_name = \"{user_name}\";";

                string acc_req = this.base_acc_req +
                                  $"where t3.user_name = \"{user_name}\";";

                string att_req = this.base_att_req +
                                  $"where t3.user_name = \"{user_name}\";";

                return Get_Trips_By_Requests(rest_req, acc_req, att_req);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public Trip Get_Trip_By_Id(int id)
        {
            try
            {
                string rest_req = this.base_rest_req +
                                  $"where t3.trip_id = {id};";

                string acc_req = this.base_acc_req +
                                  $"where t3.trip_id = {id};";

                string att_req = this.base_att_req +
                                  $"where t3.trip_id = {id};";

                List<Trip> trips = Get_Trips_By_Requests(rest_req, acc_req, att_req);
                if (trips.Count == 0)
                {
                    throw new Exception("There is no such trip_id");
                }
                return trips[0];
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Add_Trip(Trip trip)
        {
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
                        myCommand.CommandText = $"Insert into users_trips (user_name,date) VALUes (\"{trip.User_Name}\", \"{trip.Time}\")";
                        myCommand.ExecuteNonQuery();
                        myCommand.CommandText = $"select trip_id from users_trips where user_name=\"{trip.User_Name}\" and date = \"{trip.Time}\";";
                        using var reader = myCommand.ExecuteReader();
                        reader.Read();
                        int id = (int)reader["trip_id"];
                        reader.Close();
                        foreach (Attraction att in trip.Attractions)
                        {
                            myCommand.CommandText = $"Insert into trip_region (trip_id,country,city) VALUes ({id}" +
                                $", \"{att.Location.General_Location.Country}\", \"{att.Location.General_Location.City}\") " +
                                $"ON DUPLICATE KEY UPDATE trip_id=trip_id;";
                            myCommand.ExecuteNonQuery();

                            myCommand.CommandText = $"Insert into trip_attractions (trip_id, attraction_id) VALUes ({id}" +
                               $", {att.ID}) ON DUPLICATE KEY UPDATE trip_id=trip_id;";
                            myCommand.ExecuteNonQuery();
                        }
                        foreach (Restaurant rest in trip.Restaurants)
                        {
                            myCommand.CommandText = $"Insert into trip_region (trip_id,country,city) VALUes ({id}" +
                                $", \"{rest.Location.General_Location.Country}\", \"{rest.Location.General_Location.City}\") " +
                                $"ON DUPLICATE KEY UPDATE trip_id=trip_id;";
                            myCommand.ExecuteNonQuery();

                            myCommand.CommandText = $"Insert into trip_restaurants (trip_id, restaurant_id) VALUes ({id}" +
                               $", {rest.ID}) ON DUPLICATE KEY UPDATE trip_id=trip_id;";
                            myCommand.ExecuteNonQuery();
                        }
                        foreach (Accommodation acc in trip.Accommodation)
                        {
                            myCommand.CommandText = $"Insert into trip_region (trip_id,country,city) VALUes ({id}" +
                                $", \"{acc.Location.General_Location.Country}\", \"{acc.Location.General_Location.City}\") " +
                                $"ON DUPLICATE KEY UPDATE trip_id=trip_id;";
                            myCommand.ExecuteNonQuery();

                            myCommand.CommandText = $"Insert into trip_accommodation (trip_id, accommodation_id) VALUes ({id}" +
                               $", {acc.ID}) ON DUPLICATE KEY UPDATE trip_id=trip_id;";
                            myCommand.ExecuteNonQuery();
                        }
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

        public void Delete_Users_Trips(string user_name)
        {
            try
            {
                using MySqlConnection conn = GetConnection();
                conn.Open();
                string request = $"delete from users_trips where user_name=\"{user_name}\";";
                MySqlCommand cmd = new MySqlCommand(request, conn);
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void Delete_Trip(int trip_id)
        {
            try
            {
                using MySqlConnection conn = GetConnection();
                conn.Open();
                string request = $"delete from users_trips where trip_id={trip_id};";
                MySqlCommand cmd = new MySqlCommand(request, conn);
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        private void Initialize_Base_Request()
        {
            this.base_rest_req = "select distinct t3.user_name,t3.trip_id,t3.date,t1.name, t1.lat, t1.lon, country, city, t1.phone, t1.cuisine " +
                 "from users_trips as t3 " +
                 "join trip_restaurants as t2 " +
                 "on t2.trip_id = t3.trip_id " +
                 "join restaurants as t1 " +
                 "on t2.restaurant_id = t1.id " +
                 "join places as t0 " +
                 "on t0.lat = t1.lat and t0.lon = t1.lon ";

            this.base_acc_req = "select distinct t3.user_name,t3.trip_id,t3.date,t1.name, t1.lat, t1.lon, country, city, t1.phone, t1.internet, t1.type " +
                             "from users_trips as t3 " +
                             "join trip_accommodation as t2 " +
                             "on t2.trip_id = t3.trip_id " +
                             "join accommodation as t1 " +
                             "on t2.accommodation_id = t1.id " +
                             "join places as t0 " +
                             "on t0.lat = t1.lat and t0.lon = t1.lon ";

            this.base_att_req = "select distinct t3.user_name,t3.trip_id,t3.date,t1.name, t1.lat, t1.lon, country, city, t1.phone " +
                             "from users_trips as t3 " +
                             "join trip_attractions as t2 " +
                             "on t2.trip_id = t3.trip_id " +
                             "join attractions as t1 " +
                             "on t2.attraction_id = t1.id " +
                             "join places as t0 " +
                             "on t0.lat = t1.lat and t0.lon = t1.lon ";
        }
    }
}
