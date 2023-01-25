using System;

namespace AnignoraCommunication.TcpCommunication.AnignoTcpClientServer
{
    public class AnignoTcpClientServerException : Exception
    {
        private string _additionalData;

        public AnignoTcpClientServerException(Exception ex, string format, params object[] obj) :base(ex.Message,ex.InnerException)
        {
            _additionalData = string.Format(format, obj);
        }

        public string AdditionalData
        {
            get { return _additionalData; }
        }
    }
}
