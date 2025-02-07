namespace Offices.Application.Exceptions {
    public class OfficeNotFoundException: NotFoundException {
        public OfficeNotFoundException( string officeId )
            : base( $"The office with the identifier {officeId} does not exist" ) {
        }
    } 
    public class OfficeAlreadyExistException: BadRequestException {
        public OfficeAlreadyExistException( string phone )
            : base( $"The office with the phone number {phone} already exist" ) {
        }
    }
    public class PhoneAlreadyExistException: BadRequestException {

        public PhoneAlreadyExistException( string message )
            : base( message ) {
        }
    }
}
