using System.Reflection.Metadata;
using MedicalSystem.Enums;

namespace MedicalSystem.Models
{
    public class Doctor : User
    {
       public byte[]? Photo { get; set; }
       public DoctorStatus DoctorStatus { get; set; }
       public bool IsApproved { get; set; }
       public int Cabinet { get; set; }
       public string Biography {get; set; }
       public double Rating { get; set; }
       public ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();
       public ICollection<Service> Services { get; set; } = new List<Service>();
       public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
       public ICollection<Appointment> DoctorAppointments { get; set; } = new List<Appointment>();
    }
}