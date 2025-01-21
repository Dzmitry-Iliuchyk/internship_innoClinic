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

    public record SaveDocumentRequest {
        public string Name { get; set; }
        public IDictionary<string, string>? Metadata { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
    public record GetDocumentRequest {
        public string? Name { get; set; }
        public IDictionary<string, string>? Metadata { get; set; }
    }
    public class BlobDetails {
        public string Name { get; set; }
        public IDictionary<string, string>? Metadata { get; set; }
        public long? ContentLength { get; set; }
        public string ContentType { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }

}
