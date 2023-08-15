using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TasksAndAsync
{
    internal class AsyncAwaitExample2
    {
        public AsyncAwaitExample2()
        {
            Logger.Log("0-calling CallHeavyMethodAsync");
            var t = CallHeavyMethodAsync(17);
            Logger.Log($"2-after CallHeavyMethodAsync \n\n(doing other staff here)\n");
            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(500);
                Logger.Log("staff");
            }
            Logger.Log($"5-using CallHeavyMethodAsync returned value: {t.Result}");
        }

        private async Task<string> CallHeavyMethodAsync(int someNumber)
        {
            Logger.Log("1-enter CallHeavyMethodAsync");
            var result = await Task.Factory.StartNew(() => HeavyMethod(someNumber));
            Logger.Log($"4-exit CallHeavyMethodAsync with return: {result}");
            return result;
        }

        private string HeavyMethod(int someNumber)
        {
            Logger.Log($"3-enter Task method: HeavyMethod with param: {someNumber}");
            Thread.Sleep(3000);
            return $"{someNumber}*2={someNumber * 2}";
        }
    }
}
