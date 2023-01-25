using System;

namespace AnignoraCommunication.FTP.FtpByWebRequest.FtpResults
{
    public class FtpResultStringArray : FtpResult
    {
        public string[] Array { get; private set; }

        public FtpResultStringArray(bool p_isSuccessfull, string[] p_array, Exception p_exception = null)
            : base(p_isSuccessfull, p_exception)
        {
            Array = p_array;
        }
    }
}