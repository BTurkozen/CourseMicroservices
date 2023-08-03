using Course.Web.Models.OrderVMs;
using Course.Web.Services.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Course.Web.Services.Concrates
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _clientHttp;

        public OrderService(HttpClient clientHttp)
        {
            _clientHttp = clientHttp;
        }

        public Task<OrderCreatedViewModel> CreateOrderAsync(CheckoutInfoInput checkoutInfoInput)
        {
            throw new System.NotImplementedException();
        }

        public Task<OrderViewModel> GetAllOrderAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task SuspendOrderAsync(CheckoutInfoInput checkoutInfoInput)
        {
            throw new System.NotImplementedException();
        }
    }
}
