// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace webApi.Models
{
    public class YoutubeVideo
    {
        public IEnumerable<Item> Items { get; set; }
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
