using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Orbital7.Extensions.Email
{
    public abstract class EmailEngine
    {
        public void SendTextEmail(string fromAddress, string fromName, string toAddresses, string subject,
            string textBody, bool sendAsync, string ccAddresses = null, string bccAddresses = null, 
            IDictionary <string, string> textReplacementKeys = null, 
            IList<string> attachmentFilePaths = null, IDictionary<string, byte[]> attachmentFileContents = null)
        {
            Send(CreateMessage(fromAddress, fromName, toAddresses, subject, ccAddresses, bccAddresses)
                 .SetTextBody(textBody, textReplacementKeys)
                 .AttachFiles(attachmentFilePaths)
                 .AttachFiles(attachmentFileContents),
            sendAsync);
        }

        public void SendHtmlEmail(string fromAddress, string fromName, string toAddresses, string subject,
            string htmlBody, bool sendAsync, string ccAddresses = null, string bccAddresses = null, 
            IDictionary <string, string> textReplacementKeys = null, 
            IDictionary<string, byte[]> imageContentReplacementKeys = null, string imageContentType = "image/png", 
            IList<string> attachmentFilePaths = null, IDictionary<string, byte[]> attachmentFileContents = null)
        {
            Send(CreateMessage(fromAddress, fromName, toAddresses, subject, ccAddresses, bccAddresses)
                 .SetHtmlBody(htmlBody, textReplacementKeys, imageContentReplacementKeys, imageContentType)
                 .AttachFiles(attachmentFilePaths)
                 .AttachFiles(attachmentFileContents),
            sendAsync);
        }

        public void SendHtmlEmail(string fromAddress, string fromName, string toAddresses, string subject,
            string htmlBody, bool sendAsync, string ccAddresses = null, string bccAddresses = null, 
            IDictionary <string, string> textReplacementKeys = null, 
            IDictionary<string, string> imageFilePathReplacementKeys = null, string imageContentType = "image/png", 
            IList<string> attachmentFilePaths = null, IDictionary<string, byte[]> attachmentFileContents = null)
        {
            Send(CreateMessage(fromAddress, fromName, toAddresses, subject, ccAddresses, bccAddresses)
                 .SetHtmlBody(htmlBody, textReplacementKeys, imageFilePathReplacementKeys, imageContentType)
                 .AttachFiles(attachmentFilePaths)
                 .AttachFiles(attachmentFileContents),
            sendAsync);
        }

        protected abstract void Send(MailMessage message, bool sendAsync);

        public static MailMessage CreateMessage(string fromAddress, string fromName, string toAddresses, string subject, 
            string ccAddresses = null, string bccAddresses = null)
        {
            // Create the message.
            var mailMsg = new MailMessage();

            // From.
            mailMsg.From = new MailAddress(fromAddress.Trim(), fromName.Trim());

            // To.
            foreach (string address in toAddresses.Parse(";"))
                mailMsg.To.Add(new MailAddress(address.Trim()));

            // CC.
            if (!String.IsNullOrEmpty(ccAddresses))
                foreach (string address in ccAddresses.Parse(";"))
                    mailMsg.CC.Add(new MailAddress(address.Trim()));

            // BCC.
            if (!String.IsNullOrEmpty(bccAddresses))
                foreach (string address in bccAddresses.Parse(";"))
                    mailMsg.Bcc.Add(new MailAddress(address.Trim()));

            // Subject.
            mailMsg.Subject = subject;

            return mailMsg;
        }
    }
}
