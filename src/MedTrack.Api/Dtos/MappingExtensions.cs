using System;
using System.Globalization;
using MedTrack.Api.Models;

namespace MedTrack.Api.Dtos
{
    public static class MappingExtensions
    {
        public static SymptomEntryDto ToDto(this SymptomEntry entry) => new SymptomEntryDto
        {
            Id = entry.Id,
            Name = entry.Name,
            Severity = entry.Severity,
            Notes = entry.Notes,
            OccurredAt = entry.OccurredAt,
            CreatedAt = entry.CreatedAt
        };

        public static void ApplyTo(this SaveSymptomEntryRequest request, SymptomEntry entry)
        {
            entry.Name = request.Name;
            entry.Severity = request.Severity;
            entry.Notes = request.Notes;
            entry.OccurredAt = request.OccurredAt;
        }

        public static MedicationDto ToDto(this Medication medication) => new MedicationDto
        {
            Id = medication.Id,
            Name = medication.Name,
            Dosage = medication.Dosage,
            Instructions = medication.Instructions,
            StartDate = medication.StartDate,
            EndDate = medication.EndDate,
            Active = medication.Active,
            ReminderTime = medication.ReminderTime?.ToString(@"hh\:mm"),
            LastTakenAt = medication.LastTakenAt
        };

        public static void ApplyTo(this SaveMedicationRequest request, Medication medication)
        {
            medication.Name = request.Name;
            medication.Dosage = request.Dosage;
            medication.Instructions = request.Instructions;
            medication.StartDate = request.StartDate;
            medication.EndDate = request.EndDate;
            medication.Active = request.Active;
            medication.ReminderTime = string.IsNullOrWhiteSpace(request.ReminderTime)
                ? (TimeSpan?)null
                : TimeSpan.ParseExact(request.ReminderTime, @"hh\:mm", CultureInfo.InvariantCulture);
        }
    }
}
