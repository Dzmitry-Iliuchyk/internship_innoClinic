using Documents.GrpcApi;
using FacadeApi.Controllers;
using FacadeApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace FacadeApi.Profiles {
    [ApiController]
    [Route( "[controller]" )]
    public class ProfilesController: ControllerBase {


        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IImageService _imageService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly DocumentService.DocumentServiceClient _documents;

        public ProfilesController( ILogger<WeatherForecastController> logger,
                                  IHttpClientFactory clientFactory,
                                  DocumentService.DocumentServiceClient documents,
                                  IImageService imageService ) {
            _logger = logger;
            this._clientFactory = clientFactory;
            this._documents = documents;
            this._imageService = imageService;
        }

        [HttpPost( "[action]" )]
        public async Task<IResult> UploadImageToProfile( Guid id, IFormFile file ) {
            using var officeClient = GetClientWithHeaders();

            using var filestream = file.OpenReadStream();
            var photoUrl = ProfileExtensions.GetPathToProfilesImage( id, file.FileName);
            using var photo = await _imageService.ResizeImage( filestream, file.ContentType );
            photo.Position = 0;
            var byteString = Google.Protobuf.ByteString.FromStream( photo );
            var docResult = await _documents.UploadBlobAsync( new BlobUploadRequest() {
                Content = byteString,
                PathToBlob = photoUrl,
            } );
            var result = await officeClient.PatchAsync( $"Utility/SetImagePath?id={id}&path={photoUrl}",null );
            if (!result.IsSuccessStatusCode) {
                await _documents.DeleteBlobAsync( new DeleteBlobRequest { PathToBlob = photoUrl } );
            }
            return Microsoft.AspNetCore.Http.Results.Ok(await result.Content.ReadAsStringAsync());
        }
        [HttpGet( "{id:guid}/[action]" )]
        public async Task<IResult> GetPhoto( Guid id ) {
            using var officeClient = GetClientWithHeaders();

            var photoUrl = await officeClient.GetFromJsonAsync<string>( $"Utility/GetImagePath?id={id}" );
            if (string.IsNullOrEmpty( photoUrl )) {
                return Microsoft.AspNetCore.Http.Results.NotFound();
            }
            var docResult = await _documents.GetBlobAsync( new GetBlobRequest() {
                PathToBlob = photoUrl,
            } );
            return Microsoft.AspNetCore.Http.Results.Ok( docResult?.ToPhoto());
        }
        private HttpClient GetClientWithHeaders() {
            var officeClient = _clientFactory.CreateClient( "profiles" );
            if (Request.Headers.Authorization.Any()) {
                officeClient.DefaultRequestHeaders.Add( "Authorization", Request.Headers.Authorization.ToString() );
            }
            return officeClient;
        }
    }
}
