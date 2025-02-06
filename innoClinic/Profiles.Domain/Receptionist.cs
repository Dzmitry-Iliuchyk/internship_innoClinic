namespace Profiles.Domain {
    public class Receptionist: Account {
        public string? OfficeId { get; set; }
        public Receptionist( Guid id,
                             string officeId,
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
            this.OfficeId = officeId;
        }

    }

}
