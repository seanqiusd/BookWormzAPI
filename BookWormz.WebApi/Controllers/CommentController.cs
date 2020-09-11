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
    /// Crud For Comment Entities
    /// </summary>
    public class CommentController : ApiController
    {
        private CommentService CreateCommentService()
        {
            var userId = User.Identity.GetUserId();
            var CommentService = new CommentService(userId);
            return CommentService;
        }

        // Get all comments
       /// <summary>
       /// Get All Comments
       /// </summary>
       /// <returns>Returns CommentListItem Object Of All Comments In Database</returns>
        
        public IHttpActionResult Get()
        {
            CommentService commentService = CreateCommentService();
            var comments = commentService.GetComments();
            return Ok(comments);
        }

        
        // Post --Create Comment
        /// <summary>
        /// Create New Comment In Database
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Get Detailed Information From Single Comment Item by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Returns CommentDetail Model</returns>
        public IHttpActionResult Get(int Id)
        {
            CommentService commentService = CreateCommentService();
            var comment = commentService.GetCommentDetail(Id);
            return Ok(comment);
        }

        // Delete comment
        /// <summary>
        /// Remove Comment From Database By ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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
