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
        public IHttpActionResult Get()
        {
            ExchangeService exchangeService = CreateExchangeService();
            var exchanges = exchangeService.GetExchanges();
            return Ok(exchanges);
        }


        // Create
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
        public IHttpActionResult Get(int id)
        {
            ExchangeService exchangeService = CreateExchangeService();
            var exchange = exchangeService.GetExchangeById(id);
            return Ok(exchange);
        }


        // Put by ID
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
        [Route("api/Exchange/ExchangeRequest")]
        [HttpPut]
        public IHttpActionResult RequestExchange([FromUri]int id)
        {
            Exchange exchange = _context.Exchanges.Find(id);
            if (exchange == null)
            {
                return BadRequest("Exchange not found");
            }
            exchange.IsAvailable = false;
            exchange.ReceiverId = User.Identity.GetUserId();
            _context.SaveChanges();
            return Ok("Book Requested");
        }


        // Delete by ID
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
