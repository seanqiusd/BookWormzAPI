﻿using BookWormz.Data;
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
        private readonly Guid _userId;

        public ReplyService(Guid userId)
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
                ExchangeId = model.ExchangeId
            };

            using (var ctx = new ApplicationDbContext())
            {
                foreach (Reply reply in ctx.Replies)
                    if (reply.Id == entity.ExchangeId)
                        return false;

                ctx.Comments.Add(entity);
                return ctx.SaveChanges() == 1; // should this be set to == 2? we did this for socmedia for some reason

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
                    Text = entity.Text
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


    }
}