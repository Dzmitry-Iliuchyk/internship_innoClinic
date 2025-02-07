namespace Authorization.Api.Dto {
    public class UpdateEmailRequest {
        public required Guid Id { get; set; }
        public required string NewEmail{ get; set; }
    }
}