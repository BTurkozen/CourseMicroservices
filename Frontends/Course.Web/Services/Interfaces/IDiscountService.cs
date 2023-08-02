using Course.Web.Models.DiscountVMs;
using System.Threading.Tasks;

namespace Course.Web.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> GetDiscountAsync(string code);
    }
}
