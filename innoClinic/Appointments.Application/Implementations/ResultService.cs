using Appointments.Application.Dtos;
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
            var appointment = entity.Adapt<Result>();
            appointment.Id = Guid.NewGuid();

            await _repository.CreateAsync( appointment );

            return appointment.Id;
        }

        public async Task DeleteAsync( Guid id ) {
            var entity = await _repository.GetAsync(id);
            if (entity == null) {
                throw new NotImplementedException();    
            }
            await _repository.DeleteAsync( entity );
        }

        public async Task<IList<ResultDto>> GetAllAsync() {
            return (await _repository.GetAllAsync()).Adapt<IList<ResultDto>>();
        }

        public async Task<ResultFullDto> GetAsync( Guid id ) {
            return ( await _repository.GetAsync( id ) ).Adapt<ResultFullDto>( );
        }

        public async Task UpdateAsync( ResultDto entity ) {
            await _repository.UpdateAsync(entity.Adapt<Result>());
        }
    }
}
