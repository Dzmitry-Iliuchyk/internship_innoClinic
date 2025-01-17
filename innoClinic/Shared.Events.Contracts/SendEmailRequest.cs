namespace Shared.Events.Contracts {
    public record SendEmailRequest{
        public string NameFrom { get; set; }
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string TextContent { get; set; }
        public File? File { get; set; }
    }
    public record SendEmailResponse {
        public string Message {  get; set; }
    }
    public record File{
        public byte[] FileContent { get; set; }
        public string FileContentType { get; set; }
        public string FileName { get; set; }
    }
}
