using AutoMapper;
using FluentValidation;
using MassTransit;
using Offices.Application.Dtos;
using Offices.Application.Exceptions;
using Offices.Application.Interfaces.Repositories;
using Offices.Application.Interfaces.Services;
using Offices.Domain.Models;
using Shared.Events.Contracts;
using Shared.Events.Contracts.OfficesMessages;


namespace Offices.Application.Implementations.Services {
    public class OfficeService: IOfficeService {
        private readonly IOfficeRepository _repository;
        private readonly IIdGenerator _idGenerator;
        private readonly IMapper _mapper;
        private readonly IValidator<Office> _validator;
        private readonly IPublishEndpoint _publisher;
        public OfficeService( IOfficeRepository repository,
                              IValidator<Office> validator,
                              IMapper mapper,
                              IIdGenerator idGenerator,
                              IPublishEndpoint publisher ) {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
            _idGenerator = idGenerator;
            this._publisher = publisher;
        }

        public async Task<string> CreateAsync( CreateOfficeDto officeDto ) {
            var office = _mapper.Map<Office>( officeDto );
            office.Id = _idGenerator.GenerateId();
            await _validator.ValidateAndThrowAsync( office );
            if (await _repository.AnyByNumberAsync(officeDto.RegistryPhoneNumber)) {
                throw new OfficeAlreadyExistException(officeDto.RegistryPhoneNumber);
            }
            await _repository.CreateAsync( office );

            await _publisher.Publish(new OfficeCreated {
                Id = office.Id,
                Address = office.Address.ToString(),
                RegistryPhoneNumber = office.RegistryPhoneNumber,
                Status = office.Status,
            } );
            return office.Id;
        }

        public async Task DeleteAsync( string id ) {
            await _repository.DeleteAsync( id );

            await _publisher.Publish(new OfficeDeleted {
                Id = id
            } );
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
            if (!await _repository.AnyAsync(id)) {
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

            await _publisher.Publish(new OfficeUpdated {
                Id = id,
                Address = office.Address.ToString(),
                RegistryPhoneNumber = office.RegistryPhoneNumber,
                Status = office.Status
            } );
        }
        public async Task SetPathToOffice( string id, string path ) {
            if (!await _repository.AnyAsync(id)) {
                throw new OfficeNotFoundException( id );
            }
            await _repository.SetPathToOffice( id, path );
        }

        public async Task<PagedOfficesDto> GetPageAsync( int skip, int take ) {
            var (offices, total) = await _repository.GetPageAsync(skip, take);

            return new (offices: _mapper.Map<List<OfficeDto>>( offices ) ?? [], total: total);
        }
    }
}
