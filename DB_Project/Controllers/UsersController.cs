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
    public class UsersController : ControllerBase
    {
        private UsersContext context;
        private TripContext t_context;
        public UsersController(UsersContext usr_context, TripContext trip_context)
        {
            this.context = usr_context;
            this.t_context = trip_context;
        }


        [HttpGet]
        public IActionResult IsExists([FromQuery]string username, [FromQuery]string password)
        {
            User user = new User();
            user.User_Name = username;
            user.Password = password;
            Boolean isExists;
            try
            {
                isExists = context.IsExists(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(isExists);
        }

        [HttpGet("admin")]
        public IActionResult IsAdmin([FromQuery] string username, [FromQuery] string password)
        {
            User user = new User
            {
                User_Name = username,
                Password = password
            };
            Boolean IsAdmin;
            try
            {
                IsAdmin = context.IsAdmin(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(IsAdmin);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            try
            {
                context.Add_User(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] User user)
        {
            try
            {
                if (!context.IsExists(user))
                {
                    throw new Exception("The user_name or password isn't correct");
                }
                t_context.Delete_Users_Trips(user.User_Name);
                context.Delete_User(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

    }
}


