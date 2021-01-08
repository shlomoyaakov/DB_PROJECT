using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models.Contexts
{
    /// <summary>
    /// UsersContext responsible to the communication with users in the database.
    /// </summary>
    public class UsersContext : BaseContext
    {
        public UsersContext(string connectionString) : base(connectionString) { }


        /// <summary>
        /// Check wether a user is exist by request
        /// </summary>
        /// <param name="request">The sql request</param>
        /// <returns>Boolean if exist</returns>
        private Boolean IsExists_By_Req(string request)
        {
            try
            {
                Boolean result = false;
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(request, conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        result = reader.HasRows;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Check if certain user exists in the database
        /// </summary>
        /// <param name="user">The user that we want to verify if exists in the
        /// database</param>
        /// <returns>True if exist otherwise false </returns>
        public Boolean IsExists(User user)
        {
            string req = $"select 1 from users where user_name=\"{user.User_Name}\"" +
             $" and password=\"{user.Password}\";";
            try
            {
                return IsExists_By_Req(req);
            }
            catch (Exception)
            {
                throw new Exception($"There was a problem while trying to check if" +
                    $" the user {user.User_Name} exists");
            }
        }

        /// <summary>
        /// Execute a non query sql command
        /// </summary>
        /// <param name="request">The mysql commandd in string</param>
        private void ExecuteNonQuery(string request)
        {
            try
            {
                using MySqlConnection conn = GetConnection();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(request, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Add user to the database
        /// </summary>
        /// <param name="user"> The user that we want to add </param>
        public void Add_User(User user)
        {
            string req = $"insert into users(user_name,password,admin) values(\"{user.User_Name}\"," +
               $"\"{user.Password}\",0);";
            try
            {
                ExecuteNonQuery(req);
            }
            catch (Exception)
            {
                throw new Exception($"There was a problem while trying to add the user {user.User_Name}");
            }
        }

        /// <summary>
        /// Delete user from the data base with all of his trips
        /// (because of cascade in the tables)
        /// </summary>
        /// <param name="user"> The user that we want to delete </param>
        public void Delete_User(User user)
        {
            string request = $"delete from users where user_name=\"{user.User_Name}\";";
            try
            {
                ExecuteNonQuery(request);
            }
            catch (Exception)
            {
                throw new Exception($"There was a problem while trying to delete the user {user.User_Name}");
            }
        }

        /// <summary>
        /// Check if a user is an admin or not
        /// </summary>
        /// <param name="user">The user that we want to check if he is an admin</param>
        /// <returns>True if the its an admin user otherwise flase</returns>
        public Boolean IsAdmin(User user)
        {
            string req = $"select 1 from users where user_name=\"{user.User_Name}\"" +
                $" and password=\"{user.Password}\" and admin=1;";
            try
            {
                return IsExists_By_Req(req);
            }
            catch (Exception)
            {
                throw new Exception($"There was a problem while trying to" +
                    $" verify if {user.User_Name} is admin");
            }
        }

    }
}
