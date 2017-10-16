using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Orbital7.Extensions.Email
{
    // TODO: Move to Orbital7.Extensions.
    public class SendGridEmailEngine : SmtpEmailEngine
    {
        public SendGridEmailEngine(string sendGridUsername, string sendGridPassword)
            : base(null)
        {
            this.SmtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587))
            {
                Credentials = new System.Net.NetworkCredential(sendGridUsername, sendGridPassword)
            };
        }
    }
}
