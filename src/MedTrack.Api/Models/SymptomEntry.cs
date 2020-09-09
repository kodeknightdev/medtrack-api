using System;

namespace MedTrack.Api.Models
{
    public class SymptomEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Severity { get; set; }
        public string Notes { get; set; }
        public DateTime OccurredAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
