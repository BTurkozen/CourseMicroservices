using Course.Services.Discount.Dtos;
using Course.Services.Discount.Services;
using Course.Shared.ControllerBases;
using Course.Shared.Dtos;
using Course.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Course.Services.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomBaseController
    {
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IDiscountService _discountService;

        // alt + enter => create  ctor and implements

        public DiscountsController(ISharedIdentityService sharedIdentityService, IDiscountService discountService)
        {
            _sharedIdentityService = sharedIdentityService;
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _discountService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CreateActionResultInstance(await _discountService.GetByIdAsync(id));
        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var userId = _sharedIdentityService.GetUserId;

            return CreateActionResultInstance(await _discountService.GetByCodeAndUserId(code, userId));
        }

        [HttpPost]
        public async Task<IActionResult> Save(DiscountCreateDto discountCreateDto)
        {
            return CreateActionResultInstance(await _discountService.SaveAsync(discountCreateDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(DiscountUpdateDto discountUpdateDto)
        {
            return CreateActionResultInstance(await _discountService.UpdateAsync(discountUpdateDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResultInstance(await _discountService.DeleteAsync(id));
        }
    }
}
