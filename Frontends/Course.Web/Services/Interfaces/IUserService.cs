using Course.Web.Models;
using System.Threading.Tasks;

namespace Course.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();
    }
}
