using Shared.Abstractions.Entities;

namespace Profiles.Domain {
    public abstract class Account: Entity<Guid> {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsEmailVerified { get; set; }
        public string? PhotoUrl { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        private protected Account() { }
        public Account( Guid id,
                        string firstName,
                        string lastName,
                        string email,
                        string phoneNumber,
                        bool isEmailVerified,
                        Guid? createdBy,
                        DateTime createdAt,
                        Guid? updatedBy,
                        DateTime updatedAt,
                        string? photoUrl,
                        string? middleName ) {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.IsEmailVerified = isEmailVerified;
            this.CreatedBy = createdBy;
            this.CreatedAt = createdAt;
            this.UpdatedBy = updatedBy;
            this.UpdatedAt = updatedAt;
            this.PhotoUrl = photoUrl;
            this.MiddleName = middleName;
        }

    }
}
