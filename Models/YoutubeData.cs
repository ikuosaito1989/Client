using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace mygkrnk.Models
{
    public class YoutubeData
    {
        public List<Item> Items { get; set; }
    }
    public class Item
    {
        public string Id { get; set; }
        public Statistics Statistics { get; set; }
    }

    public class Statistics
    {
        public string ViewCount { get; set; }
        public string LikeCount { get; set; }
        public string DislikeCount { get; set; }
        public string FavoriteCount { get; set; }
        public string CommentCount { get; set; }
    }

}
