using LinqToDB;
using Services.Application.Abstractions.Repositories;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataAccess.Repositories {
    public class ServiceRepository: IServiceRepository {
        private readonly ServicesDataConnection _connection;
        public ServiceRepository( ServicesDataConnection connection ) {
            this._connection = connection;
        }
        public async Task<Guid> CreateAsync( Service service ) {
            return ( await _connection.InsertAsync( service ) == 1 ? service.Id : Guid.Empty);
        }

        public async Task DeleteAsync( Guid id ) {
            await _connection.Services.DeleteAsync( x=>x.Id == id );
        }

        public async Task<IList<Service>> GetAllAsync() {
            return await _connection.Services.ToListAsync();
        }

        public async Task<Service?> GetAsync( Guid id ) {
         
            return await _connection.Services
                .Join<Service,Specialization,Service>(_connection.Specializations,
                    SqlJoinType.Inner,
                    (ser,spec)=> ser.SpecializationId==spec.Id,
                    (ser, spec ) => AssignSpecialization(ser, spec)
                    )
                .Join(_connection.ServiceCategories,
                    SqlJoinType.Inner,
                    (s,sc)=> s.CategoryId == sc.Id,
                    (s,sc)=> AssignCategory(s,sc))
                .SingleOrDefaultAsync(x=>x.Id == id);
        }

        public async Task UpdateAsync( Service updatedService ) {
            await _connection.Services.Where( s => s.Id == updatedService.Id )
            .Set( s => s, updatedService )
            .UpdateAsync();
        }
        private static Service AssignCategory( Service service, ServiceCategory serviceCategory) {
            service.Category = serviceCategory;
            return service;
        }
        private static Service AssignSpecialization( Service service, Specialization specialization ) {
            service.Specialization = specialization;
            return service;
        }
    }
}
