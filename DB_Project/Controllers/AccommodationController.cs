﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB_Project.Models;
using DB_Project.Models.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace DB_Project.Controllers
{
    /*
     * Accommodation controller is the controller that activate
     * the different classes in the model that related to accommodation
     * table in the database.
     * This controllers provides us api for getting and inserting accommodation,
     * and some details about the number of accommodation depending on the region.
     */
    [Route("api/[controller]")]
    [ApiController]
    public class AccommodationController : ControllerBase
    {

        private AccommodationContext context;
        public AccommodationController(AccommodationContext acc_context)
        {
            this.context = acc_context;
        }


        /*
         * This function return all of the accommodation in certain city and country.
         * The controller activate the right function in accommodationcontext objcet and return the
         * answer. In case there are no exception.
         */
        [HttpGet("region")]
        public ActionResult<List<Accommodation>> Get_Accommodation_By_Region([FromQuery] string country, [FromQuery] string city)
        {
            List<Accommodation> acc_list;
            try
            {
                acc_list = context.Get_Accommodation_By_Region(country, city);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(acc_list);
        }
        /*This function returns all accommodation from certain city and country that the user has not
         * visited yet.
         */
        [HttpGet("region_and_user")]
        public ActionResult<List<Accommodation>> Get_Accommodation_By_Region_And_User([FromQuery] string country, [FromQuery] string city, [FromQuery] string user_name)
        {
            List<Accommodation> acc_list;
            try
            {
                acc_list = context.Get_Accommodation_By_Region_And_User(country, city, user_name);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(acc_list);
        }

        

        /*
         * With this function we can insert new accommodation to our database,
         * using the accommodationcontex.
         */
        [HttpPost]
        public IActionResult Post([FromBody] Accommodation accommodation)
        {
            try
            {
                context.Add_Accomodation(accommodation);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        /* This function gets us the the amount of visitior in each accommodation in certain city and country,
         * sorted by the amount of visitor in each accommodation.
         * we identify the accommodation by its accommdation id.
         * The function return a sorted list of KeyValuePair<int, Int64> where int is the accommodation id
         * and int64 is the amount of visitors.
         */

        [HttpGet("travelers_by_region")]
        public ActionResult<List<KeyValuePair<int, Int64>>> Get_Travelers_Amount_By_Region([FromQuery] string country, [FromQuery] string city)
        {
            try
            {
                return Ok(context.Get_Amount_By_Region(country,city));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        /// <summary>
        /// API for updating a specific accommodation in the database
        /// </summary>
        /// <param name="acc": a list that contains two accommdation the previous one and the
        /// new one with the updated values></param>
        /// <returns>status ok if the values were updated succsefuly</returns>
        [HttpPost("update")]
        public IActionResult Update([FromBody] List<Accommodation> acc)
        {
            if (acc.Count() != 2)
            {
                return BadRequest("There should be prev and new accommodation");
            }
            if(acc[0].ID != acc[1].ID)
            {
                return BadRequest("The IDs of the previous and the new accommodation do not match");
            }
            try
            {
                context.Update(acc[0], acc[1]);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        /// <summary>
        /// Api for deletion a specific accommodation
        /// </summary>
        /// <param name="acc"> the accommodation that we want to delete </param>
        /// <returns> status ok if the deletion went succsefuly otherwise badrequest</returns>
        [HttpDelete]
        public IActionResult Delete([FromBody] Accommodation acc)
        {
            try
            {
                context.Delete(acc);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }
    }
}
