using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AnignoraCommonAndHelpers.Tracers;
using AnignoraProcesses.BThreadPool;
using NUnit.Framework;

namespace AnignoraLibrary_2010_Tester.TestBThreadPool
{
    [TestFixture]
    public class TestingBThreadsPooling
    {
        private readonly BThreadPool m_pool = new BThreadPool(new TracerConsole(), 2, "MyThreads");
        private readonly TracerConsole m_tracer = new TracerConsole();

        private readonly ManualResetEvent m_allQueuedActionsEndedEvent = new
            ManualResetEvent(false);

        [Test]
        public void EnqueueStartEnqueueAndCheckAllRun()
        {
            for (int a = 1; a <= 20; a++)
            {
                m_pool.EnqueueAction(new SampleAction(100, int.MaxValue, int.MaxValue, "Action " + a.ToString("00"), 1, int.MaxValue));
            }
            Assert.AreEqual(0, m_pool.RunningActionsCount);
            m_allQueuedActionsEndedEvent.Reset();
            m_pool.Start();
            m_allQueuedActionsEndedEvent.WaitOne();
            Assert.AreEqual(0, m_pool.RunningActionsCount);
            m_allQueuedActionsEndedEvent.Reset();
            for (int a = 21; a <= 40; a++)
            {
                m_pool.EnqueueAction(new SampleAction(100, int.MaxValue, int.MaxValue, "Action " + a.ToString("00"), 1, int.MaxValue));
            }
            m_allQueuedActionsEndedEvent.WaitOne();
            Assert.AreEqual(0, m_pool.RunningActionsCount);
            m_pool.Stop();
        }

        [Test]
        public void FailedWithVariesRetries()
        {
            for (int a = 1; a <= 3; a++)
            {
                m_pool.EnqueueAction(new SampleAction(100*a, int.MaxValue, 50*a, "Action " + a.ToString("00"), a, int.MaxValue));
            }
            m_pool.Start();
            m_allQueuedActionsEndedEvent.Reset();
            m_allQueuedActionsEndedEvent.WaitOne();
            string result = m_activitiesStringBuilder.ToString();
            m_tracer.Trace(result);
            Assert.AreEqual(1, GetStringlnStringCount(result, "PoolActionFailedAll Action 01"));
            Assert.AreEqual(1, GetStringlnStringCount(result, "PoolActionFailedAll Action 02"));
            Assert.AreEqual(1, GetStringlnStringCount(result, "PoolActionFailedAll Action 03"));
            m_pool.Stop();
        }

        [Test]
        public void ExceptionWithVariesRetries()
        {
            for (int a = 1; a <= 3; a++)
            {
                m_pool.EnqueueAction(new SampleAction(100*a, 50*a, int.MaxValue, "Action " + a.ToString("00"), a, int.MaxValue));
            }
            m_pool.Start();
            m_allQueuedActionsEndedEvent.Reset();
            m_allQueuedActionsEndedEvent.WaitOne();
            string result = m_activitiesStringBuilder.ToString();
            m_tracer.Trace(result);
            Assert.AreEqual(1, GetStringlnStringCount(result, "PoolActionFailedAll Action 01"));
            Assert.AreEqual(1, GetStringlnStringCount(result, "PoolActionFailedAll Action 02"));
            Assert.AreEqual(1, GetStringlnStringCount(result, "PoolActionFailedAll Action 03"));
            Assert.AreEqual(1, GetStringlnStringCount(result,
                                                      "PoolActionExceptionOccured Action 01"));
            Assert.AreEqual(2, GetStringlnStringCount(result,
                                                      "PoolActionExceptionOccured Action 02"));
            Assert.AreEqual(3, GetStringlnStringCount(result,
                                                      "PoolActionExceptionOccured Action 03"));
            m_pool.Stop();
        }

