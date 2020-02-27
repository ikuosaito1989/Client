using System.Threading.Tasks;

namespace mygkrnk.Manager
{
    public interface IMailManager
    {
        Task SendEmailInSendGrid(string subject, string body, string to = null);
    }
}
