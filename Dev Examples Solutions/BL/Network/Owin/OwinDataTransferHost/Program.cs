#region

using System;
using Microsoft.Owin.Hosting;

#endregion

namespace OwinDataTransferHost
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            // Specify the URI to use for the local host:
            const string BASE_URI = "http://localhost:8080";
            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(BASE_URI);
            Console.WriteLine("Server running at {0} ‐ press Enter to quit. ", BASE_URI);
            Console.ReadLine();
        }

        #endregion
    }
}