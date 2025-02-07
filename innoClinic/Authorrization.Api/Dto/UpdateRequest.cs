namespace Authorization.Api.Dto {
    public class UpdatePasswordRequest {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}