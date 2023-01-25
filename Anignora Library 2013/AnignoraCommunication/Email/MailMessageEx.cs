using System.Net.Mail;
using System.Threading;

namespace AnignoraCommunication.Email
{
    /// <summary>
    /// Expends MainMessage, add unique MessageId and holds the smtp client
    /// </summary>
    public class MailMessageEx : MailMessage
    {
        private static int MessageUniqueId;
        public readonly int MessageId;
        public SmtpClientEx MessageSmtpClientEx;

        public MailMessageEx(MailAddress addressFrom, MailAddress addressTo)
            : base(addressFrom, addressTo)
        {
            MessageId = Interlocked.Increment(ref MessageUniqueId);
        }
    }
}