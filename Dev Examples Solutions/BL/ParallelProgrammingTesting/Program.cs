#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace ParallelProgrammingTesting
{
    internal class Program
    {
        #region Constructors


        public Program()
        {
            //TaskExceptionTest.Test();
            //TestTaskExceptions();
            TestTaskDelay();
        }

        private void TestTaskDelay()
        {
            Task.Factory.StartNew( () =>
            {
                for (int a = 0; a < 5; a++)
                {
                    Console.WriteLine("A {0} {1}", DateTime.Now.ToString("ss.fff"), a);
                    TaskDelay(500).Wait();
                }
            });Task.Factory.StartNew( async () =>
            {
                for (int a = 0; a < 5; a++)
                {
                    Console.WriteLine("B {0} {1}", DateTime.Now.ToString("ss.fff"), a);
                    await TaskDelay(500);
                }
            });
        }

        public static Task TaskDelay(int milliseconds)
        {
            TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
            new Timer(_ => taskCompletionSource.SetResult(null)).Change(milliseconds, -1);
            return taskCompletionSource.Task;
        }


        #endregion

        #region Private Methods

        private void TestTaskExceptions()
        {
            Task myTask = Task.Factory.StartNew(() =>
            {
                throw new Exception("Task exception number 1");
            });

            try
            {
                myTask.Wait();
            }
            catch (AggregateException aggregateException)
            {
                aggregateException.Handle(exception =>
                {
                    Console.WriteLine(exception.Message);
                    return true;
                });
            }
        }

        private void TestTAsks()
        {
            for (int b = 0; b < 10; b++)
            {
                Task<int> task = Task.Factory.StartNew(p => SomeTaskFunction(b), 99);
                Console.WriteLine(task.AsyncState);
                //task.Wait();
            }
        }

        private int SomeTaskFunction(int p_iName)
        {
            int managedThreadId = Thread.CurrentThread.ManagedThreadId;
            for (int a = 0; a < 5; a++)
            {
                Console.WriteLine("Threads:{3} Name:{0} T:{1} A:{2}", p_iName, managedThreadId, a);
                Thread.Sleep(100);
            }
            return managedThreadId;
        }

        private void ParallelForAndForeach()
        {
            var v1 = Parallel.For(0, 5, (p_i) =>
            {
                Console.WriteLine("For Thread: {0} i={1}", Thread.CurrentThread.ManagedThreadId, p_i);
                Thread.Sleep(100);
            });
            var v2 = Parallel.ForEach(Enumerable.Range(100, 5), (p_i) =>
            {
                Console.WriteLine("Foreach Thread: {0} i={1}", Thread.CurrentThread.ManagedThreadId, p_i);
                Thread.Sleep(100);
            });
        }

        private void ParallelInvoke()
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            Parallel.Invoke(new Action[] { someAction, someAction, someAction, someAction, someAction, someAction, someAction, someAction });
            sw.Stop();
            Console.WriteLine("\n *** {0} ms", sw.ElapsedMilliseconds);
        }


        private void someAction()
        {
            Console.WriteLine("+Starting thread: {0}", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(1000);
            Console.WriteLine("-   Ended thread: {0}", Thread.CurrentThread.ManagedThreadId);
        }

        private void getPrimes()
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            IEnumerable<int> numbers = Enumerable.Range(2000000, 3000000);
            ParallelQuery<int> parallelQuery = numbers.AsParallel().Where(isPrime);
            //ParallelQuery<int> parallelQuery = numbers.AsParallel().AsOrdered().Where(isPrime);
            int primes = parallelQuery.Count();
            sw.Stop();
            Console.WriteLine("\n***Parallel Primes={0} in {1}ms ***", primes, sw.ElapsedMilliseconds);
            sw.Restart();
            IEnumerable<int> query = numbers.Where(isPrime);
            primes = query.Count();
            Console.WriteLine("\n***Normal Primes={0} in {1}ms ***", primes, sw.ElapsedMilliseconds);
        }

        private bool isPrime(int p_number)
        {
            IEnumerable<int> testedNumbers = Enumerable.Range(2, (int)Math.Sqrt(p_number));
            return testedNumbers.All(p_i => p_number % p_i > 0);
        }

        private static void Main(string[] args)
        {
            new Program();
            Console.WriteLine("\n****** Finished ******");
            Console.ReadKey();
        }

        #endregion
    }
}