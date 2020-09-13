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
    }
}
