using BookWormz.Data;
using BookWormz.Models;
using BookWormz.Models.ExchangeModels;
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
                        BookTitle = entity.Book.BookTitle,
                        //using ternaries incase of null senderUser
                        PostingUser = entity.SenderUser != null ? entity.SenderUser.FullName : null,
                        PostersRating = entity.SenderUser != null ? entity.SenderUser.ExchangeRating : null,
                        Posted = entity.Posted,
                        SentDate = entity.SentDate
                    };
                foreach (Comment comment in entity.Comments)
                {
                    //If comment is Reply it will attach to its comment so can be skipped
                    if (comment is Reply)
                        continue;
                    //Add comments and their replies
                    var commentDetail = new CommentDetail { Id = comment.Id, Text = comment.Text,
                        //Using ternary incase of null commenter
                        CommentersName = comment.Commenter != null ? comment.Commenter.FullName : "Unknown",
                        //Using recursive Method to populate replies
                        Replies = AddReplies(comment.Replies) };
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

        public int UpdateExchangeById(int id, ExchangeUpdate newExchange)
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

            //Using null-coalescing operator ?? to check if information entered to update
            exchange.BookId = newExchange.BookId ?? exchange.BookId;
            exchange.Posted = newExchange.Posted ?? exchange.Posted;
            exchange.SentDate = newExchange.SentDate ?? exchange.SentDate;
            exchange.ReceiverId = newExchange.ReceiverId ?? exchange.ReceiverId;

            if (newExchange.ReceiverId == null)
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

        //Recursive function to Add replies
        private List<ReplyDetail> AddReplies(ICollection<Reply> replies)
        {
            if (replies.Count == 0)
                return new List<ReplyDetail>();

            var DetailedReplies = new List<ReplyDetail>();

            foreach (var reply in replies)
            {
                var DetailedReply = new ReplyDetail { Id = reply.Id, Text = reply.Text,
                    CommentorsName = reply.Commenter != null ? reply.Commenter.FullName : "Unknown"};
                DetailedReply.Replies = AddReplies(reply.Replies);
                DetailedReplies.Add(DetailedReply);
            }
            return DetailedReplies;
        }
    }
}
