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
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

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

            if (entity.ReceiverId != null)
                entity.IsAvailable = false;

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
                        IsAvailable = e.IsAvailable,
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
                var detailedExchange = 
                    new ExchangeDetail
                    {
                        Id = entity.Id,
                        BookId = entity.BookId,
                        Posted = entity.Posted,
                        SentDate = entity.SentDate
                    };
                foreach (Comment comment in entity.Comments)
                {
                    if (comment is Reply)
                        continue;
                    var commentDetail = new CommentDetail { Id = comment.Id, Text = comment.Text, CommentorsName = comment.Commenter.FullName };
                    foreach (Reply reply in comment.Replies)
                        commentDetail.Replies.Add(new ReplyDetail { Id = reply.Id, Text = reply.Text });
                    detailedExchange.Comments.Add(commentDetail);
                }
                return detailedExchange;
            }
        }



        // Get available books by state
        public List<ExchangeListItem> GetExchangesByState(string state)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entities = ctx.Exchanges.Where(e => e.SenderUser.State.ToLower() == state.ToLower());
                List<ExchangeListItem> exchanges = new List<ExchangeListItem>();
                foreach (var entity in entities)
                {
                    var exchangeListItem =
                       new ExchangeListItem
                       {
                           Id = entity.Id,
                           BookId = entity.BookId,
                           Posted = entity.Posted,
                           SentDate = entity.SentDate
                       };
                    exchanges.Add(exchangeListItem);
                }
                return exchanges;
            }
        }

        public int UpdateExchangeById(int id, ExchangeCreate newExchange)
        {
            Exchange exchange = _context.Exchanges.Find(id);
            if (exchange == null)
            {
                return 2;
            }
            if (exchange.SenderId != _userId)
            {
                return 3;
            }
            
            exchange.BookId = newExchange.BookId;
            exchange.Posted = newExchange.Posted;
            exchange.SentDate = newExchange.SentDate;
            exchange.ReceiverId = newExchange.ReceiverUser;

            if (newExchange.ReceiverUser == null)
                exchange.IsAvailable = true;
            else
                exchange.IsAvailable = false;
            
            var num = _context.SaveChanges();
            if (num == 1)           
                return 0;
            //If no changes are saved
            if (num == 0)
                return 4;
            return 1;
        }
        public int RequestExchange(int id)
        {
            Exchange exchange = _context.Exchanges.Find(id);
            if (exchange == null)
            {
                return 2;
            }
            if (exchange.IsAvailable == false)
                return 3;

            ////Stops user from requesting their own book. 
            ////Commented out for Testing purposes
            //if (exchange.SenderId == _userId)
            //    return 4;

            exchange.IsAvailable = false;
            exchange.ReceiverId = _userId;
            return _context.SaveChanges() == 1 ? 0 : 1;
        }

        public int DeleteExchangeById(int id)
        {
            Exchange exchange = _context.Exchanges.Find(id);

            if (exchange == null)
            {
                return 2;
            }

            _context.Exchanges.Remove(exchange);
            if (_context.SaveChanges() == 1)
            {
                return 0;
            }
            return 1;
        }
    }
}
