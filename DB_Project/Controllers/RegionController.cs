using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DB_Project.Models;


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
                region_list = context.GetAllRegions();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return region_list;
        }


        /* This function returns all of the cities inside the a certain country
         */
        [HttpGet("{country}")]
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

        /// <summary>
        /// This fuction gets us the amount of travelers in each country or each city in certain country
        /// </summary>
        /// 
        /// <param name="country"> the name of the country that we want to get the amount of travelers per
        /// city from </param>
        /// 
        /// <returns> A sorted list that consist of KeyValuePair<string, Int64> where string is
        /// the name of the country/city and int64 is tha amount of travelers </returns>
        [HttpGet("travelers_by_region")]
        public ActionResult<List<KeyValuePair<string, Int64>>> Get_Travelers_Amount_By_Region(string country=null)
        {
            try
            {
                if (country!=null)
                    return Ok(context.Get_Amount_By_City(country));
                return Ok(context.Get_Amount_By_Country());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
