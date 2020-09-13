using System;

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
        public string Name { get; set; }

        public string Dosage { get; set; }

        public string Instructions { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Active { get; set; } = true;

        public string ReminderTime { get; set; }
    }
}
