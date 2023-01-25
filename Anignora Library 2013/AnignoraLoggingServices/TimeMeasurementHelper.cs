using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AnignoraLoggingServices
{
    public static class TimeMeasurementHelper
    {
		#region (------  Const Fields  ------)

        private const long OFFSET_COUNTER = 10000;

		#endregion (------  Const Fields  ------)

		#region (------  Static Fields  ------)

        private static readonly long _offsetTicks;
        private static readonly Dictionary<string, long> _startTicks = new Dictionary<string, long>(10);
        private static readonly Stopwatch _stopwatch = new Stopwatch();
        private static readonly object _syncRoot = new object();

		#endregion (------  Static Fields  ------)

		#region (------  Constructors  ------)

        static TimeMeasurementHelper()
        {
            //Start stopwatch and set offset value
            _stopwatch.Start();
            AddMeasurementTimer("__InternalOffsetTimer0__");
            AddMeasurementTimer("__InternalOffsetTimer1__");
            AddMeasurementTimer("__InternalOffsetTimer2__");
            AddMeasurementTimer("__InternalOffsetTimer3__");
            AddMeasurementTimer("__InternalOffsetTimer4__");
            for (int a = 0; a < OFFSET_COUNTER; a++)
            {
                ResetAndStartMeasure("__InternalOffsetTimer__");
            }
            _offsetTicks = _stopwatch.ElapsedTicks / OFFSET_COUNTER;
            Console.WriteLine("TimeMeasurement Offset=" + _offsetTicks * 1000000 / Stopwatch.Frequency);
        }

		#endregion (------  Constructors  ------)

		#region (------  Static Methods  ------)

        public static void AddMeasurementTimer(string timerName)
        {
            lock (_syncRoot)
            {
                _startTicks.Add(timerName, 0);
            }
        }

        public static long GetMeasuredMicroseconds(string timerName)
        {
            lock (_syncRoot)
            {
                return (_stopwatch.ElapsedTicks - _startTicks[timerName] - _offsetTicks) * 1000000 / Stopwatch.Frequency;
            }
        }

        public static string GetMeasuredMicrosecondsString(string timerName)
        {
            lock (_syncRoot)
            {
                return ((_stopwatch.ElapsedTicks - _startTicks[timerName] - _offsetTicks) * 1000000 / Stopwatch.Frequency).ToString("0,0 uSec");
            }
        }

        public static void RemoveMeasurementTimer(string name)
        {
            lock (_syncRoot)
            {
                _startTicks.Remove(name);
            }
        }

        public static void ResetAndStartMeasure(string timerName)
        {
            lock (_syncRoot)
            {
                _startTicks[timerName] = _stopwatch.ElapsedTicks;
            }
        }

        public static void WriteToConsole(string timerName)
        {
            Console.WriteLine("Timer={0} time={1}", timerName, GetMeasuredMicrosecondsString(timerName));
        }

		#endregion (------  Static Methods  ------)
    }
}
