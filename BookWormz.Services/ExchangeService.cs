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
        private readonly Guid _userId;

        public ExchangeService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateExchange(ExchangeCreate model)
        {
            
            var entity =
                new Exchange()
                {
                    Id = model.Id,
                    BookId = model.BookId,
                    Posted = model.Posted,
                    SentDate = model.SentDate
                };

            using (var ctx = new ApplicationDbContext())
            {
                entity.ReceiverUser = ctx.Users.Where(e => e.Id = _userId).First();
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
                    .Where(e => e.ReceiverId == _userId)
                    .Select(
                        e =>
                    new ExchangeListItem
                    {
                        BookId = e.Id,
                        Posted = e.Posted,
                        SentDate = e.SentDate
                    }
                    );
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
