#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlCommon.Configurators;

#endregion

namespace BlCommon
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            Stopwatch sw1 = Stopwatch.StartNew();
            IEnumerable<int> findPrimes = PrimeFinder.FindPrimes((int) 1e5, (int) (2*1e5));
            sw1.Stop();
            TestUtils.ConsoleLog(ConsoleColor.Yellow, sw1.ElapsedMilliseconds.ToString());
            TestUtils.ConsoleLog(ConsoleColor.Yellow, findPrimes.ToString(","));
            TestUtils.WaitForEnter();

            TestUtils.WaitForEnter();
            return;

            DateTime dateTime1970 = new DateTime(1970, 1, 1, 0, 0, 0);
            long ticks1970 = dateTime1970.Ticks;
            TimeSpan timeSpan1970 = TimeSpan.FromTicks(ticks1970);
            DateTime t0 = new DateTime(ticks1970);
            DateTime t1 = new DateTime(timeSpan1970.Ticks);
            long ticks0 = (long) (ticks1970 + 1400000000*10000000.0);
            DateTime t3 = new DateTime(ticks0);


            DateTime t4 = new DateTime(2013, 12, 1, 12, 00, 00);
            long ticks4 = t4.Ticks;
            ticks4 = ticks4 - ticks1970;


            DateTime dateTime = new DateTime(2010, 1, 1);
            long ticks = dateTime.Ticks;
            TimeSpan timeSpan = TimeSpan.FromTicks(ticks);
            var timeSpanTicks = timeSpan.Ticks;
            DateTime dateTime2 = new DateTime(timeSpan.Ticks);
            var dif = ticks - timeSpanTicks;
            var miliseconds = timeSpan.TotalMilliseconds;
            var microSeconds = miliseconds*1000;
            var factor = ticks/microSeconds;
            var nanoSeconds = ticks*100;

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            Thread.Sleep(500);
            sw.Stop();
            var elapsedTicks = sw.Elapsed.Ticks;
            var secondsElapsed = elapsedTicks/10000000d;
            var swTicks = sw.ElapsedTicks;
            var swMs = sw.ElapsedMilliseconds;

            var ticksRatio = 1d*elapsedTicks/swTicks;
            var frequency = Stopwatch.Frequency;

            var secondsBySw = 1d*swTicks/frequency;

            var diffSec = secondsBySw - secondsElapsed;

            TestUtils.WaitForEnter();
        }

        #endregion
    }
}