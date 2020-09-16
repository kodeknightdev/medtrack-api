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
    public class SymptomsController : ControllerBase
    {
        private readonly MedTrackContext _context;

        public SymptomsController(MedTrackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SymptomEntryDto>>> GetAll()
        {
            var userId = User.GetUserId();
            return await _context.SymptomEntries
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OccurredAt)
                .Select(x => x.ToDto())
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SymptomEntryDto>> Get(int id)
        {
            var userId = User.GetUserId();
            var entry = await _context.SymptomEntries.Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.Id == id);
            if (entry == null)
            {
                return NotFound();
            }

            return entry.ToDto();
        }

        [HttpPost]
        public async Task<ActionResult<SymptomEntryDto>> Create(SaveSymptomEntryRequest request)
        {
            var entry = new SymptomEntry
            {
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };
            request.ApplyTo(entry);

            _context.SymptomEntries.Add(entry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = entry.Id }, entry.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SaveSymptomEntryRequest request)
        {
            var userId = User.GetUserId();
            var existing = await _context.SymptomEntries.Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.Id == id);
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
            var entry = await _context.SymptomEntries.Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.Id == id);
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
