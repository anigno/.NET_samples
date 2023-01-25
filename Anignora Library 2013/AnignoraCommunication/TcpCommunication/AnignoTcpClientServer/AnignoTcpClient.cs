using System;
using System.Net.Sockets;
using System.Net;
using AnignoraDataTypes;

namespace AnignoraCommunication.TcpCommunication.AnignoTcpClientServer
{
    /// <summary>
    /// Request connection from AnignoTcpServer, Sends and receive variable size byte arrays
    /// using non blocking methods
    /// </summary>
    public class AnignoTcpClient
    {
        private NetworkStream mNetStream;
        private int mReadRemain = 0;
        private byte[] mReceiveBuffer = new byte[AnignoTcpClientServerCommon.SEND_RECEIVE_BUFFER_SIZE];
        private QueueCircularDynamic<byte> mReceiveQueue = new QueueCircularDynamic<byte>(1024);
        private TcpClient mTcpClient;
        private string mName;

        public AnignoTcpClient(string name,IPEndPoint ipe)
        {
            mTcpClient = new TcpClient(ipe);
            mName = name;
        }

        internal AnignoTcpClient(string name,TcpClient serverSideClient)
        {
            mTcpClient = serverSideClient;
            mNetStream = mTcpClient.GetStream();
            mName = name;
            BeginRead();
        }

        public event OnBytesReceivedDelegate OnBytesReceived;

        public string Name
        {
            get { return mName; }
        }

        public void Connect(IPEndPoint remoteIpe)
        {
                mTcpClient.Connect(remoteIpe);
                mNetStream = mTcpClient.GetStream();
                BeginRead();
        }

        public void Send(byte[] bytes)
        {
            byte[] dataSize = BitConverter.GetBytes(bytes.Length);
            mNetStream.Write(dataSize, 0, 4);
            mNetStream.Write(bytes, 0, bytes.Length);
        }

        private void BeginRead()
        {
            mNetStream.BeginRead(mReceiveBuffer, 0, AnignoTcpClientServerCommon.SEND_RECEIVE_BUFFER_SIZE, BeginReceiveAsyncCallBack, null);
        }

        private void BeginReceiveAsyncCallBack(IAsyncResult asyncRes)
        {
            int bytesRead = mNetStream.EndRead(asyncRes);
            mReceiveQueue.Enqueue(mReceiveBuffer, 0, bytesRead);
            if (mReadRemain == 0)
            {
                //Read length
                byte[] sizeBuffer = mReceiveQueue.Dequeue(4);
                mReadRemain = BitConverter.ToInt32(sizeBuffer, 0);
            }
            else
            {
                if (mReceiveQueue.Avaliable >= mReadRemain)
                {
                    byte[] data = mReceiveQueue.Dequeue(mReadRemain);
                    if (OnBytesReceived != null) OnBytesReceived(data);
                }
            }
            BeginRead();
        }

    }
}