using Course.Shared.Dtos;
using Course.Shared.Services;
using Course.Web.Models.OrderVMs;
using Course.Web.Models.PaymentVMs;
using Course.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Web.Services.Concrates
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IPaymentService _paymentService;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderService(
            HttpClient httpClient,
            IPaymentService paymentService,
            IBasketService basketService,
            ISharedIdentityService sharedIdentityService)
        {
            _httpClient = httpClient;
            _paymentService = paymentService;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreatedViewModel> CreateOrderAsync(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.GetAllAsync();

            var paymentInfoInput = new PaymentInfoInput()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                CVV = checkoutInfoInput.CVV,
                Expiration = checkoutInfoInput.Expiration,
                TotalPrice = basket.TotalPrice
            };

            var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);

            if (responsePayment is false)
            {
                return new OrderCreatedViewModel { Error = "Could not receive payment", IsSuccessful = false };
            }

            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressCreateInput()
                {
                    Province = checkoutInfoInput.Province,
                    District = checkoutInfoInput.District,
                    Line = checkoutInfoInput.Line,
                    Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode,
                },
            };

            basket.BasketItems.ForEach(basketItem =>
            {
                var orderItemCreateInput = new OrderItemCreateInput()
                {
                    ProductId = basketItem.CourseId,
                    Price = basketItem.GetCurrentPice,
                    PictureUrl = "",
                    ProductName = basketItem.CourseName,
                };

                orderCreateInput.OrderItems.Add(orderItemCreateInput);
            });

            var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);

            if (response.IsSuccessStatusCode is false)
            {
                return new OrderCreatedViewModel { Error = "Order could not be created", IsSuccessful = false };
            }

            var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<OrderCreatedViewModel>();

            orderCreatedViewModel.IsSuccessful = true;

            return orderCreatedViewModel;
        }

        public async Task<List<OrderViewModel>> GetAllOrderAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");

            return response.Data;
        }

        public Task SuspendOrderAsync(CheckoutInfoInput checkoutInfoInput)
        {
            throw new System.NotImplementedException();
        }
    }
}
