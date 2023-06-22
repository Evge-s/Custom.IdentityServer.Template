using Identity.Api.Models.DTO.EmailConfirmation.Requests;
using Identity.Api.Models.DTO.Registeration.Requests;
using Identity.Api.Models.DTO.Registeration.Responses;
using Identity.Api.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
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
        [HttpPost("send-confirmation-code")]
        public async Task<ActionResult> SendConfirmCode([FromBody] SendConfirmationRequest request)
        {
            var isNewAccount = await _authService.UserExist(request.Email);
            if (!isNewAccount)
                return BadRequest("the user is already registered");

            var sendConfirmationResult = await _authService.SendConfirmationMail(request.Email);

            if (!sendConfirmationResult)
            {
                // return internal error
                return BadRequest("internal error");
            }

            return Ok("A confirmation code has been sent to the specified email");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegsterByEmailRequest request)
        {
            var registerResult = await _authService.RegisterByEmail(request.Email, request.Password);
            
            if (!registerResult)
                return BadRequest("The mail is not confirmed, or the confirmation is overdue");
            
            return Ok("The user is successfully registered");
        }

        [AllowAnonymous]
        [HttpPost("confirm-email")]
        public async Task<ActionResult> TryConfirmEmailByCode([FromQuery] ConfirmEmailRequest request)
        {
            var isConfirmed = await _authService.ConfirmEmail(request.Email, request.ConfirmationCode);
            if (!isConfirmed)
                return BadRequest("Invalid or expired code");

            return Ok("The email is successfully confirmed");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] RegsterByEmailRequest request)
        {
            var (jwtToken, refreshToken) = await _authService.LoginByEmail(request.Email, request.Password);
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