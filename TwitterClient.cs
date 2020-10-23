using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreTweet;

namespace webApi.Client
{
    public interface ITwitterClient
    {
        Task<SearchResult> SearchTweets(params Expression<Func<string, object>>[] parameters);
        UserResponse ShowUser(params Expression<Func<string, object>>[] parameters);
    }

    public class TwitterClient : ITwitterClient
    {
        private readonly Tokens _tokens;
        public TwitterClient(string consumerKey, string consumerSecret, string accessToken, string accessSecret)
        {
            _tokens = Tokens.Create(consumerKey, consumerSecret, accessToken, accessSecret);
        }

        public async Task<SearchResult> SearchTweets(params Expression<Func<string, object>>[] parameters)
        {
            return await _tokens.Search.TweetsAsync(parameters);
        }

        public UserResponse ShowUser(params Expression<Func<string, object>>[] parameters)
        {
            return _tokens.Users.Show(parameters);
        }
    }
}
