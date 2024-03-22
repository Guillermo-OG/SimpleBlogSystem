using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleBlogSystem.Interfaces;
using SimpleBlogSystem.Models;
using SimpleBlogSystem.ViewModels;
using System.Threading.Tasks;

namespace SimpleBlogSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        // Suponiendo que tienes un servicio para generar tokens JWT
        private readonly ITokenService _tokenService;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(new User { UserName = model.Username }, model.Password);
                if (result.Succeeded)
                {
                    return Ok(new { message = "User registered successfully" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    var token = _tokenService.CreateToken(user);
                    return Ok(new { token = token });
                }
                if (result.IsLockedOut)
                {
                    return Unauthorized(new { message = "User account locked out." });
                }
                else
                {
                    return Unauthorized(new { message = "Username or password is incorrect" });
                }
            }
            return BadRequest(ModelState);
        }
    }
}
