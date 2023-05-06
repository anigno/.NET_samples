#region

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

#endregion

namespace BlCommon.SimplePipeCommunicator
{
    [TestFixture]
    public class SimplePipeCommunicatorTests
    {
        #region Public Methods

        [Test]
        public void Test01()
        {
            ManualResetEvent testEndEvent = new ManualResetEvent(false);
            string[] data;
            //data = new[] { "Hello", "World", "..." };
            data = Enumerable.Range(1000, 2000).Select(p => p.ToString()).ToArray();
            Task.Factory.StartNew(() =>
            {
                SimplePipeCommunicatorTyped<string> pipeStringWriter = new SimplePipeCommunicatorTyped<string>("StringsPipe");
                pipeStringWriter.OnConnectionTimeout += () => TestUtils.ConsoleLog(ConsoleColor.Red, "Writer Connection timeout");
                pipeStringWriter.OnClosed += () => TestUtils.ConsoleLog(ConsoleColor.Yellow, "Writer Closed");
                pipeStringWriter.OnConnected += () =>
                {
                    TestUtils.ConsoleLog(ConsoleColor.Yellow, "Writer Connected");
                    Task.Factory.StartNew(async () =>
                    {
                        foreach (string s in data)
                        {
                            pipeStringWriter.BeginWriteData(s);
                            //await Task.Delay(500);
                        }
                        pipeStringWriter.BeginClose();
                    });
                };
                pipeStringWriter.BeginConnect();
            });
            Task.Factory.StartNew(async () =>
            {
                SimplePipeCommunicatorTyped<string> pipeStringReader = new SimplePipeCommunicatorTyped<string>("StringsPipe", 1024);
                pipeStringReader.OnConnected += () => TestUtils.ConsoleLog(ConsoleColor.Green, "Reader Connected");
                pipeStringReader.OnClosed += () => TestUtils.ConsoleLog(ConsoleColor.Green, "Reader Closed");
                int a = 0;
                pipeStringReader.OnDataReceived += p_data =>
                {
                    TestUtils.ConsoleLog(ConsoleColor.Green, p_data);
                    Assert.AreEqual(data[a++], p_data);
                };
                pipeStringReader.BeginWaitForConnection();
                await Task.Delay(2000);
                pipeStringReader.BeginClose();
                await Task.Delay(1000);
                testEndEvent.Set();
            });
            testEndEvent.WaitOne();
        }

        #endregion
    }
}