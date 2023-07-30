using Course.Web.Models.BasketVMs;
using Course.Web.Services.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Course.Web.Services.Concrates
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task AddBasketItem(BasketItemViewModel basketItemViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> ApplyDiscount(string discountCode)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CancelApplyDiscount()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<BasketViewModel> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RemoveBasketItem(string courseId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SaveOrUpdateAsync(BasketViewModel basketViewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
