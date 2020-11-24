using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models
{
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
