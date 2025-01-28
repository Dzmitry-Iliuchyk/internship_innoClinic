
namespace FacadeApi.Services {
    public interface IImageService {
        Task<Stream> ResizeImage( Stream imageStream, string format );
    }
}