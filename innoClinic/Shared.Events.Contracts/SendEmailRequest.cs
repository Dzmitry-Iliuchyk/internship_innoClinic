namespace Shared.Events.Contracts {
    public record SendEmailRequest{
        public string NameFrom { get; set; }
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
    public record SendEmailResponse {
        public string Message {  get; set; }
    }
}
