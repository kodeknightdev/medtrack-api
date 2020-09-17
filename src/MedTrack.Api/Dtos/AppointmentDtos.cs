using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

    public class SaveAppointmentRequest : IValidatableObject
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [StringLength(100)]
        public string Provider { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndsAt <= StartsAt)
            {
                yield return new ValidationResult(
                    "Appointment must end after it starts.",
                    new[] { nameof(EndsAt) });
            }
        }
    }
}
