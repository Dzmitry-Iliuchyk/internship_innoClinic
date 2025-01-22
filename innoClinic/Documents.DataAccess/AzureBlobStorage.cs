using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Documents.Application;
using Documents.Domain;


namespace Documents.DataAccess {
    public class AzureBlobStorage: IBlobStorage {
        private readonly BlobServiceClient _blobService;
        public AzureBlobStorage( BlobServiceClient blobService ) {
            this._blobService = blobService;
        }
        public async Task<bool> DeleteBlobAsync( string pathToBlob, CancellationToken cancellationToken = default ) {
            var (containerName, blobName) = GetParsedPath( pathToBlob );
            var blobClient = _blobService.GetBlobContainerClient( containerName );
            var result = await blobClient.DeleteBlobIfExistsAsync( blobName, cancellationToken: cancellationToken );
            return result.Value;
        }
        public async Task<Blob> GetBlobAsync( string pathToBlob, CancellationToken cancellationToken = default ) {
            var (containerName, blobName) = GetParsedPath( pathToBlob );
            var blobClient = _blobService.GetBlobContainerClient( containerName );
            var result = await blobClient.GetBlobClient( blobName ).DownloadAsync( cancellationToken );
            return new Blob {
                Content = result.Value.Content,
                Details = new BlobDetails {
                    Name = blobName,
                    ContentLength = result.Value.ContentLength,
                    ContentType = result.Value.ContentType,
                    CreatedAt = result.Value.Details.CreatedOn,
                    LastModified = result.Value.Details.LastModified,
                    Metadata = result.Value.Details.Metadata
                }
            };
        }
        public async Task<List<Blob>> GetBlobsByMetadataAsync( string containerName, KeyValuePair<string, string> metadata, CancellationToken cancellationToken = default ) {
            var blobClient = _blobService.GetBlobContainerClient( containerName );
            var blobs = blobClient.GetBlobsAsync( BlobTraits.All, cancellationToken: cancellationToken ).AsPages();

            List<Blob> blobWithRequiredMetadata = new List<Blob>();

            await foreach (Page<BlobItem> blobPage in blobs) {

                foreach (BlobItem blobItem in blobPage.Values) {

                    if (blobItem.Metadata.Contains( metadata )) {

                        var result = await blobClient.GetBlobClient( blobItem.Name ).DownloadAsync( cancellationToken );

                        blobWithRequiredMetadata.Add( new Blob {
                            Content = result.Value.Content,
                            Details = new BlobDetails {
                                Name = blobItem.Name,
                                ContentLength = result.Value.ContentLength,
                                ContentType = result.Value.ContentType,
                                CreatedAt = result.Value.Details.CreatedOn,
                                LastModified = result.Value.Details.LastModified,
                                Metadata = result.Value.Details.Metadata
                            }
                        } );
                    }
                }
            }
            return blobWithRequiredMetadata;
        }
        public async Task<List<Blob>> GetBlobsByPrefixAsync( string pathToBlobs, CancellationToken cancellationToken = default ) {
            var (containerName, prefix) = GetParsedPath( pathToBlobs );
            var blobClient = _blobService.GetBlobContainerClient( containerName );
            var pages = blobClient.GetBlobsAsync( BlobTraits.None, prefix: prefix, cancellationToken: cancellationToken ).AsPages();
            List<Blob> blobs = new List<Blob>();
           
            await foreach (Page<BlobItem> blobPage in pages) {

                foreach (BlobItem blobItem in blobPage.Values) {

                    var result = await blobClient.GetBlobClient( blobItem.Name ).DownloadAsync( cancellationToken );
                    blobs.Add( new Blob {
                        Content = result.Value.Content,
                        Details = new BlobDetails {
                            Name = blobItem.Name,
                            ContentLength = result.Value.ContentLength,
                            ContentType = result.Value.ContentType,
                            CreatedAt = result.Value.Details.CreatedOn,
                            LastModified = result.Value.Details.LastModified,
                            Metadata = result.Value.Details.Metadata
                        }
                    } );

                }
            }
            return blobs;
        }
        public async Task<BlobDetails> GetBlobDetailsAsync( string pathToBlob, CancellationToken cancellationToken = default ) {
            var (containerName, blobName) = GetParsedPath( pathToBlob );
            var blobClient = _blobService.GetBlobContainerClient( containerName );

            var result = await blobClient.GetBlobClient( blobName ).GetPropertiesAsync( cancellationToken: cancellationToken );

            return new BlobDetails {
                Name = blobName,
                ContentLength = result.Value.ContentLength,
                ContentType = result.Value.ContentType,
                CreatedAt = result.Value.CreatedOn,
                LastModified = result.Value.LastModified,
                Metadata = result.Value.Metadata
            };
        }
        public async Task<List<BlobDetails>> GetBlobsDetailsAsync( string containerName, CancellationToken cancellationToken = default ) {
            var blobClient = _blobService.GetBlobContainerClient( containerName );
            var result = blobClient.GetBlobsAsync( BlobTraits.Metadata, cancellationToken: cancellationToken ).AsPages();

            var list = new List<BlobDetails>();

            await foreach (Page<BlobItem> blobPage in result) {
                foreach (BlobItem blobItem in blobPage.Values) {
                    list.Add( new() {
                        Name = blobItem.Name,
                        ContentLength = blobItem.Properties.ContentLength,
                        ContentType = blobItem.Properties.ContentType,
                        CreatedAt = blobItem.Properties.CreatedOn,
                        LastModified = blobItem.Properties.LastModified,
                        Metadata = blobItem.Metadata
                    } );
                }
            }
            return list;
        }
        public async Task UploadBlobAsync( Stream stream, string pathToBlob, CancellationToken cancellationToken = default ) {
            var (containerName, blobName) = GetParsedPath( pathToBlob );
            var blobClient = _blobService.GetBlobContainerClient( containerName );
            var result = await blobClient.UploadBlobAsync( blobName, stream, cancellationToken );
        }



        private (string containerName, string pathToBlob) GetParsedPath( string path ) {
            var data = path.Split( ":" );
            return (data[ 0 ], data[ 1 ]);
        }
    }
}
