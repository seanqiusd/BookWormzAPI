namespace BookWormz.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BookWormz.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BookWormz.Data.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Books.AddOrUpdate(x => x.ISBN,
                new Book() { ISBN = "501", BookTitle = "Harry Potter and the Order of Phoenix", AuthorFirstName = "J.K.", AuthorLastName = "Rowling", GenreOfBook = 0, Description = "You're a wizard Harry" },
                new Book() { ISBN = "502", BookTitle = "A Song of Ice and Fire", AuthorFirstName = "George", AuthorLastName = "Martin", GenreOfBook = 0, Description = "Our boy Ned Stark dies" },
                new Book() { ISBN = "503", BookTitle = "Harry Potter and the Goblet of Fire", AuthorFirstName = "J.K.", AuthorLastName = "Rowling", GenreOfBook = 0, Description = "You're for sure a wizard Harry" }
                );

            //context.Users.AddOrUpdate(x => x.Id,
            //    new ApplicationUser() { Id = "75", Email = "seededuser@seededuser.com", PasswordHash = "seededuser", FirstName = "Seeded", LastName = "Data", State = "Indiana" }
            //    );

            context.Exchanges.AddOrUpdate(x => x.Id,
                new Exchange() { Id = 75, BookId = "501", Posted = DateTime.Now},
                new Exchange() { Id = 76, BookId = "501", Posted = DateTime.Now },
                new Exchange() { Id = 77, BookId = "503", Posted = DateTime.Now }
                );

            context.UserRatings.AddOrUpdate(x => x.Id,
            new UserRating() { Id = 85, ExchangeId = 75, ExchangeRating = 5 },
            new UserRating() { Id = 86, ExchangeId = 75, ExchangeRating = 7 },
            new UserRating() { Id = 87, ExchangeId = 75, ExchangeRating = 8 },
            new UserRating() { Id = 88, ExchangeId = 75, ExchangeRating = 2 },
            new UserRating() { Id = 89, ExchangeId = 75, ExchangeRating = 4 }
            );

            context.Comments.AddOrUpdate(x => x.Id,
                new Comment() { Id = 91, ExchangeId = 76, Text = "This is a seeded comment" },
                new Comment() { Id = 92, ExchangeId = 76, Text = "This is another seeded comment" }
                );

        }
    }
}
