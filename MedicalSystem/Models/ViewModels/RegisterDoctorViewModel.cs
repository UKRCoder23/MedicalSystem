using MedicalSystem.Enums;

namespace MedicalSystem.Models.ViewModels
{
    public class RegisterDoctorViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public Gender Gender { get; set; }
        public List<int> SelectedSpecializationIds { get; set; } = new();
    }
}