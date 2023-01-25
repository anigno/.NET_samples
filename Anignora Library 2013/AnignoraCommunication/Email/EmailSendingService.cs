using System;
using System.Collections.Generic;
using System.Threading;
using System.Net.Mail;

namespace AnignoraCommunication.Email
{
    public class EmailSendingService
    {
		#region (------  Fields  ------)

        private readonly List<MailMessageEx> EmailMessagesExList=new List<MailMessageEx>();
        private long isContinue ;
        private Thread SenderThread;

		#endregion (------  Fields  ------)

		#region (------  Delegates  ------)

        public delegate void OnSendErrorDelegate(MailMessageEx messageEx, Exception exeption);
        public delegate void OnSendSucceededDelegate(MailMessageEx messageEx);

		#endregion (------  Delegates  ------)

		#region (------  Events  ------)

        public event OnSendErrorDelegate OnSendError;

        public event OnSendSucceededDelegate OnSendSucceeded;

		#endregion (------  Events  ------)

		#region (------  Public Methods  ------)

        public int SendEmailASync(SmtpClientEx smtpClientEx, string fromName, string toName, string toEmail, string subject, string body, params string[] attachedFiles)
        {
            MailMessageEx message = PrepareMailMessage(smtpClientEx,fromName, smtpClientEx.FromEmail, toName, toEmail, subject, body, attachedFiles);
            lock (EmailMessagesExList)
            {
                EmailMessagesExList.Add(message);
                return message.MessageId;
            }
        }

        public int SendEmailSync(SmtpClientEx smtpClientEx, string fromName, string toName, string toEmail, string subject, string body, params string[] attachedFiles)
        {
            MailMessageEx message = PrepareMailMessage(smtpClientEx, fromName, smtpClientEx.FromEmail, toName, toEmail, subject, body, attachedFiles);
            try
            {
                smtpClientEx.Send(message);
                RaiseOnSendSucceeded(message);
            }
            catch(Exception ex)
            {
                RaiseOnSendError(message,ex);
            }
            return message.MessageId;
        }

        public void Start()
        {
            Interlocked.Exchange(ref isContinue, 1);
            SenderThread = new Thread(SenderThreadStart);
            SenderThread.Start();
        }

        public void Stop()
        {
            Interlocked.Exchange(ref isContinue, 0);
        }

		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)

        private static MailMessageEx PrepareMailMessage(SmtpClientEx smtpClientEx, string fromName, string fromEmail, string toName, string toEmail, string subject, string body, params string[] attachedFiles)
        {
            MailAddress fromAddress = new MailAddress(fromEmail, fromName);
            MailAddress toAddress = new MailAddress(toEmail, toName);
            MailMessageEx message = new MailMessageEx(fromAddress, toAddress);
            message.MessageSmtpClientEx = smtpClientEx;
            message.Subject = subject;
            message.Body = body;
            foreach (string filename in attachedFiles)
            {
                message.Attachments.Add(new Attachment(filename));
            }
            return message;
        }

        private void RaiseOnSendError(MailMessageEx message,Exception ex)
        {
            if (OnSendError != null)
            {
                OnSendError(message,ex);
            }
            //else
            //{
            //    throw ex;
            //}
        }

        private void RaiseOnSendSucceeded(MailMessageEx message)
        {
            if (OnSendSucceeded != null) OnSendSucceeded(message);
        }

        private void SenderThreadStart()
        {
            MailMessageEx message;
            while (Interlocked.Read(ref isContinue)==1)
            {
                lock (EmailMessagesExList)
                {
                    if (EmailMessagesExList.Count > 0)
                    {
                        message = EmailMessagesExList[0];
                        EmailMessagesExList.RemoveAt(0);
                    }
                    else
                    {
                        message = null;
                    }
                }
                try
                {
                    if (message != null)
                    {
                        message.MessageSmtpClientEx.Send(message);
                        RaiseOnSendSucceeded(message);
                    }
                }
                catch (Exception ex)
                {
                    RaiseOnSendError(message, ex);
                }
                Thread.Sleep(500);
            }
        }

		#endregion (------  Private Methods  ------)
    }
}
