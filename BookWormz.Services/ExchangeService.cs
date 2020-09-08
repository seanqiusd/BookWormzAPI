using BookWormz.Data;
using BookWormz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Services
{
    public class ExchangeService
    {
        private readonly string _userId;

        public ExchangeService(string userId)
        {
            _userId = userId;
        }

        public bool CreateExchange(ExchangeCreate model)
        {

            var entity =
                new Exchange()
                {
                    BookId = model.BookId,
                    Posted = DateTime.Now,
                    SentDate = model.SentDate,
                    ReceiverId = model.ReceiverUser,
                    IsAvailable = true
                };

            using (var ctx = new ApplicationDbContext())
            {
                entity.SenderUser = ctx.Users.Where(e => e.Id == _userId).First();
                ctx.Exchanges.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<ExchangeListItem> GetExchanges()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Exchanges
                    .Select(
                        e =>
                    new ExchangeListItem
                    {
                        Id = e.Id,
                        BookId = e.BookId,
                        Posted = e.Posted,
                        SentDate = e.SentDate,
                        //ReceiverId = e.ReceiverId,
                        ReceiverId = e.ReceiverUser.FirstName
                    }
                    ) ;
                return query.ToArray();
            }
        }

        public ExchangeDetail GetExchangeById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Exchanges
                    .Single(e => e.Id == id);
                return
                    new ExchangeDetail
                    {
                        Id = entity.Id,
                        BookId = entity.BookId,
                        Posted = entity.Posted,
                        SentDate = entity.SentDate
                    };
            }
        }
    }
}
