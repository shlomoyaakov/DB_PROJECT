using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DB_Project.Models;
using DB_Project.Models.Data_Class;

namespace DB_Project.Controllers
{
    /*
    * Region controller is the controller that activate
    * the different classes in the model that related to region
    * table in the database.
    * This controllers provides us api for getting region,
    * and some details about the number of region depending on the country/city.
    */
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private RegionContext context;
        public RegionController(RegionContext region_context)
        {
            this.context = region_context;
        }


        /*This function returns all of the cities countries we have in the database.
         * it returns it as a list of region object that consist of city and country
        */
        [HttpGet]
        public ActionResult<List<Region>> Get()
        {
            List<Region> region_list;
            try
            {
                region_list = context.Get_All_Countries();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return region_list;
        }


        /* This function returns all of the cities inside the a certain country
         */
        [HttpGet("country")]
        public ActionResult<List<Region>> Get_Cities_In_Country(string country)
        {
            List<Region> region_list;
            try
            {
                region_list = context.Get_All_Cities_In_Country(country);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return region_list;
        }

        [HttpGet("stats_per_region")]
        public ActionResult<List<Stats>> Get_Attractions_Amount_Per_Region(string country)
        {
            try
            {
                return Ok(context.Get_Stats_Per_Region(country));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
