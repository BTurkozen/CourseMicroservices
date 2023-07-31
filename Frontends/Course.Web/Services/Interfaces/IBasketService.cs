using Course.Web.Models.BasketVMs;
using System.Threading.Tasks;

namespace Course.Web.Services.Interfaces
{
    public interface IBasketService
    {
        Task<BasketViewModel> GetAllAsync();
        Task<bool> SaveOrUpdateAsync(BasketViewModel basketViewModel);
        Task<bool> DeleteAsync();
        Task AddBasketItem(BasketItemViewModel basketItemViewModel);
        Task<bool> RemoveBasketItemAsync(string courseId);
        Task<string> ApplyDiscount(string discountCode);
        Task<bool> CancelApplyDiscount();
    }
}
