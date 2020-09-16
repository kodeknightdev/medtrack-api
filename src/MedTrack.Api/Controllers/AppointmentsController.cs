using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Api.Data;
using MedTrack.Api.Dtos;
using MedTrack.Api.Extensions;
using MedTrack.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly MedTrackContext _context;

        public AppointmentsController(MedTrackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAll()
        {
            var userId = User.GetUserId();
            return await _context.Appointments
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.StartsAt)
                .Select(x => x.ToDto())
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> Get(int id)
        {
            var userId = User.GetUserId();
            var appointment = await _context.Appointments.Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return appointment.ToDto();
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> Create(SaveAppointmentRequest request)
        {
            var appointment = new Appointment
            {
                UserId = userId
            };
            request.ApplyTo(appointment);

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = appointment.Id }, appointment.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SaveAppointmentRequest request)
        {
            var userId = User.GetUserId();
            var existing = await _context.Appointments.Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null)
            {
                return NotFound();
            }

            request.ApplyTo(existing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.GetUserId();
            var appointment = await _context.Appointments.Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetUpcoming(int days = 14)
        {
            var userId = User.GetUserId();
            var from = DateTime.UtcNow;
            var to = from.AddDays(days);

            return await _context.Appointments
                .Where(x => x.UserId == userId)
                .Where(a => a.StartsAt >= from && a.StartsAt <= to)
                .OrderBy(a => a.StartsAt)
                .Select(x => x.ToDto())
                .ToListAsync();
        }
    }
}
