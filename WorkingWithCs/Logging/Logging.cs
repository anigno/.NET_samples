using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public static class Logger
    {
        private static readonly object syncRoot = new object();
        public static void Log(string message)
        {
            lock (syncRoot)
            {
                Console.WriteLine($"{DateTime.Now.ToString("HH:MM:ss.fff")} -> {message}");
            }
        }
    }
}
