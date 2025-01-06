using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Application.Exceptions {
    public abstract class BadRequestException( string message ): Exception( message ) { }
    public abstract class NotFoundException( string message ): Exception( message ) { }
    public class AppointmentNotFoundException( Guid id ): Exception( $"Appointment with given id: {id} not found." ) { }
    public class ResultNotFoundException( Guid id ): Exception( $"Appointment result with given id: {id} not found." ) { }
}
