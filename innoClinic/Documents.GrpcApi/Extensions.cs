

using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;

namespace Documents.GrpcApi {
    public static class Extensions {
        public static async Task<GrpcApi.Blob?> ToGrpcBlob( this Domain.Blob? blob ) {
            if (blob==null) {
                return null;
            }
            return new GrpcApi.Blob() {
                Content = await Google.Protobuf.ByteString.FromStreamAsync( blob?.Content ),
                Details = blob?.Details.ToGrpcDetails()
            };
        }
        public static async Task<GetBlobsResponse> ToGrpcBlobsResponce( this List<Domain.Blob> blobs ) {
            var getBlobResponce = new GetBlobsResponse();
            foreach (var blob in blobs) {
                getBlobResponce.Blobs.Add( await blob.ToGrpcBlob());
            }
            return getBlobResponce;
        }
        public static GetBlobsDetailsResponse ToGetBlobsDetailsResponse( this List<Domain.BlobDetails> blobsDetails ) {
            var getBlobsDetailsResponse = new GetBlobsDetailsResponse();
            foreach (var blob in blobsDetails) {
                getBlobsDetailsResponse.BlobsDetails.Add( blob.ToGrpcDetails());
            }
            return getBlobsDetailsResponse;
        }
        public static Documents.GrpcApi.BlobDetails ToGrpcDetails( this Domain.BlobDetails? details ) {
            if (details == null) {
                return new BlobDetails();
            }
            var blobDetails = new GrpcApi.BlobDetails() {
                ContentLength = details.ContentLength,
                ContentType = details.ContentType,
                Name = details.Name,
                CreatedAt = details.CreatedAt.Value.ConvertToProtobufTimestamp(),
                LastModified = details.LastModified.Value.ConvertToProtobufTimestamp(),
            };

            foreach (var kvp in details?.Metadata) {
                blobDetails.Metadata.Add( kvp.Key, kvp.Value );
            }
            return blobDetails;
        }
        public static Timestamp ConvertToProtobufTimestamp(this DateTimeOffset dateTimeOffset ) {
            long seconds = dateTimeOffset.ToUnixTimeSeconds();
            return new Timestamp { Seconds = seconds };
        }

    }
}
