using System;
using System.ComponentModel.DataAnnotations;

namespace MedTrack.Api.Dtos
{
    public class MedicationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Instructions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Active { get; set; }

        // "HH:mm" - System.Text.Json can't handle TimeSpan in 3.1
        public string ReminderTime { get; set; }
        public DateTime? LastTakenAt { get; set; }
    }

    public class SaveMedicationRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Dosage { get; set; }

        [StringLength(500)]
        public string Instructions { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Active { get; set; } = true;

        [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "Reminder time must be HH:mm.")]
        public string ReminderTime { get; set; }
    }
}
