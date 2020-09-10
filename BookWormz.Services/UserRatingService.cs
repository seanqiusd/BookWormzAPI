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

        public int CreateRating(UserRatingCreate model)
        {
            var exchange = _context.Exchanges.Single(e => e.Id == model.ExchangeId);
            //If Reciever Id is null the exchange has nto been completed
            if (exchange.ReceiverId is null)
                return 2;
            //Only the reciever should be able to rate exchange
            if (_userId != exchange.ReceiverId)
                return 3;
            //Every exchange Should Only Have One Rating
            if (_context.UserRatings.Where(r => r.ExchangeId == model.ExchangeId).Count() >= 1)
                return 4;


            UserRating entity = new UserRating
            {
                UserId = _context.Exchanges.Single(e => e.Id == model.ExchangeId).SenderId,
                ExchangeId = model.ExchangeId,
                ExchangeRating = model.ExchangeRating
            };

            _context.UserRatings.Add(entity);
            if (_context.SaveChanges() == 1)
                return 0;
            else return 1;
        }

        public List<UserRatingListItem> GetAllUserRatings()
        {
            var RatingEntities = _context.UserRatings.ToList();
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
            var RatingEntities = _context.UserRatings.ToList();
            var RatingList = RatingEntities.Where(r => r.UserId == userId).Select(r => new UserRatingDetail
            {
                UserId = r.UserId,
                ExchangeId = r.ExchangeId,
                ExchangeRating = r.ExchangeRating
            }).ToList();
            return RatingList;
        }

        public UserRatingDetail GetRatingOfExchange(int Id)
        {
            var RatingEntities = _context.UserRatings.ToList();
            var RatingEntity = RatingEntities.Single(r => r.Id == Id);
            var rating = new UserRatingDetail
            {
                UserId = RatingEntity.UserId,
                ExchangeId = RatingEntity.ExchangeId,
                ExchangeRating = RatingEntity.ExchangeRating
            };
            return rating;
        }

        public int UpdateUserRating(UserRatingUpdate model, int id)
        {
            var entity = _context.UserRatings.Single(e => e.Id == id);

            if (entity is null)
                return 2;
            if (entity.Exchange.ReceiverId != _userId)
                return 3;

            entity.ExchangeRating = model.ExchangeRating;
            entity.ExchangeId = model.ExchangeId;

            if (_context.SaveChanges() == 1)
                return 0;
            return 1;
        }

        public bool DeleteUserRating(int id)
        {
            var entity = _context.UserRatings.Single(e => e.Id == id);
            _context.UserRatings.Remove(entity);

            return _context.SaveChanges() == 1;
        }
    }
}
