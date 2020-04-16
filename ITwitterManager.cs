using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreTweet;

namespace mygkrnk.Manager
{
    public interface ITwitterManager
    {
        Task<SearchResult> SearchTweets(params Expression<Func<string, object>>[] parameters);
        UserResponse ShowUser(params Expression<Func<string, object>>[] parameters);
    }
}
