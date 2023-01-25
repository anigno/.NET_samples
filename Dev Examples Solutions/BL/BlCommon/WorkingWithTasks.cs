#region

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

#endregion

namespace BlCommon
{
    public class WorkingWithTasks
    {
        #region Public Methods

        [Test]
        public async void CancelationTest()
        {
            CancellationTokenSource source = new CancellationTokenSource(500);
            TestUtils.DebugLog("Start");
            Task delayTask = Task.Delay(1000, source.Token);
            try
            {
                await delayTask;
            }
            catch (TaskCanceledException taskCanceledException)
            {
                TestUtils.DebugLog("Canceled [{0}]", taskCanceledException);
            }
            TestUtils.DebugLog("End");
        }

        [Test]
        public async void PeriodicTskAsync()
        {
            for (int a = 0; a < 5; a++)
            {
                Debug.WriteLine("1 " + DateTime.Now);
                await Task.Delay(1000);
                Debug.WriteLine("2 " + DateTime.Now);
                await Task.Delay(1000);
            }
        }

        [Test]
        public void TestDelayedStartTask()
        {
            TestUtils.DebugLog("Start run");
            Task.Delay(500).ContinueWith(p_task => { TestUtils.DebugLog("Delayed run A"); });
            Task.Factory.StartNew(() =>
            {
                Task.Delay(500).Wait();
                TestUtils.DebugLog("Delayed run B");
            });
            TestUtils.DebugLog("End run");
            Task.Delay(1000).Wait();
        }


        [Test]
        public void RunTest()
        {
            Task.Factory.StartNew(TaskA);
            Task.Factory.StartNew(TaskB);
            Task.Delay(1000).Wait();
            Task.Factory.StartNew(AwaitUsage01);
            Task.Delay(1000).Wait();
        }

        public static async void TaskA()
        {
            for (int a = 0; a < 5; a++)
            {
                TestUtils.DebugLog("+TaskA before: {0}", a);
                await Task.Delay(100);
                TestUtils.DebugLog("-TaskA after: {0}", a);
            }
        }

        public static void TaskB()
        {
            TestUtils.DebugLog("++TaskB");
            for (int a = 0; a < 5; a++)
            {
                TestUtils.DebugLog("+TaskB before: {0}", a);
                Task.Delay(100).Wait();
                TestUtils.DebugLog("-TaskB after: {0}", a);
            }
        }

        public static async void AwaitUsage01()
        {
            TestUtils.DebugLog("Task started");
            Task<int> task = DoSomeAsyncWork();
            TestUtils.DebugLog("Task Doing some stuff, not needing the async data");
            int sum = await task;
            TestUtils.DebugLog("Task DoSomeAsyncWork() returned");
        }

        public static Task<int> DoSomeAsyncWork()
        {
            Task<int> t = Task<int>.Factory.StartNew(() =>
            {
                int sum = 0;
                for (int a = 0; a < 10; a++)
                {
                    sum += a;
                    Task.Delay(100).Wait();
                }
                return sum;
            });
            return t;
        }

        #endregion
    }
}