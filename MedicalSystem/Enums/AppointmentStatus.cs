namespace MedicalSystem.Enums
{
    public enum AppointmentStatus
    {
         Pending = 0,
         Confirmed = 1,
         InProgress = 2,
         Completed = 3,
         CancelledByPatient = 4,
         CancelledByDoctor = 5,
         NoShow = 6,
         AwaitingPayment = 7
    }
}