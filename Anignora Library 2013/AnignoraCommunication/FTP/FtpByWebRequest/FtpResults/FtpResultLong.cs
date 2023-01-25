using System;

namespace AnignoraCommunication.FTP.FtpByWebRequest.FtpResults
{
    public class FtpResultLong : FtpResult
    {
        public long Value { get; private set; }

        public FtpResultLong(bool p_isSuccessfull, long p_value, Exception p_exception = null)
            : base(p_isSuccessfull, p_exception)
        {
            Value = p_value;
        }
    }
}