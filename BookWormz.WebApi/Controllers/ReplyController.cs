using BookWormz.Models;
using BookWormz.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookWormz.WebApi.Controllers
{

    /// <summary>
    /// Crud For Reply Entities
    /// </summary>
    public class ReplyController : ApiController
    {
        private ReplyService CreateReplyService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var replyService = new ReplyService(userId);
            return replyService;
        }

        // Post --Create reply to comment
        /// <summary>
        /// Create New Reply In Database
        /// </summary>
        /// <param name="reply"></param>
        /// <returns></returns>

        [HttpPost]
        public IHttpActionResult PostReplyToComment(ReplyCreate reply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var service = CreateReplyService();

            if (!service.CreateReply(reply))
                return InternalServerError();

            return Ok("Reply added");
        }

        // Get all replies
        /// <summary>
        /// Get All Replies
        /// </summary>
        /// <returns>Returns ReplyListItem Object Of All Replies In Database</returns>
        public IHttpActionResult Get()
        {
            ReplyService replyService = CreateReplyService();
            var comments = replyService.GetReplies();
            return Ok(comments);
        }

        // Get reply detail by Id
        /// <summary>
        /// Get Detailed Information From Single Reply Item By ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Returns ReplyDetail Model</returns>
        public IHttpActionResult Get(int Id)
        {
            ReplyService replyService = CreateReplyService();
            var reply = replyService.GetReplyDetail(Id);
            return Ok(reply);
        }

        // Delete reply
        /// <summary>
        /// Remove Reply From Database By ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IHttpActionResult Delete(int Id)
        {
            var service = CreateReplyService();
            if (!service.DeleteReply(Id))
            {
                return InternalServerError();
            }
            return Ok($"Reply Id: {Id} has been deleted");
        }

    }
}
