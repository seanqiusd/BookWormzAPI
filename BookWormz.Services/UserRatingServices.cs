using BookWormz.Data;
using BookWormz.Models.UserRatingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Services
{
    class UserRatingServices
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public bool CreateRating(UserRatingCreate model)
        {
            UserReview entity = new UserReview
            {
                UserId = model.UserId,
                ExchangeId = model.ExchangeId,
                ExchangeRating = model.ExchangeRating
            };

            _context.UserReviews.Add(entity);
            return _context.SaveChanges() == 1;
        }

        public List<UserRatingListItem> GetAllUserRatings()
        {
            var RatingEntities = _context.UserReviews.ToList();
            var RatingList = RatingEntities.Select(r => new UserRatingListItem
            {
                UserId = r.UserId,
                ExchangeId = r.ExchangeId,
                ExchangeRating = r.ExchangeRating
            }).ToList();
            return RatingList;
        }

        public List<UserRatingDetail> GetUserRatingsByUserId(string userId)
        {
            var RatingEntities = _context.UserReviews.ToList();
            var RatingList = RatingEntities.Where(r => r.UserId == userId).Select(r => new UserRatingDetail
            {
                UserId = r.UserId,
                ExchangeId = r.ExchangeId,
                ExchangeRating = r.ExchangeRating
            }).ToList();
            return RatingList;
        }

        public UserRatingDetail GetRatingOfExchange(int exchangeId)
        {
            var RatingEntities = _context.UserReviews.ToList();
            var RatingEntity = RatingEntities.Where(r => r.ExchangeId == exchangeId).First();
            var rating = new UserRatingDetail
            {
                UserId = RatingEntity.UserId,
                ExchangeId = RatingEntity.ExchangeId,
                ExchangeRating = RatingEntity.ExchangeRating
            };
            return rating;
        }
    }
}
