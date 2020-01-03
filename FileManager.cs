using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace mygkrnk.Manager
{
    public class FileManager : IFileManager
    {
        private static HttpClient _client;
        public FileManager(HttpClient client)
        {
            _client = client;
        }
        public async Task FileDownload(string uri, string fileName)
        {
            HttpResponseMessage res = await _client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "users", fileName);
            using (var fileStream = File.Create(outputPath))
            {
                using (var httpStream = await res.Content.ReadAsStreamAsync())
                {
                    httpStream.CopyTo(fileStream);
                    fileStream.Flush();
                }
            }
        }
    }
}