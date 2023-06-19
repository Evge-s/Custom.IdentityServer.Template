using Identity.Controllers;
using Identity.Models.CustomErrors;
using Identity.Models.DTO.Registeration.Requests;
using Identity.Models.DTO.Registeration.Responses;
using Identity.Services.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Identity.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> mockAuthService;
        private readonly Mock<ILogger<AuthController>> mocklogger;
        private readonly AuthController controller;

        public AuthControllerTests()
        {
            mockAuthService = new Mock<IAuthService>();
            mocklogger = new Mock<ILogger<AuthController>>();
            var httpContext = new DefaultHttpContext();
            controller = new AuthController(mocklogger.Object, mockAuthService.Object)
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
            mockAuthService
                .Setup(service => service.Register(request.Email, request.Password))
                .ReturnsAsync(true);

            // Act
            object result = new object();
            try
            {
                result = await controller.Register(request);
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
            mockAuthService
                .Setup(service => service.Register(request.Email, request.Password))
                .ReturnsAsync(false);

            // Act
            object result = new object();
            try
            {
                result = await controller.Register(request);
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
            var request = new RegsterByEmailRequest { Email = "test@test.com", Password = "password123" };
            var jwtToken = "fakeJwtToken";
            var refreshToken = "fakeRefreshToken";
            mockAuthService
                .Setup(service => service.Login(request.Email, request.Password))
                .ReturnsAsync((jwtToken, refreshToken));

            // Act
            var result = await controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<LoginResponse>(okResult.Value);

            Assert.Equal(jwtToken, returnValue.Token);
        }

        [Fact]
        public async Task Login_ShouldReturnNotFoundResult_WhenUserNotFound()
        {
            // Arrange
            var request = new RegsterByEmailRequest { Email = "test@test.com", Password = "password123" };
            mockAuthService
                .Setup(service => service.Login(request.Email, request.Password))
                .ThrowsAsync(new UserNotFoundException("User not found"));

            // Act
            object result = new object();
            try
            {
                result = await controller.Login(request);
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
            var request = new RegsterByEmailRequest { Email = "test@test.com", Password = "password123" };
            mockAuthService
                .Setup(service => service.Login(request.Email, request.Password))
                .ThrowsAsync(new InvalidPasswordException("Invalid password"));

            // Act
            object result = new object();
            try
            {
                result = await controller.Login(request);
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
