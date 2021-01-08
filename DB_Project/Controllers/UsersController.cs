using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB_Project.Models;
using DB_Project.Models.Contexts;
using Microsoft.AspNetCore.Mvc;


/// <summary>
/// Controller that provides the API for requests that involve users.
/// </summary>
namespace DB_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UsersContext context;
        public UsersController(UsersContext usr_context)
        {
            this.context = usr_context;
        }

        /// <summary>
        /// Check if user exists in the data base
        /// </summary>
        /// <param name="username">The user name</param>
        /// <param name="password">The user password</param>
        /// <returns></returns>
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

        /// <summary>
        /// Function that checks if a user is an admin user
        /// </summary>
        /// <param name="username">The admin name</param>
        /// <param name="password">The admin password</param>
        /// <returns>True if the user is admin otherwise false</returns>
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

        /// <summary>
        /// API for adding a new user to the databse
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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

        /// <summary>
        /// With this function we can delete a certain user
        /// </summary>
        /// <param name="user"> The user that we want to delete</param>
        /// <returns>Actionresults that specify wether the deletion was successful</returns>
        [HttpDelete]
        public IActionResult Delete([FromBody] User user)
        {
            try
            {
                if (!context.IsExists(user))
                {
                    throw new Exception("The user_name or password isn't correct");
                }
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


