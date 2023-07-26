using Course.Web.Models;
using Course.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Course.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<IActionResult> SignIn()
        {
            return await Task.FromResult(View());
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInInput signInInput)
        {
            if (ModelState.IsValid is false)
            {
                return View();
            }

            var response = await _identityService.SignIn(signInInput);

            if (response.IsSuccessful is false)
            {
                response.Errors.ForEach(e =>
                {
                    ModelState.AddModelError(string.Empty, e);
                });

                return View();
            }

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
