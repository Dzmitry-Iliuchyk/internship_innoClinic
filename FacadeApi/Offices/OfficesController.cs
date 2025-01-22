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
            var result = await officeClient.GetAsync( "/offices/GetOffices" );
            return Results.Ok( result.Content );
        }

        [HttpGet( "{id}/[action]" )]
        public async Task<IResult> GetOffice( string id ) {
            using var officeClient = _clientFactory.CreateClient( "offices" );
            var office = JsonSerializer.Deserialize<OfficeDtoFromApi>( ( await officeClient.GetAsync( $"/offices/{id}/GetOffice" ) ).Content.ReadAsStream() );
            var docResult = await _documents.GetBlobAsync( new GetBlobRequest() {
                PathToBlob = office.PhotoUrl,
            } );
            return Results.Ok( new OfficeDto {
                Id = office.Id,
                Address = office.Address,
                RegistryPhoneNumber = office.RegistryPhoneNumber,
                Status = office.Status,
                Photo = new() {
                    Content = docResult.Content.ToArray(),
                    Name = docResult.Details.Name,
                }
            } );
        }

        [HttpPut( "[action]" )]
        public async Task<IResult> UpdateOffice( string id, UpdateOfficeDto updateOfficeDto ) {
            using var officeClient = _clientFactory.CreateClient( "offices" );
            var request = updateOfficeDto.ToUpdateOfficeDtoForApi();
            if (updateOfficeDto.Photo != null) {
                try {
                    var docResult = await _documents.UploadBlobAsync( new BlobUploadRequest() {
                        Content = Google.Protobuf.ByteString.CopyFrom( updateOfficeDto.Photo.Content ),
                        PathToBlob = request.PhotoUrl,
                    } );
                    if (!docResult.IsSuccess) {
                        throw new NotImplementedException();
                    }
                }
                catch (RpcException ex) {
                    throw;
                }

            }

            var result = await officeClient.PutAsync( $"/offices/UpdateOffice?id={id}", JsonContent.Create( request ) );
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
            if (!string.IsNullOrEmpty( office?.PhotoUrl )) {
                await _documents.DeleteBlobAsync( new DeleteBlobRequest() {
                    PathToBlob = office.PhotoUrl,
                } );
            }
            return Results.Ok( result.Content );
        }

        [HttpPost( "[action]" )]
        public async Task<IResult> CreateOffice( CreateOfficeDto createOfficeDto ) {
            using var officeClient = _clientFactory.CreateClient( "offices" );
            var request = createOfficeDto.ToCreateOfficeDtoForApi();

            var result = await officeClient.PostAsync( $"/offices/CreateOffice", JsonContent.Create( request ) );
            if (!result.IsSuccessStatusCode) {
                throw new NotImplementedException();
            }

            try {
                var docResult = await _documents.UploadBlobAsync( new BlobUploadRequest() {
                    Content = Google.Protobuf.ByteString.CopyFrom( createOfficeDto.Photo.Content ),
                    PathToBlob = request.PhotoUrl,
                } );
            }
            catch (RpcException ex) {
                throw;
            }
            return Results.Ok( result.Content );
        }
    }
}
