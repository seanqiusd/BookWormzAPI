using BookWormz.Data;
using BookWormz.Models;
using BookWormz.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.UI.WebControls;

namespace BookWormz.WebApi.Controllers
{

    /// <summary>
    /// Crud For Book Entities
    /// </summary>

    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    [Authorize]
    public class BookController : ApiController
    {
        private BookService CreateBookService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var bookService = new BookService(userId);
            return bookService;
        }

        // creates book
        /// <summary>
        /// Create New Book In Database
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public IHttpActionResult Post(BookCreate book)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var service = CreateBookService();

            if (!service.CreateBook(book))
            {
                return InternalServerError();
            }
            return Ok($"{book.BookTitle} added");
        }

        // get all books

        /// <summary>
        /// Get All Books
        /// </summary>
        /// <returns>Returns BookListItem Object of all books in database</returns>
        public IHttpActionResult Get()
        {
            BookService bookService = CreateBookService();
            var books = bookService.GetBooks();
            return Ok(books);

        }

        // Get single book details
        /// <summary>
        /// Get Detailed Information From Single Book Item
        /// </summary>
        /// <param name="ISBN"></param>
        /// <returns>Returns BookDetail Model</returns>
        
        public IHttpActionResult Get(string ISBN)
        {
            BookService bookService = CreateBookService();
            var book = bookService.GetBookDetail(ISBN);
            return Ok(book);
        }

        // Put --Update a book detail via ISBN
        //[HttpPut]
        /// <summary>
        /// Update Existing Book In Database
        /// </summary>
        /// <param name="ISBN"></param>
        /// <param name="newBook"></param>
        /// <returns></returns>
        public IHttpActionResult Put([FromUri] string ISBN, [FromBody] BookUpdate newBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var service = CreateBookService();
            switch(service.UpdateBookByISBN(ISBN, newBook))
            {
                case 0:
                    return Ok($"{newBook.BookTitle} has been updated");
                case 1:
                    return InternalServerError();
                case 2:
                    return BadRequest("Book not found");
                default:
                    return InternalServerError();
            }
        }

        // Delete a book
        /// <summary>
        /// Remove Book From Database
        /// </summary>
        /// <param name="ISBN"></param>
        /// <returns></returns>
        public IHttpActionResult Delete(string ISBN)
        {
            var service = CreateBookService();
            if (!service.DeleteBook(ISBN))
            {
                return InternalServerError();
            }
            return Ok($"ISBN: {ISBN} has been deleted");
        }
    }
}
