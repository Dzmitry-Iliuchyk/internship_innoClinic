namespace Shared.Events.Contracts {
    public record AppointmentCreatedEvent {
        public Guid Id { get; init; }
        #region Doctor
        public Guid DoctorId { get; init; }
        public string DoctorFirstName { get; init; }
        public string DoctorSecondName { get; init; }
        public string DoctorSpecialization { get; init; }
        #endregion Doctor
        #region Service
        public int ServiceId { get; init; }
        public string ServiceName { get; init; }
        public decimal ServicePrice { get; init; }
        #endregion Service
        #region Office
        public string OfficeId { get; init; }
        public string OfficeAddress { get; init; }

        #endregion Office
        #region Patient
        public Guid PatientId { get; init; }
        public string PatientFirstName { get; init; }
        public string PatientSecondName { get; init; }
        public string? PatientEmail { get; init; }
        #endregion Patient

        public DateTime StartTime { get; init; }
        public TimeSpan Duration { get; init; }
    }
}

