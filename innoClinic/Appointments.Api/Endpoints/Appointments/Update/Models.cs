namespace Appointments.Update {
    internal sealed class UpdateAppointmentRequest {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorSecondName { get; set; }
        public string DoctorSpecialization { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
        public string OfficeId { get; set; }
        public string OfficeAddress { get; set; }
        public Guid PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientSecondName { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }

    }

}
