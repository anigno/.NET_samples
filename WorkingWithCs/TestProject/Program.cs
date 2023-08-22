using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
            Console.WriteLine(FindBestCrossingVerticalLine(new int[] { 1, 2, 3 }, new int[] { 7, 6, 6 }));
            // _-------_
            // __-----__
            // ___----__
            //answer could be 4,5,6
        }


        int FindBestCrossingVerticalLine(int[] horLinesStart, int[] horLinesEnd)
        {
            int crossed = 0;

            for (int i = 0; i < horLinesStart.Length; i++)
            {

            }
            return crossed;
        }
    }
}
