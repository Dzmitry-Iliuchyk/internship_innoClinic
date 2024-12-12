namespace Profiles.Application.Common.Exceptions {
    public class PatientNotFoundException: NotFoundException {
        public PatientNotFoundException( string patientId )
            : base( $"The patient with the identifier {patientId} does not exist" ) {
        }
    }
}
