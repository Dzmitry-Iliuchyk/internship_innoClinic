using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Common.Behavior {
    public class LoggingPipelineBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> {
        private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

        public LoggingPipelineBehavior( ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger ) {
            _logger = logger;
        }

        public async Task<TResponse> Handle( TRequest request,
                                             RequestHandlerDelegate<TResponse> next,
                                             CancellationToken cancellationToken ) {
            var requestName = typeof( TRequest ).Name;
            _logger.LogInformation( "Starting request: {RequestName} ", requestName);

            try {
                var startTimeStamp = Stopwatch.GetTimestamp();
                var response = await next();
                _logger.LogInformation( "Completed request: {RequestName} and elapsed time is: {time} ",
                    requestName, Stopwatch.GetElapsedTime( startTimeStamp ) );
                return response;
            }
            catch (Exception ex) {
                _logger.LogError( ex, "Request {RequestName} failed", requestName );
                throw;
            }
        }
    }
}
