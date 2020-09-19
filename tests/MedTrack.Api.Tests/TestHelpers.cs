using System;
using System.Security.Claims;
using MedTrack.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Api.Tests
{
    internal static class TestHelpers
    {
        public static MedTrackContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<MedTrackContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new MedTrackContext(options);
        }

        public static T WithUser<T>(this T controller, int userId) where T : ControllerBase
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "Test");

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
            };

            return controller;
        }
    }
}
