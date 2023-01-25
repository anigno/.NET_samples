using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp01
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] sa=new string[10];
            int a = 7;
            Console.WriteLine(Process.GetCurrentProcess().Id);
            Console.ReadLine();
        }
    }
}
