using System.Net;

namespace AnignoraCommunication.Email
{
    /// <summary>
    /// Specific SmtpClientEx to use when sending by Gmail account
    /// </summary>
    public class SmtpClientByGmail : SmtpClientEx
    {
        public SmtpClientByGmail(string fromEmail, string fromGmailPassword, int timeoutSec)
            : base("smtp.gmail.com", 587, timeoutSec, fromEmail)
        {
            NetworkCredential nc = new NetworkCredential(fromEmail, fromGmailPassword);
            EnableSsl = true;
            UseDefaultCredentials = false;
            Credentials = nc;
        }
    }
}