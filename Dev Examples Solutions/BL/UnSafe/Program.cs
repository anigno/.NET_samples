using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlCommon;

namespace UnSafe
{
    public class Program
    {
        public static void Main()
        {
            new Program();
        }

        public Program()
        {
            unsafe
            {
                int* intPtr = stackalloc int[10];
                intPtr[0] = 017;
                intPtr[5] = 517;
                intPtr[9] = 917;
                for (int* a = intPtr; a < intPtr + 10; a++)
                {
                    TestUtils.ConsoleLog(ConsoleColor.Yellow, "{0}={1}",(int)a,*a);
                }
                fixed (int* intPtrFixed = new int[12])
                {
                    intPtrFixed[2] = 34;
                }
                Console.ReadLine();
            }
        }



    }
}
