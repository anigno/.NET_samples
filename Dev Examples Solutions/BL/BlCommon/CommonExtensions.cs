#region

using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

#endregion

namespace BlCommon
{
    public static class CommonExtensions
    {
        #region Public Methods

        public static string ToString(this IEnumerable p_enumerable, string p_seperator)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var v in p_enumerable)
            {
                sb.AppendFormat("{0}{1}", v, p_seperator);
            }
            if (sb.Length == 0) return "";
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static long ElapsedMicroSeconds(this Stopwatch p_stopwatch)
        {
            long microSeconds = 1000000L * p_stopwatch.ElapsedTicks / Stopwatch.Frequency;
            return microSeconds;
        }

        public static Random CreateRandom()
        {
            return new Random((int)Stopwatch.GetTimestamp());
        }

        #endregion
    }
}