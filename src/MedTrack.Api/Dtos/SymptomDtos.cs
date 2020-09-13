using System;

namespace MedTrack.Api.Dtos
{
    public class SymptomEntryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Severity { get; set; }
        public string Notes { get; set; }
        public DateTime OccurredAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class SaveSymptomEntryRequest
    {
        public string Name { get; set; }

        public int Severity { get; set; }

        public string Notes { get; set; }

        public DateTime OccurredAt { get; set; }
    }
}
