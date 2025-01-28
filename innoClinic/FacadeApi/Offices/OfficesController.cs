using Documents.GrpcApi;
using FacadeApi.Offices.Dtos;
using Grpc.Core;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<IResult> GetAllPhotos(string id) {
            var officePhotos = await _documents.GetBlobsByPrefixAsync( new GetBlobsByPrefixRequest() {
                Path = Extensions.GetPathToOfficeBlob("", id)
            } );
            
            return Microsoft.AspNetCore.Http.Results.Ok( officePhotos.ToPhotos() );
        }

        [HttpGet( "[action]" )]
        public async Task<IResult> GetOffices() {
            using var officeClient = GetClientWithHeaders();
            var httpResult = await officeClient.GetAsync( $"api/Offices/GetOffices" );
            var offices = JsonSerializer.Deserialize<List<OfficeDtoFromApi>>(
                    ( httpResult ).Content.ReadAsStream(),
                    new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true
                    }
                );
            var res = new List<OfficeDto>();
            foreach (var office in offices) {
                var docResult = await _documents.GetBlobAsync( new GetBlobRequest() {
                    PathToBlob = office.PhotoUrl,
                } );
                var photo = new Photo();
                if (docResult != null) {
                    photo = new() {
                        Content = docResult.Content.ToBase64(),
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

            return Microsoft.AspNetCore.Http.Results.Ok( res );
        }

        [HttpGet( "{id}/[action]" )]
        public async Task<IResult> GetOffice( string id ) {
            using var officeClient = GetClientWithHeaders();
            var office = JsonSerializer.Deserialize<OfficeDtoFromApi>(
                    ( await officeClient.GetAsync( $"api/Offices/{id}/GetOffice" ) ).Content.ReadAsStream(),
                    new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true
                    }
                );
            Photo photo = null;
            if (string.IsNullOrEmpty( office.PhotoUrl )) {
                var docResult = await _documents.GetBlobAsync( new GetBlobRequest() {
                    PathToBlob = office.PhotoUrl,
                } );
                photo = new() {
                    Content = docResult.Content.ToBase64(),
                    Name = docResult.Details.Name,
                };
            }

            return Microsoft.AspNetCore.Http.Results.Ok( new OfficeDto {
                Id = office.Id,
                Address = office.Address,
                RegistryPhoneNumber = office.RegistryPhoneNumber,
                Status = office.Status,
                Photo = photo
            } );
        }

        [HttpPut( "[action]" )]
        public async Task<IResult> UpdateOffice( UpdateOfficeDto updateOfficeDto, IFormFile? file ) {
            using var officeClient = GetClientWithHeaders();
            var request = updateOfficeDto.ToUpdateOfficeDtoForApi();
            if (file != null) {
                try {
                    using var filestream = file.OpenReadStream();
                    var photoUrl = Extensions.GetPathToOfficeBlob( file.FileName, updateOfficeDto.Id );
                    var docResult = await _documents.UploadBlobAsync( new BlobUploadRequest() {
                        Content = Google.Protobuf.ByteString.FromStream(filestream ),
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

            var result = await officeClient.PutAsync( $"api/Offices/UpdateOffice?id={updateOfficeDto.Id}", JsonContent.Create( request ) );
            if (!result.IsSuccessStatusCode) {
                await _documents.DeleteBlobAsync( new DeleteBlobRequest() {
                    PathToBlob = request.PhotoUrl,
                } );
                throw new NotImplementedException();
            }
            return Microsoft.AspNetCore.Http.Results.Ok( result.Content );
        }

        [HttpDelete( "[action]" )]
        public async Task<IResult> DeleteOffice( string id ) {
            using var officeClient = GetClientWithHeaders();
            var office = JsonSerializer.Deserialize<OfficeDtoFromApi>( ( await officeClient.GetAsync( $"api/Offices/GetOffice?id={id}" ) ).Content.ReadAsStream(),
                new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            } );
            var result = await officeClient.DeleteAsync( $"api/Offices/DeleteOffice?id={id}" );
            if (result.IsSuccessStatusCode && !string.IsNullOrEmpty( office?.PhotoUrl )) {
                await _documents.DeleteBlobAsync( new DeleteBlobRequest() {
                    PathToBlob = office.PhotoUrl,
                } );
            }
            return Microsoft.AspNetCore.Http.Results.StatusCode( (int)result.StatusCode );
        }

        [HttpPost( "[action]" )]
        public async Task<IResult> CreateOffice( CreateOfficeDto createOfficeDto) {
            using var officeClient = GetClientWithHeaders();
            var request = createOfficeDto.ToCreateOfficeDtoForApi();
            var result = await officeClient.PostAsync( $"api/Offices/CreateOffice", JsonContent.Create( request ) );
            if (!result.IsSuccessStatusCode) {
                throw new NotImplementedException();
            }
            if (createOfficeDto.File != null) {
                var id = JsonSerializer.Deserialize<string>( await result.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions {
                            PropertyNameCaseInsensitive = true
                        } );
                var path = Extensions.GetPathToOfficeBlob( createOfficeDto.File.FileName, id );

                try {
                    using var filestream = createOfficeDto.File.OpenReadStream();
                    var docResult = await _documents.UploadBlobAsync( new BlobUploadRequest() {
                        Content = Google.Protobuf.ByteString.FromStream( filestream ),
                        PathToBlob = path,
                    } );
                    await officeClient.PatchAsync( $"api/Offices/SetPathToOffice?id={id}&path={path}", null );
                }
                catch (RpcException ex) {
                    throw;
                }

            }
            return Microsoft.AspNetCore.Http.Results.Ok( result.Content.ReadAsStringAsync() );
        }
        private HttpClient GetClientWithHeaders() {
            var officeClient = _clientFactory.CreateClient( "offices" );
            if (Request.Headers.Authorization.Any()) {
                officeClient.DefaultRequestHeaders.Add( "Authorization", Request.Headers.Authorization.ToString() );
            }
            return officeClient;
        }
    }
}
