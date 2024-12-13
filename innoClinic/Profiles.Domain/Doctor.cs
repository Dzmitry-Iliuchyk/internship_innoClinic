namespace Profiles.Domain {
    public class Doctor: Account {
        private Doctor() : base() { }
        public DateTime DateOfBirth { get; set; }
        public DateTime CareerStartYear { get; set; }
        public string OfficeId { get; set; }
        public DoctorStatuses Status { get; set; }
        public int SpecializationId { get; set; }
        public Specialization? Specialization { get; set; }

        public Doctor( Guid id,
                       DateTime dateOfBirth,
                       DateTime careerStartYear,
                       string officeId,
                       DoctorStatuses status,
                       int specializationId,
                       string firstName,
                       string lastName,
                       string email,
                       string phoneNumber,
                       bool isEmailVerified,
                       Guid createdBy,
                       DateTime createdAt,
                       Guid updatedBy, 
                       DateTime updatedAt,
                       string? photoUrl,
                       string? middleName) 
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
            this.CareerStartYear = careerStartYear;
            this.OfficeId = officeId;
            this.Status = status;
            this.SpecializationId = specializationId;
        }
      
    }
}
