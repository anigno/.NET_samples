using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSamples
{
    internal class Samples
    {
        public static void TestBreakContinue()
        {
            for (int a = 0; a < 5; a++)
            {
                for(int b = 0; b < 5; b++)
                {
                    Console.Write(b);
                    if (b == 1) break;
                }
                foreach (char c in "abcd") { 
                    Console.Write(c);
                    if (c == 'b') break;
                }
                Console.WriteLine($"->{a}");
            }
        }

        public static void TestStrings()
        {
            string s = "abcdefg";
            foreach (char c in s)
            {
                Console.WriteLine(c);
            }
        }

        public static void RunExceptions(int num)
        {
            Console.WriteLine("--------------");
            try
            {
                int b = 1 / num;
                if (num < 10) throw new Exception("less then 10 exception");
                Console.WriteLine("no exception");
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("division by zero catched");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("finally called");
            }
        }
    }
}



/*
merge sort
54321
543 21
54 3 21
45 3 12
345 12
12345

merge sorted lists
135 24
1 35 24 take 1 move first
12 35 4 take 2 move second
123 5 4 take 3 move first
1234 5 take 4 move second
12345 take 5 move first
nothing to take
 */