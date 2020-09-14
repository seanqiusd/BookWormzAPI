using BookWormz.Data;
using BookWormz.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.Services
{
    public class BookService
    {
        private readonly Guid _userId;

        public BookService(Guid userId)
        {
            _userId = userId;
        }
        // Post -- Create
        public bool CreateBook(BookCreate book)
        {
            var entity =
                new Book()
                {
                    ISBN = book.ISBN,
                    BookTitle = book.BookTitle,
                    AuthorFirstName = book.AuthorFirstName,
                    AuthorLastName = book.AuthorLastName,
                    GenreOfBook = book.GenreOfBook,
                    Description = book.Description,
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Books.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        // Get --All
        public IEnumerable<BookListItem> GetBooks()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Books.ToList()
                    .Select(
                        e =>
                        new BookListItem
                        {
                            ISBN = e.ISBN,
                            BookTitle = e.BookTitle,
                            AuthorFirstName = e.AuthorFirstName,
                            AuthorLastName = e.AuthorLastName,
                            GenreOfBook = e.GenreOfBook,
                            Description = e.Description,
                            NumberAvailable = e.NumberAvailable
                        }
                        );
                return query.ToArray(); // returning BookListItem, remember don't ever return raw data from data layer; transform beforehand in service layer before sending to api                    
            }
        }

        // Get --single book by isbn (id)
        public BookDetail GetBookDetail(string ISBN)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Books.Single(e => e.ISBN == ISBN);

                var detailedBook = new BookDetail
                {
                    ISBN = entity.ISBN,
                    BookTitle = entity.BookTitle,
                    AuthorFirstName = entity.AuthorFirstName,
                    AuthorLastName = entity.AuthorLastName,
                    GenreOfBook = entity.GenreOfBook,
                    Description = entity.Description,
                    NumberAvailable = entity.NumberAvailable
                };
                foreach (Exchange exchange in entity.Exchanges)  // To Display All the exchanges
                {
                    detailedBook.ExchangeListItems.Add(new ExchangeSmListItem { Id = exchange.Id, IsAvailable = exchange.IsAvailable, Posted = exchange.Posted, SenderName = exchange.SenderUser.FirstName, SenderRating = exchange.SenderUser.ExchangeRating });
                }
                return detailedBook;
            }
        }

        //Update
        public int UpdateBookByISBN(string ISBN, BookUpdate newBook)
        {
            using (var ctx = new ApplicationDbContext())
            {
                Book book = ctx.Books.Find(ISBN);
                if (book == null)
                {
                    return 2;
                }
                book.BookTitle = newBook.BookTitle;
                book.AuthorFirstName = newBook.AuthorFirstName;
                book.AuthorLastName = newBook.AuthorLastName;
                book.GenreOfBook = newBook.GenreOfBook;
                book.Description = newBook.Description;
                if (ctx.SaveChanges() == 1)
                    return 0;
                return 1;
            }
        }

        // Delete
        public bool DeleteBook(string ISBN)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Books.Single(e => e.ISBN == ISBN);
                ctx.Books.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
