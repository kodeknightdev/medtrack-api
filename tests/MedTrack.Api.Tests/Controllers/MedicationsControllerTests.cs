using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Api.Controllers;
using MedTrack.Api.Dtos;
using MedTrack.Api.Models;
using Xunit;

namespace MedTrack.Api.Tests.Controllers
{
    public class MedicationsControllerTests
    {
        private static Medication Med(string name, TimeSpan? reminder, DateTime? lastTaken = null, bool active = true) =>
            new Medication
            {
                Name = name,
                UserId = 1,
                StartDate = DateTime.Today.AddDays(-10),
                Active = active,
                ReminderTime = reminder,
                LastTakenAt = lastTaken
            };

        [Fact]
        public async Task GetDue_SkipsMedicationsAlreadyTakenToday()
        {
            using var context = TestHelpers.CreateContext();
            context.Medications.AddRange(
                Med("Not taken", TimeSpan.Zero),
                Med("Taken", TimeSpan.Zero, DateTime.UtcNow),
                Med("Taken yesterday", TimeSpan.Zero, DateTime.UtcNow.AddDays(-1)));
            await context.SaveChangesAsync();

            var controller = new MedicationsController(context).WithUser(1);
            var result = await controller.GetDue();

            var names = result.Value.Select(m => m.Name).ToList();
            Assert.Contains("Not taken", names);
            Assert.Contains("Taken yesterday", names);
            Assert.DoesNotContain("Taken", names);
        }

        [Fact]
        public async Task GetDue_SkipsInactiveAndNoReminder()
        {
            using var context = TestHelpers.CreateContext();
            context.Medications.AddRange(
                Med("Inactive", TimeSpan.Zero, active: false),
                Med("No reminder", null));
            await context.SaveChangesAsync();

            var controller = new MedicationsController(context).WithUser(1);
            var result = await controller.GetDue();

            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task MarkTaken_SetsLastTakenAt()
        {
            using var context = TestHelpers.CreateContext();
            var med = Med("Ibuprofen", TimeSpan.FromHours(8));
            context.Medications.Add(med);
            await context.SaveChangesAsync();

            var controller = new MedicationsController(context).WithUser(1);
            await controller.MarkTaken(med.Id);

            Assert.NotNull(context.Medications.Single().LastTakenAt);
        }
    }
}
