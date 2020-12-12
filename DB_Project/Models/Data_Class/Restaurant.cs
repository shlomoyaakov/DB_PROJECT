using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models
{
    public class Restaurant
    {
        public string Name
        {
            get;
            set;
        }

        public string Cuisine
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
    }
}
