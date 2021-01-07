using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models.Contexts
{
    public class UsersContext : BaseContext
    {
        public UsersContext(string connectionString) : base(connectionString) { }

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
        public Boolean IsExists(User user)
        {
            string req = $"select 1 from users where user_name=\"{user.User_Name}\"" +
             $" and password=\"{user.Password}\";";
            try
            {
                return IsExists_By_Req(req);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

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
        public void Add_User(User user)
        {
            string req = $"insert into users(user_name,password,admin) values(\"{user.User_Name}\"," +
               $"\"{user.Password}\",0);";
            try
            {
                ExecuteNonQuery(req);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete_User(User user)
        {
            string request = $"delete from users where user_name=\"{user.User_Name}\";";
            try
            {
                ExecuteNonQuery(request);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Boolean IsAdmin(User user)
        {
            string req = $"select 1 from users where user_name=\"{user.User_Name}\"" +
                $" and password=\"{user.Password}\" and admin=1;";
            try
            {
                return IsExists_By_Req(req);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
