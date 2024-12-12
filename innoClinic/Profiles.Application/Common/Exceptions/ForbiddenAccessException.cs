namespace Profiles.Application.Common.Exceptions {
    public class ForbiddenAccessException: Exception {
        public ForbiddenAccessException() : base(message: "The user cannot pass the requirements for this function") { }
    }
}
