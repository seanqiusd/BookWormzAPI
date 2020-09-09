using AspNetCore.Http.Extensions;
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

        private static string _token;

        //private readonly BookController _bookController = new BookController();
        //private List<Book> book = new List<Book>();
        private static readonly ApplicationDbContext _context = new ApplicationDbContext();

        private static readonly HttpClient _httpClient = new HttpClient();

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
                    "\n" +
                    "--- Register/Login ---\n" +
                    "1.) Register\n" +
                    "2.) Login\n" +
                    "\n" +
                    "\n" +
                    "--- Books ---\n" +
                    "3.) View all Books\n" +
                    "4.) Find Book by ID\n" +
                    "5.) Add Book\n" +
                    "6.) Delete Book by ISBN\n" +
                    "7.) Delete Rating\n");

                Console.Write("Enter a #: ");

                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        Register();
                        break;

                    case "2":
                        Login();
                        break;

                    case "3":
                        ViewAllBooks();
                        break;

                    case "4":
                        // Find Book by Id
                        break;

                    case "5":
                        AddBook();
                        break;

                    case "6":
                        // DeleteBook();
                        break;

                    case "7":
                        DeleteRating();
                        break;

                    default:
                        return;
                }
                Console.Write("Press any key to return\n");
                Console.ReadKey();
            }
        }

        private HttpResponseMessage CallAPI()
        {
            HttpClient httpClient = new HttpClient();

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

        //private HttpResponseMessage FindBookByID()
        //{
        //    HttpClient httpClient = new HttpClient();
        //    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44331/api/Book");
        //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        //}

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
                Console.WriteLine("Much success, you are now registered. Please go back and login\n" +
                    "");
            else
                Console.WriteLine("I have failed you");
        }

        private static async Task Login()
        {
            Console.Clear();
            Console.Write("Grant Type, please type password: ");
            Dictionary<string, string> login = new Dictionary<string, string>
            {
                {"grant_type", Console.ReadLine() }
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
                Console.WriteLine("Success, you are logged in with a token");
            else
                Console.WriteLine("Login failed");
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

        private static async Task DeleteRating()
        {
            Console.Clear();
            Console.Write("Enter Rating ID to delete: ");
            string userInput = Console.ReadLine();

            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, "https://localhost:44331/api/UserRating");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response2 = await httpClient.DeleteAsync(userInput);

            if (await _context.SaveChangesAsync() == 1)
                Console.WriteLine("Book was deleted");
            else
                Console.WriteLine("Book could not be deleted");
        }

        //private static async Task DeleteBook()
        //{
        //    Console.Clear();
        //    Console.Write("Enter ISBN to delete: ");
        //    string userInput = Console.ReadLine();

        //    HttpClient httpClient = new HttpClient();

        //    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, "https://localhost:44331/api/Book");
        //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        //    Book entity = await _context.Books.FindAsync(userInput);

        //    var response2 = await httpClient.DeleteAsync(entity);

        //    if (await _context.SaveChangesAsync() == 1)
        //        Console.WriteLine("Book was deleted");
        //    else
        //        Console.WriteLine("Book could not be deleted");
        //}
    }

    // Created this helper class for getting the token.
    public class Token
    {
        [JsonProperty("access_token")]
        public string Value { get; set; }
    }
}





