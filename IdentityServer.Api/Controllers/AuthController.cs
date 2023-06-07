using IdentityServer.Api.Models.DTO.Registeration.Requests;
using IdentityServer.Api.Models.DTO.Registeration.Responses;
using IdentityServer.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegsterByEmailRequest request)
        {
            var isNewAccount = await _authService.Register(request.Email, request.Password);
            if (!isNewAccount)
                return BadRequest("the user is already registered");

            return Ok("The user is successfully registered");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] RegsterByEmailRequest request)
        {
            var (jwtToken, refreshToken) = await _authService.Login(request.Email, request.Password);
            var response = new LoginResponse { Token = jwtToken };

            Response.Cookies.Append(
                "RefreshToken",
                refreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true
                });

            return Ok(response);
        }
    }
}
