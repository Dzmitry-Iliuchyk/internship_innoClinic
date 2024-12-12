namespace Profiles.Application.Common.Exceptions {
    public class DoctorNotFoundException: NotFoundException {
        public DoctorNotFoundException( string doctorId )
            : base( $"The doctor with the identifier {doctorId} does not exist" ) {
        }
    }
}
