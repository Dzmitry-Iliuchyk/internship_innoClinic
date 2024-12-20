using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Exceptions {
    public class SpecializationExistException: BadRequestException {
        public SpecializationExistException( int specializationId )
            : base( $"The specialization with the identifier {specializationId} already exists" ) {
        }
        public SpecializationExistException( string name )
            : base( $"The specialization with the name {name} already exists" ) {
        }
    }
}
