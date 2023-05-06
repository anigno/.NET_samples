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
    public class PubSub
    {
        #region Public Methods

        [Test]
        public void TestTwoPubsTwoSubs()
        {
            ZmqContext zmqContext = ZmqContext.Create();

            StartPubServer("Pub Server A", zmqContext, SERVER_TCP_ENDPOINT_A);
            StartPubServer("Pub Server B", zmqContext, SERVER_TCP_ENDPOINT_B);

            StartSubClient("ClientA (All)", zmqContext);
            StartSubClient("ClientB (TypeA only)", zmqContext, "TypeA");
            StartSubClient("ClientC (TypeA and TypeB)", zmqContext, "TypeA", "TypeB");

            const int MILLISECONDS_DELAY = 2000;
            Task.Delay(MILLISECONDS_DELAY).Wait();
            Interlocked.Exchange(ref m_continueFlag, 0);
        }

        #endregion

        #region Private Methods

        private void StartPubServer(string p_serverName, ZmqContext p_zmqContext, string p_endPoint)
        {
            ZmqSocket serverSocket = p_zmqContext.CreateSocket(SocketType.PUB);
            serverSocket.Bind(p_endPoint);
            TestUtils.DebugLog("{0} -ready", p_serverName);

            Task.Factory.StartNew(() =>
            {
                int a = 0;
                while (Interlocked.Read(ref m_continueFlag) == 1)
                {
                    Task.Delay(100).Wait();
                    string message = "TypeNone message";
                    if (a%2 == 0) message = "TypeA message";
                    if (a%3 == 0) message = "TypeB message";
                    serverSocket.Send(message, Encoding.UTF8);
                    TestUtils.DebugLog("* {0} sent message: [{1}]", p_serverName, message);
                    a++;
                }
                serverSocket.Dispose();
                p_zmqContext.Dispose();
            });
        }

        private void StartSubClient(string p_clientName, ZmqContext p_zmqContext, params string[] p_subscriptionPrefixs)
        {
            var clientSocket = p_zmqContext.CreateSocket(SocketType.SUB);
            clientSocket.Connect(SERVER_TCP_ENDPOINT_A);
            clientSocket.Connect(SERVER_TCP_ENDPOINT_B);
            if (p_subscriptionPrefixs.Length == 0)
            {
                clientSocket.SubscribeAll();
            }
            foreach (string subscriptionPrefix in p_subscriptionPrefixs)
            {
                clientSocket.Subscribe(Encoding.UTF8.GetBytes(subscriptionPrefix));
            }
            TestUtils.DebugLog("{0} -ready", p_clientName);

            Task.Factory.StartNew(() =>
            {
                Task.Factory.StartNew(() =>
                {
                    while (Interlocked.Read(ref m_continueFlag) == 1)
                    {
                        string message = clientSocket.Receive(Encoding.UTF8);
                        TestUtils.DebugLog("- {0} received: [{1}]", p_clientName, message);
                    }
                    clientSocket.Dispose();
                });
            });
        }

        #endregion

        #region Fields

        private long m_continueFlag = 1;

        #endregion

        #region Constants

        public const string SERVER_TCP_ENDPOINT_A = "tcp://127.0.0.1:5000";
        public const string SERVER_TCP_ENDPOINT_B = "tcp://127.0.0.1:5001";

        #endregion
    }
}