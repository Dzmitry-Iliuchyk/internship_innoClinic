using Shared.Abstractions.Entities;

namespace Profiles.Domain {
    public class Patient: Account {
        public DateTime DateOfBirth { get; set; }
        
        public Patient( Guid id,
                        DateTime dateOfBirth,
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
                        string? middleName ) 
                                : base( id,
                                        firstName,
                                        lastName,
                                        email, 
                                        phoneNumber,
                                        isEmailVerified,
                                        createdBy,
                                        createdAt, 
                                        updatedBy,
                                        updatedAt, 
                                        photoUrl,
                                        middleName ) {
            this.DateOfBirth = dateOfBirth;
        }

    }
}
