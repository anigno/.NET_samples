using System;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AnignoraCommunication.UdpCommunication.UdpAckCommunication
{
    [Serializable]
    public class UdpAckQueuedMessage
    {
        [NonSerialized]
        private static BinaryFormatter formatter = new BinaryFormatter();
        [NonSerialized]
        private static int _uniqueId = 1000;
        [NonSerialized]
        private static object _uniqueIdSync = new object();
        private byte[] _buffer;
        private int _messageId;
        private IPEndPoint _remoteIPE;
        private bool _isAck;
        private int _ackForMessageId;

        private int UniqueId
        {
            get
            {
                int retId;
                lock (_uniqueIdSync)
                {
                    retId = _uniqueId++;
                }
                return retId;
            }
        }

        public byte[] Buffer
        {
            get { return _buffer; }
        }

        public int MessageId
        {
            get { return _messageId; }
        }

        public IPEndPoint RemoteIPE
        {
            get { return _remoteIPE; }
        }

        public bool IsAck
        {
            get { return _isAck; }
        }

        public int AckForMessageId
        {
            get { return _ackForMessageId; }
        }

        /// <summary>
        /// CTOR, for message without ack
        /// </summary>
        public UdpAckQueuedMessage(byte[] buffer, IPEndPoint remoteIPE)
        {
            _messageId = UniqueId;
            _isAck = false;
            _buffer = buffer;
            _remoteIPE = remoteIPE;
            _ackForMessageId = -1;
        }

        /// <summary>
        /// CTOR, for Ack message
        /// </summary>
        public UdpAckQueuedMessage(IPEndPoint remoteIPE,int ackForMessageId)
        {
            _messageId = UniqueId;
            _isAck = true;
            _buffer = null;
            _remoteIPE = remoteIPE;
            _ackForMessageId = ackForMessageId;
        }

        /// <summary>
        /// CTOR, convert byte[] to UdpAckMessage, no new messageId is created
        /// </summary>
        /// <param name="byteArray"></param>
        public UdpAckQueuedMessage(byte[] byteArray)
        {
            MemoryStream memStream = new MemoryStream(byteArray);
            //memStream.Write(byteArray, 0, byteArray.Length);
            object obj = formatter.Deserialize(memStream);
            UdpAckQueuedMessage message = (UdpAckQueuedMessage)obj;
            this._buffer = message._buffer;
            this._isAck = message._isAck;
            this._ackForMessageId = message._ackForMessageId;
            this._messageId = message._messageId;
            this._remoteIPE = message._remoteIPE;
            memStream.Close();
        }

        public byte[] GetByteArray()
        {
            MemoryStream memStream = new MemoryStream();
            formatter.Serialize(memStream, this);
            byte[] retByteArray = memStream.ToArray();
            memStream.Close();
            return retByteArray;
        }

        ///// <summary>
        ///// Build a byte array contains messageId, isAck and message data
        ///// </summary>
        ///// <returns></returns>
        //private byte[] BuildSendHeader(int messageId, byte[] buffer,bool isAck)
        //{
        //    byte[] messageIdBytes = Encoding.ASCII.GetBytes(messageId.ToString() + "#");
        //    byte[] isAckBytes = Encoding.ASCII.GetBytes(isAck.ToString() + "#");
        //    MemoryStream memStream = new MemoryStream();
        //    memStream.Write(messageIdBytes, 0, messageIdBytes.Length);
        //    memStream.Write(isAckBytes, 0, isAckBytes.Length);
        //    memStream.Write(buffer, 0, buffer.Length);
        //    return memStream.ToArray();
        //}

        ///// <summary>
        ///// Strip given receivedBuffer to it's parts, message format: MESSAGE_ID#IS_ACK#MESSAGE_DATA
        ///// </summary>
        //public static void StripReceivedHeader(byte[] receivedBuffer, out int messageId, out byte[] data, out bool isAck)
        //{
        //    int messageIdBytesEnd = -1;
        //    int isAckEnd = -1;
        //    for (int a=0; a < receivedBuffer.Length; a++)
        //    {
        //        if (receivedBuffer[a] == '#')
        //        {
        //            if (messageIdBytesEnd < 0)
        //            {
        //                messageIdBytesEnd = a;
        //            }
        //            else
        //            {
        //                isAckEnd = a;
        //                break;
        //            }
        //        }
        //    }
        //    byte[] messageIdBytes = new byte[messageIdBytesEnd];
        //    byte[]
        //    data = new byte[receivedBuffer.Length - messageIdBytesEnd];
        //    for (int a = 0; a < messageIdBytesEnd; a++)
        //    {
        //        messageIdBytes[a] = receivedBuffer[a];
        //    }
        //    for (int a = messageIdBytesEnd; a < receivedBuffer.Length; a++)
        //    {
        //        data[a - messageIdBytesEnd] = receivedBuffer[a];
        //    }
        //    string messageIdString = Encoding.ASCII.GetString(messageIdBytes);
        //    messageId = int.Parse(messageIdString);
        //    isAck = false;
        //}

    }
}
