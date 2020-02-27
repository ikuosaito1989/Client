using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreTweet;

namespace mygkrnk.Manager
{
    public class TwitterManager : ITwitterManager
    {
        private Tokens _tokens;
        public TwitterManager(string consumerKey, string consumerSecret, string accessToken, string accessSecret)
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
