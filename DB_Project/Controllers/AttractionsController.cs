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


        [HttpGet]
        public ActionResult<List<Attraction>> Get()
        {
            List<Attraction> att_list;
            try
            {
                att_list = context.GetAllAttractions();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(att_list);
        }

        [HttpGet("location")]
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

        [HttpGet("details")]
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

        [HttpGet("(amount)")]
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
    }
}
