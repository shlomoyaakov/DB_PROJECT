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
    /// <summary>
    ///The trips Controller provides the API for request that involve The trip object.
    /// </summary>
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

       /// <summary>
       /// Gets the trip with the specific id
       /// </summary>
       /// <param name="id">The trip id</param>
       /// <returns>Trip with the specific id</returns>
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

        /// <summary>
        /// Get all the trips that certain user has created.
        /// </summary>
        /// <param name="user_name">the user name</param>
        /// <returns>List of trips that the user created</returns>
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

        /// <summary>
        /// Add trip to the database
        /// </summary>
        /// <param name="trip">The trip that we want to add</param>
        /// <returns>Ok if could add the trip</returns>
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

        /// <summary>
        /// Delete the trip with this id
        /// </summary>
        /// <param name="id">the trip id</param>
        /// <returns></returns>
        [HttpDelete("id")]
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

        /// <summary>
        /// Delete all trips of certain user
        /// </summary>
        /// <param name="user"> the user that we want to delete his trips</param>
        /// <returns></returns>
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
