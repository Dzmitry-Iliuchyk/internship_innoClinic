namespace Shared.Events.Contracts {
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
