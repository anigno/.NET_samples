using System;
using System.Linq;

namespace OopExample
{
    //Extesion method
    public static class MyExtensions
    {
        public static RationalNumber MultiplyIt(this RationalNumber extendedObject, int multiplier)
        {
            RationalNumber r = new RationalNumber(extendedObject.Counter * multiplier,
                extendedObject.Devider * multiplier);
            return r;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            RationalNumber r1 = new RationalNumber(1, 4);
            RationalNumber r2 = new RationalNumber(2, 3);
            RationalNumber r3 = new RationalNumber(r2);
            RationalNumber r4 = (RationalNumber)(r1 + r3);
            Console.WriteLine($"{r4}");
            var r5 = r4.MultiplyIt(3);
            Console.WriteLine($"{r5}");

            Console.WriteLine($"Any key to exit");
            Console.ReadKey();
        }


    }
}
