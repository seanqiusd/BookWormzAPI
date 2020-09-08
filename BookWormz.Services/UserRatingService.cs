using BookWormz.Data;
using BookWormz.Models.UserRatingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Services
{
    public class UserRatingService
    {
        private readonly string _userId;
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public UserRatingService(string userId)
        {
            _userId = userId; //To Be used later for verifying Reviewer info.
        }

        public bool CreateRating(UserRatingCreate model)
        {
            //TODO Add functionality so only one review per exchange
            UserReview entity = new UserReview
            {
                UserId = _context.Exchanges.Single(e => e.Id == model.ExchangeId).SenderId,
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
            var RatingEntity = RatingEntities.Single(r => r.ExchangeId == exchangeId);
            var rating = new UserRatingDetail
            {
                UserId = RatingEntity.UserId,
                ExchangeId = RatingEntity.ExchangeId,
                ExchangeRating = RatingEntity.ExchangeRating
            };
            return rating;
        }

        public bool UpdateUserRating(UserRatingUpdate model)
        {
            var entity = _context.UserReviews.Single(e => e.ExchangeId == model.ExchangeId);

            entity.ExchangeRating = model.ExchangeRating;

            return _context.SaveChanges() == 1;
        }

        public bool DeleteUserRating(int id)
        {
            var entity = _context.UserReviews.Single(e => e.Id == id);
            _context.UserReviews.Remove(entity);

            return _context.SaveChanges() == 1;
        }
    }
}
