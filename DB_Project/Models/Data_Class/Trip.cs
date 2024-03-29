﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_Project.Models.Data_Class
{
    // A class that represents the trips tables from the database
    public class Trip
    {
        public Trip()
        {
            this.Accommodation = new List<Accommodation>();
            this.Attractions = new List<Attraction>();
            this.Restaurants = new List<Restaurant>();
        }
        public string User_Name
        {
            get;
            set;
        }
        public string Time
        {
            get;
            set;
        }
        
        public string Country
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }
        public List<Attraction> Attractions
        {
            get;
            set;
        }

        public List<Restaurant> Restaurants
        {
            get;
            set;
        }

        public List<Accommodation> Accommodation
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
