using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DB_Project.Models;


namespace DB_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private RegionContext context;
        public RegionController(RegionContext region_context)
        {
            this.context = region_context;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public ActionResult<List<Region>> Get()
        {
            List<Region> region_list;
            try
            {
                region_list = context.Get_All_Countries();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return region_list;
        }

        [HttpGet]
        public ActionResult<List<Region>> Get_Cities_In_Country([FromQuery] string country)
        {
            List<Region> region_list;
            try
            {
                region_list = context.Get_All_Cities_In_Country(country);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return region_list;
        }

    }
}
