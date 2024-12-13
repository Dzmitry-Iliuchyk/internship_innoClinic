namespace Profiles.Application.Common.Exceptions {
    public class ReceptionistNotFoundException: NotFoundException {
        public ReceptionistNotFoundException( string receptionistId )
            : base( $"The receptionist with the identifier {receptionistId} does not exist" ) {
        }
    }
}
