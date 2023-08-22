using Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadsController
{
    #region job items
    public interface IJobItem
    {
        int Id { get; set; }
        string Name { get; set; }
        int RunJob();
    }

    public abstract class JobItemBase : IJobItem
    {
        private readonly static object syncLock = new object();
        private static int uniqueueId = 1000;
        public int Id { get; set; }
        public string Name { get; set; }
        protected Random rand = new Random();

        private static int GetUniqueueId()
        {
            lock (syncLock)
            {
                return uniqueueId++;
            }
        }
        private JobItemBase()
        {
            Id = GetUniqueueId();
        }
        public JobItemBase(string name) : this()
        {
            Name = name;
        }
        public abstract int RunJob();
    }

    internal class CalcSubJobItem : JobItemBase
    {
        int num1;
        int num2;
        public CalcSubJobItem(string name, int a, int b) : base(name)
        {
            num1 = a;
            num2 = b;
        }
        public override int RunJob()
        {
            int c = num1 + num2;
            Thread.Sleep(rand.Next(500, 1500));
            return c;
        }
    }
    #endregion

    internal class ProducersConsumers
    {
        private readonly BlockingCollection<IJobItem> queue = new BlockingCollection<IJobItem>();
        Random rand = new Random();

        public void Start()
        {
            Thread producerThread = new Thread(ProducerThreadStart);
            producerThread.IsBackground = true;
            producerThread.Start("Producer");
            for (int i = 0; i < 2; i++)
            {
                Thread thread = new Thread(ConsumerThreadStart);
                thread.IsBackground = true;
                thread.Name = $"worker{i}";
                thread.Start();
            }
        }

        private void ConsumerThreadStart(object obj)
        {
            string threadName=Thread.CurrentThread.Name;
            foreach (IJobItem item in queue.GetConsumingEnumerable())
            {
                Logger.Log($"{threadName} run: {item.Name}");
                int result = item.RunJob();
            }
        }

        private void ProducerThreadStart(object obj)
        {
            Logger.Log((string)obj);
            for (int i = 0; i < 10; i++)
            {
                CalcSubJobItem item = new CalcSubJobItem($"item{i}", rand.Next(0, 10), rand.Next(0, 10));
                Logger.Log($"enqueue job: {item.Name}");
                queue.Add(item);
                Thread.Sleep(rand.Next(100, 500));
            }
            Thread.Sleep(2000);
            CalcSubJobItem item1 = new CalcSubJobItem($"item{99}", rand.Next(0, 10), rand.Next(0, 10));
            Logger.Log($"enqueue job: {item1.Name}");
            queue.Add(item1);

        }
    }
}
