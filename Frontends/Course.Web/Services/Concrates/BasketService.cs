using Course.Shared.Dtos;
using Course.Web.Models.BasketVMs;
using Course.Web.Services.Interfaces;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Web.Services.Concrates
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly IDiscountService _discountService;

        public BasketService(HttpClient httpClient, IDiscountService discountService)
        {
            _httpClient = httpClient;
            _discountService = discountService;
        }

        public async Task AddBasketItem(BasketItemViewModel basketItemViewModel)
        {
            var basket = await GetAllAsync();

            if (basket is not null)
            {
                if (basket.BasketItems.Any(b => b.CourseId == basketItemViewModel.CourseId) is false)
                {
                    basket.BasketItems.Add(basketItemViewModel);
                }
            }
            else
            {
                basket = new BasketViewModel();
                basket.BasketItems.Add(basketItemViewModel);
            }

            await SaveOrUpdateAsync(basket);
        }

        public Task<string> ApplyDiscount(string discountCode)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> CancelApplyDiscount()
        {
            var basket = await GetAllAsync();

            if (basket is  null && string.IsNullOrEmpty(basket.DiscountCode)) return false;

            // Discount null ise indirim uygulanmıyor.
            basket.DiscountCode = null;

            return await SaveOrUpdateAsync(basket);
        }

        public async Task<bool> DeleteAsync()
        {
            var response = await _httpClient.DeleteAsync("baskets");

            return response.IsSuccessStatusCode;
        }

        public async Task<BasketViewModel> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("baskets");

            if (response.IsSuccessStatusCode is false)
            {
                return null;
            }
            var basketViewModel = await response.Content.ReadFromJsonAsync<Response<BasketViewModel>>();

            return basketViewModel.Data;
        }

        public async Task<bool> RemoveBasketItemAsync(string courseId)
        {
            var basket = await GetAllAsync();

            if (basket is null)
            {
                return false;
            }
            var basketItem = basket.BasketItems.FirstOrDefault(b => b.CourseId == courseId);

            if (basketItem is null)
            {
                return false;
            }

            var result = basket.BasketItems.Remove(basketItem);

            if (result is false)
            {
                return false;
            }

            if (basket.BasketItems.Any())
            {
                basket.DiscountCode = null;
            }

            return await SaveOrUpdateAsync(basket);
        }

        public async Task<bool> SaveOrUpdateAsync(BasketViewModel basketViewModel)
        {
            var response = await _httpClient.PostAsJsonAsync<BasketViewModel>("baskets", basketViewModel);

            return response.IsSuccessStatusCode;
        }
    }
}
