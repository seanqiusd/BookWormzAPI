using AspNetCore.Http.Extensions;
using BookWormz.Data;
using BookWormz.Models;
using BookWormz.Models.UserRatingModels;
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

        //Using httpClient field to carry authorization once user is logged in
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
                    _isRunning = false;
                    _loggedIn = true;
                    return;
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
                    "6.) Check available books by State\n" +
                    "\n" +
                    "\n" +
                    "--- Ratings ---\n" +
                    "7.) View Exchange Rating\n" +
                    "8.) Add Exchange Rating\n" +
                    "9.) Update Exchange Rating\n" +
                    "10.) Get My Ratings\n" +
                    "11.) Delete Exchange Rating\n" +
                    "\n" +
                    "\n" +
                    "--- Exchanges ---\n" +
                    "12.) View Exchanges\n" +
                    "13.) View Exchange By ID\n" +
                    "14.) Add Exchange\n" +
                    "15.) Update Exchange\n" +
                    "16.) Request Exchange\n" +
                    "17.) Delete Exchange\n" +
                    "\n" +
                    "\n" +
                    "--- Comment on Exchanges ---\n" +
                    "18.) Add Comment\n" +
                    "19.) Reply to Comment\n" +
                    "\n" +
                    "--- Exit ---\n" +
                    "20.) Exit Program");

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
                        AvailabeBooksByState();
                        break;

                    case "7":
                        GetRatingByID();
                        break;

                    case "8":
                        AddRating();
                        break;

                    case "9":
                        UpdateRatings();
                        break;

                    case "10":
                        GetMyRating();
                        break;

                    case "11":
                        DeleteRating();
                        break;

                    case "12":
                        GetExchanges();
                        break;

                    case "13":
                        GetExchangeById();
                        break;

                    case "14":
                        AddExchange();
                        break;

                    case "15":
                        UpdateExchange();
                        break;
                    case "16":
                        RequestExchange();
                        break;
                    case "17":
                        DeleteExchange();
                        break;

                    case "18":
                        AddComment();
                        break;

                    case "19":
                        AddReply();
                        break;

                    case "20":
                        return;

                    default:
                        continue;
                }
                //Console.Write("Press any key to return\n");
                Console.ReadKey();
            }
        }

        private HttpResponseMessage CallAPI()
        {
            //HttpClient httpClient = new HttpClient();

            //// fix
            //httpClient.DefaultRequestHeaders.Authorization =
            //  new AuthenticationHeaderValue("Bearer", _token);

            Task<HttpResponseMessage> getTask = _httpClient.GetAsync("https://localhost:44331/api/Book");
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
            //HttpClient httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44331/api/Book?ISBN={userInput}");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:44331/api/Book?ISBN={userInput}");

            if (response.IsSuccessStatusCode)
            {
                BookDetail book = await response.Content.ReadAsAsync<BookDetail>();
                Console.WriteLine($"\n" +
                    $"ISBN: {book.ISBN}");
                Console.WriteLine($"Title: {book.BookTitle}");
                Console.WriteLine($"Author First Name: {book.AuthorFirstName}");
                Console.WriteLine($"Author Last Name: {book.AuthorLastName}");
                Console.WriteLine($"Description: {book.Description}\n");
                Console.WriteLine("Exchanges for this book");
                foreach (ExchangeSmListItem e in book.ExchangeListItems)
                {
                    Console.WriteLine();
                    Console.WriteLine($"exchange Id: {e.Id}\n" +
                        $"Exchanger Rating: {e.SenderRating}\n" +
                        $"Is book still available: {e.IsAvailable}");
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

            Console.Write("State: ");
            register.Add("State", Console.ReadLine());

            //HttpClient httpClient = new HttpClient();

            var registerRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/api/Account/Register");
            registerRequest.Content = new FormUrlEncodedContent(register.AsEnumerable());
            var response = await _httpClient.SendAsync(registerRequest);


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

            //HttpClient httpClient = new HttpClient();

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/token");
            tokenRequest.Content = new FormUrlEncodedContent(login.AsEnumerable());
            var response = await _httpClient.SendAsync(tokenRequest);
            var tokenString = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Token>(tokenString).Value;
            _token = token;



            tokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);


            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("\n" +
                    "Success, you are logged in with a token");

                //If loggin is successful using token to authorize http client
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
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
            //HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/api/Book");
            request.Content = new FormUrlEncodedContent(book.AsEnumerable());
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response2 = await _httpClient.SendAsync(request);

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
            //HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:44331/api/Book?ISBN={userInput}");
            request.Content = new FormUrlEncodedContent(book.AsEnumerable());

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Your book was updated");
            else
                Console.WriteLine("There was a problem updating your book");
        }



        // Check available books by state
        private static async Task AvailabeBooksByState()
        {
            Console.Clear();

            Console.Write("Enter State: ");
            string userInput = Console.ReadLine();

            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:44331/api/Exchange?state={userInput}");
            if (response.IsSuccessStatusCode)
            {
                List<ExchangeListItem> bookByState = await response.Content.ReadAsAsync<List<ExchangeListItem>>();
                if (bookByState.Count == 0)
                    Console.WriteLine($"No availabe books in {userInput}");
                else
                {
                    foreach (ExchangeListItem book in bookByState)
                    {
                        Console.WriteLine($"\n" +
                            $"Book ISBN: {book.BookId}\n" +
                            $"Exchange ID: {book.Id}\n" +
                            $"Book Title: {book.IsAvailable}");
                    }
                }

                //else
                //    Console.WriteLine($"No available books in {userInput}");
            }
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

            //HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44331/api/Exchange");

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await _httpClient.SendAsync(request);

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

        private static async Task GetExchangeById()
        {
            Console.Clear();
            Console.Write("Exchange ID to lookup:");
            string userInput = Console.ReadLine();
            Console.Clear();

            var response = await _httpClient.GetAsync($"https://localhost:44331/api/Exchange/{userInput}");

            if (response is null)
            {
                Console.WriteLine($"No exchange found with ID: {userInput}");
                return;
            }

            ExchangeDetail exchange = response.Content.ReadAsAsync<ExchangeDetail>().Result;

            Console.WriteLine($"" +
                $"Exchange ID: {exchange.Id}\n" +
                $"Book ISBN: {exchange.BookId}\n" +
                $"Book Title: {exchange.BookTitle}\n" +
                $"Posting User: {exchange.PostingUser}\n" +
                $"Users Rating: {exchange.PostersRating}\n" +
                $"Exchange Posted Date: {exchange.Posted}\n");
            if (exchange.Comments.Count > 0)
            {
                Console.WriteLine("\n\n" +
                    "_____________________\n" +
                    "Comments: \n" +
                    "_____________________");
                foreach (var comment in exchange.Comments)
                {
                    string indentation = "     ";
                    Console.WriteLine($"{indentation}Comment By:{comment.CommentorsName}\n" +
                        $"{indentation}Comment ID:{comment.Id}\n" +
                        $"{indentation}{comment.Text}\n\n");
                    if (comment.Replies.Count > 0)
                    {
                        Console.WriteLine(indentation +
                            $"Replies to {comment.CommentorsName}:");
                        PrintReplies(comment.Replies, indentation);
                    }
                    Console.WriteLine("_____________________");
                }
            }
        }

        //Recursive method to print replies to comments and replies
        private static void PrintReplies(ICollection<ReplyDetail> replies, string indent)
        {
            indent += "     ";
            foreach (var reply in replies)
            {
                Console.WriteLine($"" +
                    $"{indent}_____________________________\n" +
                    $"{indent}Reply by: {reply.CommentorsName}\n" +
                    $"{indent}Reply ID: {reply.Id}\n" +
                    $"{indent}{reply.Text}");
                if (reply.Replies.Count > 0)
                {
                    Console.WriteLine($"" +
                        $"{indent}~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n" +
                        $"{indent}Replies to {reply.CommentorsName}:");
                    PrintReplies(reply.Replies, indent);
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

            //HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/api/Exchange");
            request.Content = new FormUrlEncodedContent(exchange.AsEnumerable());
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await _httpClient.SendAsync(request);

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

            //Console.Write("Receiver User (ID): ");
            //exchange.Add("ReceiverUser", Console.ReadLine());

            //HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:44331/api/Exchange/{userInput}");
            request.Content = new FormUrlEncodedContent(exchange.AsEnumerable());

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Your Exchange was updated");
            else
                Console.WriteLine("There was a problem updating your exchange");
        }

        //Request Exchange
        private static async Task RequestExchange()
        {
            Console.Clear();
            Console.Write("Enter Exchange Id to request: ");
            string userInput = Console.ReadLine();
            Dictionary<string, string> exchange = new Dictionary<string, string>
            {
                {"id", userInput }
            };
            HttpContent content = new FormUrlEncodedContent(exchange);

            var response = await _httpClient.PutAsync($"https://localhost:44331/api/Exchange/ExchangeRequest?id={userInput}", content);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Book Requested");
            else
                Console.WriteLine("There was a problem with your exchange");
        }


        // Delete Exchange by ID
        private static async Task DeleteExchange()
        {
            Console.Clear();
            Console.Write("Enter Exchange ID to delete: ");
            string userInput = Console.ReadLine();

            //HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:44331/api/Exchange/{userInput}");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Exchange was deleted");
            else
                Console.WriteLine("There was a problem deleting your exchange");
        }



        // Get Rating by Exchange ID
        private static async Task GetRatingByID()
        {
            Console.Clear();
            Console.Write("Enter ID of exchange: ");
            string userInput = Console.ReadLine();

            //HttpClient httpClient = new HttpClient();

            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44331/api/UserRating/{userInput}");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:44331/api/UserRating/{userInput}");
            if (response.IsSuccessStatusCode)
            {
                UserRatingDetail userRating = await response.Content.ReadAsAsync<UserRatingDetail>();
                if (userRating != null)
                    Console.WriteLine($"\n" +
                        $"ID: {userRating.Id}\n" +
                        $"User ID: {userRating.UserId}\n" +
                        $"Exchange ID: {userRating.ExchangeId}\n" +
                        $"Exchange Rating: {userRating.ExchangeRating}\n");
                else
                    Console.WriteLine("Exchange hasn't been rated");
            }
        }

        //Get Ratings about Logged in user
        private static async Task GetMyRating()
        {
            Console.Clear();
            HttpResponseMessage ratingsresponse = await _httpClient.GetAsync($"https://localhost:44331/api/UserRating");
            HttpResponseMessage userresponse = await _httpClient.GetAsync("https://localhost:44331/api/Account/GetUserRating");
            if (!ratingsresponse.IsSuccessStatusCode || !userresponse.IsSuccessStatusCode)
            {
                Console.WriteLine("An error has occured");
                return;
            }
            List<UserRatingListItem> ratings = await ratingsresponse.Content.ReadAsAsync<List<UserRatingListItem>>();
            double? rating = await userresponse.Content.ReadAsAsync<double?>();
            foreach (var r in ratings)
            {
                Console.WriteLine($"\n" +
                    $"Exchange Id: {r.ExchangeId}\n" +
                    $"Exchange Rating: {r.ExchangeRating}\n");
            }
            if (rating is null)
                Console.WriteLine("you dont have enough ratings for overall score");
            else
            {
                Console.WriteLine($"\n" +
                    $"Your overall user rating: {rating}");
            }

        }



        // Add Rating
        private static async Task AddRating()
        {

            GetExchanges().Wait();

            Console.WriteLine("Enter your Exchange ID and Rating below\n");
            Console.Write("Exchange ID: ");
            Dictionary<string, string> rating = new Dictionary<string, string>();

            string exchangeIdInput = Console.ReadLine();
            rating.Add("ExchangeId", exchangeIdInput);

            Console.Write("Exchange Rating: ");
            string exchangeRatingInput = Console.ReadLine();
            rating.Add("ExchangeRating", exchangeRatingInput);





            //HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/api/UserRating");
            request.Content = new FormUrlEncodedContent(rating.AsEnumerable());
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Exchange Rating was added");
            else
                Console.WriteLine("There was a problem adding your Exchange Rating");

        }


        // Update Ratings by ID
        private static async Task UpdateRatings()
        {
            Console.Clear();

            Console.Write("Enter the Rating ID to update:");
            string userInput = Console.ReadLine();

            Console.Write("Exchange ID: ");
            Dictionary<string, string> rating = new Dictionary<string, string>();

            string exchangeIdInput = Console.ReadLine();
            rating.Add("ExchangeId", exchangeIdInput);

            Console.Write("Exchange Rating: ");
            string exchangeRatingInput = Console.ReadLine();
            rating.Add("ExchangeRating", exchangeRatingInput);

            //HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:44331/api/UserRating/{userInput}");
            request.Content = new FormUrlEncodedContent(rating.AsEnumerable());

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await _httpClient.SendAsync(request);

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

            //HttpClient httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:44331/api/UserRating/{userInput}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _httpClient.SendAsync(request);


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

            //HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:44331/api/Book?ISBN={userInput}");

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            //Book entity = await _context.Books.FindAsync(userInput);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Book was deleted");
            else
                Console.WriteLine("Book could not be deleted");
        }



        // Create Comment
        private static async Task AddComment()
        {

            GetExchanges().Wait();

            Console.WriteLine("Enter your Exchange ID and and Comment below\n");
            Console.Write("Exchange ID: ");
            Dictionary<string, string> comment = new Dictionary<string, string>();

            string exchangeIdInput = Console.ReadLine();
            comment.Add("ExchangeId", exchangeIdInput);

            Console.Write("Comment: ");
            string commentInput = Console.ReadLine();
            comment.Add("CommentText", commentInput);





            //HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/api/Comment");
            request.Content = new FormUrlEncodedContent(comment.AsEnumerable());
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Comment was added");
            else
                Console.WriteLine("There was a problem adding your Comment");

        }



        // Get Comments
        private static async Task GetComments()
        {
            Console.Clear();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44331/api/Comment");

            var response = await _httpClient.SendAsync(request);

            if (response != null)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var list = JsonConvert.DeserializeObject<List<CommentDetail>>(json);

                foreach (CommentDetail comment in list)
                {
                    Console.WriteLine($"ID: {comment.Id}\n" +
                        $"Comment: {comment.Text}\n" +
                        $"Commenter: {comment.CommentorsName}\n" +
                        $"Replies: {comment.Replies.Count}\n" +
                        $"\n");
                }
            }
        }

        // Create Reply with list of comments
        private static async Task AddReply()
        {

            GetComments().Wait();

            Console.Write("Enter Comment ID you'd like to reply too: ");
            Dictionary<string, string> reply = new Dictionary<string, string>();

            string commentIdInput = Console.ReadLine();
            reply.Add("CommentId", commentIdInput);

            Console.Write("Reply: ");
            string replyInput = Console.ReadLine();
            reply.Add("CommentText", replyInput);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44331/api/Reply");
            request.Content = new FormUrlEncodedContent(reply.AsEnumerable());
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Reply was added");
            else
                Console.WriteLine("There was a problem adding your Reply");
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
