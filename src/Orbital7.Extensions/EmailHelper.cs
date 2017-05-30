using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Orbital7.Extensions
{
    public static class EmailHelper
    {
        public static void SendEmailViaSendGrid(string fromAddress, string fromName, string toAddresses, string subject, string textBody, string htmlBody, bool sendAsync, 
            string sendGridUsername, string sendGridPassword)
        {
            // Source: https://sendgrid.com/docs/Code_Examples/csharp.html

            // Create the message.
            MailMessage mailMsg = new MailMessage();

            // To.
            foreach (string toAddress in toAddresses.Parse(";"))
                mailMsg.To.Add(new MailAddress(toAddress));

            // From.
            mailMsg.From = new MailAddress(fromAddress, fromName);

            // Subject and multipart/alternative Body
            mailMsg.Subject = subject;
            if (!String.IsNullOrEmpty(textBody)) mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(textBody, null, MediaTypeNames.Text.Plain));
            if (!String.IsNullOrEmpty(htmlBody)) mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html));

            // Initialize SmtpClient.
            SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(sendGridUsername, sendGridPassword);
            smtpClient.Credentials = credentials;

            // Send.
            if (!sendAsync)
                smtpClient.Send(mailMsg);
            else
                smtpClient.SendAsync(mailMsg, null);
        }

        public static void SendEmailViaSMTP(string toAddresses, string fromAddress, string replyToAddress, string subject, string messageBody,
            SmtpClient server, bool sendAsync, string ccAddresses = null)
        {
            // Create the message.
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromAddress);
            if (!String.IsNullOrEmpty(replyToAddress))
                mail.ReplyToList.Add(new MailAddress(replyToAddress));
            if (!String.IsNullOrEmpty(ccAddresses))
            {
                foreach (string emailAddress in ccAddresses.Parse(";"))
                    mail.CC.Add(new MailAddress(emailAddress));
            }
            mail.To.Add(toAddresses);
            mail.Subject = subject;
            mail.Body = messageBody;

            // Send.
            if (!sendAsync)
                server.Send(mail);
            else
                server.SendAsync(mail, null);
        }
    }
}
