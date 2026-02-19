using MedicalSystem.Models;
using MedicalSystem.Enums;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
           
        }

        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Specialization> Specializations { get; set; } = null!;
        public DbSet<Schedule> Schedules { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<MedicalRecord> MedicalRecords { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<Doctor>("Doctor")
                .HasValue<Patient>("Patient");

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.MedicalRecord)
                .WithOne(m => m.Appointment)
                .HasForeignKey<MedicalRecord>(m => m.AppointmentId);

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.DoctorId, a.TimeAppointment })
                .IsUnique();

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Specializations)
                .WithMany(s => s.Doctors);

            modelBuilder.Entity<Schedule>(entity =>
            {
               entity.Property(e => e.StartTime).HasColumnType("time");
               entity.Property(e => e.EndTime).HasColumnType("time");
            });

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Doctor)
                .WithMany(d => d.Schedules)
                .HasForeignKey(s => s.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}