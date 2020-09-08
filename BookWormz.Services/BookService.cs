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

        public IEnumerable<BookListItem> GetBooks()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Books
                    .Select(
                        e =>
                        new BookListItem
                        {
                            ISBN = e.ISBN,
                            BookTitle = e.BookTitle,
                            AuthorFistName = e.AuthorFirstName,
                            AuthorLastName = e.AuthorLastName,
                            GenreOfBook = e.GenreOfBook,
                            Description = e.Description
                        }
                        );
                return query.ToArray();
                    
            }
        }

        public BookDetail GetBookDetail(string ISBN)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Books.Single(e => e.ISBN == ISBN);

                return new BookDetail
                {
                    ISBN = entity.ISBN,
                    BookTitle = entity.BookTitle,
                    AuthorFistName = entity.AuthorFirstName,
                    AuthorLastName = entity.AuthorLastName,
                    GenreOfBook = entity.GenreOfBook,
                    Description = entity.Description
                };
            }
            

        }



    }
}
