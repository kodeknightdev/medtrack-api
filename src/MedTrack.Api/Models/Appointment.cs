using System;

namespace MedTrack.Api.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Provider { get; set; }
        public string Location { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public string Notes { get; set; }
    }
}
