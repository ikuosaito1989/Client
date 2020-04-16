using System.Threading.Tasks;

namespace mygkrnk.Manager
{
    public interface IFileManager
    {
        Task FileDownload(string uri, string fileName);
    }
}
