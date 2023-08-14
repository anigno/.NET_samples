using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace TestProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            Console.ReadKey();
        }

        public Program()
        {
            IEnumerable<int> r = Enumerable.Range(3, 10).Where(a => a % 2 != 0);
            foreach (int i in r) { Console.WriteLine(i); }
            r.ToList().ForEach(a => Console.WriteLine(a));

        }
    }
}
