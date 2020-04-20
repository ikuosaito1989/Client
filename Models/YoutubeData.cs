using System.Collections.Generic;

namespace webApi.Models
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
