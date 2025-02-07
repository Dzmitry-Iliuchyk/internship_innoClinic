using FastEndpoints;


namespace Appointments.Get {
    public  class GetAppointmentRequest {
        public string AppointmentId { get; set; }
    }

    public sealed class GetAppointmentResponse {
        public Guid Id { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorSecondName { get; set; }
        public string DoctorSpecialization { get; set; }
        public string ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
        public string OfficeAddress { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientSecondName { get; set; }
        public string? PatientEmail { get; set; }

        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
