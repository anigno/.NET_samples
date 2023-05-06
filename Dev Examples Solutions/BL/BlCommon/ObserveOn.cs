#region

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

#endregion

namespace BlCommon
{
    internal class ObserveOn
    {
        #region Public Methods

        [Test]
        public void Test1()
        {
            Subject<int> subject = new Subject<int>();

            //ObserveOn current thread as the producer
            subject.ObserveOn(Scheduler.CurrentThread).Subscribe(p_i => { WriteThread(string.Format("SubscriberA:{0}", p_i)); });

            //ObserveOn Different thread then the producer
            subject.ObserveOn(Scheduler.Default).Subscribe(p_i => { WriteThread(string.Format("SubscriberB:{0}", p_i)); });

            //The task is running on a different thread (LongRunning task)
            Task.Factory.StartNew(() =>
            {
                for (int a = 0; a < 3; a++)
                {
                    WriteThread(string.Format("Producer:{0}", a));
                    subject.OnNext(a);
                    Task.Delay(200).Wait();
                }
            }, TaskCreationOptions.LongRunning);

            Thread.Sleep(2500);
        }


        [Test]
        public void Test_Observable_()
        {
            IObservable<long> observable = Observable.Interval(TimeSpan.FromMilliseconds(250), Scheduler.Default);
            observable.Subscribe(p_l => WriteThread(p_l.ToString()));
            Thread.Sleep(1000);
        }

        [Test]
        public void Test_Observable_Timer()
        {
            //IObservable<long> updateObservable = Observable.Timer(TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(200), DispatcherScheduler.Current);
            WriteThread("Test2");
            IObservable<long> observable = Observable.Timer(TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(200), Scheduler.Default);
            observable.Subscribe(p_l => WriteThread(p_l.ToString()));
            Thread.Sleep(1000);
        }

        public static void WriteThread(string p_text)
        {
            TestUtils.DebugLog("{0} Thread: {1}", p_text, Thread.CurrentThread.ManagedThreadId);
        }

        #endregion
    }
}