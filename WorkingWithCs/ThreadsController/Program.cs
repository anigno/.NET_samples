using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadsController
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ThreadController controller = new ThreadController();
            //controller.StartWithThreadPool();
            //controller.StartTasks();
            //ProducersConsumers producersConsumers = new ProducersConsumers();
            //producersConsumers.Start();
            //ThreadLocks threadLocks = new ThreadLocks();
            EmptyFullSemaphores emptyFull = new EmptyFullSemaphores();

            Console.WriteLine("Press Any key to exit");
            Console.ReadKey();
        }
    }
}
