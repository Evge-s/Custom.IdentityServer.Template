using Identity.Api.Controllers;
using Identity.Api.Models.CustomErrors;
using Identity.Api.Models.DTO.Login.Request;
using Identity.Api.Models.DTO.Registeration.Requests;
using Identity.Api.Models.DTO.Registeration.Responses;
using Identity.Api.Services.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Identity.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            Mock<ILogger<AuthController>> mocklogger = new();
            var httpContext = new DefaultHttpContext();
            _controller = new AuthController(mocklogger.Object, _mockAuthService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public async Task Register_ShouldReturnOkResult_WhenUserIsNew()
        {
            // Arrange
            var request = new RegsterByEmailRequest { Email = "test11@test.com", Password = "Password!123" };
            _mockAuthService
                .Setup(service => service.RegisterByEmail(request.Email, request.Password))
                .ReturnsAsync(true);

            // Act
            object result;
            try
            {
                result = await _controller.Register(request);
            }
            catch (Exception ex)
            {
                result = ex;
            }

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("The user is successfully registered", okResult.Value);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenUserExist()
        {
            // Arrange
            var request = new RegsterByEmailRequest { Email = "test11@test.com", Password = "Password!123" };
            _mockAuthService
                .Setup(service => service.RegisterByEmail(request.Email, request.Password))
                .ReturnsAsync(false);

            // Act
            object result;
            try
            {
                result = await _controller.Register(request);
            }
            catch (Exception ex)
            {
                result = ex;
            }

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("the user is already registered", okResult.Value);
        }


        [Fact]
        public async Task Login_ShouldReturnOkResult_WhenUserExists()
        {
            // Arrange
            var request = new LoginRequest { Email = "test@test.com", Password = "password123" };
            var jwtToken = "fakeJwtToken";
            var refreshToken = "fakeRefreshToken";
            _mockAuthService
                .Setup(service => service.LoginByEmail(request.Email, request.Password))
                .ReturnsAsync((jwtToken, refreshToken));

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<LoginResponse>(okResult.Value);

            Assert.Equal(jwtToken, returnValue.Token);
        }

        [Fact]
        public async Task Login_ShouldReturnNotFoundResult_WhenUserNotFound()
        {
            // Arrange
            var request = new LoginRequest { Email = "test@test.com", Password = "password123" };
            _mockAuthService
                .Setup(service => service.LoginByEmail(request.Email, request.Password))
                .ThrowsAsync(new UserNotFoundException("User not found"));

            // Act
            object result;
            try
            {
                result = await _controller.Login(request);
            }
            catch (UserNotFoundException ex)
            {
                result = ex;
            }
            catch (Exception ex)
            {
                result = ex;
            }

            // Assert
            var notFoundResult = Assert.IsType<UserNotFoundException>(result);
            Assert.Equal("User not found", notFoundResult.Message);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequestResult_WhenPasswordIsInvalid()
        {
            // Arrange
            var request = new LoginRequest { Email = "test@test.com", Password = "password123" };
            _mockAuthService
                .Setup(service => service.LoginByEmail(request.Email, request.Password))
                .ThrowsAsync(new InvalidPasswordException("Invalid password"));

            // Act
            object result;
            try
            {
                result = await _controller.Login(request);
            }
            catch (InvalidPasswordException ex)
            {
                result = ex;
            }
            catch (Exception ex)
            {
                result = ex;
            }

            // Assert
            var badRequestResult = Assert.IsType<InvalidPasswordException>(result);
            Assert.Equal("Invalid password", badRequestResult.Message);
        }
    }
}
