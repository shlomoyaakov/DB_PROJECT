using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB_Project.Models.Contexts;
using DB_Project.Models.Data_Class;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DB_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private TripContext context;
        public TripsController(TripContext trip_context)
        {
            this.context = trip_context;
        }
        [HttpGet]
        public ActionResult<List<Trip>> Get()
        {
            return null;
        }

        // GET api/<TripsController>/5
        [HttpGet("{id}")]
        public ActionResult<Trip> Get(int id)
        {
            try
            {
                return context.Get_Trip_By_Id(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("user")]
        public ActionResult<List<Trip>> Get([FromQuery] string user_name)
        {
            try
            {
                return context.Get_Trips_By_User_Name(user_name);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // POST api/<TripsController>
        [HttpPost]
        public IActionResult Post([FromBody] Trip trip)
        {
            try
            {
                context.Add_Trip(trip);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
            return Ok();

        }
    }
}
