using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models
{
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

        public int ID
        {
            get;
            set;
        }
    }
}
