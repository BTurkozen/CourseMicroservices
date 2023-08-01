using Course.Shared.Dtos;
using Course.Web.Models.DiscountVMs;
using Course.Web.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Web.Services.Concrates
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DiscountViewModel> GetDiscountAsync(string code)
        {
            var response = await _httpClient.GetAsync($"discount/GetByCode/{code}");

            if (response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<Response<DiscountViewModel>>();

            return result.Data;
        }
    }
}
