#region

using System;
using Microsoft.Owin.Hosting;

#endregion

namespace OwinHost
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            // Specify the URI to use for the local host:
            string baseUri = "http://localhost:8080";
            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(baseUri);
            Console.WriteLine("Server running at {0} ‐ press Enter to quit. ", baseUri);
            Console.ReadLine();
        }

        #endregion
    }


    // Add the following usings:
}