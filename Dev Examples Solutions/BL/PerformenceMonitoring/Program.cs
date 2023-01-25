#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace PerformenceMonitoring
{
    internal class Program
    {
        #region Constructors

        public Program()
        {
            p = new APerformenceMonitor(1);
        }

        #endregion

        #region Private Methods

        private static void Main(string[] args)
        {
            new Program();
            Console.ReadKey();
        }

        #endregion

        #region Fields

        private APerformenceMonitor p;

        #endregion
    }
}