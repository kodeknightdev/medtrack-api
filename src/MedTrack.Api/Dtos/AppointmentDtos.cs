using System;

namespace MedTrack.Api.Dtos
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Provider { get; set; }
        public string Location { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public string Notes { get; set; }
    }

    public class SaveAppointmentRequest
    {
        public string Title { get; set; }

        public string Provider { get; set; }

        public string Location { get; set; }

        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }

        public string Notes { get; set; }
    }
}
