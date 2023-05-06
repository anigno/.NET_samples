using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessesUpDown
{
    class Program
    {
        private const string applicationPath = @"C:\Program Files\Internet Explorer\iexplore.exe";
        public Program()
        {

            Process process = new Process();
            for (int a = 0; a < 5; a++)
            {
                process.StartInfo.FileName = applicationPath;
                process.StartInfo.Arguments = @"https://rnet/Pages/default.aspx?a=1&b=2";
                process.Start();
                Thread.Sleep(500);
            }
            Thread.Sleep(1000);
            Process killer=new Process();
            killer.StartInfo.FileName="killer.bat";
            killer.Start();
        }

        static void Main(string[] args)
        {
            new Program();
            Console.WriteLine("Enter to exit");
            Console.ReadLine();
        }
    }
}
