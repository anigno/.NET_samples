#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

#endregion

namespace BlCommon
{
    [TestFixture]
    internal class ConcurrencyExamples
    {
        #region Public Methods

        [Test]
        public void TestConcurrentQueue()
        {
            ConcurrentQueue<long> queue = new ConcurrentQueue<long>();
            m_stopwatch.Restart();

            Task.Factory.StartNew(async () =>
            {
                //Fill queue
                while (m_stopwatch.ElapsedMilliseconds < ENQUEUE_TIME_MS)
                {
                    long enqueueMs = m_stopwatch.ElapsedMilliseconds;
                    queue.Enqueue(enqueueMs);
                    Debug.WriteLine("------------------>Enqueue: {0}", enqueueMs);
                    await Task.Delay(100);
                }
            }, TaskCreationOptions.LongRunning);

            Task.Factory.StartNew(async () =>
            {
                long prevItem = -1;
                //Dequeue
                while (m_stopwatch.ElapsedMilliseconds < ENQUEUE_TIME_MS)
                {
                    await Task.Delay(500);
                    Debug.WriteLine("** Start dequeue **");
                    for (int a = 0; a < queue.Count; a++)
                    {
                        //Debug.WriteLine("<==Dequeue {0}", enumerator.Current);
                        long dequeuedItem;
                        bool b = queue.TryDequeue(out dequeuedItem);
                        Debug.WriteLine("<==Dequeue {0} {1}", dequeuedItem, b);
                        //Assert.AreEqual(enumerator.Current, dequeuedItem);
                        Assert.True(b);
                        Assert.Greater(dequeuedItem, prevItem);
                        prevItem = dequeuedItem;
                        await Task.Delay(50);
                    }
                    Debug.WriteLine("** End dequeue **");
                }
            }, TaskCreationOptions.LongRunning);
            Thread.Sleep((int) ENQUEUE_TIME_MS);
        }

        [Test]
        public void TestBlockingCollection()
        {
            BlockingCollection<long> blockingCollection = new BlockingCollection<long>(new ConcurrentQueue<long>());
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            ManualResetEvent endEvent = new ManualResetEvent(false);
            m_stopwatch.Restart();

            Task.Factory.StartNew(async () =>
            {
                //Fill queue
                while (m_stopwatch.ElapsedMilliseconds < ENQUEUE_TIME_MS)
                {
                    long enqueueMs = m_stopwatch.ElapsedMilliseconds;
                    blockingCollection.Add(enqueueMs);
                    Debug.WriteLine("------------------>Enqueue: {0}", enqueueMs);
                    await Task.Delay(100, cancellationTokenSource.Token);
                }
                Debug.WriteLine("End enqueue");
                cancellationTokenSource.Cancel(false);
                Debug.WriteLine("After Cancel");
                await Task.Delay(1000);
                endEvent.Set();
            }, TaskCreationOptions.LongRunning);

            Task.Factory.StartNew(async () =>
            {
                long prevItem = -1;
                //Dequeue
                await Task.Delay(500);
                foreach (long dequeuedItem in blockingCollection.GetConsumingEnumerable(cancellationTokenSource.Token))
                {
                    try
                    {
                        Debug.WriteLine("<==Dequeue {0}", dequeuedItem);
                        Assert.Greater(dequeuedItem, prevItem);
                        prevItem = dequeuedItem;
                        await Task.Delay(50, cancellationTokenSource.Token);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
                Debug.WriteLine("Out of blockingCollection.GetConsumingEnumerable()");
            }, TaskCreationOptions.LongRunning);

            endEvent.WaitOne();
            Debug.WriteLine("End all");
        }

        #endregion

        #region Fields

        private readonly Stopwatch m_stopwatch = new Stopwatch();

        #endregion

        #region Constants

        public const long ENQUEUE_TIME_MS = 2500;

        #endregion
    }
}