// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace webApi.Client
{
    public interface IMailClient
    {
#pragma warning disable CA1716
        Task SendEmailInSendGrid(string subject, string body, string to = null);
#pragma warning restore CA1716
    }

    public class MailClient : IMailClient
    {
        public MailClient(string from, string to, string sendGridApiKey, string fromName = "")
        {
            From = from;
            To = to;
            SendGridApiKey = sendGridApiKey;
            FromName = fromName;
        }

        public string From { get; set; }
        public string FromName { get; set; }
        public string To { get; set; }
        public string SendGridApiKey { get; set; }

        public async Task SendEmailInSendGrid(string subject, string body, string to = null)
        {
            to = string.IsNullOrEmpty(to) ? To : to;
            var client = new SendGridClient(SendGridApiKey);
            var from = new EmailAddress(From, FromName);
            var toAddress = new EmailAddress(to);
            var plainTextContent = body;
            var htmlContent = body.Replace("\r\n", "<br>").Replace("\n", "<br>");
            var msg = MailHelper.CreateSingleEmail(from, toAddress, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}
