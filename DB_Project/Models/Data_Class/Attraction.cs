using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models
{
    // A class that represents the attractions table from the database
    public class Attraction
    {
        public string Name
        {
            get;
            set;
        }

        public string Phone
        {
            get; 
            set;
        }

        public Location Location
        {
            get;
            set;
        }

        // an attraction id from the table
        public int ID
        {
            get;
            set;
        }
    }
}
