using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace AnignoraCommunication.TcpCommunication.AnignoTcpCommunicatorForObject
{
    /// <summary>
    /// Tcp listener for AnignoTcpCommunicator
    /// </summary>
    public class AnignoTcpListener
    {
		#region (------  Fields  ------)

        private Thread _acceptThread;
        private readonly TcpListener _listener;

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public AnignoTcpListener(string ipString, int port)
        {
            IPAddress ipa = IPAddress.Parse(ipString);
            _listener = new TcpListener(ipa, port);
        }

		#endregion (------  Constructors  ------)

		#region (------  Delegates  ------)

        public delegate void ConnectionAcceptedHandler(AnignoTcpCommunicator communicator);

		#endregion (------  Delegates  ------)

		#region (------  Events  ------)

        public event ConnectionAcceptedHandler ConnectionAccepted;

		#endregion (------  Events  ------)

		#region (------  Public Methods  ------)

        public void Start()
        {
            _acceptThread = new Thread(AcceptThreadStart);
            _acceptThread.Start();
        }

        public void Stop()
        {
            _listener.Stop();
            _acceptThread.Abort();
        }

		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)

        private void AcceptThreadStart()
        {
            _listener.Start();
            while (true)
            {
                TcpClient tcpClient = _listener.AcceptTcpClient();
                AnignoTcpCommunicator communicator = new AnignoTcpCommunicator(tcpClient);
                if (ConnectionAccepted != null) ConnectionAccepted(communicator);
            }
        }

		#endregion (------  Private Methods  ------)
    }
}