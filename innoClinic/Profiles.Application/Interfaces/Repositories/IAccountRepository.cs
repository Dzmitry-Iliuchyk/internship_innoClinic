namespace Profiles.Application.Interfaces.Repositories {
    public interface IAccountRepository {
        Task SetPathToImage( Guid id, string path);
    }
    public interface IAccountReadRepository {
        Task<string> GetPathToImage( Guid id );
    }
}
