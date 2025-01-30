namespace Authorization.Application.Dtos {
    public class UpdateModel {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
