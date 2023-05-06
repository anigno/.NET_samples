#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BlCommon
{
    public static class PrimeFinder
    {
        #region Public Methods

        public static bool IsPrime(int p_number)
        {
            IEnumerable<int> numbersCheckList = Enumerable.Range(2, p_number/2 - 2 + 1);
            bool isPrime = numbersCheckList.All(p_i => p_number%p_i != 0);
            return isPrime;
        }

        public static IEnumerable<int> FindPrimes(int p_startNumber, int p_endNumber)
        {
            IEnumerable<int> range = Enumerable.Range(p_startNumber, p_endNumber - p_startNumber + 1);
            IEnumerable<int> primes = range.AsParallel().Where(p_i => IsPrime(p_i));
            return primes;
        }

        #endregion
    }
}