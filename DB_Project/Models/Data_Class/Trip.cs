using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models.Data_Class
{
    public class Trip
    {
        public User Traveler
        {
            get;
            set;
        }
        public DateTime Time
        {
            get;
            set;
        }

        public Attraction[] Attractions
        {
            get;
            set;
        }

        public Restaurant[] Restaurants
        {
            get;
            set;
        }

        public Accommodation[] Accommodation
        {
            get;
            set;
        }
    }
}
