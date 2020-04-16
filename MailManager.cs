using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace mygkrnk.Manager
{
    public class MailManager : IMailManager
    {
        public MailManager(string from, string to, string sendGridApiKey)
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
            var response = await client.SendEmailAsync(msg);
        }
    }
}
