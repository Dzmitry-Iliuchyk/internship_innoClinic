using Appointments.Domain;

namespace Appointments.Application.Dtos {
    public class ResultFullDto {
        public Guid Id { get; set; }
        public string Complaints { get; set; }
        public string Conclusion { get; set; }
        public string? Recomendations { get; set; }
        public string? DocumentUrl { get; set; }
        public Appointment Appointment { get; set; }
    }
}