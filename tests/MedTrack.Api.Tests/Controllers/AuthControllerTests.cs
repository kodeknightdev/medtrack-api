using System.Threading.Tasks;
using MedTrack.Api.Controllers;
using MedTrack.Api.Data;
using MedTrack.Api.Dtos;
using MedTrack.Api.Models;
using MedTrack.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Xunit;

namespace MedTrack.Api.Tests.Controllers
{
    public class AuthControllerTests
    {
        private static AuthController CreateController(MedTrackContext context)
        {
            var settings = Options.Create(new JwtSettings
            {
                Issuer = "test",
                Audience = "test",
                Secret = "unit-test-secret-that-is-long-enough-123"
            });
            return new AuthController(context, new PasswordHasher(), new TokenService(settings));
        }

        [Fact]
        public async Task Register_ReturnsConflict_WhenEmailExists()
        {
            using var context = TestHelpers.CreateContext();
            context.Users.Add(new User { Email = "jane@example.com", PasswordHash = "x" });
            await context.SaveChangesAsync();

            var result = await CreateController(context).Register(new RegisterRequest
            {
                Email = "  Jane@Example.com ",
                Password = "password123"
            });

            Assert.IsType<ConflictObjectResult>(result.Result);
        }

        [Fact]
        public async Task Login_ReturnsToken_ForValidCredentials()
        {
            using var context = TestHelpers.CreateContext();
            var controller = CreateController(context);
            await controller.Register(new RegisterRequest { Email = "sam@example.com", Password = "password123" });

            var result = await controller.Login(new LoginRequest { Email = "sam@example.com", Password = "password123" });

            Assert.False(string.IsNullOrEmpty(result.Value.Token));
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_ForWrongPassword()
        {
            using var context = TestHelpers.CreateContext();
            var controller = CreateController(context);
            await controller.Register(new RegisterRequest { Email = "sam@example.com", Password = "password123" });

            var result = await controller.Login(new LoginRequest { Email = "sam@example.com", Password = "nope" });

            Assert.IsType<UnauthorizedResult>(result.Result);
        }
    }
}
