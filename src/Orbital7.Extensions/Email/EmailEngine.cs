using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Orbital7.Extensions.Email
{
    public abstract class EmailEngine
    {
        public void SendTextEmail(string fromAddress, string fromName, string toAddresses, string subject,
            string textBody, bool sendAsync, IDictionary<string, string> textReplacementKeys = null, 
            IList<string> attachmentFilePaths = null, IDictionary<string, byte[]> attachmentFileContents = null)
        {
            Send(CreateMessage(fromAddress, fromName, toAddresses, subject)
                 .SetTextBody(textBody, textReplacementKeys)
                 .AttachFiles(attachmentFilePaths)
                 .AttachFiles(attachmentFileContents),
            sendAsync);
        }

        public void SendHtmlEmail(string fromAddress, string fromName, string toAddresses, string subject,
            string htmlBody, bool sendAsync, IDictionary<string, string> textReplacementKeys = null, 
            IDictionary<string, byte[]> imageContentReplacementKeys = null, string imageContentType = "image/png", 
            IList<string> attachmentFilePaths = null, IDictionary<string, byte[]> attachmentFileContents = null)
        {
            Send(CreateMessage(fromAddress, fromName, toAddresses, subject)
                 .SetHtmlBody(htmlBody, textReplacementKeys, imageContentReplacementKeys, imageContentType)
                 .AttachFiles(attachmentFilePaths)
                 .AttachFiles(attachmentFileContents),
            sendAsync);
        }

        public void SendHtmlEmail(string fromAddress, string fromName, string toAddresses, string subject,
            string htmlBody, bool sendAsync, IDictionary<string, string> textReplacementKeys = null, 
            IDictionary<string, string> imageFilePathReplacementKeys = null, string imageContentType = "image/png", 
            IList<string> attachmentFilePaths = null, IDictionary<string, byte[]> attachmentFileContents = null)
        {
            Send(CreateMessage(fromAddress, fromName, toAddresses, subject)
                 .SetHtmlBody(htmlBody, textReplacementKeys, imageFilePathReplacementKeys, imageContentType)
                 .AttachFiles(attachmentFilePaths)
                 .AttachFiles(attachmentFileContents),
            sendAsync);
        }

        protected abstract void Send(MailMessage message, bool sendAsync);

        public static MailMessage CreateMessage(string fromAddress, string fromName, string toAddresses, string subject)
        {
            // Create the message.
            var mailMsg = new MailMessage();

            // To.
            foreach (string toAddress in toAddresses.Parse(";"))
                mailMsg.To.Add(new MailAddress(toAddress));

            // From.
            mailMsg.From = new MailAddress(fromAddress, fromName);

            // Subject.
            mailMsg.Subject = subject;

            return mailMsg;
        }
    }
}
