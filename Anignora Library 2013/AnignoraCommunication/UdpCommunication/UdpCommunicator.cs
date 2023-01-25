using System;
using System.Net;
using System.Net.Sockets;
using AnignoraCommunication.UdpCommunication.CommunicationBase;

namespace AnignoraCommunication.UdpCommunication
{
    public class UdpCommunicator : CommunicatorBase
    {
        #region Fields
        public static readonly string CHECK_BYTES_FAILED_ERROR_MESSAGE = "CheckBytesFailed";
        private UdpClient _client;
        #endregion

        #region CTOR
        public UdpCommunicator(IPEndPoint localIPE)
            : base(localIPE)
        {
            _client = new UdpClient(localIPE);
        }
        #endregion

        #region Public
        /// <summary>
        /// Creates a new CommunicationMessage and add it to send queue, returns the newly created message id.
        /// </summary>
        /// <returns>The newly created message id</returns>
        public override void Send(byte[] buffer, IPEndPoint remoteIPE)
        {
            byte[] bufferWithCheckBytes = GetBytesWithCheckBytes(buffer);
            CommunicationMessage message = new CommunicationMessage(LocalIPE, remoteIPE, bufferWithCheckBytes);
            CommunicationBaseSendMessage(message);
            //return message.MessageId;
        }
        #endregion

        #region CommunicatorBase implementation
        public override void SendCommunicationMessageCallBack(CommunicationMessage message)
        {
            _client.Send(message.Buffer, message.Buffer.Length, message.RemoteIPE);
        }

        public override void StartReceiving()
        {
            _client.BeginReceive(BeginReceiveCallBack, _client);
        }

        public override void StopReceiving()
        {
            throw new NotImplementedException();
        }

        public override void CommunicationMessageSentCallBack(CommunicationMessage message)
        {
            RemoveMessageFromQueue(message);
        }
        #endregion

        #region Private call backs
        private void BeginReceiveCallBack(IAsyncResult asyncRes)
        {
            IPEndPoint remoteIPE = null;
            byte[] receivedBuffer = _client.EndReceive(asyncRes, ref remoteIPE);
            byte[] dataReceived;
            if (IsCheckBytesCorrect(receivedBuffer, out dataReceived))
            {
                RaiseOnBytesReceived(remoteIPE, dataReceived);
            }
            else
            {
                RaiseOnBytesReceivedError(CHECK_BYTES_FAILED_ERROR_MESSAGE, "Data received was corrupted");
            }
            StartReceiving();
        }
        #endregion

        #region Private
        /// <summary>
        /// Returns a byte array contain both the data and the checkBytes used for verification
        /// </summary>
        /// <param name="buffer">Data bytes to be sent</param>
        /// <returns>Data bytes and check bytes to be sent and verified</returns>
        private byte[] GetBytesWithCheckBytes(byte[] buffer)
        {
            byte[] retBytes = new byte[buffer.Length * 2];
            for (int a = 0; a < buffer.Length; a++)
            {
                retBytes[a] = buffer[a];
                retBytes[a + buffer.Length] = (byte)(255 - buffer[a]);
            }
            return retBytes;
        }

        /// <summary>
        /// Checks that the byte data received is correct using XOR with the check bytes received with the message
        /// </summary>
        /// <param name="checkedBuffer">The received buffer with check bytes</param>
        /// <param name="dataBuffer">Returned byte array with the only the data</param>
        /// <returns>True is verified, else false</returns>
        private bool IsCheckBytesCorrect(byte[] checkedBuffer, out byte[] dataBuffer)
        {
            int b = checkedBuffer.Length / 2;
            dataBuffer = new byte[b];
            for (int a = 0; a < b; a++)
            {
                dataBuffer[a] = checkedBuffer[a];
                int checkByte = checkedBuffer[a] ^ checkedBuffer[a + b];
                if (checkByte != 255) return false;
            }
            return true;
        }
        #endregion

    }
}
