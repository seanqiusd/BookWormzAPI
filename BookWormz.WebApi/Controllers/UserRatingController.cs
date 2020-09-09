using BookWormz.Models.UserRatingModels;
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
    [Authorize]
    public class UserRatingController : ApiController
    {
        private UserRatingService CreateRatingService()
        {
            var reviewService = new UserRatingService(User.Identity.GetUserId());
            return reviewService;
        }

        [HttpPost]
        public IHttpActionResult PostRating(UserRatingCreate rating)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var service = CreateRatingService();

            if (!service.CreateRating(rating))
                return InternalServerError();
            return Ok("Rating added");
        }

        [HttpGet]
        public IHttpActionResult GetRatingById(int id)
        {
            var service = CreateRatingService();
            var rating = service.GetRatingOfExchange(id);
            return Ok(rating);
        }

        [HttpGet]
        public IHttpActionResult GetRatingsByUserId(string userId)
        {
            var service = CreateRatingService();
            var ratings = service.GetUserRatingsByUserId(userId);
            return Ok(ratings);
        }

        [HttpPut]
        public IHttpActionResult UpdateUserRating([FromUri] int id, [FromBody] UserRatingUpdate updatedRating)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var service = CreateRatingService();

            if (!service.UpdateUserRating(updatedRating, id))
                return InternalServerError();
            return Ok("Rating Updated");
        }

        [HttpDelete]
        public IHttpActionResult DeleteRating(int id)
        {
            var service = CreateRatingService();

            if (!service.DeleteUserRating(id))
                return InternalServerError();
            return Ok();
        }
    }
}
