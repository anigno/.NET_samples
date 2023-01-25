using System.Net.Mail;
using System.Threading;

namespace AnignoraCommunication.Email
{
    /// <summary>
    /// Expends SmtpClient, add unique SmtpClientId
    /// </summary>
    public class SmtpClientEx : SmtpClient
    {
        private static int SmtpClientUniqueId;
        public readonly int SmtpClientId = Interlocked.Increment(ref SmtpClientUniqueId);
        public readonly string FromEmail;
        public readonly int TimeOutSec;

        public SmtpClientEx(string host, int port, int timeoutSec, string fromEmail)
            : base(host, port)
        {
            FromEmail = fromEmail;
            TimeOutSec = Timeout = timeoutSec * 1000;
        }

    }
}