using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Orbital7.Extensions.Email
{
    public class SmtpEmailEngine : EmailEngine
    {
        protected SmtpClient SmtpClient { get; set; }

        public SmtpEmailEngine(SmtpClient smtpClient)
        {
            this.SmtpClient = smtpClient;
        }

        protected override void Send(MailMessage message, bool sendAsync)
        {
            if (!sendAsync)
                this.SmtpClient.Send(message);
            else
                this.SmtpClient.SendAsync(message, null);
        }
    }
}
