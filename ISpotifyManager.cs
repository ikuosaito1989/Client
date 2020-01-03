using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyAPI.Web.Models;

namespace mygkrnk.Manager
{
    public interface ISpotifyManager
    {
        Task<SearchItem> SearchAirtistItemsAsync(string q, int limit = 1, int offset = 0, string market = "");
        Task<FullArtist> GetArtistAsync(string id);
        Task<SeveralArtists> GetSeveralArtists(List<string> ids);
    }
}