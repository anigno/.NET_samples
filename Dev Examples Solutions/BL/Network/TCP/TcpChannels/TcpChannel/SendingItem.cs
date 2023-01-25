namespace TcpChannel
{
    public class SendingItem
    {
        #region Constructors

        public SendingItem(byte[] lengthBytesBytes, byte[] bytes)
        {
            LengthBytes = lengthBytesBytes;
            DataBytes = bytes;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Format("[{0}]", DataBytes.Length);
        }

        #endregion

        #region Fields

        public readonly byte[] LengthBytes;
        public readonly byte[] DataBytes;

        #endregion
    }
}