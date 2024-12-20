namespace Services.Application.Exceptions {
    public class ServiceNotFoundException: NotFoundException {
        public ServiceNotFoundException( Guid serviceId )
            : base( $"The service with the identifier {serviceId} does not exist" ) {
        }
        public ServiceNotFoundException( string name )
            : base( $"The service with the name {name} does not exist" ) {
        }
    }
}
