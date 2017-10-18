using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
    public static class MailExtensions
    {
        public static void SendViaSmtp(this MailMessage mailMessage, SmtpClient smtpClient, bool sendAsync)
        {
            if (!sendAsync)
                smtpClient.Send(mailMessage);
            else
                smtpClient.SendAsync(mailMessage, null);
        }

        public static void SendViaSendGrid(this MailMessage mailMessage, string sendGridUsername, string sendGridPassword, bool sendAsync)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
            NetworkCredential credentials = new NetworkCredential(sendGridUsername, sendGridPassword);
            smtpClient.Credentials = credentials;

            mailMessage.SendViaSmtp(smtpClient, sendAsync);
        }

        public static MailMessage AttachFiles(this MailMessage mailMessage, IList<string> filePaths)
        {
            if (filePaths != null)
                foreach (var filePath in filePaths)
                    mailMessage.Attachments.Add(new Attachment(filePath));

            return mailMessage;
        }

        public static MailMessage AttachFiles(this MailMessage mailMessage, IDictionary<string, byte[]> fileContents)
        {
            if (fileContents != null)
                foreach (var fileContent in fileContents)
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(fileContent.Value), fileContent.Key));

            return mailMessage;
        }

        public static MailMessage SetTextBody(this MailMessage mailMessage, string textBody, IDictionary<string, string> textReplacementKeys = null)
        {
            var alternateView = AlternateView.CreateAlternateViewFromString(textBody.Replace(textReplacementKeys), null, MediaTypeNames.Text.Plain);
            mailMessage.AlternateViews.Add(alternateView);
            return mailMessage;
        }

        public static MailMessage SetHtmlBody(this MailMessage mailMessage, string htmlBody, IDictionary<string, string> textReplacementKeys = null)
        {
            return mailMessage.SetHtmlBody(htmlBody, textReplacementKeys, (IDictionary<string, LinkedResource>)null);
        }

        public static MailMessage SetHtmlBody(this MailMessage mailMessage, string htmlBody, IDictionary<string, string> textReplacementKeys, 
            IDictionary<string, string> imageFilePathReplacementKeys, string imageContentType = "image/png")
        {
            var resourceReplacements = new Dictionary<string, LinkedResource>();
            if (imageFilePathReplacementKeys != null)
            {
                foreach (var imageReplacement in imageFilePathReplacementKeys)
                {
                    resourceReplacements.Add(imageReplacement.Key, new LinkedResource(imageReplacement.Value, 
                        new ContentType(imageContentType))
                    {
                        ContentId = Guid.NewGuid().ToString()
                    });
                }
            }

            return mailMessage.SetHtmlBody(htmlBody, textReplacementKeys, resourceReplacements);
        }

        public static MailMessage SetHtmlBody(this MailMessage mailMessage, string htmlBody, IDictionary<string, string> textReplacementKeys,
            IDictionary<string, byte[]> imageContentReplacementKeys, string imageContentType = "image/png")
        {
            var resourceReplacements = new Dictionary<string, LinkedResource>();
            if (imageContentReplacementKeys != null)
            {
                foreach (var imageReplacement in imageContentReplacementKeys)
                {
                    resourceReplacements.Add(imageReplacement.Key, new LinkedResource(new MemoryStream(imageReplacement.Value),
                        new ContentType(imageContentType))
                    {
                        ContentId = Guid.NewGuid().ToString(),
                        TransferEncoding = TransferEncoding.Base64,
                    });
                }
            }

            return mailMessage.SetHtmlBody(htmlBody, textReplacementKeys, resourceReplacements);
        }

        private static MailMessage SetHtmlBody(this MailMessage mailMessage, string htmlBody, IDictionary<string, string> textReplacementKeys, 
            IDictionary<string, LinkedResource> imageResourceReplacementKeys)
        {
            string html = htmlBody.Replace(textReplacementKeys);

            var resources = new List<LinkedResource>();
            if (imageResourceReplacementKeys != null)
            {
                foreach (var imageReplacement in imageResourceReplacementKeys)
                {
                    html = html.Replace(imageReplacement.Key, "<img src='cid:" + imageReplacement.Value.ContentId + "'/>");
                    resources.Add(imageReplacement.Value);
                }
            }

            var alternateView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
            foreach (var resource in resources)
                alternateView.LinkedResources.Add(resource);
            mailMessage.AlternateViews.Add(alternateView);

            mailMessage.IsBodyHtml = true;

            return mailMessage;
        }
    }
}
