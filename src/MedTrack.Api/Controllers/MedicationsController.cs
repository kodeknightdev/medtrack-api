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
    public class MedicationsController : ControllerBase
    {
        private readonly MedTrackContext _context;

        public MedicationsController(MedTrackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medication>>> GetAll()
        {
            return await _context.Medications
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medication>> Get(int id)
        {
            var medication = await _context.Medications.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }

            return medication;
        }

        [HttpPost]
        public async Task<ActionResult<Medication>> Create(Medication medication)
        {
            _context.Medications.Add(medication);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = medication.Id }, medication);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Medication medication)
        {
            if (id != medication.Id)
            {
                return BadRequest();
            }

            var existing = await _context.Medications.FindAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = medication.Name;
            existing.Dosage = medication.Dosage;
            existing.Instructions = medication.Instructions;
            existing.StartDate = medication.StartDate;
            existing.EndDate = medication.EndDate;
            existing.Active = medication.Active;
            existing.ReminderTime = medication.ReminderTime;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var medication = await _context.Medications.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }

            _context.Medications.Remove(medication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("due")]
        public async Task<ActionResult<IEnumerable<Medication>>> GetDue()
        {
            var now = DateTime.Now;
            var candidates = await _context.Medications
                .Where(m => m.Active && m.ReminderTime != null)
                .ToListAsync();

            return candidates
                .Where(m => m.ReminderTime <= now.TimeOfDay
                    && (m.LastTakenAt == null || m.LastTakenAt.Value.Date < now.Date))
                .OrderBy(m => m.ReminderTime)
                .ToList();
        }

        [HttpPost("{id}/taken")]
        public async Task<IActionResult> MarkTaken(int id)
        {
            var medication = await _context.Medications.FindAsync(id);
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
