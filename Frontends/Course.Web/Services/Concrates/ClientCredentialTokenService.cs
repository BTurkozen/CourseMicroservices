using Course.Web.Models;
using Course.Web.Services.Interfaces;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace Course.Web.Services.Concrates
{
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly ClientSettings _clientSettings;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;
        private readonly HttpClient _httpClient;

        public ClientCredentialTokenService(IClientAccessTokenCache clientAccessTokenCache, IOptions<ServiceApiSettings> serviceApiSettings, IOptions<ClientSettings> clientSettings, HttpClient httpClient)
        {
            _clientAccessTokenCache = clientAccessTokenCache;
            _serviceApiSettings = serviceApiSettings.Value;
            _clientSettings = clientSettings.Value;
            _httpClient = httpClient;
        }

        public async Task<string> GetTokenAsync()
        {
            // Cache içerisinde token varmı onu kontrol etmek amaçlı token'ı çekiyoruz.
            var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken", null);

            // Var olup olmadığını kontrol ediyoruz. Var ise dönderiyoruz.
            if (currentToken is not null)
            {
                return currentToken.AccessToken;
            }

            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
            {
                ClientId = _clientSettings.WebClient.ClientId,
                ClientSecret = _clientSettings.WebClient.ClientSecret,
                Address = discovery.TokenEndpoint,
            };

            var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);

            if (newToken.IsError)
            {
                throw newToken.Exception;
            }

            // Cache service içerisine ekleme işlemi yapıyoruz.
            await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken, newToken.ExpiresIn, null);

            return newToken.AccessToken;
        }
    }
}
