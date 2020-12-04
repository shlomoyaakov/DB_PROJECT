﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models.Contexts
{
    public class UsersContext : BaseContext
    {
        public UsersContext(string connectionString) : base(connectionString) { }

        public Boolean IsExists(User user)
        {
            Boolean result = false;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string req = $"select 1 from users where user_name=\"{user.User_Name}\"" +
                             $" and email=\"{user.Email}\" and password=\"{user.Password}\";";
                MySqlCommand cmd = new MySqlCommand(req, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    result = reader.HasRows;
                }
            }
            return result;
        }

        public void Add_User(User user)
        {
            using MySqlConnection conn = GetConnection();
            conn.Open();
            string req = $"insert into users(user_name,password,email) values(\"{user.User_Name}\"," +
                $"\"{user.Password}\",\"{user.Email}\");";
            MySqlCommand cmd = new MySqlCommand(req, conn);
            try
            {
                using var reader = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete_User(User user)
        {
            using MySqlConnection conn = GetConnection();
            conn.Open();
            string req = $"delete from users where user_name=\"{user.User_Name}\"" +
                $" and email=\"{user.Email}\";";
            MySqlCommand cmd = new MySqlCommand(req, conn);
            try
            {
                using var reader = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
