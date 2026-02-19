using System.Reflection.Metadata;

namespace MedicalSystem.Models
{
    public class Patient : User
    {
       public DateTime BirthDate { get; set; }
       public ICollection<Appointment> PatientAppointments { get; set; } = new List<Appointment>();
    }
}