using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadsLockTest
{
    class Program
    {
        public Program()
        {
            object A = new object();
            object B = new object();
            Task.Factory.StartNew(() =>
            {
                int a = 0;
                string b = "aaaa";
                lock (A)
                {
                    Console.WriteLine("3+");
                    Task.Delay(1000).Wait();
                    Console.WriteLine("3-");
                    lock (B)
                    {
                        Console.WriteLine("1+");
                        Task.Delay(1000).Wait();
                        Console.WriteLine("1-");
                    }
                }
            }, TaskCreationOptions.LongRunning);
            Task.Factory.StartNew(() =>
            {
                int a = 1;
                string b = "bbbb";
                lock (B)
                {
                    Console.WriteLine("4+");
                    Task.Delay(1000).Wait();
                    Console.WriteLine("4-");
                    lock (A)
                    {
                        Console.WriteLine("2+");
                        Task.Delay(1000).Wait();
                        Console.WriteLine("2-");
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        static void Main(string[] args)
        {
            var p = new Program();
            Console.ReadLine();
            GC.KeepAlive(p);

        }
    }
}
