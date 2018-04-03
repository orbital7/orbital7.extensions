using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.Email
{
    public class SmtpEmailService : EmailServiceBase
    {
        protected SmtpClient SmtpClient { get; set; }

        public SmtpEmailService(SmtpClient smtpClient)
        {
            this.SmtpClient = smtpClient;
        }

        protected override async Task SendAsync(MailMessage message, bool sendAsync)
        {
            if (!sendAsync)
                await this.SmtpClient.SendMailAsync(message);
            else
                this.SmtpClient.SendAsync(message, null);
        }
    }
}
