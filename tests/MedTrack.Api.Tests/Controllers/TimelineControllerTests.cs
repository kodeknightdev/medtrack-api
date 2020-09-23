using System;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Api.Controllers;
using MedTrack.Api.Models;
using Xunit;

namespace MedTrack.Api.Tests.Controllers
{
    public class TimelineControllerTests
    {
        [Fact]
        public async Task Get_MergesAllTypesNewestFirst()
        {
            using var context = TestHelpers.CreateContext();
            var now = DateTime.UtcNow;
            context.SymptomEntries.Add(new SymptomEntry { Name = "Migraine", Severity = 6, UserId = 1, OccurredAt = now.AddDays(-2) });
            context.Appointments.Add(new Appointment { Title = "Neurologist", UserId = 1, StartsAt = now.AddDays(3), EndsAt = now.AddDays(3).AddHours(1) });
            context.Medications.Add(new Medication { Name = "Sumatriptan", UserId = 1, StartDate = now.AddDays(-1) });
            context.SymptomEntries.Add(new SymptomEntry { Name = "Other user", Severity = 1, UserId = 2, OccurredAt = now });
            await context.SaveChangesAsync();

            var controller = new TimelineController(context).WithUser(1);
            var result = await controller.Get();

            var items = result.Value.ToList();
            Assert.Equal(new[] { "appointment", "medication", "symptom" }, items.Select(i => i.Type));
        }

        [Fact]
        public async Task Get_RespectsDateRange()
        {
            using var context = TestHelpers.CreateContext();
            context.SymptomEntries.Add(new SymptomEntry { Name = "Old", Severity = 2, UserId = 1, OccurredAt = new DateTime(2020, 1, 1) });
            context.SymptomEntries.Add(new SymptomEntry { Name = "In range", Severity = 2, UserId = 1, OccurredAt = new DateTime(2020, 7, 1) });
            await context.SaveChangesAsync();

            var controller = new TimelineController(context).WithUser(1);
            var result = await controller.Get(new DateTime(2020, 6, 1), new DateTime(2020, 8, 1));

            Assert.Equal("In range", Assert.Single(result.Value).Title);
        }
    }
}
