﻿namespace Result.Update {
    internal sealed class ResultUpdateRequest {
        public Guid Id { get; set; }
        public string Complaints { get; set; }
        public string Conclusion { get; set; }
        public string? Recomendations { get; set; }
        public string? DocumentUrl { get; set; }
        public Guid AppointmentId { get; set; }
    }
}
