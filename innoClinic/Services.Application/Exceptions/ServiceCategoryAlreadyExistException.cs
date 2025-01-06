namespace Services.Application.Exceptions {
    public class ServiceCategoryAlreadyExistException: BadRequestException {
        public ServiceCategoryAlreadyExistException( int serviceCategoryId )
            : base( $"The service category with the identifier {serviceCategoryId} already exists" ) {
        }
        public ServiceCategoryAlreadyExistException( string name )
            : base( $"The service category with the name {name} already exists" ) {
        }
    }
}
