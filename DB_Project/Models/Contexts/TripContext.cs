using DB_Project.Models.Data_Class;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models.Contexts
{
    /// <summary>
    /// TripContext responsible to the communication with
    /// the tables that related to trips in the database.
    /// </summary>
    public class TripContext: BaseContext
    {
        private string base_rest_req;
        private string base_att_req;
        private string base_acc_req;
        public TripContext(string connectionString) : base(connectionString) {
            Initialize_Base_Request();
        }

        /// <summary>
        /// map between accommodations to certain trip by request
        /// </summary>
        /// <param name="request">The mysql request string</param>
        /// <param name="trip_map"> A map between id to Trip</param>
        /// <param name="conn">A MySqlConnection</param>
        private void Accommodation_In_Trip(string request, Dictionary<int, Trip> trip_map, MySqlConnection conn)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(request, conn);
                ReaderConversion convert = new ReaderConversion();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //here we check if we already have that trip_id in the map
                    // if we don't have then we create a new trip object
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
                    // adding the accommodation to its trip
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

        /// <summary>
        /// map between attracions to certain trip by request
        /// </summary>
        /// <param name="request">The mysql request string</param>
        /// <param name="trip_map"> A map between id to Trip</param>
        /// <param name="conn">A MySqlConnection</param>
        private void Attractions_In_Trip(string request, Dictionary<int, Trip> trip_map, MySqlConnection conn)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(request, conn);
                ReaderConversion convert = new ReaderConversion();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //here we check if we already have that trip_id in the map
                    // if we don't have then we create a new trip object
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
                    // adding the attraction to its trip
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

        /// <summary>
        /// map between restaurants to certain trip by request
        /// </summary>
        /// <param name="request">The mysql request string</param>
        /// <param name="trip_map"> A map between id to Trip</param>
        /// <param name="conn">A MySqlConnection</param>
        private void Restaurants_In_Trip(string request, Dictionary<int, Trip> trip_map, MySqlConnection conn)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(request, conn);
                ReaderConversion convert = new ReaderConversion();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //here we check if we already have that trip_id in the map
                    // if we don't have then we create a new trip object
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
                    // adding the restaurants to its trip
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

        /// <summary>
        /// Gets trips from the database according to the request
        /// </summary>
        /// <param name="rest_req">mysql request to map between restaurants to trips</param>
        /// <param name="acc_req">mysql request to map between accommodation to trips</param>
        /// <param name="att_req">mysql request to map between attractions to trips</param>
        /// <returns>A list of trips according to the requests</returns>
        public List<Trip> Get_Trips_By_Requests(string rest_req, string acc_req, string att_req)
        {
            List<Trip> list = new List<Trip>();
            Dictionary<int, Trip> trip_map = new Dictionary<int, Trip>();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    //map between trips to restaurants,accommodation, and attractions
                    Restaurants_In_Trip(rest_req, trip_map, conn);
                    Accommodation_In_Trip(acc_req, trip_map, conn);
                    Attractions_In_Trip(att_req, trip_map, conn);   
                }
                // return the list of trips after the mapping process has done
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

        /// <summary>
        /// Gets all the trips of certain user
        /// </summary>
        /// <param name="user_name"> The user name </param>
        /// <returns>List of trips of certain user</returns>
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
            catch (Exception)
            {
                throw new Exception($"There was a problem while trying to get all {user_name}'s trips");
            }
        }

        /// <summary>
        /// Get a certain trip
        /// </summary>
        /// <param name="id">The trip id</param>
        /// <returns>The trip with that id</returns>
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
                    throw new Exception("");
                }
                return trips[0];
            }
            catch(Exception)
            {
                throw new Exception($"There was a problem while trying to get trip with id:{id}");
            }
        }

        /// <summary>
        /// Adding a new trip to the data base
        /// </summary>
        /// <param name="trip">The trip that we wan't ot add </param>
        public void Add_Trip(Trip trip)
        {
            try
            {
                using (MySqlConnection myConnection = GetConnection())
                {
                    myConnection.Open();
                    MySqlCommand myCommand = myConnection.CreateCommand();
                    //we use transaction because the adding proccess involves sevral tables
                    MySqlTransaction myTrans;
                    myTrans = myConnection.BeginTransaction();
                    myCommand.Connection = myConnection;
                    myCommand.Transaction = myTrans;
                    try
                    {
                        //inserting into users_trips
                        myCommand.CommandText = $"Insert into users_trips (user_name,date) VALUes (\"{trip.User_Name}\", \"{trip.Time}\")";
                        myCommand.ExecuteNonQuery();
                        //geting the id
                        myCommand.CommandText = $"select trip_id from users_trips where user_name=\"{trip.User_Name}\" and date = \"{trip.Time}\";";
                        using var reader = myCommand.ExecuteReader();
                        reader.Read();
                        int id = (int)reader["trip_id"];
                        reader.Close();
                        //adding all of the attraction in the trips
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
                        //adding all of the restuarants in the trip
                        foreach (Restaurant rest in trip.Restaurants)
                        {
                            //adding also the region of the trips in case we the trip doesn't have attraction
                            myCommand.CommandText = $"Insert into trip_region (trip_id,country,city) VALUes ({id}" +
                                $", \"{rest.Location.General_Location.Country}\", \"{rest.Location.General_Location.City}\") " +
                                $"ON DUPLICATE KEY UPDATE trip_id=trip_id;";
                            myCommand.ExecuteNonQuery();

                            myCommand.CommandText = $"Insert into trip_restaurants (trip_id, restaurant_id) VALUes ({id}" +
                               $", {rest.ID}) ON DUPLICATE KEY UPDATE trip_id=trip_id;";
                            myCommand.ExecuteNonQuery();
                        }
                        //adding all of the accommodation in the trip
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
                    //if one of the commits failed
                    catch (MySqlException ex)
                    {
                        myTrans.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("There was a problem while trying to add the trip");
            }
        }

        /// <summary>
        /// Delete all of trips of certain user
        /// </summary>
        /// <param name="user_name">The user that we want to delete its trips</param>
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
            catch (Exception)
            {
                throw new Exception($"There was a problem while trying to delete {user_name}'s trips");
            }
        }

        /// <summary>
        /// Delete certain trip
        /// </summary>
        /// <param name="trip_id"> The id of the trip that we want to delete</param>
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
            catch (Exception)
            {
                throw new Exception($"There was a problem while trying to delete the trip with id:{trip_id}");
            }
        }
        

        /// <summary>
        /// Initialize the base request for the different tables
        /// </summary>
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
