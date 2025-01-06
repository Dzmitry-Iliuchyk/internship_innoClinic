namespace Services.Application.Exceptions {
    public class ServiceCategoryNotFoundException: NotFoundException {
        public ServiceCategoryNotFoundException( int serviceCategoryId )
            : base( $"The service category with the identifier {serviceCategoryId} does not exist" ) {
        }
        public ServiceCategoryNotFoundException( string name )
            : base( $"The service category with the name {name} does not exist" ) {
        }
    }
}
