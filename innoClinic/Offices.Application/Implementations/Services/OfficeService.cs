using AutoMapper;
using FluentValidation;
using Offices.Application.Dtos;
using Offices.Application.Exceptions;
using Offices.Application.Interfaces.Repositories;
using Offices.Application.Interfaces.Services;
using Offices.Domain.Models;


namespace Offices.Application.Implementations.Services {
    public class OfficeService: IOfficeService {
        private readonly IOfficeRepository _repository;
        private readonly IIdGenerator _idGenerator;
        private readonly IMapper _mapper;
        private readonly IValidator<Office> _validator;

        public OfficeService( IOfficeRepository repository,
                              IValidator<Office> validator,
                              IMapper mapper,
                              IIdGenerator idGenerator ) {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
            _idGenerator = idGenerator;
        }

        public async Task<string> CreateAsync( CreateOfficeDto officeDto ) {
            var office = _mapper.Map<Office>( officeDto );
            office.Id = _idGenerator.GenerateId();
            await _validator.ValidateAndThrowAsync( office );
            if (await _repository.AnyByNumberAsync(officeDto.RegistryPhoneNumber)) {
                throw new OfficeAlreadyExistException(officeDto.RegistryPhoneNumber);
            }
            await _repository.CreateAsync( office );
            return office.Id;
        }

        public async Task DeleteAsync( string id ) {
            await _repository.DeleteAsync( id );
        }

        public async Task<OfficeDto> GetAsync( string id ) {
            var office = await _repository.GetAsync( id );
            if (office == null) {
                throw new OfficeNotFoundException(id);
            }

            return _mapper.Map<OfficeDto>( office );
        }

        public async Task<List<OfficeDto>> GetAllAsync() {
            var offices = await _repository.GetAllAsync( );
     
            return _mapper.Map<List<OfficeDto>>( offices )??[];
        }

        public async Task UpdateAsync( string id, UpdateOfficeDto officeDto ) {
            if (await _repository.AnyAsync(id)) {
                throw new OfficeNotFoundException( id );
            }
            var doc = await _repository.GetAsync( id );
            if (await _repository.AnyByNumberAsync( officeDto.RegistryPhoneNumber ) 
                && officeDto.RegistryPhoneNumber != doc.RegistryPhoneNumber ) {
                throw new PhoneAlreadyExistException( $"This phone number {officeDto.RegistryPhoneNumber} already fixed for another office." );
            }
            var office = _mapper.Map<Office>( officeDto );
            office.Id = id;
            await _validator.ValidateAndThrowAsync( office );

            await _repository.UpdateAsync( office );
        }
    }
}
