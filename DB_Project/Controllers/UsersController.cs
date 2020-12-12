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
        public UsersController(UsersContext usr_context)
        {
            this.context = usr_context;
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


