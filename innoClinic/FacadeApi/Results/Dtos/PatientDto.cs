namespace FacadeApi.Results.Dtos {
    public record PatientDto(
                    Guid id,
                    DateTime dateOfBirth,
                    string firstName,
                    string lastName,
                    string email,
                    string phoneNumber,
                    bool isEmailVerified,
                    string? photoUrl,
                    string? middleName
);
}
