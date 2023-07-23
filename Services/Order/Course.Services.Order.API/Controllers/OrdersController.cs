using Course.Services.Order.Application.Commands;
using Course.Services.Order.Application.Dtos;
using Course.Services.Order.Application.Queries;
using Course.Shared.ControllerBases;
using Course.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Course.Services.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : CustomBaseController
    {
        private readonly IMediator _mediatr;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrdersController(IMediator mediatr, ISharedIdentityService sharedIdentityService)
        {
            _mediatr = mediatr;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _mediatr.Send(new GetOrdersByUserIdQuery { UserId = _sharedIdentityService.GetUserId });

            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> Save(CreateOrderCommand createOrderCommand)
        {
            var response = await _mediatr.Send(createOrderCommand);

            return CreateActionResultInstance(response);
        }
    }
}
