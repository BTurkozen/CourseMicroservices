using Course.Web.Models.OrderVMs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Course.Web.Services.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// Senkron İletişim
        /// </summary>
        /// <param name="checkoutInfoInput"></param>
        /// <returns></returns>
        Task<OrderCreatedViewModel> CreateOrderAsync(CheckoutInfoInput checkoutInfoInput);

        /// <summary>
        /// Asenkron İletişim.
        /// </summary>
        /// <param name="checkoutInfoInput"></param>
        /// <returns></returns>
        Task<OrderSuspendViewModel> SuspendOrderAsync(CheckoutInfoInput checkoutInfoInput);

        Task<List<OrderViewModel>> GetAllOrderAsync();
    }
}
