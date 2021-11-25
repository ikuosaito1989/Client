using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using Newtonsoft.Json;
using webApi.Models;

namespace webApi.Client
{
    public interface IApiClient
    {
        Task<string> GetWikiContents(string urlString);
        Task<List<BillboardDom>> GetBillboardDom(DateTime week);
    }

    public class ApiClient : IApiClient
    {
        private static HttpClient s_client;
        private readonly CultureInfo _cultureInfo = CultureInfo.CreateSpecificCulture("ja-JP");
        public ApiClient(HttpClient client)
        {
            s_client = client;
        }

        public async Task<string> GetWikiContents(string urlString)
        {
            var url = new Uri(urlString);
            var requestUrl = $"https://ja.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exsentences=10&explaintext=&titles={url.Segments.Last().Replace("/", "")}";
            var response = await s_client.GetAsync(requestUrl);
            var json = await response.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<dynamic>(json);
            var content = new Regex("\"extract\":.*\".*?\"").Match(jsonObj.query.pages.ToString()).Value;
            return content.Replace("\"extract\": \"", "").Replace("\"", "");
        }

        public async Task<List<BillboardDom>> GetBillboardDom(DateTime week = default)
        {
            var dateString = "";
            if (week != default)
            {
                dateString = week.ToString("yyyy-MM-dd", _cultureInfo);
            }
            var url = "https://www.billboard.com/charts/hot-100/" + dateString;
            var response = await s_client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();

            var parser = new HtmlParser();
            var doc = parser.Parse(html);
            var dom = new List<BillboardDom>();

            var domItem = new BillboardDom();

            var weeks = doc.GetElementsByClassName("dropdown__date-selector-option");
            var prevString = new Regex("\\d{4}-\\d{1,2}-\\d{1,2}").Match(weeks[0].InnerHtml).Value;
            domItem.PrevWeek = DateTime.Parse(prevString, _cultureInfo);

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
            return dom.OrderBy(x => int.Parse(x.Rank, _cultureInfo.NumberFormat)).Take(10).ToList();
        }
    }
}
