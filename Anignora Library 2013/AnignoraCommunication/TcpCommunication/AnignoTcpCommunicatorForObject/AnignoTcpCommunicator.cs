using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System;

namespace AnignoraCommunication.TcpCommunication.AnignoTcpCommunicatorForObject
{
    /// <summary>
    /// TCP client using objects
    /// </summary>
    public class AnignoTcpCommunicator
    {
		#region (------------  Const Fields  ------------)
        public const int DefaultBufferSize = 8192;
        public string LastError { get; set; }
		#endregion (------------  Const Fields  ------------)


		#region (------------  Fields  ------------)
        readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();
        private byte[] _receivedBytes;
        readonly ByteQueueExt _receiveQueue = new ByteQueueExt();
        private long _nextObjectSize = -1;
        private readonly TcpClient _client;
        private Thread _receiveThread;
		#endregion (------------  Fields  ------------)


		#region (------------  Constructors  ------------)
        public AnignoTcpCommunicator(string ipString, int port)
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ipString), port);
            _client = new TcpClient(ipe);
        }

        internal AnignoTcpCommunicator(TcpClient worker)
        {
            _client = worker;
            StartReceive();
        }
		#endregion (------------  Constructors  ------------)


		#region (------------  Events  ------------)
        public event ObjectReceivedHandler ObjectReceived;
		#endregion (------------  Events  ------------)


		#region (------------  Delegates  ------------)
        public delegate void ObjectReceivedHandler(object obj);
		#endregion (------------  Delegates  ------------)


		#region (------------  Private Methods  ------------)
        private void ReceiveThreadStart()
        {

            while (true)
            {
                int i = _client.Client.Receive(_receivedBytes);
                if (i > 0)
                {
                    _receiveQueue.Enqueue(_receivedBytes, i);
                    if (_nextObjectSize == -1)
                    {
                        byte[] bSize = _receiveQueue.Dequeue(8);
                        _nextObjectSize = BitConverter.ToInt64(bSize, 0);
                    }
                    if (_receiveQueue.Count >= _nextObjectSize)
                    {
                        byte[] bObj = _receiveQueue.Dequeue(_nextObjectSize);
                        MemoryStream receivedStream = new MemoryStream(bObj);
                        object obj = _binaryFormatter.Deserialize(receivedStream);
                        if (ObjectReceived != null) ObjectReceived(obj);
                        _nextObjectSize = -1;
                    }
                }
            }
        }

        private bool Send(byte[] bytes)
        {
            try
            {
                int i=_client.Client.Send(bytes);
                return true;
            }
            catch(Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
        }

        private void StartReceive()
        {
            _client.SendBufferSize = _client.ReceiveBufferSize = DefaultBufferSize;
            _receivedBytes = new byte[_client.ReceiveBufferSize];
            _receiveThread = new Thread(ReceiveThreadStart);
            _receiveThread.Start();
        }
		#endregion (------------  Private Methods  ------------)


		#region (------------  Public Methods  ------------)
        public bool Connect(string ipString, int port)
        {
            try
            {
                IPAddress ipa = IPAddress.Parse(ipString);
                _client.Connect(ipa, port);
                StartReceive();
                return true;
            }
            catch(Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
        }

        public bool Send(object obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            MemoryStream memoryStreamToSend = new MemoryStream();
            _binaryFormatter.Serialize(memoryStream, obj);
            long length = memoryStream.Length;
            byte[] bLength = BitConverter.GetBytes(length);
            memoryStreamToSend.Write(bLength, 0, bLength.Length);
            memoryStreamToSend.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
            return Send(memoryStreamToSend.ToArray());
        }

        public void Stop()
        {
            _client.Client.Disconnect(false);
            _receiveThread.Abort();
        }
		#endregion (------------  Public Methods  ------------)
    }
}