using Appointments.Application.Dtos;
using Appointments.Application.Exceptions;
using Appointments.Application.Interfaces.Repositories;
using Appointments.Application.Interfaces.Services;
using Appointments.Domain;
using Mapster;

namespace Appointments.Application.Implementations {
    public class ResultService: IResultService {
        private readonly IResultRepository _repository;

        public ResultService( IResultRepository repository ) {
            this._repository = repository;
        }

        public async Task<Guid> CreateAsync( ResultCreateDto entity ) {
            var result = entity.Adapt<Result>();
            result.Id = Guid.NewGuid();
            result.CreatedDate = DateTime.UtcNow;

            await _repository.CreateAsync( result );

            return result.Id;
        }

        public async Task DeleteAsync( Guid id ) {
            var entity = await _repository.GetAsync(id);
            if (entity == null) {
                throw new ResultNotFoundException(id);    
            }
            await _repository.DeleteAsync( entity );
        }

        public async Task<IList<ResultDto>> GetAllAsync() {
            return (await _repository.GetAllAsync()).Adapt<IList<ResultDto>>();
        }

        public async Task<ResultFullDto> GetAsync( Guid id ) {
            var result = await _repository.GetAsync(id);
            if (result == null) {
                throw new ResultNotFoundException( id );
            }
            return ( result ).Adapt<ResultFullDto>( );
        }

        public async Task UpdateAsync( ResultDto entity ) {
            var result = await _repository.GetAsync( entity.Id );
            if (result == null) {
                throw new ResultNotFoundException( entity.Id );
            }
            await _repository.UpdateAsync(entity.Adapt<Result>());
        }
    }
}
