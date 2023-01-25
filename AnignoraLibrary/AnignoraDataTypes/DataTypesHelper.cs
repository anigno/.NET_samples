namespace AnignoraDataTypes
{
    public static class DataTypesHelper
    {
        #region Public Methods

        /// <summary>
        /// Creates and initialize an array
        /// </summary>
        public static T[] CreateInitArray<T>(int p_items) where T : new()
        {
            T[] array = new T[p_items];
            array.DoForAll(p_obj => new T());
            return array;
        }

        public static double[] GetRangeOfDouble(double p_min, double p_max, int p_steps)
        {
            double[] results = new double[p_steps];
            double dStep = (p_max - p_min)/(p_steps - 1);
            for (int a = 0; a < p_steps; a++)
            {
                results[a] = dStep*a + p_min;
            }
            results[p_steps - 1] = p_max;
            return results;
        }

        #endregion
    }
}