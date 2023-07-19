using Course.Services.Basket.Dtos;
using Course.Shared.Dtos;
using System.Threading.Tasks;

namespace Course.Services.Basket.Services
{
    public interface IBasketService
    {
        Task<Response<BasketDto>> GetBasketAsync(string userId);
        Task<Response<bool>> SaveOrUpdate(BasketDto basketDto);
        Task<Response<bool>> Delete(string userId);
    }
}
