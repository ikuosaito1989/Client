// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreTweet;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.V2;

namespace webApi.Client
{
    public interface ITwitterClient
    {
        [Obsolete("`SearchTweets` is deprecated and will be removed in a future release, please use `SearchTweetsV2`.")]
        Task<SearchResult> SearchTweets(params Expression<Func<string, object>>[] parameters);

        /// <summary>
        /// GET /2/tweets/search/recent
        /// 検索クエリに一致する過去7日間のツイートを返す
        /// https://developer.twitter.com/en/docs/twitter-api/tweets/search/api-reference/get-tweets-search-recent
        /// </summary>
        /// <param name="keyword">キーワード</param>
        /// <param name="hashtags">ハッシュタグ</param>
        /// <param name="pageSize">取得件数</param>
        /// <param name="ignoreUserNames">無視するユーザー名</param>
        /// <param name="untilId"></param>
        /// <param name="operators"></param>
        Task<SearchTweetsV2Response> SearchTweetsV2(
            string keyword,
            IEnumerable<string> hashtags = null,
            int pageSize = 10,
            IEnumerable<string> ignoreUserNames = null,
            string untilId = null,
            string operators = "-is:retweet has:images lang:ja"
        );

        /// <summary>
        /// ツイートを取得する
        /// </summary>
        /// <param name="tweetId"></param>
        Task<TweetV2Response> GetTweetAsync(long tweetId);

        /// <summary>
        /// ツイートする
        /// </summary>
        /// <param name="text"></param>
        Task<Tweetinvi.Models.ITweet> PublishTweetV2(string text);

        /// <summary>
        /// リプライする
        /// </summary>
        /// <param name="tweetId"></param>
        /// <param name="text"></param>
        Task<Tweetinvi.Models.ITweet> ReplyTweetV2(long tweetId, string text);

        UserResponse ShowUser(params Expression<Func<string, object>>[] parameters);
    }

    public class TwitterClient : ITwitterClient
    {
        private readonly Tokens _tokens;
        private readonly Tweetinvi.TwitterClient _client;

        public TwitterClient(string consumerKey, string consumerSecret, string accessToken, string accessSecret)
        {
            _tokens = Tokens.Create(consumerKey, consumerSecret, accessToken, accessSecret);

            _client = new Tweetinvi.TwitterClient(consumerKey, consumerSecret, accessToken, accessSecret);
        }

        public async Task<SearchResult> SearchTweets(params Expression<Func<string, object>>[] parameters)
        {
            return await _tokens.Search.TweetsAsync(parameters);
        }

        public async Task<SearchTweetsV2Response> SearchTweetsV2(
            string keyword,
            IEnumerable<string> hashtags = null,
            int pageSize = 10,
            IEnumerable<string> ignoreUserNames = null,
            string untilId = null,
            string operators = "-is:retweet has:images lang:ja"
        )
        {
            var tags = "";
            if (hashtags != null)
            {
                tags = string.Join(" OR ", hashtags.Select(x => $"#{x}"));
            }

            var userNames = "";
            if (ignoreUserNames != null)
            {
                userNames = string.Join(" ", ignoreUserNames.Select(x => $"-from:{x}"));
            }

            var queries = new string[] { keyword, tags, userNames, operators }
                .Where(x => !string.IsNullOrEmpty(x));
            var query = string.Join(" ", queries);

            var param = new SearchTweetsV2Parameters(query) { PageSize = pageSize, UntilId = untilId };

            return await _client.SearchV2.SearchTweetsAsync(param);
        }

        public async Task<TweetV2Response> GetTweetAsync(long tweetId)
        {
            return await _client.TweetsV2.GetTweetAsync(tweetId);
        }

        public async Task<Tweetinvi.Models.ITweet> PublishTweetV2(string text)
        {
            return await _client.Tweets.PublishTweetAsync(text);
        }

        public async Task<Tweetinvi.Models.ITweet> ReplyTweetV2(long tweetId, string text)
        {
            var tweet = await _client.Tweets.GetTweetAsync(tweetId);
            var parameters = new PublishTweetParameters($"@{tweet.CreatedBy} {text}") { InReplyToTweet = tweet };
            return await _client.Tweets.PublishTweetAsync(parameters);
        }

        public UserResponse ShowUser(params Expression<Func<string, object>>[] parameters)
        {
            return _tokens.Users.Show(parameters);
        }
    }
}
