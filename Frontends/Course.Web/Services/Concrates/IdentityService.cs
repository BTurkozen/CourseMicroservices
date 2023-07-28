using Course.Shared.Dtos;
using Course.Web.Models;
using Course.Web.Services.Interfaces;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Course.Web.Services.Concrates
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor contextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            throw new System.NotImplementedException();
        }

        public Task RevokeRefreshToken()
        {
            throw new System.NotImplementedException();
        }

        public async Task<Response<bool>> SignIn(SignInInput signInInput)
        {
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.BaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = true },
            });

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = signInInput.Email,
                Password = signInInput.Password,
                Address = discovery.TokenEndpoint,
            };

            var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (token.IsError)
            {
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();

                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Response<bool>.Fail(errorDto.Errors, 404);
            }

            var userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = discovery.UserInfoEndpoint,
            };

            var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            // Schema default olarak verıyoruz.
            // Uygulamaya Username ve role bilgilerini nereden alacağını veriyoruz.
            var claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

            // Cookie oluşturmak için bunu oluşturuyoruz. Cookie oluşacak ama hangi kimlikten oluşacak.
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // AccessToken ve resfresh token tutacağımız yer.
            var authenticationProperties = new AuthenticationProperties();

            // Tokenlarımızı ekledik.
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name= OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken},
                new AuthenticationToken{Name= OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken},
                new AuthenticationToken{Name= OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("O",CultureInfo.InvariantCulture)},
            });

            authenticationProperties.IsPersistent = signInInput.IsRemember;

            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return Response<bool>.Success(200);

        }
    }
}
