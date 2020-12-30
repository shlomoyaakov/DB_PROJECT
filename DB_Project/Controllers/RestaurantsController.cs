using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB_Project.Models;
using DB_Project.Models.Contexts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DB_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {

        private RestaurantsContext context;
        public RestaurantsController(RestaurantsContext res_context)
        {
            this.context = res_context;
        }


        [HttpGet]
        public ActionResult<List<Restaurant>> Get()
        {
            List<Restaurant> res_list;
            try
            {
                res_list = context.GetALLRestaurants();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(res_list);
        }

        [HttpGet("location")]
        public ActionResult<List<Restaurant>> Get_Restaurants_By_Region([FromQuery] string country, [FromQuery] string city)
        {
            List<Restaurant> res_list;
            try
            {
                res_list = context.Get_Restaurants_By_Region(country, city);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(res_list);
        }

        [HttpGet("details")]
        public ActionResult<List<Restaurant>> Get_Restaurants_By_Region_And_User([FromQuery] string country, [FromQuery] string city, [FromQuery] string user_name)
        {
            List<Restaurant> res_list;
            try
            {
                res_list = context.Get_Restaurants_By_Region_And_User(country, city, user_name);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(res_list);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Restaurant restaurant)
        {
            try
            {
                context.Add_Restaurant(restaurant);
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
