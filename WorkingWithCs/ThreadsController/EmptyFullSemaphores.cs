using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ThreadsController
{
    internal class EmptyFullSemaphores
    {
        private static int MAX_CONCURENT_WORK = 4;
        private Semaphore emptySem = new Semaphore(MAX_CONCURENT_WORK, MAX_CONCURENT_WORK);
        private Semaphore fullSem = new Semaphore(0, MAX_CONCURENT_WORK);
        private Queue<int> queue = new Queue<int>();

        public EmptyFullSemaphores()
        {
            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread(ProducerMethod);
                thread.IsBackground = true;
                thread.Start(i + 1);
            }
            Thread.Sleep(100);
            Logger.Log("queue is full, producers waits");
            Thread.Sleep(1000);
            for (int i = 0; i < 99; i++)
            {
                Thread thread = new Thread(ConsumerMethod);
                thread.IsBackground = true;
                thread.Start(i + 1);
            }
        }

        private void ProducerMethod(object state)
        {
            for (int i = 0; i < 3; i++)
            {
                emptySem.WaitOne(); //limit queue size by waiting producers
                int value = i + 100 * (int)state;
                lock (queue)
                {
                    queue.Enqueue(value);
                }
                Logger.Log($"producer: {state} enqueued: {value}");
                fullSem.Release(); //item enqueue, enable consumer to enter
            }
        }

        private void ConsumerMethod(object state)
        {
            for (int i = 0; i < 3; i++)
            {
                fullSem.WaitOne(); //consumer waits until items exist in queue
                lock (queue)
                {
                    int value = queue.Dequeue();
                    Logger.Log($"consumer: {state} dequeued: {value}");
                }
                emptySem.Release(); //after item was dequeue, release producers to enqueue more item
            }
        }
    }
}
