using System;
using System.Threading;
using AnignoraProcesses.BThreadPool;

namespace AnignoraLibrary_2010_Tester.TestBThreadPool
{
    public class SampleAction : ActionltemBase
    {
        public SampleAction(int p_actionEndDelayMs, int p_actionExceptionDelayMs, int p_actionFailedMs, string p_name, int p_retries, int p_timeoutMs)
            : base(p_name, p_retries, p_timeoutMs)
        {
            m_actionEndDelayMs = p_actionEndDelayMs;
            m_actionExceptionDelayMs = p_actionExceptionDelayMs;
            m_actionFailedMs = p_actionFailedMs;
        }

        public override void Abort()
        {
            Interlocked.Exchange(ref m_continueFlag, 0);
        }

        public override bool Run()
        {
            Interlocked.Exchange(ref m_continueFlag, 1);
            int cycles = 0;
            const int CYCLE_MS = 10;
            while (Interlocked.Read(ref m_continueFlag) == 1)
            {
                Thread.Sleep(CYCLE_MS);
                cycles++;
                int cyclesTime = cycles*CYCLE_MS;
                if (cyclesTime >= m_actionEndDelayMs) return true;
                if (cyclesTime >= m_actionFailedMs) return false;
                if (cyclesTime >= m_actionExceptionDelayMs) throw new Exception("Sample Action Exception");
            }
            return false;
        }

        private readonly int m_actionEndDelayMs;
        private readonly int m_actionExceptionDelayMs;
        private readonly int m_actionFailedMs;
        private long m_continueFlag = 0;
    }
}