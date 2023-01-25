#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

#endregion

namespace PerformenceMonitoring
{
    public class APerformenceMonitor
    {
        #region Constructors

        public APerformenceMonitor(int p_sampleRateSec)
        {
            string file = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);
            m_totalCpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            m_appCpu = new PerformanceCounter("Process", "% Processor Time", file);
            m_appWorkingSet = new PerformanceCounter("Process", "Working Set", file);

            m_sampleTimer = new Timer(Callback, null, 100, p_sampleRateSec*1000);
        }

        #endregion

        #region Private Methods

        private void Callback(object p_state)
        {
            Console.WriteLine("{0}% cpu, {1}% appCpu {2}kb", m_totalCpu.NextValue(), m_appCpu.NextValue(), m_appWorkingSet.NextValue()/1024);
            for (int a = 0; a < 99999999; a++)
            {
                double b = a*3.12334;
            }
        }

        #endregion

        #region Fields

        private PerformanceCounter m_appCpu;
        private PerformanceCounter m_totalCpu;
        private PerformanceCounter m_appWorkingSet;
        private Timer m_sampleTimer;

        #endregion
    }
}