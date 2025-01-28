using Documents.GrpcApi;
using FacadeApi.Offices.Dtos;
using FacadeApi.Offices.Exceptions;
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
        public async Task<IResult> GetAllPhotos( string id ) {
            var officePhotos = await _documents.GetBlobsByPrefixAsync( new GetBlobsByPrefixRequest() {
                Path = Extensions.GetPathToOfficeBlob( "", id )
            } );

            return Microsoft.AspNetCore.Http.Results.Ok( officePhotos?.ToPhotos() );
        }

        [HttpGet( "[action]" )]
        public async Task<IResult> GetOfficesWithPhoto() {
            using var officeClient = GetClientWithHeaders();
            var httpResult = await officeClient.GetAsync( $"api/Offices/GetOffices" );
            if (!httpResult.IsSuccessStatusCode) {
                throw new NotSuccessHttpRequest( httpResult );
            }
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
                res.Add( new OfficeDto {
                    Id = office.Id,
                    Address = office.Address,
                    RegistryPhoneNumber = office.RegistryPhoneNumber,
                    Status = office.Status,
                    Photo = docResult != null
                        ? new() {
                            Content = docResult.Content.ToBase64(),
                            Name = docResult.Details.Name,
                        }
                        : null
                } );
            }

            return Microsoft.AspNetCore.Http.Results.Ok( res );
        }

        [HttpGet( "{id}/[action]" )]
        public async Task<IResult> GetOfficeWithPhoto( string id ) {
            using var officeClient = GetClientWithHeaders();
            var httpResult = await officeClient.GetAsync( $"api/Offices/{id}/GetOffice" );
            if (!httpResult.IsSuccessStatusCode) {
                throw new NotSuccessHttpRequest( httpResult );
            }
            var office = JsonSerializer.Deserialize<OfficeDtoFromApi>(
                    httpResult.Content.ReadAsStream(),
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
                using var filestream = file.OpenReadStream();
                var photoUrl = Extensions.GetPathToOfficeBlob( file.FileName, updateOfficeDto.Id );
                await _documents.UploadBlobAsync( new BlobUploadRequest() {
                    Content = Google.Protobuf.ByteString.FromStream( filestream ),
                    PathToBlob = photoUrl,
                } );
                request.PhotoUrl = photoUrl;

            }

            var httpResult = await officeClient.PutAsync( $"api/Offices/UpdateOffice?id={updateOfficeDto.Id}",
                JsonContent.Create( request ) );
            if (!httpResult.IsSuccessStatusCode) {
                await _documents.DeleteBlobAsync( new DeleteBlobRequest() {
                    PathToBlob = request.PhotoUrl,
                } );
                throw new NotSuccessHttpRequest( httpResult );
            }
            return Microsoft.AspNetCore.Http.Results.Content( await httpResult.Content.ReadAsStringAsync(), statusCode: (int)httpResult.StatusCode );
        }

        [HttpDelete( "[action]" )]
        public async Task<IResult> DeleteOffice( string id ) {
            using var officeClient = GetClientWithHeaders();
            var office = JsonSerializer.Deserialize<OfficeDtoFromApi>( ( await officeClient.GetAsync( $"api/Offices/GetOffice?id={id}" ) ).Content.ReadAsStream(),
                new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                } );
            var httpResult = await officeClient.DeleteAsync( $"api/Offices/DeleteOffice?id={id}" );
            if (httpResult.IsSuccessStatusCode && !string.IsNullOrEmpty( office?.PhotoUrl )) {
                await _documents.DeleteBlobAsync( new DeleteBlobRequest() {
                    PathToBlob = office.PhotoUrl,
                } );
            }
            return Microsoft.AspNetCore.Http.Results.Content( await httpResult.Content.ReadAsStringAsync(), statusCode: (int)httpResult.StatusCode );
        }

        [HttpPost( "[action]" )]
        public async Task<IResult> CreateOffice( CreateOfficeDto createOfficeDto ) {
            using var officeClient = GetClientWithHeaders();
            var request = createOfficeDto.ToCreateOfficeDtoForApi();
            var httpResult = await officeClient.PostAsync( $"api/Offices/CreateOffice", JsonContent.Create( request ) );
            if (!httpResult.IsSuccessStatusCode) {
                throw new NotSuccessHttpRequest( httpResult );
            }
            if (createOfficeDto.File != null) {
                var id = JsonSerializer.Deserialize<string>( await httpResult.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions {
                            PropertyNameCaseInsensitive = true
                        } );
                var path = Extensions.GetPathToOfficeBlob( createOfficeDto.File.FileName, id );


                using var filestream = createOfficeDto.File.OpenReadStream();
                var docResult = await _documents.UploadBlobAsync( new BlobUploadRequest() {
                    Content = Google.Protobuf.ByteString.FromStream( filestream ),
                    PathToBlob = path,
                } );
                await officeClient.PatchAsync( $"api/Offices/SetPathToOffice?id={id}&path={path}", null );
            }
            return Microsoft.AspNetCore.Http.Results.Content( await httpResult.Content.ReadAsStringAsync(), statusCode: (int)httpResult.StatusCode );
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
