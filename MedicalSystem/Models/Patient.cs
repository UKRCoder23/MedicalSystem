using System.Reflection.Metadata;

namespace MedicalSystem.Models
{
    public class Patient : User
    {
       public DateTime BirthDate { get; set; }
       public virtual ICollection<Appointment> PatientAppointments { get; set; } = new List<Appointment>();
    }
}