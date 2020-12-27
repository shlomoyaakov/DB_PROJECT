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
    public class AttractionsController : ControllerBase
    {
        private AttractionsContext context;
        public AttractionsController(AttractionsContext att_context)
        {
            this.context = att_context;
        }


        [HttpGet]
        public ActionResult<List<Attraction>> Get()
        {
            List<Attraction> att_list;
            try
            {
                att_list = context.GetAllAttractions();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(att_list);
        }

        [HttpGet("location")]
        public ActionResult<List<Attraction>> Get_Attractions_By_Region([FromQuery] string country, [FromQuery] string city)
        {
            List<Attraction> att_list;
            try
            {
                att_list = context.Get_Attractions_By_Region(country, city);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(att_list);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Attraction attraction)
        {
            try
            {
                context.Add_Attraction(attraction);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }
    }
}
