using System;
using System.Threading.Tasks;
using System.Net.Http;
using webApi.Models;
using Newtonsoft.Json;

namespace webApi.Client
{
    public interface IYoutubeClient
    {
        Task<YoutubeVideo> GetVideoAsync(string videoId);
    }

    public class YoutubeClient : IYoutubeClient
    {
        private string _apiKey;
        private string _url = "https://www.googleapis.com/youtube/v3";
        private static HttpClient _client;
        public YoutubeClient(HttpClient client, string apiKey)
        {
            _client = client;
            _apiKey = apiKey;
        }

        public async Task<YoutubeVideo> GetVideoAsync(string videoId)
        {
            var url = $"{_url}/videos?part=statistics&id={videoId}&key={_apiKey}";
            var response = await _client.GetAsync(url);
            return JsonConvert.DeserializeObject<YoutubeVideo>(
                await response.Content.ReadAsStringAsync());
        }
    }
}
