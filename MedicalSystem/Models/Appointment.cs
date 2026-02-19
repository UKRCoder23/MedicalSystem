using System.Data;
using MedicalSystem.Enums;

namespace MedicalSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime TimeAppointment { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;
        public string? Comment { get; set; }
        public AppointmentStatus Status { get; set; }
        public MedicalRecord? MedicalRecord { get; set; }
    }
}