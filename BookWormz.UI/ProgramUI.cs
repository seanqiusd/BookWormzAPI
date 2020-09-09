using AspNetCore.Http.Extensions;
using BookWormz.Data;
using BookWormz.Models;
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
        private static bool _loggedIn = false;

        private static string _token;

        //private readonly BookController _bookController = new BookController();
        //private List<Book> book = new List<Book>();
        private static readonly ApplicationDbContext _context = new ApplicationDbContext();

        private static readonly HttpClient _httpClient = new HttpClient();

        public async void Start()
        {
            while (!_loggedIn)
                await RunLoginMenu();
            RunMenu();
        }

        private async Task RunLoginMenu()
        {
            Console.Clear();
            Console.WriteLine(
               "-- BookWormz API --\n" +
               "\n" +
               "\n" +
               "1.) Register\n" +
               "2.) Login\n" +
               "3.) Exit\n");
            Console.Write("Enter a number: ");
            switch (Console.ReadLine())
            {
                case "1":
                    Task register = Register();
                    Console.WriteLine("\nprocessing\n");
                    register.Wait();
                    break;

                case "2":
                    Task login = Login();
                    Console.WriteLine("\nprocessing\n");
                    login.Wait();
                    await login;
                    break;
                case "3":
                    break;
                default:
                    return;
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private void RunMenu()
        {
            while (_isRunning)
            {
                Console.Clear();

                Console.WriteLine(
                    "--- Books ---\n" +
                    "1.) View all Books\n" +
                    "2.) Find Book by ID\n" +
                    "3.) Add Book\n" +
                    "4.) Update Book\n" +
                    "5.) Delete Book by ISBN\n" +
                    "\n" +
                    "\n" +
                    "--- Ratings ---\n" +
                     "6.) View Exchange Ratings\n" +
                    "7.) Add Exchange Rating\n" +
                    "8.) Update Exchange Rating\n" +
                    "9.) Delete Exchange Rating\n" +
                    "\n" +
                    "\n" +
                    "--- Exchanges ---\n" +
                    "10.) View Exchanges\n" +
                    "11.) Add Exchange\n" +
                    "12.) Update Exchange\n" +
                    "13.) Delete Exchange\n");

                Console.Write("Enter a #: ");

                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                       
                        ViewAllBooks();
                        break;

                    case "2":
                        FindBookByID();
                        break;

                    case "3":
                        AddBook();
                        break;

                    case "4":
                        UpdateBook();
                        break;

                    case "5":
                        DeleteBook();
                        break;

                    case "6":
                        GetRatingByID();
                        break;

                    case "7":
                        AddRating();
                        break;

                    case "8":
                        UpdateRatings();
                        break;

                    case "9":
                        DeleteRating();
                        break;

                    case "10":
                        GetExchanges();
                        break;

                    case "11":
                        AddExchange();
                        break;

                    case "12":
                        UpdateExchange();
                        break;

                    case "13":
                        DeleteExchange();
                        break;

                    default:
                        return;
                }
                //Console.Write("Press any key to return\n");
                Console.ReadKey();
            }
        }

        private HttpResponseMessage CallAPI()
        {
            HttpClient httpClient = new HttpClient();

            // fix
            httpClient.DefaultRequestHeaders.Authorization =
              new AuthenticationHeaderValue("Bearer", _token);

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

        private static async Task FindBookByID()
        {
            Console.Clear();
            Console.Write("Enter ISBN: ");
            string userInput = Console.ReadLine();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44331/api/Book?ISBN={userInput}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:44331/api/Book?ISBN={userInput}");
            if (response.IsSuccessStatusCode)
            {
                BookDetail book = await response.Content.ReadAsAsync<BookDetail>();
                Console.WriteLine($"\n" +
                    $"ISBN: {book.ISBN}");
                Console.WriteLine($"Title: {book.BookTitle}");
                Console.WriteLine($"Author First Name: {book.AuthorFirstName}");
                Console.WriteLine($"Author Last Name: {book.AuthorLastName}");
                Console.WriteLine($"Description: {book.Description}\n");
                foreach (ExchangeSmListItem e in book.ExchangeListItems)
                {
                    Console.WriteLine();
                    Console.WriteLine(e.Id);
                    Console.WriteLine(e.IsAvailable);
                    Console.WriteLine(e.Posted);
                }
            }
        }

        //private static async Task AddBook()
        //{

        //    // --------------------------------------------- Input Data --------------------------------

        //    Console.Clear();
        //    Console.Write("ISBN: ");
        //    Dictionary<string, string> book = new Dictionary<string, string>
        //    {
        //        {"ISBN", Console.ReadLine() }

        //    };
        //    Console.Write("Book Title: ");
        //    book.Add("BookTitle", Console.ReadLine());

        //    Console.Write("Author First Name: ");
        //    book.Add("AuthorFirstName", Console.ReadLine());

        //    Console.Write("Author Last Name: ");
        //    book.Add("AuthorLastName", Console.ReadLine());

        //    Console.Write("Genre Type: ");
        //    book.Add("GenreOfBook", Console.ReadLine());

        //    Console.Write("Description: ");
        //    book.Add("Description", Console.ReadLine());

        //    // --------------------------------------------- Attempt 1 - no worky --------------------------------

        //    // New HTTP Client and HEADERS
        //    //HttpClient httpClient = new HttpClient();

        //    //httpClient.DefaultRequestHeaders.Authorization =
        //    //  new AuthenticationHeaderValue("Bearer", "EBCWE2br-EPiikuYJSIdekgB5NviWv7BbwrSxMsuPiOPn7xMg0U94vOXZcbxLOuyw4gYlVZbmsR2fXJYCPYJwEtc9pmK3ero-EUyflAiAKh0dSTTAWvevNsmjJFAnqS_F7hrYMLk0stMlFheyqPmmGk51WNxb3zgzdmgLgvbDJNchtZPX0C-w_E4WLKNJKLBNJ5N7LmVHC6T6STSFILeXQKLdpP-wsv3LR504RkE1JxeNWGJI6coSHg7rNDbVZMczlZW_IBG_wuZ8pfQA1RluykrVTdTsmoJfIKcj07S9zQK967AZ8CGoTPWrpv3Qpq8hE5AtIEpS6IqmUMHQRlMDOzdMwWtzi82HZmSyTM1bj0c2EGRA7H6QHa0HFRHgojHgxr2dv1a1VfIj-4Gpgszs7c0zFKN3HMXuHjYfDEomzGMVVRaKGw0Ox8C9NERcIqW08nql2o4rkA-332Kg7GksqUQDiGTyAJhH_4T-XZUTDU");

        //    //// Convert to JSON data
        //    //var dataAsString = JsonConvert.SerializeObject(book);
        //    //var dataContent = new StringContent(dataAsString);

        //    //HttpResponseMessage responseTwo = await httpClient.PostAsJsonAsync("https://localhost:44331/api/Book", dataContent);

        //    //// Success or fail
        //    //if (responseTwo.IsSuccessStatusCode)
        //    //{
        //    //    Uri ncrUrl = responseTwo.Headers.Location;
        //    //    Console.WriteLine("The book was added");
        //    //}
        //    //else
        //    //{
        //    //    Console.WriteLine("The book was not added");
        //    //}


        //    // --------------------------------------------- Attempt 2 - no worky --------------------------------

        //    // New HTTP Client and HEADERS


        //    //httpClient.DefaultRequestHeaders.Authorization =
        //    //  new AuthenticationHeaderValue("Bearer", "EBCWE2br-EPiikuYJSIdekgB5NviWv7BbwrSxMsuPiOPn7xMg0U94vOXZcbxLOuyw4gYlVZbmsR2fXJYCPYJwEtc9pmK3ero-EUyflAiAKh0dSTTAWvevNsmjJFAnqS_F7hrYMLk0stMlFheyqPmmGk51WNxb3zgzdmgLgvbDJNchtZPX0C-w_E4WLKNJKLBNJ5N7LmVHC6T6STSFILeXQKLdpP-wsv3LR504RkE1JxeNWGJI6coSHg7rNDbVZMczlZW_IBG_wuZ8pfQA1RluykrVTdTsmoJfIKcj07S9zQK967AZ8CGoTPWrpv3Qpq8hE5AtIEpS6IqmUMHQRlMDOzdMwWtzi82HZmSyTM1bj0c2EGRA7H6QHa0HFRHgojHgxr2dv1a1VfIj-4Gpgszs7c0zFKN3HMXuHjYfDEomzGMVVRaKGw0Ox8C9NERcIqW08nql2o4rkA-332Kg7GksqUQDiGTyAJhH_4T-XZUTDU");


        //    //HttpContent httpContent = new FormUrlEncodedContent(book);

        //    //var dataAsString = JsonConvert.SerializeObject(book);
        //    //var dataContent = new StringContent(dataAsString);
        //    ////_httpClient.BaseAddress = new Uri("https://localhost:44331/api/Book");

        //    //var response = await _httpClient.PostAsync("https://localhost:44331/api/Book", dataContent);
        //    //// ------- OR -----------
        //    ////var response = _httpClient.PostAsync("https://localhost:44331/api/Book", dataContent).Result;
        //    //if (response.IsSuccessStatusCode)
        //    //    Console.WriteLine("\n" +
        //    //        "Your book was added");
        //    //else
        //    //    Console.WriteLine("\n" +
        //    //        "There was a problem adding your book");
        //    //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/api/Book");

        //    //var dataAsString = JsonConvert.SerializeObject(book);
        //    //var dataContent = new StringContent(dataAsString);

        //    //request.Content = dataContent;
        //    //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "EBCWE2br-EPiikuYJSIdekgB5NviWv7BbwrSxMsuPiOPn7xMg0U94vOXZcbxLOuyw4gYlVZbmsR2fXJYCPYJwEtc9pmK3ero-EUyflAiAKh0dSTTAWvevNsmjJFAnqS_F7hrYMLk0stMlFheyqPmmGk51WNxb3zgzdmgLgvbDJNchtZPX0C-w_E4WLKNJKLBNJ5N7LmVHC6T6STSFILeXQKLdpP-wsv3LR504RkE1JxeNWGJI6coSHg7rNDbVZMczlZW_IBG_wuZ8pfQA1RluykrVTdTsmoJfIKcj07S9zQK967AZ8CGoTPWrpv3Qpq8hE5AtIEpS6IqmUMHQRlMDOzdMwWtzi82HZmSyTM1bj0c2EGRA7H6QHa0HFRHgojHgxr2dv1a1VfIj-4Gpgszs7c0zFKN3HMXuHjYfDEomzGMVVRaKGw0Ox8C9NERcIqW08nql2o4rkA-332Kg7GksqUQDiGTyAJhH_4T-XZUTDU");

        //    //var response = await httpClient.SendAsync(request);
        //    //if (response.IsSuccessStatusCode)
        //    //    Console.WriteLine("\n" +
        //    //        "Your book was added");
        //    //else
        //    //    Console.WriteLine("\n" +
        //    //        "There was a problem adding your book");

        //    var login = new Dictionary<string, string>()
        //    {
        //        {"grant_type", "password" },
        //        {"Username", "hustin@hustin.com" },
        //        {"Password", "Test1!" }
        //    };

        //    HttpClient httpClient = new HttpClient();

        //    // Get the token from the API
        //    var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/token");
        //    tokenRequest.Content = new FormUrlEncodedContent(login.AsEnumerable());
        //    var response = await httpClient.SendAsync(tokenRequest);
        //    var tokenString = await response.Content.ReadAsStringAsync();
        //    var token = JsonConvert.DeserializeObject<Token>(tokenString).Value;

        //    // Post a Book
        //    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/api/Book");
        //    request.Content = new FormUrlEncodedContent(book.AsEnumerable());
        //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    var response2 = await httpClient.SendAsync(request);

        //    if (response2.IsSuccessStatusCode)
        //        Console.WriteLine("Your book was added");
        //    else
        //        Console.WriteLine("There was a problem adding your book");
        //}

        //    public class Token
        //{
        //    [JsonProperty("access_token")]
        //    public string Value { get; set; }
        //}

        private static async Task Register()
        {
            Console.Clear();
            Console.Write("Email: ");
            Dictionary<string, string> register = new Dictionary<string, string>
            {
                {"Email", Console.ReadLine() }
            };
            Console.Write("Password: ");
            register.Add("Password", Console.ReadLine());

            Console.Write("Confirm Password: ");
            register.Add("ConfirmPassword", Console.ReadLine());

            Console.Write("First Name: ");
            register.Add("FirstName", Console.ReadLine());

            Console.Write("Last Name: ");
            register.Add("LastName", Console.ReadLine());

            Console.Write("Address: ");
            register.Add("Address", Console.ReadLine());

            HttpClient httpClient = new HttpClient();

            var registerRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/api/Account/Register");
            registerRequest.Content = new FormUrlEncodedContent(register.AsEnumerable());
            var response = await httpClient.SendAsync(registerRequest);


            if (response.IsSuccessStatusCode)
                Console.WriteLine("\n" +
                    "Much success, you are now registered. Please go back and login\n" +
                    "");
            else
                Console.WriteLine("\n" +
                    "I have failed you");

            return;
        }

        private static async Task Login()
        {
            Console.Clear();
            Dictionary<string, string> login = new Dictionary<string, string>
            {
                {"grant_type", "password" }
            };
            Console.Write("Email: ");
            login.Add("Username", Console.ReadLine());

            Console.Write("Password: ");
            login.Add("Password", Console.ReadLine());

            HttpClient httpClient = new HttpClient();

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/token");
            tokenRequest.Content = new FormUrlEncodedContent(login.AsEnumerable());
            var response = await httpClient.SendAsync(tokenRequest);
            var tokenString = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Token>(tokenString).Value;
            _token = token;



            tokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("\n" +
                    "Success, you are logged in with a token");
                _loggedIn = true;
            }
            else
                Console.WriteLine("\n" +
                    "Login failed");

            return;
        }

        private static async Task AddBook()
        {

            // Input Data

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


            // Post a Book
            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/api/Book");
            request.Content = new FormUrlEncodedContent(book.AsEnumerable());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response2 = await httpClient.SendAsync(request);

            if (response2.IsSuccessStatusCode)
                Console.WriteLine("Your book was added");
            else
                Console.WriteLine("There was a problem adding your book");
        }


        // Update Book by ISBN
        private static async Task UpdateBook()
        {
            Console.Clear();

            Console.Write("ISBN of the book to update: ");
            string userInput = Console.ReadLine();

            Console.Write("Book Title: ");
            Dictionary<string, string> book = new Dictionary<string, string>
            {
                {"BookTitle", Console.ReadLine() }

            };

            Console.Write("Author First Name: ");
            book.Add("AuthorFirstName", Console.ReadLine());

            Console.Write("Author Last Name: ");
            book.Add("AuthorLastName", Console.ReadLine());

            Console.Write("Genre Type: ");
            book.Add("GenreOfBook", Console.ReadLine());

            Console.Write("Description: ");
            book.Add("Description", Console.ReadLine());


            // Post a Book
            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:44331/api/Book?ISBN={userInput}");
            request.Content = new FormUrlEncodedContent(book.AsEnumerable());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Your book was updated");
            else
                Console.WriteLine("There was a problem updating your book");
        }


        // Get Ratings
        private static async Task GetExchanges()
        {
            //Console.Clear();

            //HttpClient httpClient = new HttpClient();

            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44331/api/UserRating");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            //var response = await httpClient.SendAsync(request);

            //if (response.IsSuccessStatusCode)
            //    Console.WriteLine("Here are your Exchange Ratings");
            //else
            //    Console.WriteLine("Failed to load Exchange Ratings");

            Console.Clear();

            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44331/api/Exchange");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await httpClient.SendAsync(request);

            if (response != null)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var list = JsonConvert.DeserializeObject<List<Exchange>>(json);

                foreach (Exchange exchange in list)
                {
                    Console.WriteLine($"Exchange ID: {exchange.Id}\n" +
                        $"Book ID: {exchange.BookId}\n" +
                        $"Avaiable: {exchange.IsAvailable}\n" +
                        $"Receiver of Book: {exchange.ReceiverId}\n" +
                        $"\n");
                }
            }
        }


        // Add Exchange
        private static async Task AddExchange()
        {
            Console.Clear();
            Console.Write("Book ISBN: ");
            Dictionary<string, string> exchange = new Dictionary<string, string>
                {
                    {"BookId", Console.ReadLine() }
                };
            Console.Write("Receiver User (ID): ");
            exchange.Add("ReceiverUser", Console.ReadLine());

            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/api/Exchange");
            request.Content = new FormUrlEncodedContent(exchange.AsEnumerable());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Your Exchange was created");
            else
                Console.WriteLine("There was a problem creating your exchange");
        }



        // Update Exchange by ID
        private static async Task UpdateExchange()
        {
            Console.Clear();
            Console.Write("Enter Exchange ID to update: ");
            string userInput = Console.ReadLine();


            Console.Write("Book ISBN: ");
            Dictionary<string, string> exchange = new Dictionary<string, string>
                {
                    {"BookId", Console.ReadLine() }
                };
            Console.Write("Receiver User (ID): ");
            exchange.Add("ReceiverUser", Console.ReadLine());

            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:44331/api/Exchange/{userInput}");
            request.Content = new FormUrlEncodedContent(exchange.AsEnumerable());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Your Exchange was updated");
            else
                Console.WriteLine("There was a problem updating your exchange");
        }


        // Delete Exchange by ID
        private static async Task DeleteExchange()
        {
            Console.Clear();
            Console.Write("Enter Exchange ID to delete: ");
            string userInput = Console.ReadLine();

            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:44331/api/Exchange/{userInput}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Exchange was deleted");
            else
                Console.WriteLine("There was a problem deleting your exchange");
        }



        // Get Ratings
        private static async Task GetRatingByID()
        {
            Console.Clear();
            Console.Write("Enter Rating ID: ");
            string userInput = Console.ReadLine();

            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44331/api/UserRating/{userInput}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:44331/api/UserRating/{userInput}");
            if (response.IsSuccessStatusCode)
            {
                UserRating userRating = await response.Content.ReadAsAsync<UserRating>();
                Console.WriteLine($"\n" +
                    $"ID: {userInput}\n" +
                    $"User ID: {userRating.UserId}\n" +
                    $"Exchange ID: {userRating.ExchangeId}\n" +
                    $"Exchange Rating: {userRating.ExchangeRating}\n");
            }
        }

    

        // Add Rating
        private static async Task AddRating()
        {

            GetExchanges();

            Console.WriteLine("Enter your Exchange ID and Rating below\n");
            Console.Write("Exchange ID: ");
            Dictionary<string, string> rating = new Dictionary<string, string>();

            string exchangeIdInput = Console.ReadLine();
            rating.Add("ExchangeId", exchangeIdInput);

            Console.Write("Exchange Rating: ");
            string exchangeRatingInput = Console.ReadLine();
            rating.Add("ExchangeRating", exchangeRatingInput);





            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/api/UserRating");
            request.Content = new FormUrlEncodedContent(rating.AsEnumerable());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Exchange Rating was added");
            else
                Console.WriteLine("There was a problem adding your Exchange Rating");

        }


        // Update Ratings by ID
        private static async Task UpdateRatings()
        {
            Console.WriteLine("Enter the Rating ID to update: \n");
            string userInput = Console.ReadLine();

            Console.Write("Exchange ID: ");
            Dictionary<string, string> rating = new Dictionary<string, string>();

            string exchangeIdInput = Console.ReadLine();
            rating.Add("ExchangeId", exchangeIdInput);

            Console.Write("Exchange Rating: ");
            string exchangeRatingInput = Console.ReadLine();
            rating.Add("ExchangeRating", exchangeRatingInput);

            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:44331/api/UserRating/{userInput}");
            request.Content = new FormUrlEncodedContent(rating.AsEnumerable());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Exchange Rating was updated");
            else
                Console.WriteLine("There was a problem updating your Exchange Rating");
        }


        // Delete User Rating by ID
        private static async Task DeleteRating()
        {
            Console.Clear();
            Console.Write("Enter Rating ID to delete: ");
            string userInput = Console.ReadLine();

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:44331/api/UserRating/{userInput}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await httpClient.SendAsync(request);


            if (response.IsSuccessStatusCode)
                Console.WriteLine("Rating Deleted");
            else
                Console.WriteLine("Error rating not deleted");

        }

        // Delete Book by ISBN
        private static async Task DeleteBook()
        {
            Console.Clear();
            Console.Write("Enter ISBN to delete: ");
            string userInput = Console.ReadLine();

            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:44331/api/Book?ISBN={userInput}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            //Book entity = await _context.Books.FindAsync(userInput);

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Book was deleted");
            else
                Console.WriteLine("Book could not be deleted");
        }
    }

    // Helper class for token
    public class Token
    {
        [JsonProperty("access_token")]
        public string Value { get; set; }
    }
}


// Used this to create register a user. Didn't need after first test.
//var user = new Dictionary<string, string>()
//{
//    {"Email", "test1@test.com" },
//    {"Password", "tesTing1$" },
//    {"ConfirmPassword", "tesTing1$" },
//    {"FirstName", "Hustin" },
//    {"LastName", "Jeffers" },
//    {"Address", "123 Main St" }
//};

//var login = new Dictionary<string, string>()
//{
//    {"grant_type", "password" },
//    {"Username", "hustin@hustin.com" },
//    {"Password", "Test1!" }
//};


//Used this to register an account. Didn't need after first test.
//var registerRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/api/Account/Register");
//registerRequest.Content = new FormUrlEncodedContent(user.AsEnumerable());
//await httpClient.SendAsync(registerRequest);

// Get the token from the API
//var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/token");
//tokenRequest.Content = new FormUrlEncodedContent(login.AsEnumerable());
//var response = await httpClient.SendAsync(tokenRequest);
//var tokenString = await response.Content.ReadAsStringAsync();
//var token = JsonConvert.DeserializeObject<Token>(tokenString).Value;


