using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Processing;

namespace FacadeApi.Services {
    public class ImageService: IImageService {
        private readonly ImageOptions _options;

        public ImageService( IOptions<ImageOptions> options ) {
            this._options = options.Value;
        }

        public async Task<Stream> ResizeImage( Stream imageStream, string format ) {
            try {

                using var image = await SixLabors.ImageSharp.Image.LoadAsync( imageStream );

                var outputStream = new MemoryStream();
                image.Mutate( x => x.Resize( _options.Width, _options.Height ) );
                switch (format.ToLower()) {
                    case "image/png":
                        await image.SaveAsync( outputStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder() );
                        break;
                    case "image/jpeg":
                    case "image/jpg":
                        await image.SaveAsync( outputStream, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder() );
                        break;
                    default:
                        throw new NotImplementedException();
                }
                return outputStream;
            }
            catch (Exception e) {
                throw new NotImplementedException( "Failed to save image", e );
            }
        }
    }
}
