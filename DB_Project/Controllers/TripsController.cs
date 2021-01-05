using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB_Project.Models;
using DB_Project.Models.Contexts;
using DB_Project.Models.Data_Class;
using Microsoft.AspNetCore.Mvc;



namespace DB_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private TripContext context;
        private UsersContext u_context;
        public TripsController(TripContext trip_context, UsersContext usr_context)
        {
            this.context = trip_context;
            this.u_context = usr_context;
        }
        [HttpGet]
        public ActionResult<List<Trip>> Get()
        {
            return null;
        }

       
        [HttpGet("{id}")]
        public ActionResult<Trip> Get(int id)
        {
            try
            {
                return Ok(context.Get_Trip_By_Id(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("user")]
        public ActionResult<List<Trip>> Get([FromQuery] string user_name)
        {
            try
            {
                return Ok(context.Get_Trips_By_User_Name(user_name));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Trip trip)
        {
            try
            {
                context.Add_Trip(trip);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();

        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                this.context.Delete_Trip(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete_By_User([FromBody] User user)
        {
            try
            {
                if (!u_context.IsExists(user))
                    throw new Exception("There is no such user");
                this.context.Delete_Users_Trips(user.User_Name);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }
    }
}
