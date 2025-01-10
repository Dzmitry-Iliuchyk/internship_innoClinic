using Shared.Abstractions.Entities;

namespace Appointments.Domain {
    public class Appointment: Entity<Guid> {
        #region Doctor
        public Guid DoctorId { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorSecondName { get; set; }
        public string DoctorSpecialization { get; set; }
        #endregion Doctor
        #region Service
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
        #endregion Service
        #region Office
        public string OfficeId { get; set; }
        public string OfficeAddress { get; set; }

        #endregion Office
        #region Patient
        public Guid PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientSecondName { get; set; }
        #endregion Patient

        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public ICollection<Result> Results { get; set; }
    }

}
