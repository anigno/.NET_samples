using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace WorkingWithHttp.OwinHosting
{
    public class Program
    {
        static void Main()
        {
            const string BASE_ADDRESS = "http://localhost:9000/";
            Task.Factory.StartNew(() =>
            {
                WebApp.Start<Startup>(BASE_ADDRESS);
                Console.WriteLine("WebApp started");

            });
            Task.Factory.StartNew(() =>
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(BASE_ADDRESS + "api/values").Result;
                client.PostAsync(BASE_ADDRESS + "api/values", new ByteArrayContent(new byte[] { 1, 2, 3 }));
                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            });
            Console.ReadLine();
        }
    }
}