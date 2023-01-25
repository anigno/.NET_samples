using System;
using System.Collections.Generic;
using System.Linq;

namespace AnignoraCommonAndHelpers.Helpers
{
    public static class LinqHelper
    {
        #region Public Methods

        public static IEnumerable<T> TakeLast<T>(this IList<T> p_source, int p_n)
        {
            int count = p_source.Count();
            IEnumerable<T> takeLast = p_source.Skip(Math.Max(0, count - p_n));
            return takeLast;
        }

        #endregion
    }
}