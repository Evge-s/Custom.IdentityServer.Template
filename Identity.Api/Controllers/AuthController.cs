using Identity.Models.DTO.Registeration.Requests;
using Identity.Models.DTO.Registeration.Responses;
using Identity.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IAuthService authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            this.logger = logger;
            this.authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegsterByEmailRequest request)
        {
            var isNewAccount = await authService.Register(request.Email, request.Password);
            if (!isNewAccount)
                return BadRequest("the user is already registered");

            return Ok("The user is successfully registered");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] RegsterByEmailRequest request)
        {
            var (jwtToken, refreshToken) = await authService.Login(request.Email, request.Password);
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
