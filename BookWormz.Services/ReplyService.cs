using BookWormz.Data;
using BookWormz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Services
{
    public class ReplyService
    {
        private readonly string _userId;

        public ReplyService(string userId)
        {
            _userId = userId;
        }

        // Post --Create a reply
        public bool CreateReply(ReplyCreate model)
        {
            var entity = new Reply()
            {
                Text = model.CommentText,
                CommentId = model.CommentId,
                //Using user Id to identify commenter
                CommenterId = _userId
            };

            using (var ctx = new ApplicationDbContext())
            {
                // grabbing exchange Id from comment
                entity.ExchangeId = ctx.Comments.Single(e => e.Id == entity.CommentId).ExchangeId;                

                ctx.Comments.Add(entity);
                return ctx.SaveChanges() == 1; 

            }

        }

        // Get --Retrieve replies 
        public IEnumerable<ReplyListItem> GetReplies()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Replies.ToList()
                    .Select(
                        e =>
                        new ReplyListItem
                        {
                            ExchangeId = e.ExchangeId,
                            Text = e.Text,
                            RepliersName = e.Commenter != null ? e.Commenter.FullName : "unknown"
                        }
                        );
                return query.ToArray();

            }
        }

        // Get --Retrieve Reply Detail by id
        public ReplyDetail GetReplyDetail(int Id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Replies.Single(e => e.Id == Id);

                var detailedReply = new ReplyDetail
                {
                    Id = entity.Id,
                    Text = entity.Text,
                    CommentorsName = entity.Commenter != null ? entity.Commenter.FullName : "unknown",
                    Replies = AddReplies(entity.Replies)
                };
                return detailedReply;
            }
        }

        // Delete --remove a reply
        public bool DeleteReply(int Id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Replies.Single(e => e.Id == Id);
                ctx.Comments.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }



        //Recursive function to Add replies
        private List<ReplyDetail> AddReplies(ICollection<Reply> replies)
        {
            if (replies.Count == 0)
                return new List<ReplyDetail>();

            var DetailedReplies = new List<ReplyDetail>();

            foreach (var reply in replies)
            {
                var DetailedReply = new ReplyDetail
                {
                    Id = reply.Id,
                    Text = reply.Text,
                    //Using ternary incase of comment not having author(corrupt data)
                    CommentorsName = (reply.Comment != null ? reply.Commenter.FullName : "Unknown")
                };
                DetailedReply.Replies = AddReplies(reply.Replies);
                DetailedReplies.Add(DetailedReply);
            }
            return DetailedReplies;
        }
    }
}
