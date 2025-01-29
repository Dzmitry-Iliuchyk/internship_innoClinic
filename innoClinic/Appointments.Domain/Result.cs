using Shared.Abstractions.Entities;

namespace Appointments.Domain {
    public class Result: Entity<Guid> {
        public string Complaints { get; set; }
        public string Conclusion { get; set; }
        public string? Recomendations { get; set; }
        public string? DocumentUrl { get; set; }
        public Guid AppointmentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Appointment? Appointment { get; set; }
    }
}
