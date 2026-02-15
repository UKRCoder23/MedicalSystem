using System.Reflection.Metadata;

namespace MedicalSystem.Models
{
    public class Doctor : User
    {
       public DoctorStatus DoctorStatus { get; set; }
       public int Cabinet { get; set; }
       public string Biography {get; set; }
       public double Rating { get; set; }
       public virtual ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();
       public virtual ICollection<Service> Services { get; set; } = new List<Service>();
       public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
       public virtual ICollection<Appointment> DoctorAppointments { get; set; } = new List<Appointment>();
    }
}