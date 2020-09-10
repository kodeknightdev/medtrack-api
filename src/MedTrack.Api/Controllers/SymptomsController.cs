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
    }
}
