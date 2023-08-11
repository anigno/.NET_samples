using System;
using System.Linq;

namespace ReadersWritersProblem
{
    internal class Program
    {
        ReaderWriter readerWriter = new ReaderWriter();
        public Program()
        {
            readerWriter.Start();
        }

        static void Main(string[] args)
        {
            new Program();
            Console.ReadKey();
        }
    }
}
