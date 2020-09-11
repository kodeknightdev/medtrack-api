using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Api.Data;
using MedTrack.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly MedTrackContext _context;

        public AppointmentsController(MedTrackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAll()
        {
            return await _context.Appointments
                .OrderBy(x => x.StartsAt)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> Get(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        [HttpPost]
        public async Task<ActionResult<Appointment>> Create(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = appointment.Id }, appointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return BadRequest();
            }

            var existing = await _context.Appointments.FindAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Title = appointment.Title;
            existing.Provider = appointment.Provider;
            existing.Location = appointment.Location;
            existing.StartsAt = appointment.StartsAt;
            existing.EndsAt = appointment.EndsAt;
            existing.Notes = appointment.Notes;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetUpcoming(int days = 14)
        {
            var from = DateTime.UtcNow;
            var to = from.AddDays(days);

            return await _context.Appointments
                .Where(a => a.StartsAt >= from && a.StartsAt <= to)
                .OrderBy(a => a.StartsAt)
                .ToListAsync();
        }
    }
}
