using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialFileCommon
{
    public class SerialFileTransferPort
    {
		#region (--- Constructors (1) ---)

        public SerialFileTransferPort(uint p_portNumber)
        {
            m_port = new SerialPort("COM" + p_portNumber, 115200, Parity.Odd, 8);
            m_port.Handshake = Handshake.None;
            m_port.DataReceived += onDataReceived;
            m_port.Open();
        }

		#endregion (--- Constructors ---)

		#region (--- Public Methods (2) ---)

        public void RegisterReceivedData(Action<byte[],int,int>  p_receivedBytes )
        {
            m_receivedBytesCallback = p_receivedBytes;
        }

        public void SendData(byte[] p_buffer)
        {
            m_port.Write(p_buffer, 0, p_buffer.Length);
        }

		#endregion (--- Public Methods ---)

		#region (--- Callbacks (1) ---)

        void onDataReceived(object p_sender, SerialDataReceivedEventArgs p_e)
        {
            byte[] bytes = new byte[m_port.BytesToRead];
            int nBytes = m_port.Read(bytes, 0, bytes.Length);
            m_memoryStream.Write(bytes, 0, bytes.Length);
            m_totalBytes += nBytes;
            m_nReads++;
            if (m_receivedBytesCallback != null) m_receivedBytesCallback(bytes,m_nReads,m_totalBytes);
        }

		#endregion (--- Callbacks ---)

		#region (--- Fields (5) ---)

        private readonly MemoryStream m_memoryStream = new MemoryStream();
        private int m_nReads;
        private readonly SerialPort m_port;
        private Action<byte[],int,int> m_receivedBytesCallback;
        private int m_totalBytes;

		#endregion (--- Fields ---)
    }
}
