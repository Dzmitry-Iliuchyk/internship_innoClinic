using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Common.Behavior {
    public class LoggingPipelineBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> {
        private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

        public LoggingPipelineBehavior( ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger ) {
            _logger = logger;
        }

        public async Task<TResponse> Handle( TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken ) {
            var requestName = typeof( TRequest ).Name;
            _logger.LogInformation( "Starting request: {RequestName} at {DateTime}", requestName, DateTime.UtcNow );

            try {
                var response = await next();
                _logger.LogInformation( "Completed request: {RequestName} at {DateTime}", requestName, DateTime.UtcNow );
                return response;
            }
            catch (Exception ex) {
                _logger.LogError( ex, "Request {RequestName} failed at {DateTime}", requestName, DateTime.UtcNow );
                throw;
            }
        }
    }
}
