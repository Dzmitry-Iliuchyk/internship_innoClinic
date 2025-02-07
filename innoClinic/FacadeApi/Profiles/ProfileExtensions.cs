using Documents.GrpcApi;
using FacadeApi.Offices.Dtos;

namespace FacadeApi.Profiles {
    public static class ProfileExtensions {
        public static string GetPathToProfilesImage( Guid id, string fileName ) {
            return $"profiles:{id}/{fileName}";
        }
        public static Photo ToPhoto( this Blob blob ) {

            return new Photo() {
                Name = blob.Details.Name,
                Content = blob.Content.ToBase64(),
                Metadata = blob.Details.Metadata
            };

        }
    }
}
