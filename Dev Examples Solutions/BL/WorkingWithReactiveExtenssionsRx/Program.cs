using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkingWithReactiveExtenssionsRx
{
    class Program
    {
        public Program()
        {
        }

        public void ConditionExample()
        {
            Subject<DateTime> subject = new Subject<DateTime>();
            subject.Where((time) => { return time.Millisecond < 500; }).Subscribe(time => { Console.WriteLine("{0}", time.Millisecond); });
            Observable.Interval(TimeSpan.FromMilliseconds(100)).Subscribe(l => { subject.OnNext(DateTime.Now); });
        }


        public void SampleExample()
        {
            IObservable<long> intervalObservable = Observable.Interval(TimeSpan.FromMilliseconds(1000));
            intervalObservable.Subscribe(l => { Console.WriteLine("Direct {0}", l); });
            intervalObservable.Sample(TimeSpan.FromMilliseconds(2000)).Subscribe(l => { Console.WriteLine("Sample 2000 {0}", l); });
            intervalObservable.Sample(TimeSpan.FromMilliseconds(5000)).Subscribe(l => { Console.WriteLine("Sample 5000 {0}", l); });
        }

        public void TimingExample()
        {
            IObservable<long> intervalObservable = Observable.Interval(TimeSpan.FromMilliseconds(500));
            intervalObservable.Subscribe(l => { Console.WriteLine("Direct {0}", l); });
            intervalObservable.Delay(TimeSpan.FromMilliseconds(5000)).Subscribe(l => { Console.WriteLine("Delay 5000 {0}", l); });
            //Will display because it's wait 400ms for "No input"
            intervalObservable.Throttle(TimeSpan.FromMilliseconds(400)).Subscribe(l => { Console.WriteLine("Throttle 400 {0}", l); });
            //Will not display because it's wait 600ms for "No input" but every 500ms there is an input
            intervalObservable.Throttle(TimeSpan.FromMilliseconds(600)).Subscribe(l => { Console.WriteLine("Throttle 600 {0}", l); });

            //timeout operation
            Observable.Timer(TimeSpan.FromMilliseconds(2000)).Subscribe(l => { Console.WriteLine(l); });
        }

        public void AsyncFunctionCallExample()
        {
            Console.WriteLine("Thread: {0} for Main call", Thread.CurrentThread.ManagedThreadId);
            for (int a = 0; a < 10; a++)
            {
                AsyncFunction(a).Subscribe(x => { Console.WriteLine("Thread: {0:00} return value: {1}", Thread.CurrentThread.ManagedThreadId, x); });
            }
        }

        IObservable<long> AsyncFunction(int index)
        {
            return Observable.Start(() => SyncFunction(index));
        }

        long SyncFunction(int index)
        {
            Thread.Sleep(m_random.Next(10, 50));
            Console.WriteLine("Thread: {0:00} index: {1} for async function", Thread.CurrentThread.ManagedThreadId, index);
            return index;
        }


        readonly Random m_random = new Random((int)DateTime.Now.Ticks);

        public void SimpleSubscribtionExample()
        {
            Subject<int> producerSubject = new Subject<int>();

            IDisposable disposable = producerSubject.Subscribe(i => { Console.WriteLine("(All ) Thread: {0} received: {1}", Thread.CurrentThread.ManagedThreadId, i); });
            producerSubject.Take(4).Subscribe(i => { Console.WriteLine("(Take) Thread: {0} received: {1}", Thread.CurrentThread.ManagedThreadId, i); });
            producerSubject.Skip(2).Subscribe(i => { Console.WriteLine("(skip) Thread: {0} received: {1}", Thread.CurrentThread.ManagedThreadId, i); });
            producerSubject.Distinct().Subscribe(i => { Console.WriteLine("(Distinct) Thread: {0} received: {1}", Thread.CurrentThread.ManagedThreadId, i); });

            Task.Run(async () =>
            {
                Console.WriteLine("Thread: {0}", Thread.CurrentThread.ManagedThreadId);
                for (int a = 0; a < 10; a++)
                {
                    producerSubject.OnNext(m_random.Next(0, 5));
                    await Task.Delay(200);
                }
            });
        }

        public void RangeObservableExample()
        {
            IObservable<int> rangeObservable = Observable.Range(1, 10);
            rangeObservable.Subscribe(i => { Console.Write("{0} ", i); });
        }

        static void Main(string[] args)
        {
            var p = new Program();
            Console.ReadLine();
            GC.KeepAlive(p);
        }
    }
}
