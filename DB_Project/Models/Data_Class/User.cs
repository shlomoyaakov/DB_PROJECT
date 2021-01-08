using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models
{
    // A class that represents the users table in the database
    public class User
    {
        public string User_Name { get; set; }
      
        public string Password { get; set; }

        public Boolean Admin { get; set; }

    }
}
