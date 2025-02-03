namespace Profiles.Application.Common.Exceptions {
    public class SpecializationNotFoundException: NotFoundException {
        public SpecializationNotFoundException( string specialization )
            : base( $"The Specialization with the name {specialization} does not exist" ) {
        }
    }
}
