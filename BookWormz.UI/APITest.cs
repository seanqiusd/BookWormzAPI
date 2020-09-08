using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookWormz.UI
{
    public class APITest
    {
        private static string APIUrl = "https://localhost:44331/api/";
        public static async Task GetDataWithAuthentication()
        {
            var authCredential = Encoding.UTF8.GetBytes("{hustin@hustin.com}:{Test1!}");
            using (var client = new HttpClient())
            {


                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authCredential));
                client.BaseAddress = new Uri(APIUrl);
                HttpResponseMessage response = await client.GetAsync(APIUrl + "Book");

                if (response.IsSuccessStatusCode)
                {
                    var readTask = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var rawResponse = readTask.GetAwaiter().GetResult();
                    Console.WriteLine(rawResponse);
                }
                Console.WriteLine("Complete");
            }
        }
    }
}
