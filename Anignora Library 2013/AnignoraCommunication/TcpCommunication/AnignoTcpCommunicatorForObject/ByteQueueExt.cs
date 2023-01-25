using System.Collections.Generic;

namespace AnignoraCommunication.TcpCommunication.AnignoTcpCommunicatorForObject
{
    public class ByteQueueExt : Queue<byte>
    {
		#region (------------  Public Methods  ------------)
        public byte[] Dequeue(long length)
        {
            byte[] bytes = new byte[length];
            for (int a = 0; a < length; a++)
            {
                bytes[a] = Dequeue();
            }
            return bytes;
        }

        public void Enqueue(byte[] bytes,long length)
        {
            for (int a = 0; a < length; a++)
            {
                Enqueue(bytes[a]);
            }
        }
		#endregion (------------  Public Methods  ------------)
    }
}
