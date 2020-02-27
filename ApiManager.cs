using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using mygkrnk.Models;
using Newtonsoft.Json;

namespace mygkrnk.Manager
{
    public class ApiManager : IApiManager
    {
        private static HttpClient _client;
        public ApiManager(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetWikiContents(string urlString)
        {
            var url = new Uri(urlString);
            var requestUrl = string.Format(@"https://ja.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exsentences=10&explaintext=&titles={0}", url.Segments.Last().Replace("/", ""));
            var response = await _client.GetAsync(requestUrl);
            var json = await response.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<dynamic>(json);
            var content = Regex.Match(jsonObj.query.pages.ToString(), "\"extract\":.*\".*?\"").Value;
            return content.Replace("\"extract\": \"", "").Replace("\"", "");
        }

        public async Task<YoutubeData> GetYoutubeContents(string videoId, string youtubeDataApiUrl, string youtubeDataApiKey)
        {
            var url = string.Format(youtubeDataApiUrl, videoId, youtubeDataApiKey);
            var response = await _client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<YoutubeData>(json);
        }

        public async Task<List<BillboardDom>> GetBillboardDom(DateTime week = default(DateTime))
        {

            var dateString = "";
            if (week != default(DateTime))
            {
                dateString = week.ToString("yyyy-MM-dd");
            }
            var url = "https://www.billboard.com/charts/hot-100/" + dateString;
            var response = await _client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();

            var parser = new HtmlParser();
            var doc = parser.Parse(html);
            var dom = new List<BillboardDom>();

            var domItem = new BillboardDom();
            //dropdown__date-selector-option 

            var weeks = doc.GetElementsByClassName("dropdown__date-selector-option");
            var prevString = Regex.Match(weeks[0].InnerHtml, "\\d{4}-\\d{1,2}-\\d{1,2}").Value;
            domItem.PrevWeek = DateTime.Parse(prevString);

            domItem.Title = doc.GetElementsByClassName("chart-number-one__title").First().TextContent;
            domItem.Rank = "1";
            dom.Add(domItem);

            foreach (var item in doc.GetElementsByClassName("chart-list-item"))
            {
                domItem = new BillboardDom()
                {
                    Rank = item.Attributes["data-rank"].Value,
                    Title = item.Attributes["data-title"].Value
                };
                dom.Add(domItem);
            }
            return dom.OrderBy(x => int.Parse(x.Rank)).Take(10).ToList();
        }
    }
}
