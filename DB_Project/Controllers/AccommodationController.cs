using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB_Project.Models;
using DB_Project.Models.Contexts;
using Microsoft.AspNetCore.Mvc;
//test
namespace DB_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccommodationController : ControllerBase
    {

        private AccommodationContext context;
        public AccommodationController(AccommodationContext acc_context)
        {
            this.context = acc_context;
        }



        [HttpGet("location")]
        public ActionResult<List<Accommodation>> Get_Accommodation_By_Region([FromQuery] string country, [FromQuery] string city)
        {
            List<Accommodation> acc_list;
            try
            {
                acc_list = context.Get_Accommodation_By_Region(country, city);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(acc_list);
        }

        [HttpGet("details")]
        public ActionResult<List<Accommodation>> Get_Accommodation_By_Region_And_User([FromQuery] string country, [FromQuery] string city, [FromQuery] string user_name)
        {
            List<Accommodation> acc_list;
            try
            {
                acc_list = context.Get_Accommodation_By_Region_And_User(country, city, user_name);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(acc_list);
        }

        [HttpGet]
        public ActionResult<List<Accommodation>> Get()
        {
            List<Accommodation> acc_list;
            try
            {
                acc_list = context.GetAllAccommodation();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(acc_list);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Accommodation accommodation)
        {
            try
            {
                context.Add_Accomodation(accommodation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [HttpGet("amount")]
        public ActionResult<List<KeyValuePair<int, Int64>>> Get_Travelers_Amount_By_Region([FromQuery] string country, [FromQuery] string city)
        {
            try
            {
                return Ok(context.Get_Amount_By_Region(country,city));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
