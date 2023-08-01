using Course.Web.Models.DiscountVMs;
using Course.Web.Services.Interfaces;
using System.Net.Http;
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

        public Task<DiscountViewModel> GetDiscountAsync(string code)
        {
            throw new System.NotImplementedException();
        }
    }
}
