using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models
{
    // A class that represents the accommodation class from the database
    public class Accommodation
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

        public string Internet
        {
            get;
            set;
        }

        // hotel/guest_house/ hostels etc..
        public string Type
        {
            get;
            set;
        }

        //an attraction id from the table
        public int ID
        {
            get;
            set;
        }
    }
}
