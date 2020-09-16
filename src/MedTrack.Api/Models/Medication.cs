using System;

namespace MedTrack.Api.Models
{
    public class Medication
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Instructions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Active { get; set; } = true;
        
        // time of day the reminder should fire, local to the user
        public TimeSpan? ReminderTime { get; set; }
        public DateTime? LastTakenAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
