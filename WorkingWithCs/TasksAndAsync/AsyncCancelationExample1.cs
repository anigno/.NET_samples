using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TasksAndAsync
{
    internal class AsyncCancelationExample1
    {

        public AsyncCancelationExample1()
        {
            CancellationTokenSource cts = new CancellationTokenSource(8000);

            Task<int> t = LongTimeMethodAsync(cts.Token);
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(500);
                Logger.Log("staff");
            }
            Logger.Log("canceled");
            cts.Cancel();
            Logger.Log($"LongTimeMethodAsync returned: {t.Result}");
        }

        private async Task<int> LongTimeMethodAsync(CancellationToken token)
        {
            return await Task.Factory
                .StartNew(
                    () =>
                    {
                        try
                        {
                            LongTimeMethod(token);
                        }
                        catch (TaskCanceledException tex)
                        {
                            Logger.Log(tex.ToString());
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex.ToString());
                        }
                        finally { Logger.Log("finally"); }
                        return -1;
                    });
        }

        private int LongTimeMethod(CancellationToken token)
        {
            Logger.Log($"start long time method");
            for (int i = 0; i < 10; i++)
            {
                if (token.IsCancellationRequested)
                    throw new TaskCanceledException();
                Thread.Sleep(500);
            }
            Logger.Log($"end long time method");
            return 17;
        }
    }
}
