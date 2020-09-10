using BookWormz.Data;
using BookWormz.Models;
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
    /// CRUD for Exchange entities
    /// </summary>
    public class ExchangeController : ApiController
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        private ExchangeService CreateExchangeService()
        {
            var userId = User.Identity.GetUserId();
            var exchangeService = new ExchangeService(userId);
            return exchangeService;
        }

        // Get all
        /// <summary>
        /// Get All Exchange items
        /// </summary>
        /// <returns>returns ExchangeListItem models</returns>
        public IHttpActionResult Get()
        {
            ExchangeService exchangeService = CreateExchangeService();
            var exchanges = exchangeService.GetExchanges();
            return Ok(exchanges);
        }


        // Create
        /// <summary>
        /// Post new exchange
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
        /// Get detailed information about individual exchange
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ExchangeDetail model</returns>
        public IHttpActionResult Get(int id)
        {
            ExchangeService exchangeService = CreateExchangeService();
            var exchange = exchangeService.GetExchangeById(id);
            return Ok(exchange);
        }


        // Put by ID
        /// <summary>
        /// Update exchange by exchange ID
        /// </summary>
        /// <param name="id">Id of exchange to update</param>
        /// <param name="newExchange">Updated exchange information</param>
        /// <returns></returns>
        public IHttpActionResult Put([FromUri] int id, [FromBody] Exchange newExchange)
        {
            if (ModelState.IsValid)
            {
                Exchange exchange = _context.Exchanges.Find(id);
                if (exchange == null)
                {
                    return BadRequest("Exchange not found");
                }
                exchange.BookId = newExchange.BookId;
                exchange.Posted = newExchange.Posted;
                exchange.SentDate = newExchange.SentDate;

                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        //Request Book By ExchangeId
        /// <summary>
        /// Request available exchange
        /// </summary>
        /// <param name="id">Id of requested exchange</param>
        /// <returns></returns>
        [Route("api/Exchange/ExchangeRequest")]
        [HttpPut]
        public IHttpActionResult RequestExchange(int id)
        {
            Exchange exchange = _context.Exchanges.Find(id);
            if (exchange == null)
            {
                return BadRequest("Exchange not found");
            }
            if (exchange.IsAvailable == false)
                return BadRequest("Book Not available");

            exchange.IsAvailable = false;
            exchange.ReceiverId = User.Identity.GetUserId();
            _context.SaveChanges();
            return Ok("Book Requested");
        }


        // Delete by ID
        /// <summary>
        /// Delete Exchange by ID
        /// </summary>
        /// <param name="id">Id of exchange to be deleted</param>
        /// <returns></returns>
        public IHttpActionResult Delete(int id)
        {
            Exchange exchange = _context.Exchanges.Find(id);

            if (exchange == null)
            {
                return NotFound();
            }

            _context.Exchanges.Remove(exchange);
            if (_context.SaveChanges() == 1)
            {
                return Ok("Exchange has been deleted");
            }
            return InternalServerError();
        }

    }
}
