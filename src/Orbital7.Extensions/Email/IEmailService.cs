using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.Email
{
    public interface IEmailService
    {
        Task SendHtmlEmailAsync(
            string fromAddress,
            string fromName,
            string toAddresses,
            string subject,
            string htmlBody,
            bool sendAsync,
            string ccAddresses = null,
            string bccAddresses = null,
            IDictionary<string, string> textReplacementKeys = null,
            IDictionary<string, byte[]> imageContentReplacementKeys = null,
            string imageContentType = "image/png",
            IList<string> attachmentFilePaths = null,
            IDictionary<string, byte[]> attachmentFileContents = null);

        Task SendHtmlEmailAsync(
            string fromAddress,
            string fromName,
            string toAddresses,
            string subject,
            string htmlBody,
            bool sendAsync,
            string ccAddresses = null,
            string bccAddresses = null,
            IDictionary<string, string> textReplacementKeys = null,
            IDictionary<string, string> imageFilePathReplacementKeys = null,
            string imageContentType = "image/png",
            IList<string> attachmentFilePaths = null,
            IDictionary<string, byte[]> attachmentFileContents = null);

        Task SendTextEmailAsync(
            string fromAddress,
            string fromName,
            string toAddresses,
            string subject,
            string textBody,
            bool sendAsync,
            string ccAddresses = null,
            string bccAddresses = null,
            IDictionary<string, string> textReplacementKeys = null,
            IList<string> attachmentFilePaths = null,
            IDictionary<string, byte[]> attachmentFileContents = null);
    }
}
