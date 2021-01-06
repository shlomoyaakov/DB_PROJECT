using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models.Data_Class
{
    public class Stats
    {
        public Stats()
        {
            Data = new Dictionary<string, Int64>();
            Data["Attractions"] = 0;
            Data["Restaurants"] = 0;
            Data["Accommodation"] = 0;
            Data["Trips"] = 0;
        }
        public Region General_Location
        {
            get;
            set;
        }
        public Dictionary<string, Int64> Data
        {
            get;
            set;
        }
    }
}
