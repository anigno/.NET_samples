#region

using System;
using System.Threading.Tasks;
using BlCommon;

#endregion

namespace TimedQueue
{
    internal class Program
    {
        #region Constructors

        public Program()
        {
            m_queue.Start();
            m_queue.OnDequeue += m_queue_OnDequeue;
            Task.Factory.StartNew(async () =>
            {
                for (int a = 0; a < 10; a++)
                {
                    QueueTestElement element = new QueueTestElement(a);
                    m_queue.Enqueue(element);
                    TestUtils.ConsoleLog(ConsoleColor.Cyan, "--> Enqueue with ReleaseTime: {0}", element.ReleaseTime.ToString("ss.fff"));
                    await Task.Delay(a*200);
                }
            });
            Console.WriteLine("****** Press Enter to Exit ******");
            Console.ReadLine();
            m_queue.Stop();
        }

        #endregion

        #region Private Methods

        private void m_queue_OnDequeue(object sender, QueueTestElement e)
        {
            TestUtils.ConsoleLog(ConsoleColor.Yellow, "<-- Dequeue Release time: {0}", e.ReleaseTime.ToString("ss.fff"));
        }

        private static void Main(string[] args)
        {
            new Program();
        }

        #endregion

        #region Fields

        private QueueTimedDequeue<QueueTestElement> m_queue = new QueueTimedDequeue<QueueTestElement>();

        #endregion
    }

    internal class QueueTestElement : IQueueTimedItem
    {
        #region Constructors

        public QueueTestElement(int p_addedSeconds)
        {
            ReleaseTime = DateTime.Now.AddSeconds(p_addedSeconds);
            DequeueTime = ReleaseTime;
        }

        #endregion

        #region Public Properties

        public DateTime DequeueTime { get; private set; }
        public DateTime ReleaseTime { get; private set; }

        #endregion
    }
}