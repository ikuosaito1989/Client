using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;

namespace webApi.Client
{
    public interface ISpotifyClient
    {
        Task<SearchItem> SearchAirtistItemsAsync(string q, int limit = 1, int offset = 0, string market = "");
        Task<FullArtist> GetArtistAsync(string id);
        Task<SeveralArtists> GetSeveralArtists(List<string> ids);
    }

    public class SpotifyClient : ISpotifyClient
    {
        private static SpotifyWebAPI s_spotify;
        public SpotifyClient(string clientId, string clientSecret)
        {
            var auth = new CredentialsAuth(clientId, clientSecret);
            var token = auth.GetToken().Result;
            s_spotify = new SpotifyWebAPI() { TokenType = token.TokenType, AccessToken = token.AccessToken };
        }

        public Task<SearchItem> SearchAirtistItemsAsync(string q, int limit = 1, int offset = 0, string market = "")
        {
            return s_spotify.SearchItemsAsync(q, SearchType.Artist);
        }

        public Task<FullArtist> GetArtistAsync(string id)
        {
            return s_spotify.GetArtistAsync(id);
        }

        public Task<SeveralArtists> GetSeveralArtists(List<string> ids)
        {
            return s_spotify.GetSeveralArtistsAsync(ids);
        }
    }
}
