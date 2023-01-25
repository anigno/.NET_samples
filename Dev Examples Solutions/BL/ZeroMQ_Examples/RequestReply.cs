#region

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlCommon;
using NUnit.Framework;
using ZeroMQ;

#endregion

namespace ZeroMQ_Examples
{
    public class RequestReply
    {
        #region Public Methods

        [Test]
        public void TestRequestReply()
        {
            ZmqContext zmqContext = ZmqContext.Create();

            StartRepServer(zmqContext);

            StartReqClient("ClientA", zmqContext, 1);
            StartReqClient("ClientB", zmqContext, 3);
            StartReqClient("ClientC", zmqContext, 7);

            const int MILLISECONDS_DELAY = 5000;
            Task.Delay(MILLISECONDS_DELAY).Wait();
            Interlocked.Exchange(ref m_continueFlag, 0);
            long cnt = Interlocked.Read(ref m_counter);
            TestUtils.DebugLog("Total messages={0} {1} Hrz", cnt, cnt*1000/MILLISECONDS_DELAY);
        }

        #endregion

        #region Private Methods

        private void StartRepServer(ZmqContext p_zmqContext)
        {
            ZmqSocket serverSocket = p_zmqContext.CreateSocket(SocketType.REP);
            serverSocket.Bind(SERVER_TCP_ENDPOINT_A);
            TestUtils.DebugLog("Server ready");

            Task.Factory.StartNew(() =>
            {
                while (Interlocked.Read(ref m_continueFlag) == 1)
                {
                    string receivedMessage = serverSocket.Receive(Encoding.UTF8);
                    TestUtils.DebugLog("Server Received message: [{0}]", receivedMessage);
                    serverSocket.Send(string.Format("Server reply to: [{0}]", receivedMessage), Encoding.UTF8);
                }
                serverSocket.Dispose();
                p_zmqContext.Dispose();
            });
        }

        private void StartReqClient(string p_clientName, ZmqContext p_zmqContext, int p_interval)
        {
            var clientSocket = p_zmqContext.CreateSocket(SocketType.REQ);
            clientSocket.Connect(SERVER_TCP_ENDPOINT_A);
            TestUtils.DebugLog("{0} ready", p_clientName);

            Task.Factory.StartNew(() =>
            {
                Task.Factory.StartNew(() =>
                {
                    while (Interlocked.Read(ref m_continueFlag) == 1)
                    {
                        Task.Delay(p_interval).Wait();
                        clientSocket.Send(string.Format("{0}: Hello #{1}", p_clientName, Interlocked.Increment(ref m_counter)), Encoding.UTF8);
                        string reply = clientSocket.Receive(Encoding.UTF8);
                        TestUtils.DebugLog("Client received reply: [{0}]", reply);
                    }
                    clientSocket.Dispose();
                });
            });
        }

        #endregion

        #region Fields

        private long m_continueFlag = 1;

        private long m_counter;

        #endregion

        #region Constants

        public const string SERVER_TCP_ENDPOINT_A = "tcp://127.0.0.1:5000";
        public const string SERVER_IPC_ENDPOINT_B = "ipc://weather.ipc";
        public const string SERVER_IPC_ENDPOINT_C = "inproc://somename";

        #endregion
    }
}