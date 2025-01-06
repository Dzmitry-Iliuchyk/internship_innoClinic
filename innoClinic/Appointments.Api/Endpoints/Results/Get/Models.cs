using Appointments.Application.Dtos;
using Appointments.Domain;

namespace Result.Get {
    internal sealed class ResultGetRequest {
        public Guid Id { get; set; }
    }

    internal sealed class ResultGetResponse {
        public Guid Id { get; set; }
        public string Complaints { get; set; }
        public string Conclusion { get; set; }
        public string? Recomendations { get; set; }
        public string? DocumentUrl { get; set; }
        public AppointmentDto Appointment { get; set; }
    }
}
