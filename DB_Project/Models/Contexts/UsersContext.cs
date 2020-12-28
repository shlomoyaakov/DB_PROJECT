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
            try
            {
                Boolean result = false;
                using (MySqlConnection conn = GetConnection())
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

        public void Delete_User(User user)
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
                        myCommand.CommandText = $"delete from users_trip where user_name=\"{user.User_Name}\";";
                        myCommand.ExecuteNonQuery();
                        myCommand.CommandText = $"delete from users where user_name=\"{user.User_Name}\";";
                        myCommand.ExecuteNonQuery();
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
    }
}
