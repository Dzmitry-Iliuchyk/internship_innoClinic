using Mapster;
using MediatR;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Receptionists.Commands.Create {
    public record CreateReceptionistCommand( 
                                    [Required] string officeId,
                                    [Required] string firstName,
                                    [Required] string lastName,
                                    [Required] string email,
                                    [Required] string phoneNumber,
                                    [Required] Guid createdBy,
                                    [Required] string? photoUrl,
                                    [Required] string? middleName ): IRequest<Guid>;

    public class CreateReceptionistCommandHandler: IRequestHandler<CreateReceptionistCommand, Guid> {
        private readonly IReceptionistCommandRepository _repository;
        public CreateReceptionistCommandHandler( IReceptionistCommandRepository repository ) {
            this._repository = repository;
        }

        public async Task<Guid> Handle( CreateReceptionistCommand request, CancellationToken cancellationToken = default ) {
            var receptionist = request.Adapt<Receptionist>();
            await _repository.CreateAsync( receptionist );
            return receptionist.Id;
        }
    }
}
