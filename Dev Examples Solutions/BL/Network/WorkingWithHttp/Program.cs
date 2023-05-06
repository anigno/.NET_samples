using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WorkingWithHttp
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Factory.StartNew(async () =>
            {
                HttpClient client = new HttpClient();

                //HttpResponseMessage response = await client.GetAsync("http://dvd8moovprd/moovex/worker.asp?shiftidmode=11");
                //response.EnsureSuccessStatusCode();
                //string responseBody = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(responseBody);
                HttpResponseMessage httpResponseMessage = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "http://dvd8moovprd/moovex/worker.asp?shiftidmode=11"));
                Console.WriteLine(httpResponseMessage);

            });

            Console.ReadLine();
        }
    }
}
/*
http://dvd8moovprd/moovex/worker.asp?action=addshiftform&d=06/09/2016
 */