namespace Profiles.Application.Receptionists.Queries {
    public record ReceptionistDto( Guid id,
                             string officeId,
                             string firstName,
                             string lastName,
                             string email,
                             string phoneNumber,
                             bool isEmailVerified,
                             string? photoUrl,
                             string? middleName );
}
