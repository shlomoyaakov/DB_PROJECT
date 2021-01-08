using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB_Project.Models;
using DB_Project.Models.Contexts;
using Microsoft.AspNetCore.Mvc;

/*
 * Attractions controller is the controller that activate
 * the different classes in the model that related to Attractions
 * table in the database.
 * This controllers provides us api for getting and inserting Attractions,
 * and some details about the number of Attractions depending on the region.
 */

namespace DB_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionsController : ControllerBase
    {
        private AttractionsContext context;
        public AttractionsController(AttractionsContext att_context)
        {
            this.context = att_context;
        }


        /*
        * This function return all of the attractions in certain city and country.
        * The controller activate the right function in attractionscontext objcet and return the
        * answer. In case there are no exception.
        */
        [HttpGet("region")]
        public ActionResult<List<Attraction>> Get_Attractions_By_Region([FromQuery] string country, [FromQuery] string city)
        {
            List<Attraction> att_list;
            try
            {
                att_list = context.Get_Attractions_By_Region(country, city);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(att_list);
        }

        /*This function returns all Attractions from certain city and country that the user has not
        * visited yet
        */
        [HttpGet("region_and_user")]
        public ActionResult<List<Attraction>> Get_Attractions_By_Region_And_User([FromQuery] string country, [FromQuery] string city, [FromQuery] string user_name)
        {
            List<Attraction> att_list;
            try
            {
                att_list = context.Get_Attractions_By_Region_And_User(country, city, user_name);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(att_list);
        }

        /*
        * With this function we can insert new Attractions to our database,
        * using the AttractionsnContex.
        */
        [HttpPost]
        public IActionResult Post([FromBody] Attraction attraction)
        {
            try
            {
                context.Add_Attraction(attraction);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }
        /* This function gets us the the amount of visitior in each attractions in certain city and country,
        * sorted by the amount of visitor in each attractions.
        * we identify the attractions by its attractions id.
        * The function return a sorted list of KeyValuePair<int, Int64> where int is the attractions id
        * and int64 is the amount of visitors.
        */
        [HttpGet("travelers_by_region")]
        public ActionResult<List<KeyValuePair<int, Int64>>> Get_Travelers_Amount_By_Region(string country, string city)
        {
            try
            {
                return Ok(context.Get_Amount_By_Region(country, city));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// API for updating a specific Attraction in the database
        /// </summary>
        /// <param name="att": a list that contains two Attraction the previous one and the
        /// new one with the updated values></param>
        /// <returns>status ok if the values were updated succsefuly</returns>
        [HttpPost("update")]
        public IActionResult Update([FromBody] List<Attraction> att)
        {
            if (att.Count() != 2)
            {
                return BadRequest("There should be prev and new attractions");
            }
            try
            {
                context.Update(att[0], att[1]);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        /// <summary>
        /// Api for deletion a specific attraction
        /// </summary>
        /// <param name="acc"> the attraction that we want to delete </param>
        /// <returns> status ok if the deletion went succsefuly otherwise badrequest</returns>
        [HttpDelete]
        public IActionResult Delete([FromBody] Attraction att)
        {
            try
            {
                context.Delete(att);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }
    }
}
