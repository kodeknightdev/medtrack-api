using Microsoft.EntityFrameworkCore;

namespace MedTrack.Api.Data
{
    public class MedTrackContext : DbContext
    {
        public MedTrackContext(DbContextOptions<MedTrackContext> options)
            : base(options)
        {
        }
    }
}
