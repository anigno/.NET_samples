using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksAndAsync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //AsyncAwaitExample2 example2 = new AsyncAwaitExample2();
            //AsyncCancelationExample1 example1= new AsyncCancelationExample1();
            ParallelRunExample parallelRunExample = new ParallelRunExample();
            
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
