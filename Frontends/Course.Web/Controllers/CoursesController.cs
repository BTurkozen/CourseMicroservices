using Course.Shared.Services;
using Course.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Course.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _identityService;

        public CoursesController(ICatalogService catalogService, ISharedIdentityService identityService)
        {
            _catalogService = catalogService;
            _identityService = identityService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _catalogService.GetAllCourseByUserIdAsync(_identityService.GetUserId));
        }
    }
}
