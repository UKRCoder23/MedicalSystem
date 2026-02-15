using System.Data;

namespace MedicalSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime TimeAppointment { get; set; }
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; } = null!;
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; } = null!;
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; } = null!;
        public string? Comment { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}