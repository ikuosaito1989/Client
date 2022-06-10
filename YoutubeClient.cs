// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using webApi.Models;

namespace webApi.Client
{
    public interface IYoutubeClient
    {
        Task<YoutubeVideo> GetVideoAsync(string videoId);
    }

    public class YoutubeClient : IYoutubeClient
    {
        private readonly string _apiKey;
        private readonly string _url = "https://www.googleapis.com/youtube/v3";
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
