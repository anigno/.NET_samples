namespace AnignoraCommunication.TcpCommunication.AnignoTcpClientServer
{
    public delegate void OnConnectionAcceptedDelegate(AnignoTcpClient serverSideClient);

    public delegate void OnBytesReceivedDelegate(byte[] bytes);

    public class AnignoTcpClientServerCommon
    {

		#region Const Fields (1) 

        public const int SEND_RECEIVE_BUFFER_SIZE = 10;

		#endregion Const Fields 

    }
}