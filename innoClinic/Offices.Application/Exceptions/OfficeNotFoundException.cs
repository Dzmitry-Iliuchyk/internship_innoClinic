namespace Offices.Application.Exceptions {
    public class OfficeNotFoundException: NotFoundException {
        public OfficeNotFoundException( string officeId )
            : base( $"The office with the identifier {officeId} does not exist" ) {
        }
    }
}
