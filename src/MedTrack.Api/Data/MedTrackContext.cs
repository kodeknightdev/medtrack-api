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
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }

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

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.Property(e => e.Title).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Provider).HasMaxLength(100);
                entity.Property(e => e.Location).HasMaxLength(200);
                entity.Property(e => e.Notes).HasMaxLength(1000);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}
