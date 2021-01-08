using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace webApi.Client
{
    public interface IMailClient
    {
        Task SendEmailInSendGrid(string subject, string body, string to = null);
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
