using Documents.GrpcApi;
using FacadeApi.Offices.Dtos;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FacadeApi.Offices {
    [Route( "api/[controller]" )]
    [ApiController]
    public class OfficesController: ControllerBase {
        private readonly IHttpClientFactory _clientFactory;
        private readonly DocumentService.DocumentServiceClient _documents;
        public OfficesController( IHttpClientFactory clientFactory, DocumentService.DocumentServiceClient client ) {
            this._clientFactory = clientFactory;
            _documents = client;
        }

        [HttpGet( "[action]" )]
        public async Task<IResult> GetOffices() {
            using var officeClient = _clientFactory.CreateClient( "offices" );
            var offices = JsonSerializer.Deserialize<List<OfficeDtoFromApi>>(
                    ( await officeClient.GetAsync( $"/offices/GetOffices" ) ).Content.ReadAsStream()
                );
            var res = new List<OfficeDto>();
            foreach (var office in offices) {
                var docResult = await _documents.GetBlobAsync( new GetBlobRequest() {
                    PathToBlob = office.PhotoUrl,
                } );
                var photo = new Photo();
                if (docResult != null) {
                    photo = new() {
                        Content = docResult.Content.ToArray(),
                        Name = docResult.Details.Name,
                    };
                }

                res.Add( new OfficeDto {
                    Id = office.Id,
                    Address = office.Address,
                    RegistryPhoneNumber = office.RegistryPhoneNumber,
                    Status = office.Status,
                    Photo = photo
                } );
            }

            return Results.Ok( res );
        }

        [HttpGet( "{id}/[action]" )]
        public async Task<IResult> GetOffice( string id ) {
            using var officeClient = _clientFactory.CreateClient( "offices" );
            var office = JsonSerializer.Deserialize<OfficeDtoFromApi>(
                    ( await officeClient.GetAsync( $"/offices/{id}/GetOffice" ) ).Content.ReadAsStream()
                );
            Photo photo = null;
            if (string.IsNullOrEmpty( office.PhotoUrl )) {
                var docResult = await _documents.GetBlobAsync( new GetBlobRequest() {
                    PathToBlob = office.PhotoUrl,
                } );
                photo = new() {
                    Content = docResult.Content.ToArray(),
                    Name = docResult.Details.Name,
                };
            }

            return Results.Ok( new OfficeDto {
                Id = office.Id,
                Address = office.Address,
                RegistryPhoneNumber = office.RegistryPhoneNumber,
                Status = office.Status,
                Photo = photo
            } );
        }

        [HttpPut( "[action]" )]
        public async Task<IResult> UpdateOffice( UpdateOfficeDto updateOfficeDto ) {
            using var officeClient = _clientFactory.CreateClient( "offices" );
            var request = updateOfficeDto.ToUpdateOfficeDtoForApi();
            if (updateOfficeDto.Photo != null) {
                try {
                    var photoUrl = Extensions.GetPathToOfficeBlob( updateOfficeDto.Photo.Name, updateOfficeDto.Id );
                    var docResult = await _documents.UploadBlobAsync( new BlobUploadRequest() {
                        Content = Google.Protobuf.ByteString.CopyFrom( updateOfficeDto.Photo.Content ),
                        PathToBlob = photoUrl,
                    } );
                    if (!docResult.IsSuccess) {
                        throw new NotImplementedException();
                    }
                    request.PhotoUrl = photoUrl;
                }
                catch (RpcException ex) {
                    throw;
                }

            }

            var result = await officeClient.PutAsync( $"/offices/UpdateOffice?id={updateOfficeDto.Id}", JsonContent.Create( request ) );
            if (!result.IsSuccessStatusCode) {
                await _documents.DeleteBlobAsync( new DeleteBlobRequest() {
                    PathToBlob = request.PhotoUrl,
                } );
                throw new NotImplementedException();
            }
            return Results.Ok( result.Content );
        }

        [HttpDelete( "[action]" )]
        public async Task<IResult> DeleteOffice( string id ) {
            using var officeClient = _clientFactory.CreateClient( "offices" );
            var office = JsonSerializer.Deserialize<OfficeDtoFromApi>( ( await officeClient.GetAsync( $"/offices/GetOffice?id={id}" ) ).Content.ReadAsStream() );
            var result = await officeClient.DeleteAsync( $"/offices/DeleteOffice?id={id}" );
            if (result.IsSuccessStatusCode && !string.IsNullOrEmpty( office?.PhotoUrl )) {
                await _documents.DeleteBlobAsync( new DeleteBlobRequest() {
                    PathToBlob = office.PhotoUrl,
                } );
            }
            return Results.StatusCode( (int)result.StatusCode );
        }

        [HttpPost( "[action]" )]
        public async Task<IResult> CreateOffice( CreateOfficeDto createOfficeDto ) {
            using var officeClient = _clientFactory.CreateClient( "offices" );
            var request = createOfficeDto.ToCreateOfficeDtoForApi();
            var result = await officeClient.PostAsync( $"/offices/CreateOffice", JsonContent.Create( request ) );
            if (!result.IsSuccessStatusCode) {
                throw new NotImplementedException();
            }
            if (createOfficeDto.Photo != null) {
                var path = Extensions.GetPathToOfficeBlob( createOfficeDto.Photo.Name, await result.Content.ReadAsStringAsync() );

                try {
                    var docResult = await _documents.UploadBlobAsync( new BlobUploadRequest() {
                        Content = Google.Protobuf.ByteString.CopyFrom( createOfficeDto.Photo.Content ),
                        PathToBlob = path,
                    } );
                    await officeClient.PatchAsync( $"/offices/SetPathToOffice",
                        JsonContent.Create( ( new {
                            id = await result.Content.ReadAsStringAsync(),
                            path = path
                        } ) ) );
                }
                catch (RpcException ex) {
                    throw;
                }

            }
            return Results.Ok( result.Content );
        }
    }
}
