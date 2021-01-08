using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models
{
    // A class that holds with holds coordinates and its country and city (region)
    public class Location
    {
        public Region General_Location
        {
            get;
            set;
        }

        public Coordinates Coordinates
        {
            get;
            set;
        }
    }
}
