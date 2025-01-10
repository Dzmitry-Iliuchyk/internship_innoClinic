namespace Services.Application.Exceptions {
    public class ServiceAlreadyExistException: BadRequestException {
        public ServiceAlreadyExistException( int serviceCategoryId )
            : base( $"The service with the identifier {serviceCategoryId} already exists" ) {
        }
        public ServiceAlreadyExistException( string name )
            : base( $"The service with the name {name} already exists" ) {
        }
    }
}
