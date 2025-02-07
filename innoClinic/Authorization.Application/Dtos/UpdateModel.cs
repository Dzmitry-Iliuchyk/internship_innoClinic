namespace Authorization.Application.Dtos {
    public class UpdatePasswordModel {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UpdateEmailModel {
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
