using BookWormz.Data;
using BookWormz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Services
{
    public class CommentService
    {
        private readonly string _userId;

        public CommentService(string userId)
        {
            _userId = userId;
        }

        // Post --Create a comment
        public bool CreateComment(CommentCreate model)
        {
            var entity =
               new Comment()
               {
                  CommenterId = _userId, // this is cool
                  ExchangeId = model.ExchangeId,
                  Text = model.CommentText
               };

            using (var ctx = new ApplicationDbContext())
            {
                foreach (Comment comment in ctx.Comments)
                    if (comment.Id == entity.ExchangeId)
                        return false;

                ctx.Comments.Add(entity);
                return ctx.SaveChanges() == 1; // Should this be set to 2, socmedia did this for some reason
                    
            }
        }

        // Get --Retrieve comments and in theory should also return list of replies
        public IEnumerable<CommentListItem> GetComments()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Comments.ToList()
                    .Select(
                        e =>
                        {

                            var listItem = new CommentListItem
                            {
                                Id = e.Id,
                                ExchangeId = e.ExchangeId,
                                Text = e.Text,
                            };
                            foreach (var reply in e.Replies)
                            {
                                var r = new ReplyDetail
                                {
                                    Text = reply.Text
                                };
                                listItem.Replies.Add(r);
                            }
                            return listItem;
                        });
                return query.ToArray();
                        
            }
        }

        // Get --single comment by comment Id, and hopefully pulling all the replies with it, too
        public CommentDetail GetCommentDetail(int Id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Comments.Single(e => e.Id == Id);

                var detailedComment = new CommentDetail
                {
                    Id = entity.Id,
                    Text = entity.Text
                };
                foreach (Comment comment in entity.Replies)
                {
                    detailedComment.Replies.Add(new ReplyDetail{ Id = comment.Id, Text = comment.Text });
                }
                return detailedComment;
            }
        }

        // Delete a comment
        public bool DeleteComment(int Id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Comments.Single(e => e.Id == Id);
                ctx.Comments.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }


    }

}

