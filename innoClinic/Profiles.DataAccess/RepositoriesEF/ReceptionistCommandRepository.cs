using Microsoft.EntityFrameworkCore;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using Shared.Abstractions.Repository;

namespace Profiles.DataAccess.Repositories {
    public class ReceptionistCommandRepository: BaseRepository<Receptionist>, IReceptionistCommandRepository {
        public ReceptionistCommandRepository( ProfilesDbContext context ) : base( context ) {
        }
    }
    public class AccountRepository: IAccountRepository {
        private readonly ProfilesDbContext _profilesDbContext;
        public AccountRepository( ProfilesDbContext profilesDbContext ) {
            this._profilesDbContext = profilesDbContext;
        }

        public async Task SetPathToImage( Guid id, string path ) {
             _profilesDbContext.Accounts.Where( x => x.Id == id ).ExecuteUpdate(x=>x.SetProperty(e=>e.PhotoUrl, path));
            await _profilesDbContext.SaveChangesAsync();
        }
    }
}
