﻿namespace FacadeApi.Results.Dtos {
    public sealed class ResultUpdateRequest {
        public Guid Id { get; set; }
        public string Complaints { get; set; }
        public string Conclusion { get; set; }
        public string? Recomendations { get; set; }
        public Guid AppointmentId { get; set; }
    }
}
