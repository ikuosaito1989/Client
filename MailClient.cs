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
        public MailClient(string from, string to, string sendGridApiKey)
        {
            From = from;
            To = to;
            SendGridApiKey = sendGridApiKey;
        }

        public string From { get; set; }
        public string To { get; set; }
        public string SendGridApiKey { get; set; }

        public async Task SendEmailInSendGrid(string subject, string body, string to = null)
        {
            to = string.IsNullOrEmpty(to) ? this.To : to;
            var client = new SendGridClient(this.SendGridApiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(this.From),
                Subject = subject,
                PlainTextContent = body,
                HtmlContent = body.Replace("\r\n", "<br>").Replace("\n", "<br>")
            };
            msg.AddTo(new EmailAddress(to));
            await client.SendEmailAsync(msg);
        }
    }
}
