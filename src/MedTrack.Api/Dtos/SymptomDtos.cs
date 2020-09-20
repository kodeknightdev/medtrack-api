using System;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(1, 10)]
        public int Severity { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        public DateTime OccurredAt { get; set; }
    }
}
