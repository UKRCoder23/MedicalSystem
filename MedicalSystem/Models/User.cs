using MedicalSystem.Enums;

namespace MedicalSystem.Models
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Email {get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Gender UserGender { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string HashPassword { get ; set; } = string.Empty;
    }
}