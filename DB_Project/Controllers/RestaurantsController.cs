using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB_Project.Models;
using DB_Project.Models.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace DB_Project.Controllers
{
    /*
    * Restaurants controller is the controller that activate
    * the different classes in the model that related to restaurants
    * table in the database.
    * This controllers provides us api for getting and inserting restaurants,
    * and some details about the number of restaurants depending on the region.
    */
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {

        private RestaurantsContext context;
        public RestaurantsController(RestaurantsContext res_context)
        {
            this.context = res_context;
        }

        /* This function reuturn all Restaurants that we have in database.
         */
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

        /*
        * This function return all of the Restaurants in certain city and country.
        * The controller activate the right function in Restaurantscontext objcet and return the
        * answer. In case there are no exception.
        */
        [HttpGet("region")]
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

        /*This function returns all Restaurants from certain city and country that the user has not
        * visited yet.
        */
        [HttpGet("region_and_user")]
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

        /*
        * With this function we can insert new Restaurants to our database,
        * using the RestaurantsContex.
        */
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

        /* This function gets us the the amount of visitior in each restaurants in certain city and country,
        * sorted by the amount of visitor in each restaurants.
        * we identify the restaurants by its restaurants id.
        * The function return a sorted list of KeyValuePair<int, Int64> where int is the restaurants id
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
        /// API for updating a specific Restaurant in the database
        /// </summary>
        /// <param name="acc": a list that contains two Restaurant the previous one and the
        /// new one with the updated values></param>
        /// <returns>status ok if the values were updated succsefuly</returns>
        [HttpPost("update")]
        public IActionResult Update([FromBody] List<Restaurant> ret)
        {
            if (ret.Count() != 2)
            {
                return BadRequest("There should be prev and new restaurants");
            }
            try
            {
                context.Update(ret[0], ret[1]);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }
        /// <summary>
        /// Api for deletion a specific restaurants
        /// </summary>
        /// <param name="ret"> the restaurants that we want to delete </param>
        /// <returns> status ok if the deletion went succsefuly otherwise badrequest</returns>
        [HttpDelete]
        public IActionResult Delete([FromBody] Restaurant ret)
        {
            try
            {
                context.Delete(ret);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }
    }
}
