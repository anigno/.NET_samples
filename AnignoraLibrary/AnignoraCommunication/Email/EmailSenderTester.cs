using System;
using System.Threading;

namespace AnignoraCommunication.Email
{
    public class EmailSenderTester
    {
        EmailSendingService es=new EmailSendingService();

        public EmailSenderTester()
        {
            es.OnSendError += es_OnSendError;
            es.Start();
            SmtpClientByGmail c=new SmtpClientByGmail(@"anignorafinanceanalyzer@gmail.com",@"anignora",10);
            es.SendEmailSync(c, "from", "to", @"anigno@gmail.com", "subject", "body");


            es.SendEmailASync(c, "from", "to", @"anigno@gmail.com", "subject1", "body");
            es.SendEmailASync(c, "from", "to", @"anigno@gmail.com", "subject2", "body");
            es.SendEmailASync(c, "from", "to", @"anigno@gmail.com", "subject3", "body");
            es.SendEmailASync(c, "from", "to", @"anigno@gmail.com", "subject4", "body");

            Thread.Sleep(4000);
            es.Stop();
        }

        

        void es_OnSendError(MailMessageEx messageEx, Exception exeption)
        {
            
        }

        
    }
}
