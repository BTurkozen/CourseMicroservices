using System.Threading.Tasks;

namespace Course.Web.Services.Interfaces
{
    public interface IClientCredentialTokenService
    {
        Task<string> GetTokenAsync();
    }
}
