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
        private static SpotifyWebAPI _spotify;
        public SpotifyClient(string clientId, string clientSecret)
        {
            var auth = new CredentialsAuth(clientId, clientSecret);
            var token = auth.GetToken().Result;
            _spotify = new SpotifyWebAPI() { TokenType = token.TokenType, AccessToken = token.AccessToken };
        }

        public Task<SearchItem> SearchAirtistItemsAsync(string q, int limit = 1, int offset = 0, string market = "")
        {
            return _spotify.SearchItemsAsync(q, SearchType.Artist);
        }

        public Task<FullArtist> GetArtistAsync(string id)
        {
            return _spotify.GetArtistAsync(id);
        }

        public Task<SeveralArtists> GetSeveralArtists(List<string> ids)
        {
            return _spotify.GetSeveralArtistsAsync(ids);
        }
    }
}
