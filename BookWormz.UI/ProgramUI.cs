using BookWormz.Data;
using BookWormz.WebApi.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.UI
{
    public class ProgramUI
    {
        private bool _isRunning = true;

        private readonly BookController _bookController = new BookController();
        private List<Book> book = new List<Book>();
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        private readonly HttpClient _httpClient = new HttpClient();

        public void Start()
        {
            RunMenu();
        }

        private void RunMenu()
        {
            while (_isRunning)
            {
                Console.Clear();

                Console.WriteLine(
                    "-- BookWormz API --\n" +
                    "\n" +
                    "1.) View all Books\n" +
                    "2.) Add book\n");

                Console.Write("Enter a #: ");

                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        ViewAllBooks();
                        break;

                    case "2":
                        AddBook();
                        break;

                    default:
                        return;
                }
                Console.Write("Press any key to return");
                Console.ReadKey();
            }
        }

        private HttpResponseMessage CallAPI()
        {
            HttpClient httpClient = new HttpClient();
            //using (var client = new HttpClient())

            httpClient.DefaultRequestHeaders.Authorization =
              new AuthenticationHeaderValue("Bearer", "EBCWE2br-EPiikuYJSIdekgB5NviWv7BbwrSxMsuPiOPn7xMg0U94vOXZcbxLOuyw4gYlVZbmsR2fXJYCPYJwEtc9pmK3ero-EUyflAiAKh0dSTTAWvevNsmjJFAnqS_F7hrYMLk0stMlFheyqPmmGk51WNxb3zgzdmgLgvbDJNchtZPX0C-w_E4WLKNJKLBNJ5N7LmVHC6T6STSFILeXQKLdpP-wsv3LR504RkE1JxeNWGJI6coSHg7rNDbVZMczlZW_IBG_wuZ8pfQA1RluykrVTdTsmoJfIKcj07S9zQK967AZ8CGoTPWrpv3Qpq8hE5AtIEpS6IqmUMHQRlMDOzdMwWtzi82HZmSyTM1bj0c2EGRA7H6QHa0HFRHgojHgxr2dv1a1VfIj-4Gpgszs7c0zFKN3HMXuHjYfDEomzGMVVRaKGw0Ox8C9NERcIqW08nql2o4rkA-332Kg7GksqUQDiGTyAJhH_4T-XZUTDU");

            Task<HttpResponseMessage> getTask = httpClient.GetAsync("https://localhost:44331/api/Book");
            HttpResponseMessage response = getTask.Result;

            return response;
        }

        private void ViewAllBooks()
        {
            Console.Clear();
            var response = CallAPI();

            if (response != null)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var list = JsonConvert.DeserializeObject<List<Book>>(json);

                foreach (Book book in list)
                {
                    Console.WriteLine($"Title: {book.BookTitle}\n" +
                        $"Author Last Name: {book.AuthorLastName}\n" +
                        $"ISBN: {book.ISBN}\n" +
                        $"\n");
                }
            }
        }

        private void AddBook()
        {
            Console.Clear();
            Console.Write("ISBN: ");
            Dictionary<string, string> book = new Dictionary<string, string>
            {
                {"ISBN", Console.ReadLine() }

            };
            Console.Write("Book Title: ");
            book.Add("BookTitle", Console.ReadLine());

            Console.Write("Author First Name: ");
            book.Add("AuthorFirstName", Console.ReadLine());

            Console.Write("Author Last Name: ");
            book.Add("AuthorLastName", Console.ReadLine());

            Console.Write("Genre Type: ");
            book.Add("GenreOfBook", Console.ReadLine());

            Console.Write("Description: ");
            book.Add("Description", Console.ReadLine());

            HttpContent httpContent = new FormUrlEncodedContent(book);


            var response = CallAPI();
            response = _httpClient.PostAsync("https://localhost:44331/api/Book", httpContent).Result;
            if (response.IsSuccessStatusCode)
                Console.WriteLine("Success");
            else
                Console.WriteLine("Fail");
        }
    }
}

