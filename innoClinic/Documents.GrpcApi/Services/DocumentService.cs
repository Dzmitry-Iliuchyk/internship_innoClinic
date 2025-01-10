using Documents.Application;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Documents.GrpcApi.Services {
    [Authorize]
    public class DocumentService: GrpcApi.DocumentService.DocumentServiceBase {
        private readonly IBlobStorage _blobStorage;
        public DocumentService( IBlobStorage blobStorage ) {
            this._blobStorage = blobStorage;
        }

        public override async Task<DeleteBlobResponse> DeleteBlob( DeleteBlobRequest request, ServerCallContext context ) {
            var result = await _blobStorage.DeleteBlobAsync( request.PathToBlob );
            return new() { IsSuccess = result };
        }

        public override async Task<Blob> GetBlob( GetBlobRequest request, ServerCallContext context ) {
            return await ( await _blobStorage.GetBlobAsync( request.PathToBlob ) ).ToGrpcBlob();
        }

        [Authorize]
        public override async Task<BlobDetails> GetBlobDetails( GetBlobDetailsRequest request, ServerCallContext context ) {
            return ( await _blobStorage.GetBlobDetailsAsync( request.PathToBlob, context.CancellationToken ) ).ToGrpcDetails();
        }

        public override async Task<GetBlobsResponse> GetBlobs( GetBlobsRequest request, ServerCallContext context ) {
            return await ( await _blobStorage
                .GetBlobsByMetadataAsync( request.ContainerName,
                                          new KeyValuePair<string, string>(
                                              request.MetadataKey,
                                              request.MetadataValue
                                              ),
                                          context.CancellationToken ) )
                .ToGrpcBlobsResponce();
        }
        public override async Task<GetBlobsDetailsResponse> GetBlobsDetails( GetBlobsDetailsRequest request, ServerCallContext context ) {
            return ( await _blobStorage
                .GetBlobsDetailsAsync( request.ContainerName, context.CancellationToken ) )
                .ToGetBlobsDetailsResponse();
        }
        public override async Task<UploadResponse> UploadBlob( BlobUploadRequest request, ServerCallContext context ) {
            await _blobStorage.UploadBlobAsync( new MemoryStream( request.Content.ToByteArray() ), request.PathToBlob, context.CancellationToken );
            var uploadResponse = new UploadResponse() {
                IsSuccess = true
            };
            return await ValueTask.FromResult( uploadResponse );
        }
    }
}
