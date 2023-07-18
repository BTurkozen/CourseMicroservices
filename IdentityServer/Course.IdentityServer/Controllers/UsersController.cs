using Course.IdentityServer.Dtos;
using Course.IdentityServer.Models;
using Course.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace Course.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupDto signupDto)
        {
            var user = new ApplicationUser()
            {
                UserName = signupDto.UserName,
                Email = signupDto.Email,
                City = signupDto.City,
            };

            var result = await _userManager.CreateAsync(user, signupDto.Password);

            if (result.Succeeded is false)
            {
                return BadRequest(Response<NoContent>.Fail(result.Errors.Select(e => e.Description).ToList(), 400));
            }

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userIdCliam = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub);

            if (userIdCliam is null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userIdCliam.Value);

            if (user is null)
            {
                return BadRequest();
            }

            return Ok(new { user.Id, user.UserName, user.Email, user.City });
        }
    }
}
