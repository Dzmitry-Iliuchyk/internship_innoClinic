namespace Result.Create {
    internal sealed class ResultCreateRequest {
        public string Complaints { get; set; }
        public string Conclusion { get; set; }
        public string? Recomendations { get; set; }
        public string? DocumentUrl { get; set; }
        public Guid AppointmentId { get; set; }
    }

    internal sealed class ResultCreateResponse {
        public Guid Id { get; set; }
    }
}
