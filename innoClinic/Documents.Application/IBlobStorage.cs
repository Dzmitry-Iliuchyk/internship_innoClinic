using Documents.Domain;

namespace Documents.Application {
    public interface IBlobStorage {
        Task UploadBlobAsync( Stream stream, string pathToBlob, CancellationToken cancellationToken = default );
        Task<List<BlobDetails>> GetBlobsDetailsAsync( string containerName, CancellationToken cancellationToken = default );
        Task<bool> DeleteBlobAsync( string pathToBlob, CancellationToken cancellationToken = default );
        Task<Blob> GetBlobAsync( string pathToBlob, CancellationToken cancellationToken = default );
        Task<BlobDetails> GetBlobDetailsAsync( string pathToBlob, CancellationToken cancellationToken = default );
        Task<List<Blob>> GetBlobsByMetadataAsync( string containerName, KeyValuePair<string, string> metadata, CancellationToken cancellationToken = default );
    }
}