        [Test]
        public void TimeoutWithVariesRetries()
        {
            for (int a = 1; a <= 3; a++)
            {
                m_pool.EnqueueAction(new SampleAction(100*a, int.MaxValue, int.MaxValue, "Action " + a.ToString("00"), a, 50*a));
            }
            m_pool.Start();
            m_allQueuedActionsEndedEvent.Reset();
            m_allQueuedActionsEndedEvent.WaitOne();
            string result = m_activitiesStringBuilder.ToString();
            m_tracer.Trace(result);
            //Assert.AreEqual(1, GetStringlnStringCount(result, "PoolActionFailedAll Action 01"));
            Assert.AreEqual(1, GetStringlnStringCount(result, "PoolActionFailedAll Action 02"));
            Assert.AreEqual(1, GetStringlnStringCount(result, "PoolActionFailedAll Action 03"));
            Assert.AreEqual(1, GetStringlnStringCount(result, "PoolActionTimeout Action 01"));
            Assert.AreEqual(2, GetStringlnStringCount(result, "PoolActionTimeout Action 02"));
            Assert.AreEqual(3, GetStringlnStringCount(result, "PoolActionTimeout Action 03"));
            m_pool.Stop();
        }

        [Test]
        public void TimeoutForOneAction()
        {
            for (int a = 1; a <= 1; a++)
            {
                m_pool.EnqueueAction(new SampleAction(100*a, int.MaxValue, int.MaxValue, "Action " + a.ToString("00"), a, 50*a));
            }
            m_pool.Start();
            m_allQueuedActionsEndedEvent.Reset();
            m_allQueuedActionsEndedEvent.WaitOne();
            string result = m_activitiesStringBuilder.ToString();
            m_tracer.Trace(result);
            Assert.AreEqual(1, GetStringlnStringCount(result, "PoolActionFailedAll Action 01"));
            Assert.AreEqual(1, GetStringlnStringCount(result, "PoolActionTimeout Action 01"));
            m_pool.Stop();
        }

        [SetUp]
        public void BeforeEachTest()
        {
            m_activitiesStringBuilder.Clear();
        }

        private int GetStringlnStringCount(string p_container, string p_contained)
        {
            Regex rex = new Regex(p_contained);
            int count = rex.Matches(p_container).Count;
            return count;
        }

        [TestFixtureSetUp]
        public void TestSetup()
        {
            m_pool.AllQueuedActionsEnded += PoolAllQueuedActionsEnded;
            m_pool.ActionFailedAll += PoolActionFailedAll;
            m_pool.ActionExceptionOccured += PoolActionExceptionOccured;
            m_pool.ActionSucceeded += PoolActionSucceeded;
            m_pool.ActionTimeout += PoolActionTimeout;
            m_pool.ActionStarted += PoolActionStarted;
        }

        private readonly StringBuilder m_activitiesStringBuilder = new StringBuilder();

        private void appentString(string p_string)
        {
            lock (m_activitiesStringBuilder)
            {
                m_activitiesStringBuilder.Append(p_string);
            }
        }

        private void PoolActionStarted(object sender, ActionEventArgs e)
        {
            m_tracer.Trace("pool_ActionStarted {0} ", e.Actionltem.Descriptor);

            appentString(string.Format("[PoolActionStarted {0}] ", e.Actionltem.Descriptor));
        }

        private void PoolActionTimeout(object sender, ActionEventArgs e)
        {
            m_tracer.Trace("pool__ActionTimeout {0}", e.Actionltem.Descriptor);
            appentString(string.Format("[PoolActionTimeout {0}] ", e.Actionltem.Descriptor));
        }

        private void PoolActionSucceeded(object sender, ActionEventArgs e)
        {
            m_tracer.Trace("pool_ActionSucceeded {0}", e.Actionltem.Descriptor);
            appentString(string.Format("[PoolActionSucceeded {0}] ", e.Actionltem.Descriptor));
        }

        private void PoolActionExceptionOccured(object sender, ActionExceptionEventArgs e)
        {
            m_tracer.Trace("pool_ActionExceptionOccured {0}", e.Actionltem.Descriptor);
            appentString(string.Format("[PoolActionExceptionOccured {0}] ", e.Actionltem.Descriptor));
        }

        private void PoolActionFailedAll(object sender, ActionEventArgs e)
        {
            m_tracer.Trace(" PoolActionFailedAll {0}", e.Actionltem.Descriptor);
            appentString(string.Format("[PoolActionFailedAll {0}] ", e.Actionltem.Descriptor));
        }

        private void PoolAllQueuedActionsEnded(object sender, EventArgs e)
        {
            m_tracer.Trace("PoolAllQueuedActionsEnded");
            appentString(" [PoolAllQueuedActionsEnded] ");
            m_allQueuedActionsEndedEvent.Set();
        }

      
    }
}