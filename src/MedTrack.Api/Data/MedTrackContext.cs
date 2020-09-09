using MedTrack.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MedTrack.Api.Data
{
    public class MedTrackContext : DbContext
    {
        public MedTrackContext(DbContextOptions<MedTrackContext> options)
            : base(options)
        {
        }

        public DbSet<SymptomEntry> SymptomEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SymptomEntry>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Notes).HasMaxLength(1000);
            });
        }
    }
}
