using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mygkrnk.Models;

namespace mygkrnk.Manager
{
    public interface IApiManager
    {
        Task<string> GetWikiContents(string urlString);
        Task<YoutubeData> GetYoutubeContents(string videoId, string youtubeDataApiUrl, string youtubeDataApiKey);
        Task<List<BillboardDom>> GetBillboardDom(DateTime week);
    }
}