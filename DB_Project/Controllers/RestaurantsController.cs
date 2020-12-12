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
    }
}
