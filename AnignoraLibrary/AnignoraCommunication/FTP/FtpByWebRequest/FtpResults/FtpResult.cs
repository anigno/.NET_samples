using System;

namespace AnignoraCommunication.FTP.FtpByWebRequest.FtpResults
{
    public class FtpResult
    {
        public bool IsSuccessfull { get; private set; }
        public Exception Exception { get; private set; }

        public FtpResult(bool p_isSuccessfull,Exception p_exception=null)
        {
            IsSuccessfull = p_isSuccessfull;
            Exception = p_exception;
        }

        public override string ToString()
        {
            return string.Format("IsSuccessfull={0} Exception=[{1}]", IsSuccessfull, Exception);
        }
    }
}
