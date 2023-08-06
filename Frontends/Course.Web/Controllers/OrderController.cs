using Course.Web.Models.OrderVMs;
using Course.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Course.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        {
            ViewBag.basket = await _basketService.GetAllAsync();

            return View(new CheckoutInfoInput());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutInfoInput checkoutInfoInput)
        {
            // Senkron iletişim.
            //var orderStatus = await _orderService.CreateOrderAsync(checkoutInfoInput);

            // Asenkron iletişim.
            var orderStatus = await _orderService.SuspendOrderAsync(checkoutInfoInput);

            if (orderStatus.IsSuccessful is false)
            {
                ViewBag.basket = await _basketService.GetAllAsync();

                ViewBag.error = orderStatus.Error;

                return View();
            }

            //return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = orderStatus.OrderId });
            return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = new Random().Next(1, 1000) });
        }

        public async Task<IActionResult> SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;

            return await Task.FromResult(View());
        }

        public async Task<IActionResult> CheckoutHistory()
        {
            return View(await _orderService.GetAllOrderAsync());
        }
    }
}
