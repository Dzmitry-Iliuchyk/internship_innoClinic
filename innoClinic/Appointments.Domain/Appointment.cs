using Shared.Abstractions.Entities;

namespace Appointments.Domain {
    public class Doctor: Entity<Guid> {
        public string DoctorFirstName { get; set; }
        public string DoctorSecondName { get; set; }
        public string DoctorSpecialization { get; set; }
    }
    public class Service: Entity<int> {
        public string ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
    }
    public class Office: Entity<string> {
        public string OfficeAddress { get; set; }
    }
    public class Patient: Entity<Guid> {
        public string PatientFirstName { get; set; }
        public string PatientSecondName { get; set; }
        public string? PatientEmail { get; set; }
    }
    public class Appointment: Entity<Guid> {

        public Guid DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public int ServiceId {  get; set; } 
        public Service? Service { get; set; }
        public string OfficeId { get; set; }
        public Office? Office { get; set; }
        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public ICollection<Result>? Results { get; set; }
    }

}
