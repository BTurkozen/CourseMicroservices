using Course.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Course.Web.Handlers
{
    public class ResourceOwnerPasswordTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;
        private readonly ILogger<ResourceOwnerPasswordTokenHandler> _logger;

        public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor httpContextAccessor, IIdentityService identityService, ILogger<ResourceOwnerPasswordTokenHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
            _logger = logger;
        }

        /// <summary>
        /// Her istek başlatıldığında bu araya girerek işlemi başlatıcak.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Access Token çekiyoruz.
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            // Gelen Tokeni isteğin Header'ına ekliyorum.
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // istek gönderip sonucunu alıyoruz.
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // AccessToken Geçersiz ise Refresh token alıyoruz.
                var tokenResponse = await _identityService.GetAccessTokenByRefreshToken();

                if (tokenResponse is not null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

                    response = await base.SendAsync(request, cancellationToken);
                }
            }

            if (response.StatusCode is System.Net.HttpStatusCode.Unauthorized)
            {
                // Delegate içerisinde iken bir Action sınıfına redirect yapılamaz.
                // Bunun için burada hata fırlatılması gerekmektedir.
                // Fırlatılan hatayı yakalayarak redirect işlemi gerçekleştirebiliriz. 

                throw new Exceptions.UnAuthorizeException();
            }

            return response;
        }
    }
}
