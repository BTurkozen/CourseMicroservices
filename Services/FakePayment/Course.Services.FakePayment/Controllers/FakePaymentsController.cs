using Course.Shared.ControllerBases;
using Course.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Course.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentsController : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> ReceviePayment()
        {
            return CreateActionResultInstance(Response<NoContentResult>.Success(200));
        }
    }
}
