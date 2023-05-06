#region

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using BlCommon;

#endregion

namespace SimpleConcurrentLogger
{
    public class SimpleLogger
    {
        #region Public Methods

        public void Log(string p_text)
        {
            m_blockingCollection.Add(string.Format("[{0}] [{1}]", DateTime.Now.ToString("HH:mm:ss.fff"), p_text));
        }

        public void Init(Action<string> p_logWriter)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (string dequeuedItem in m_blockingCollection.GetConsumingEnumerable(m_cancellationTokenSource.Token))
                {
                    p_logWriter(dequeuedItem);
                    Console.WriteLine(dequeuedItem);
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void Close()
        {
            m_cancellationTokenSource.Cancel();
        }

        #endregion

        #region Fields

        private readonly BlockingCollection<string> m_blockingCollection = new BlockingCollection<string>(new ConcurrentQueue<string>());
        private readonly CancellationTokenSource m_cancellationTokenSource = new CancellationTokenSource();

        #endregion
    }
}