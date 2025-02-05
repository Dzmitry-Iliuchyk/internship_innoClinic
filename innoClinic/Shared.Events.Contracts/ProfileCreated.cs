
namespace Shared.Events.Contracts {
    public record DoctorCreated {
        public Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string SecondName { get; init; }
        public required string Email { get; init; }
        public required string Specialization { get; init; }
    }
    public record ReceptionistCreated {
        public Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string SecondName { get; init; }
        public required string Email { get; init; }
    }
    public record PatientCreated {
        public Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string SecondName { get; init; }
        public required string Email { get; init; }
    }
    public record DoctorUpdated {
        public Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string SecondName { get; init; }
        public required string Email { get; init; }
        public required string Specialization { get; init; }
    }
    public record PatientUpdated {
        public Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string SecondName { get; init; }
        public required string Email { get; init; }
    }
    public record ReceptionistUpdated {
        public Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string SecondName { get; init; }
        public required string Email { get; init; }
    }
    public record PatientDeleted {
        public Guid Id { get; init; }
    }
    public record DoctorDeleted {
        public Guid Id { get; init; }
    }
    public record ReceptionistDeleted {
        public Guid Id { get; init; }
    }
    public enum Roles {
        Admin = 1,
        Patient = 2,
        Doctor = 3,
        Receptionist = 4
    }
}