using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace AnignoraCommunication.TcpCommunication.AnignoTcpClientServer
{
    /// <summary>
    /// Listen to connection requests and produce an AnignoTcpClient worker for each connection request
    /// using non blocking methods
    /// </summary>
    public class AnignoTcpServer
    {
        public event OnConnectionAcceptedDelegate OnConnectionAccepted;

        TcpListener _listener;
        AsyncCallback _beginAcceptAsyncCallback;
        private List<AnignoTcpClient> _workerClients = new List<AnignoTcpClient>();



        public List<AnignoTcpClient> WorkerClients
        {
            get { return _workerClients; }
            set { _workerClients = value; }
        }

        public AnignoTcpServer(IPEndPoint ipe)
        {
            _listener = new TcpListener(ipe);
            _beginAcceptAsyncCallback = BeginAcceptAsyncCallBack;
        }

        public void StartListening()
        {
            _listener.Start();
            _listener.BeginAcceptTcpClient(_beginAcceptAsyncCallback, _listener);
        }

        public void StopListening()
        {
            _listener.Stop();
        }

        private void BeginAcceptAsyncCallBack(IAsyncResult asyncRes)
        {
            TcpClient createdTcpClient = _listener.EndAcceptTcpClient(asyncRes);
            AnignoTcpClient serverSideClient = new AnignoTcpClient("ServerSideClient", createdTcpClient);
            WorkerClients.Add(serverSideClient);
            if (OnConnectionAccepted != null) OnConnectionAccepted(serverSideClient);
            _listener.BeginAcceptTcpClient(_beginAcceptAsyncCallback, _listener);
        }


    }
}