using System;

namespace MedTrack.Api.Dtos
{
    public class TimelineItemDto
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
    }
}
