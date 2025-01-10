namespace Documents.Domain {
    public class BlobDetails {
        public string Name { get; set; }
        public IDictionary<string, string>? Metadata { get; set; }
        public long? ContentLength { get; set; }
        public string ContentType { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }


    }
    public class Blob {
        public BlobDetails? Details { get; set; }
        public Stream? Content { get; set; }
    }
}
