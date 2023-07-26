using Course.Shared.Dtos;
using Course.Web.Models;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace Course.Web.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SignInInput signInInput);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
    }
}
