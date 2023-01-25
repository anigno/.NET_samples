using System;
using System.Threading;

namespace AnignoraCommonAndHelpers.Tracers
{
    public class TracerConsole : ITracer
    {
        #region Public Methods

        public void Trace(string p_format, params object[] p_params)
        {
            Console.WriteLine("[{0} ] [{1}] {2}", DateTime.Now.ToString("hh:mm:ss.fff"), Thread.CurrentThread.Name, string.Format(p_format, p_params));
        }

        #endregion
    }
}