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


        [HttpGet]
        public ActionResult<List<Accommodation>> Get()
        {
            List<Accommodation> acc_list;
            try {
                acc_list = context.GetAllAccommodation();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(acc_list);
        }
    }
}
