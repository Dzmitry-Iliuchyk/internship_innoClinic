﻿using Documents.GrpcApi;
using FacadeApi.Offices.Dtos;

namespace FacadeApi.Offices {
    public static class Extensions {
        public static UpdateOfficeDtoForApi ToUpdateOfficeDtoForApi( this UpdateOfficeDto updateOfficeDto ) {
            var updateOfficeDtoForApi = new UpdateOfficeDtoForApi() {
                 Address = updateOfficeDto.Address,
                 RegistryPhoneNumber = updateOfficeDto.RegistryPhoneNumber,
                 Status = updateOfficeDto.Status,
                 PhotoUrl = ""
            };

            return updateOfficeDtoForApi;
        }
        public static List<Photo> ToPhotos(this GetBlobsResponse blobs) {
            var photos = new List<Photo>();
            foreach (var blob in blobs.Blobs) {
                photos.Add( new Photo() { 
                    Name = blob.Details.Name,
                    Content = blob.Content.ToBase64(),
                    Metadata = blob.Details.Metadata
                } );
            }
            return photos;
        }
        public static string GetPathToOfficeBlob( string fileName, string id ) {
            return $"office:{id}/{fileName}";
        }

        public static CreateOfficeDtoForApi ToCreateOfficeDtoForApi( this CreateOfficeDto updateOfficeDto ) {
            var createOfficeDtoForApi = new CreateOfficeDtoForApi() {
                Address = updateOfficeDto.Address,
                RegistryPhoneNumber = updateOfficeDto.RegistryPhoneNumber,
                Status = updateOfficeDto.Status,
                PhotoUrl = ""
            };
            return createOfficeDtoForApi;
        }
    }
}
