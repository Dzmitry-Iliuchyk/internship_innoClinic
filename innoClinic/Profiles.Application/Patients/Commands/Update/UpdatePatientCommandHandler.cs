﻿using Mapster;
using MediatR;
using Profiles.Application.Common;
using Profiles.Application.Common.Exceptions;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Patients.Commands.Update {
    public record UpdatePatientCommand( [Required] Guid Id,
                                       [Required] DateTime DateOfBirth,
                                       [Required] string FirstName,
                                       [Required] string LastName,
                                       [Required] string Email,
                                       [Required] string PhoneNumber,
                                       [Required] Guid UpdatedBy,
                                       string? PhotoUrl,
                                       string? MiddleName )
        : UpdatePersonCommandBase( Id, FirstName, LastName, Email, PhoneNumber, UpdatedBy, PhotoUrl, MiddleName );
    public class UpdatePatientCommandHandler: IRequestHandler<UpdatePatientCommand> {
        private readonly IPatientReadRepository _repoRead;
        private readonly IPatientCommandRepository _repository;
        public UpdatePatientCommandHandler( IPatientCommandRepository repository, IPatientReadRepository repoRead ) {
            this._repository = repository;
            this._repoRead = repoRead;
        }

        public async Task Handle( UpdatePatientCommand request, CancellationToken cancellationToken = default ) {
            var patient = await _repoRead.GetAsync( request.Id);
            if (patient == null) {
                throw new PatientNotFoundException( request.Id.ToString() );
            }
            var updatedPatient = (request, patient).Adapt<Patient>();
            await _repository.UpdateAsync( updatedPatient );
        }
    }
}
