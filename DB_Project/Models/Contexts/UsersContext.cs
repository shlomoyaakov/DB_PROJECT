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

        public Boolean IsExists(string username, string password)
        {
            try
            {
<<<<<<< HEAD
                Boolean result = false;
                using (MySqlConnection conn = GetConnection())
=======
                conn.Open();
                string req = $"select 1 from users where user_name=\"{username}\" and password=\"{password}\";";
                MySqlCommand cmd = new MySqlCommand(req, conn);
                using (var reader = cmd.ExecuteReader())
>>>>>>> b2973b27ceffdaf816b767cd50dff4bd754ed943
                {
                    conn.Open();
                    string req = $"select 1 from users where user_name=\"{user.User_Name}\"" +
                                 $" and password=\"{user.Password}\";";
                    MySqlCommand cmd = new MySqlCommand(req, conn);
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


        public void Add_User(User user)
        {
<<<<<<< HEAD
=======
            if (IsUsernameExists(user.User_Name) || IsEmailExists(user.Email)) {
                throw new Exception("email or username already exists");
            }
            using MySqlConnection conn = GetConnection();
            conn.Open();
            string req = $"insert into users(user_name,password,email) values(\"{user.User_Name}\"," +
                $"\"{user.Password}\",\"{user.Email}\");";
            MySqlCommand cmd = new MySqlCommand(req, conn);
>>>>>>> b2973b27ceffdaf816b767cd50dff4bd754ed943
            try
            {
                using MySqlConnection conn = GetConnection();
                conn.Open();
                string req = $"insert into users(user_name,password) values(\"{user.User_Name}\"," +
                $"\"{user.Password}\");";
                MySqlCommand cmd = new MySqlCommand(req, conn);
                using var reader = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Boolean IsUsernameExists(string username)
        {
            Boolean result = false;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string req = $"select 1 from users where user_name=\"{username}\";";
                MySqlCommand cmd = new MySqlCommand(req, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    result = reader.HasRows;
                }
            }
            return result;
        }

        public Boolean IsEmailExists(string email)
        {
            Boolean result = false;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string req = $"select 1 from users where email=\"{email}\";";
                MySqlCommand cmd = new MySqlCommand(req, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    result = reader.HasRows;
                }
            }
            return result;
        }

        public void Delete_User(User user)
        {
            try
            {
                using MySqlConnection conn = GetConnection();
                conn.Open();
                string req = $"delete from users where user_name=\"{user.User_Name}\";";
                MySqlCommand cmd = new MySqlCommand(req, conn);
                using var reader = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}