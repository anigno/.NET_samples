using System;
using System.Collections.Generic;
using System.Linq;

namespace AnignoraCommonAndHelpers.Helpers
{
    public class MathHelper
    {
        #region Public Methods

        public static double Average(double[] p_items, int p_startIndex, int p_nItems)
        {
            double sum = default(float);
            for (int a = p_startIndex; a < p_startIndex + p_nItems; a++)
            {
                sum += p_items[a];
            }
            return sum/p_nItems;
        }

        /// <summary>
        /// Fix the problem of inaccurate float numbers (E.G. 5.999995 changed to 6)
        /// </summary>
        public static float FixFloat(float f)
        {
            long l = (long) (f*10000000);
            f = l/10000000f;
            return f;
        }

        /// <summary>
        /// Gets the Cosine value from a degree angle
        /// </summary>
        public static float CosA(float angle)
        {
            return (float) Math.Cos(angle/57.295779);
        }

        /// <summary>
        /// Gets the Sine value from a degree angle
        /// </summary>
        public static float SinA(float angle)
        {
            return (float) Math.Sin(angle/57.295779);
        }

        public static double GetStandardDeviation(IList<double> p_doubleList)
        {
            double average = p_doubleList.Average();
            double sumOfDerivation = 0;
            foreach (double value in p_doubleList)
            {
                sumOfDerivation += (value)*(value);
            }
            double sumOfDerivationAverage = sumOfDerivation/(p_doubleList.Count - 1);
            return Math.Sqrt(sumOfDerivationAverage - (average*average));
        }

        #endregion
    }
}