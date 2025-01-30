namespace Authorization.Api.Dto {
    public class UpdateRequest {
        public required Guid Id { get; set; }    
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}