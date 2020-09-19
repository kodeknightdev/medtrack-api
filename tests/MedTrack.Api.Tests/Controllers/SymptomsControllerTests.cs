using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Api.Controllers;
using MedTrack.Api.Dtos;
using MedTrack.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MedTrack.Api.Tests.Controllers
{
    public class SymptomsControllerTests
    {
        [Fact]
        public async Task Create_AssignsCurrentUser()
        {
            using var context = TestHelpers.CreateContext();
            var controller = new SymptomsController(context).WithUser(7);

            var result = await controller.Create(new SaveSymptomEntryRequest
            {
                Name = "Headache",
                Severity = 4,
                OccurredAt = new DateTime(2020, 7, 1, 9, 0, 0)
            });

            Assert.IsType<CreatedAtActionResult>(result.Result);
            var saved = context.SymptomEntries.Single();
            Assert.Equal(7, saved.UserId);
            Assert.Equal("Headache", saved.Name);
        }

        [Fact]
        public async Task GetAll_OnlyReturnsCurrentUsersEntries()
        {
            using var context = TestHelpers.CreateContext();
            context.SymptomEntries.AddRange(
                new SymptomEntry { Name = "Mine", Severity = 2, UserId = 1, OccurredAt = DateTime.UtcNow },
                new SymptomEntry { Name = "Theirs", Severity = 5, UserId = 2, OccurredAt = DateTime.UtcNow });
            await context.SaveChangesAsync();

            var controller = new SymptomsController(context).WithUser(1);
            var result = await controller.GetAll();

            var items = Assert.IsAssignableFrom<IEnumerable<SymptomEntryDto>>(result.Value);
            Assert.Equal("Mine", Assert.Single(items).Name);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_ForAnotherUsersEntry()
        {
            using var context = TestHelpers.CreateContext();
            var entry = new SymptomEntry { Name = "Theirs", Severity = 5, UserId = 2, OccurredAt = DateTime.UtcNow };
            context.SymptomEntries.Add(entry);
            await context.SaveChangesAsync();

            var controller = new SymptomsController(context).WithUser(1);
            var result = await controller.Get(entry.Id);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Delete_RemovesEntry()
        {
            using var context = TestHelpers.CreateContext();
            var entry = new SymptomEntry { Name = "Nausea", Severity = 3, UserId = 1, OccurredAt = DateTime.UtcNow };
            context.SymptomEntries.Add(entry);
            await context.SaveChangesAsync();

            var controller = new SymptomsController(context).WithUser(1);
            var result = await controller.Delete(entry.Id);

            Assert.IsType<NoContentResult>(result);
            Assert.Empty(context.SymptomEntries);
        }
    }
}
