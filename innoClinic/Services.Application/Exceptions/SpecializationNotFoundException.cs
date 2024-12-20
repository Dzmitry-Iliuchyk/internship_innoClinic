namespace Services.Application.Exceptions {
    public class SpecializationNotFoundException: NotFoundException {
        public SpecializationNotFoundException( int specializationId )
            : base( $"The specialization with the identifier {specializationId} does not exist" ) {
        }
        public SpecializationNotFoundException( string name )
            : base( $"The specialization with the name {name} does not exist" ) {
        }
    }
}
