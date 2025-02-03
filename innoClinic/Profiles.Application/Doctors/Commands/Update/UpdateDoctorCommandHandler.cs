﻿using Mapster;
using MediatR;
using Profiles.Application.Common;
using Profiles.Application.Common.Exceptions;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using System.ComponentModel.DataAnnotations;

namespace Profiles.Application.Doctors.Commands.Update {

    public record UpdateDoctorCommand([Required] Guid DoctorId,
                                      [Required] DateTime DateOfBirth,
                                      [Required] DateTime CareerStartYear,
                                      [Required] string OfficeId,
                                      [Required] DoctorStatuses Status,
                                      [Required] int SpecializationId,
                                      [Required] string FirstName,
                                      [Required] string LastName,
                                      [Required] string Email,
                                      [Required] string PhoneNumber,
                                      [Required] Guid UpdatedBy,
                                      string? PhotoUrl,
                                      string? MiddleName ):
        UpdatePersonCommandBase( DoctorId, FirstName, LastName, Email, PhoneNumber, UpdatedBy, PhotoUrl, MiddleName );
    public class UpdateDoctorCommandHandler: IRequestHandler<UpdateDoctorCommand> {
        private readonly IDoctorCommandRepository _repository;
        private readonly IDoctorReadRepository _readRepo;
        public UpdateDoctorCommandHandler( IDoctorCommandRepository repository, IDoctorReadRepository readRepo ) {
            this._repository = repository;
            this._readRepo = readRepo;
        }

        public async Task Handle( UpdateDoctorCommand request, CancellationToken cancellationToken = default ) {
            var doc = await _readRepo.GetAsync( request.DoctorId );
            if (doc == null) {
                throw new DoctorNotFoundException( request.DoctorId.ToString() );
            }
            await _repository.UpdateAsync( (request, doc).Adapt<Doctor>() );
        }
    }

}
