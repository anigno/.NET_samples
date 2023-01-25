using System;
using System.Collections.Generic;
using System.Text;
using AnignoraDataTypes;

namespace AnignoraCommonAndHelpers.Helpers
{
    public class StringHelper
    {
        #region Public Methods

        public static string GetDateTimeString()
        {
            DateTime d = DateTime.Now;
            return string.Format("{0:D4}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}", d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
        }

        public static string GetTimeString(int seconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, seconds);
            return string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
        }

        public static string GetDateTimeFileString(DateTime dateTime, string seperator)
        {
            return GetDateFileString(dateTime, seperator) + "_" + GetTimeFileString(dateTime, seperator);
        }

        public static string GetDateFileString(DateTime dateTime, string seperator)
        {
            return string.Format("{0}{3}{1:00}{3}{2:00}", dateTime.Year, dateTime.Month, dateTime.Day, seperator);
        }

        public static string GetTimeFileStringWithMs(DateTime dateTime, string seperator)
        {
            return string.Format("{0}{2}{1:000}", GetDateTimeFileString(dateTime, seperator), dateTime.Millisecond, seperator);
        }

        public static string GetTimeFileString(DateTime dateTime, string seperator)
        {
            return string.Format("{0:00}{3}{1:00}{3}{2:00}", dateTime.Hour, dateTime.Minute, dateTime.Second, seperator);
        }

        public static string PrintArray<T>(IEnumerable<T> p_array)
        {
            StringBuilder sb = new StringBuilder();
            p_array.DoForAll(p => sb.Append("[" + p.ToString() + "] "));
            return sb.ToString();
        }

        #endregion
    }
}