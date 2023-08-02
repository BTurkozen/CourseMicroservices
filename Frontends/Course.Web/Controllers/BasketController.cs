using Course.Web.Models.BasketVMs;
using Course.Web.Models.DiscountVMs;
using Course.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public BasketController(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _basketService.GetAllAsync());
        }

        public async Task<IActionResult> AddBasketItem(string courseId)
        {
            var course = await _catalogService.GetByCourseIdAsync(courseId);

            var basketItemViewModel = new BasketItemViewModel()
            {
                CourseId = courseId,
                CourseName = course.Name,
                Price = course.Price,
            };

            await _basketService.AddBasketItem(basketItemViewModel);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveBasketItem(string courseId)
        {
            await _basketService.RemoveBasketItemAsync(courseId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ApplyDiscount(DiscountApplyInput discountApplyInput)
        {
            if (ModelState.IsValid is false)
            {
                TempData["DiscountError"] = ModelState.Values.SelectMany(ms => ms.Errors).Select(ms => ms.ErrorMessage).First();

                return RedirectToAction(nameof(Index));
            }

            var discountStatus = await _basketService.ApplyDiscount(discountApplyInput.Code);

            // Kodu uyguladıktan sonra ındex sayfasına yönlendireceğiz.
            // bir actiondan diğer actina data tasıyabilmek için "TempData" kullanıyoruz.
            TempData["discountStatus"] = discountStatus;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CancelAppliedDiscount()
        {
            await _basketService.CancelApplyDiscount();

            return RedirectToAction(nameof(Index));
        }
    }
}
