using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Offices.Application.Interfaces.Repositories;
using Offices.DataAccess.DIConfiguration;
using Offices.DataAccess.Models;

using Offices.Domain.Models;

namespace Offices.DataAccess.Repositories {
    public class OfficeRepository: IOfficeRepository {
        private readonly IMongoCollection<OfficeEntity> _offices;
        private readonly IMapper _mapper;

        public OfficeRepository( IMongoDatabase mongodb, IOptions<OfficesDatabaseSettings> options, IMapper mapper ) {
            _offices = mongodb.GetCollection<OfficeEntity>( options.Value.OfficesCollectionName );
            _mapper = mapper;
        }

        public async Task CreateAsync( Office entity ) {
            await _offices
                .InsertOneAsync( _mapper.Map<OfficeEntity>( entity ) );
        }

        public async Task DeleteAsync( string id ) {
            await _offices
                .DeleteOneAsync( x => x.Id == id );
        }

        public async Task<bool> AnyAsync( string id ) {
            return await _offices
                .Find( x => x.Id == id )
                .AnyAsync();
        }
        public async Task<bool> AnyByNumberAsync( string phone ) {
            return await _offices
                .Find( x => x.RegistryPhoneNumber == phone )
                .AnyAsync();
        }

        public async Task<Office> GetAsync( string id ) {
            var office = await _offices
                 .Find( x => x.Id == id )
                 .FirstOrDefaultAsync();

            return _mapper.Map<Office>( office );
        }

        public async Task<List<Office>> GetAllAsync() {
            var offices = await _offices
                .Find( _ => true )
                .ToListAsync();

            return _mapper.Map<List<Office>>( offices );
        }

        public async Task UpdateAsync( Office office ) {
            var entity = _mapper.Map<OfficeEntity>( office );
            await _offices.ReplaceOneAsync( x => x.Id == entity.Id, entity );
        }
    }
}
