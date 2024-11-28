using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Application.Exceptions {
    public abstract class NotFoundException(string message): Exception(message) { }
    public abstract class BadRequestException(string message): Exception(message) { }

    public class UserNotFoundException: NotFoundException {
        public UserNotFoundException( Guid userId ) 
            : base( $"The user with the identifier {userId} does not exist") { 
        }
        public UserNotFoundException( string email ) 
            : base( $"The user with the email {email} does not exist") { 
        }
    }
    public class UserAlredyExistException: BadRequestException {
        public UserAlredyExistException( Guid userId ) 
            : base( $"The user with the identifier {userId} already exists") { 
        }
        public UserAlredyExistException( string email ) 
            : base( $"The user with the email {email} already exists") { 
        }
    }
    public class WrongPasswordException: BadRequestException {
        public WrongPasswordException( string email ) 
            : base( $" Wrong password for the user with the email {email}") { 
        }
    }
    public class RoleNotFoundException: NotFoundException {
        public RoleNotFoundException( int roleId )
                : base( $"The role with the identifier {roleId} does not exist" ) {
        }
    }

}
