using BookWormz.Data;
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
    public class BookController : ApiController
    {
        private BookService CreateBookService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var bookService = new BookService(userId);
            return bookService;
        }

        // get all books
        public IHttpActionResult Get()
        {
            BookService bookService = CreateBookService();
            var books = bookService.GetBooks();
            return Ok(books);

        }

        // creates book
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

        // Get single book details
        public IHttpActionResult Get(string ISBN)
        {
            BookService bookService = CreateBookService();
            var book = bookService.GetBookDetail(ISBN);
            return Ok(book);
        }

    }
}
