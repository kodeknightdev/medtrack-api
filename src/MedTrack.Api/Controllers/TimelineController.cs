using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Api.Data;
using MedTrack.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Api.Controllers
{
    [Route("api/timeline")]
    public class TimelineController : ApiControllerBase
    {
        private readonly MedTrackContext _context;

        public TimelineController(MedTrackContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Symptoms, appointments and medication start dates merged into a single list, newest first.
        /// Defaults to the 90 days before today plus the next 30.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimelineItemDto>>> Get(DateTime? from = null, DateTime? to = null)
        {
            var userId = CurrentUserId;
            var end = to ?? DateTime.UtcNow.AddDays(30);
            var start = from ?? DateTime.UtcNow.AddDays(-90);

            var symptoms = await _context.SymptomEntries
                .Where(s => s.UserId == userId && s.OccurredAt >= start && s.OccurredAt <= end)
                .Select(s => new TimelineItemDto
                {
                    Type = "symptom",
                    Id = s.Id,
                    Date = s.OccurredAt,
                    Title = s.Name,
                    Detail = "Severity " + s.Severity + "/10"
                })
                .ToListAsync();

            var appointments = await _context.Appointments
                .Where(a => a.UserId == userId && a.StartsAt >= start && a.StartsAt <= end)
                .Select(a => new TimelineItemDto
                {
                    Type = "appointment",
                    Id = a.Id,
                    Date = a.StartsAt,
                    Title = a.Title,
                    Detail = a.Provider
                })
                .ToListAsync();

            var medications = await _context.Medications
                .Where(m => m.UserId == userId && m.StartDate >= start && m.StartDate <= end)
                .Select(m => new TimelineItemDto
                {
                    Type = "medication",
                    Id = m.Id,
                    Date = m.StartDate,
                    Title = "Started " + m.Name,
                    Detail = m.Dosage
                })
                .ToListAsync();

            return symptoms
                .Concat(appointments)
                .Concat(medications)
                .OrderByDescending(i => i.Date)
                .ThenBy(i => i.Type)
                .ThenBy(i => i.Id)
                .ToList();
        }
    }
}
