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
        public DbSet<Medication> Medications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SymptomEntry>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Notes).HasMaxLength(1000);
            });

            modelBuilder.Entity<Medication>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Dosage).HasMaxLength(50);
                entity.Property(e => e.Instructions).HasMaxLength(500);
            });
        }
    }
}
