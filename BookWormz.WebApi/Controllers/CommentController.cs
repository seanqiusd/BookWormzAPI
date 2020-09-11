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
    public class CommentController : ApiController
    {
        private CommentService CreateCommentService()
        {
            var userId = User.Identity.GetUserId();
            var CommentService = new CommentService(userId);
            return CommentService;
        }

        // Get all comments
        public IHttpActionResult Get()
        {
            CommentService commentService = CreateCommentService();
            var comments = commentService.GetComments();
            return Ok(comments);
        }

        // Post --Create Comment
        public IHttpActionResult Post(CommentCreate comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var service = CreateCommentService();

            if (!service.CreateComment(comment))
            {
                return InternalServerError();
            }
            return Ok("Comment added");
        }

        // Get comment detail
        public IHttpActionResult Get(int Id)
        {
            CommentService commentService = CreateCommentService();
            var comment = commentService.GetCommentDetail(Id);
            return Ok(comment);
        }

        // Delete comment
        public IHttpActionResult Delete(int Id)
        {
            var service = CreateCommentService();
            if (!service.DeleteComment(Id))
            {
                return InternalServerError();
            }
            return Ok($"Comment Id: {Id} has been deleted");
        }




    }
}
