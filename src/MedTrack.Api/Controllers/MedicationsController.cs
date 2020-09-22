using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedTrack.Api.Data;
using MedTrack.Api.Dtos;
using MedTrack.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Api.Controllers
{
    [Route("api/[controller]")]
    public class MedicationsController : ApiControllerBase
    {
        private readonly MedTrackContext _context;

        public MedicationsController(MedTrackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicationDto>>> GetAll()
        {
            return await _context.Medications
                .Where(x => x.UserId == CurrentUserId)
                .OrderBy(x => x.Name)
                .Select(x => x.ToDto())
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicationDto>> Get(int id)
        {
            var medication = await _context.Medications.Where(x => x.UserId == CurrentUserId).FirstOrDefaultAsync(x => x.Id == id);
            if (medication == null)
            {
                return NotFound();
            }

            return medication.ToDto();
        }

        [HttpPost]
        public async Task<ActionResult<MedicationDto>> Create(SaveMedicationRequest request)
        {
            var medication = new Medication
            {
                UserId = CurrentUserId
            };
            request.ApplyTo(medication);

            _context.Medications.Add(medication);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = medication.Id }, medication.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SaveMedicationRequest request)
        {
            var existing = await _context.Medications.Where(x => x.UserId == CurrentUserId).FirstOrDefaultAsync(x => x.Id == id);
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
            var medication = await _context.Medications.Where(x => x.UserId == CurrentUserId).FirstOrDefaultAsync(x => x.Id == id);
            if (medication == null)
            {
                return NotFound();
            }

            _context.Medications.Remove(medication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("due")]
        public async Task<ActionResult<IEnumerable<MedicationDto>>> GetDue()
        {
            var now = DateTime.Now;
            var candidates = await _context.Medications
                .Where(x => x.UserId == CurrentUserId)
                .Where(m => m.Active && m.ReminderTime != null)
                .ToListAsync();

            return candidates
                .Where(m => m.ReminderTime <= now.TimeOfDay
                    && (m.LastTakenAt == null || m.LastTakenAt.Value.ToLocalTime().Date < now.Date))
                .OrderBy(m => m.ReminderTime)
                .Select(x => x.ToDto())
                .ToList();
        }

        [HttpPost("{id}/taken")]
        public async Task<IActionResult> MarkTaken(int id)
        {
            var medication = await _context.Medications.Where(x => x.UserId == CurrentUserId).FirstOrDefaultAsync(x => x.Id == id);
            if (medication == null)
            {
                return NotFound();
            }

            medication.LastTakenAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
