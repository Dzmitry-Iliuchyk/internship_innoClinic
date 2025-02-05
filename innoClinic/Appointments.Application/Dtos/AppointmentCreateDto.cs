using Appointments.Domain;

namespace Appointments.Application.Dtos {
    public class AppointmentCreateDto {
        public Guid DoctorId { get; set; }
        public int ServiceId { get; set; }
        public string OfficeId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}