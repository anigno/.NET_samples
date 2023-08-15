using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TasksAndAsync
{
    internal class ParallelRunExample
    {
        private readonly Random random = new Random();

        public ParallelRunExample()
        {
            Logger.Log($"RunCalculationsSync");
            RunCalculationsSync(10);
            Logger.Log($"RunCalculationAsync");
            RunCalculationAsync(10);
            Logger.Log($"after RunCalculationAsync called");
            Logger.Log($"RunCalculationParallel");
            RunCalculationParallel(10);
            Logger.Log($"after RunCalculationParallel");

        }

        private int Calculate(int value)
        {
            Thread.Sleep(random.Next(100, 1300));
            return value * value;
        }

        public void RunCalculationsSync(int n)
        {
            Logger.Log($"RunCalculationsSync started");
            for (int i = 0; i < n; i++)
            {
                int ret = Calculate(i);
                Logger.Log($"s {i}: {ret}");
            }
            Logger.Log($"RunCalculationsSync ended");
        }

        public async Task RunCalculationAsync(int n)
        {
            Logger.Log($"RunCalculationAsync started");
            for (int i = 0; i < n; i++)
            {
                int ret = await Task.Factory.StartNew(() => Calculate(i));
                Logger.Log($"a {i}: {ret}");
            }
            Logger.Log($"RunCalculationAsync ended");
        }

        public void RunCalculationParallel(int n)
        {
            Parallel.For(0, n, (i) =>
                {
                    int ret = Calculate(i);
                    Logger.Log($"p {i}: {ret}");
                });
        }
    }
}
