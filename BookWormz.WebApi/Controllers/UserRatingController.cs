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
    /// <summary>
    /// CRUD for user ratings
    /// </summary>
    [Authorize]    
    public class UserRatingController : ApiController
    {

        private UserRatingService CreateRatingService()
        {
            var reviewService = new UserRatingService(User.Identity.GetUserId());
            return reviewService;
        }

        /// <summary>
        /// Used to Post New Rating
        /// </summary>
        /// <param name="rating"> Rating Paramaters </param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult PostRating(UserRatingCreate rating)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreateRatingService();

            switch (service.CreateRating(rating))
            {
                case 0:
                    return Ok("Rating Added");
                case 1:
                    return InternalServerError();
                case 2:
                    return BadRequest("Exchange has no RecieverId");
                case 3:
                    return BadRequest("Only the reciever can rate exchanges");
                case 4:
                    return BadRequest("Exchange has already been rated, if you would like to modify rating please use update method");
                default:
                    return InternalServerError();
            }
            
        }

        /// <summary>
        /// Get user Rating By exchange Id
        /// </summary>
        /// <param name="id">Exchange Id of rating to lookup</param>
        /// <returns>rating info of selected exchange</returns>
        [HttpGet]
        public IHttpActionResult GetRatingByExchangeId(int id)
        {
            var service = CreateRatingService();
            var rating = service.GetRatingOfExchangeByExchangeId(id);
            return Ok(rating);
        }

        /// <summary>
        /// Get Ratings for individual Users
        /// </summary>
        /// <param name="userId">Users user Id</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetRatingsByUserId(string userId)
        {
            var service = CreateRatingService();
            var ratings = service.GetUserRatingsByUserId(userId);
            return Ok(ratings);
        }

        /// <summary>
        /// Update Rating By Exchange Id
        /// </summary>
        /// <param name="id">Exchange Id to update rating for</param>
        /// <param name="updatedRating"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateUserRating([FromUri] int id, [FromBody] UserRatingUpdate updatedRating)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var service = CreateRatingService();

            switch (service.UpdateUserRatingByExchangeId(updatedRating, id))
            {
                case 0:
                    return Ok("Rating Added");
                case 1:
                    return InternalServerError();
                case 2:
                    return BadRequest("Rating not found");
                case 3:
                    return BadRequest("Only the reciever can update ratings");
                default:
                    return InternalServerError();
            }
        }

        /// <summary>
        /// Delete Rating by rating Id
        /// </summary>
        /// <param name="id">Rating Id to be deleted</param>
        /// <returns></returns>
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
