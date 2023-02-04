using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalConsole
{
    public static class MyExtensionMethods
    {
        public static string ToStringListLines<T>(this IEnumerable<T> source)
        {
            StringBuilder sb = new("\n");
            foreach (object? o in source)
            {
                sb.Append($"{o?.ToString()}\n");
            }
            return sb.ToString();
        }
    }

}
