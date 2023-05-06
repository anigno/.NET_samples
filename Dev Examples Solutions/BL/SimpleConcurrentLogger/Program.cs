#region

using System.IO;
using System.Threading.Tasks;
using BlCommon;

#endregion

namespace SimpleConcurrentLogger
{
    internal class Program
    {
        #region Constructors

        public Program()
        {
            s_logger.Init(p_s => { File.AppendAllText("Log.txt", p_s + "\n"); });
            File.WriteAllText("Log.txt", "");
            Task.Factory.StartNew(async () =>
            {
                for (int a = 0; a < 500; a += 100)
                {
                    s_logger.Log(a.ToString());
                    await Task.Delay(100);
                }
            }, TaskCreationOptions.LongRunning);

            Task.Factory.StartNew(async () =>
            {
                for (int a = 0; a < 250; a += 50)
                {
                    s_logger.Log(a.ToString());
                    await Task.Delay(50);
                }
            }, TaskCreationOptions.LongRunning);

            Task.Factory.StartNew(async () =>
            {
                for (int a = 0; a < 1000; a += 200)
                {
                    s_logger.Log(a.ToString());
                    await Task.Delay(200);
                }
            }, TaskCreationOptions.LongRunning);
        }

        #endregion

        #region Private Methods

        private static void Main(string[] args)
        {
            new Program();
            TestUtils.WaitForEnter();
        }

        #endregion

        #region Fields

        private static readonly SimpleLogger s_logger = new SimpleLogger();

        #endregion
    }
}