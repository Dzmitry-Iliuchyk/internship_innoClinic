using Profiles.Domain;

namespace Profiles.Application.Doctors.Queries {
    public class DoctorDto {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsEmailVerified { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CareerStartYear { get; set; }
        public string OfficeId { get; set; }
        public DoctorStatuses Status { get; set; }
        public string Specialization { get; set; }

    }
}
