using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;

namespace AutofacOwinAuth.WebAPI.OwinHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";
            var startOptions = new StartOptions(baseAddress);
            // Start OWIN host 
            using (WebApp.Start<Startup>(startOptions))
            {
                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();

                var response = client.GetAsync(baseAddress + "api/contacts").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine("Press Enter Key to exit...");
                Console.ReadLine();
            }             
        }
    }
}
