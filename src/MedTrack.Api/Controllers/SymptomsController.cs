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
    public class SymptomsController : ControllerBase
    {
        private readonly MedTrackContext _context;

        public SymptomsController(MedTrackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SymptomEntry>>> GetAll()
        {
            return await _context.SymptomEntries
                .OrderByDescending(x => x.OccurredAt)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SymptomEntry>> Get(int id)
        {
            var entry = await _context.SymptomEntries.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            return entry;
        }

        [HttpPost]
        public async Task<ActionResult<SymptomEntry>> Create(SymptomEntry entry)
        {
            entry.CreatedAt = DateTime.UtcNow;
            _context.SymptomEntries.Add(entry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = entry.Id }, entry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SymptomEntry entry)
        {
            if (id != entry.Id)
            {
                return BadRequest();
            }

            var existing = await _context.SymptomEntries.FindAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = entry.Name;
            existing.Severity = entry.Severity;
            existing.Notes = entry.Notes;
            existing.OccurredAt = entry.OccurredAt;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entry = await _context.SymptomEntries.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            _context.SymptomEntries.Remove(entry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
