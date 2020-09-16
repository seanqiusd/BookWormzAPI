using BookWormz.Data;
using BookWormz.Models;
using BookWormz.Models.ExchangeModels;
using BookWormz.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookWormz.WebApi.Controllers
{
    /// <summary>
    /// Crud For Exchange Entities
    /// </summary>
    [Authorize]
    public class ExchangeController : ApiController
    {
        private ExchangeService CreateExchangeService()
        {
            var userId = User.Identity.GetUserId();
            var exchangeService = new ExchangeService(userId);
            return exchangeService;
        }

        // Get all
        /// <summary>
        /// Get All Exchange Items
        /// </summary>
        /// <returns>Returns ExchangeListItem Models</returns>
        public IHttpActionResult Get()
        {
            ExchangeService exchangeService = CreateExchangeService();
            var exchanges = exchangeService.GetExchanges();
            return Ok(exchanges);
        }


        // Create
        /// <summary>
        /// Post New Exchange
        /// </summary>
        /// <param name="exchange"></param>
        /// <returns></returns>
        public IHttpActionResult Post(ExchangeCreate exchange)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreateExchangeService();

            if (!service.CreateExchange(exchange))
                return InternalServerError();

            return Ok();
        }


        // Get by ID
        /// <summary>
        /// Get Detailed Information About Individual Exchange
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ExchangeDetail model</returns>
        public IHttpActionResult Get(int id)
        {
            ExchangeService exchangeService = CreateExchangeService();
            var exchange = exchangeService.GetExchangeById(id);
            return Ok(exchange);
        }

        //Get by State
        /// <summary>
        /// Get List Of exchanges By State
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(string state)
        {
            var exchangeService = CreateExchangeService();
            var exchanges = exchangeService.GetExchangesByState(state);
            return Ok(exchanges);
        }


        // Put by ID
        /// <summary>
        /// Update Exchange By Exchange ID
        /// </summary>
        /// <param name="id">Id of exchange to update</param>
        /// <param name="newExchange">Updated exchange information</param>
        /// <returns></returns>
        public IHttpActionResult Put([FromUri] int id, [FromBody] ExchangeUpdate newExchange)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var service = CreateExchangeService();
            switch (service.UpdateExchangeById(id, newExchange))
            {
                case 0:
                    return Ok("Exchange Updated");
                case 1:
                    return InternalServerError();
                case 2:
                    return BadRequest("Exchange Not Found");
                case 3:
                    return BadRequest("You cannot update other users exchanges");
                case 4:
                    return BadRequest("No Changes Made");
                default:
                    return InternalServerError();
            }
        }

        //Request Book By ExchangeId
        /// <summary>
        /// Request Available Exchange
        /// </summary>
        /// <param name="id">Id of requested exchange</param>
        /// <returns></returns>
        [Route("api/Exchange/ExchangeRequest")]
        [HttpPut]
        public IHttpActionResult RequestExchange(int id)
        {
            var service = CreateExchangeService();
            switch (service.RequestExchange(id))
            {
                case 0:
                    return Ok("Book Requested");

                case 1:
                    return InternalServerError();

                case 2:
                    return NotFound();

                case 3:
                    return BadRequest("Book Not available");

                case 4:
                    return BadRequest("You Cannot Request Your Own Book");

                default:
                    return InternalServerError();
            }
        }


        // Delete by ID
        /// <summary>
        /// Delete Exchange By ID
        /// </summary>
        /// <param name="id">Id of exchange to be deleted</param>
        /// <returns></returns>
        public IHttpActionResult Delete(int id)
        {
            var service = CreateExchangeService();

            switch (service.DeleteExchangeById(id))
            {
                case 0:
                    return Ok("Exchange has been deleted");
                case 1:
                    return InternalServerError();
                case 2:
                    return NotFound();
                default:
                    return InternalServerError();
            }
        }
    }
}
