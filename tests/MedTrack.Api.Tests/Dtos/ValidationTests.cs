using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MedTrack.Api.Dtos;
using Xunit;

namespace MedTrack.Api.Tests.Dtos
{
    public class ValidationTests
    {
        private static List<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
            return results;
        }

        [Fact]
        public void Appointment_EndingBeforeStart_IsInvalid()
        {
            var start = new DateTime(2020, 8, 3, 14, 0, 0);
            var request = new SaveAppointmentRequest { Title = "GP", StartsAt = start, EndsAt = start.AddMinutes(-30) };

            var results = Validate(request);

            Assert.Contains(results, r => r.MemberNames.Contains(nameof(SaveAppointmentRequest.EndsAt)));
        }

        [Fact]
        public void Appointment_WithValidRange_IsValid()
        {
            var start = new DateTime(2020, 8, 3, 14, 0, 0);
            var request = new SaveAppointmentRequest { Title = "GP", StartsAt = start, EndsAt = start.AddMinutes(30) };

            Assert.Empty(Validate(request));
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(10, true)]
        [InlineData(11, false)]
        public void Symptom_SeverityRange(int severity, bool valid)
        {
            var request = new SaveSymptomEntryRequest { Name = "Cough", Severity = severity, OccurredAt = DateTime.UtcNow };

            Assert.Equal(valid, Validate(request).Count == 0);
        }

        [Theory]
        [InlineData("08:00", true)]
        [InlineData("23:59", true)]
        [InlineData("24:00", false)]
        [InlineData("8am", false)]
        public void Medication_ReminderTimeFormat(string time, bool valid)
        {
            var request = new SaveMedicationRequest { Name = "Vitamin D", ReminderTime = time };

            Assert.Equal(valid, Validate(request).Count == 0);
        }
    }
}
