using Profiles.Application.Common.Exceptions;
using System.Net;

namespace Profiles.Api.Middleware {
    public class ExceptionHandlingMiddleware: IMiddleware {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware( ILogger<ExceptionHandlingMiddleware> logger ) {
            _logger = logger;
        }

        public async Task InvokeAsync( HttpContext context, RequestDelegate next ) {
            try {
                await next( context );
            }
            catch (NotFoundException ex) {
                await HandleApplicationException( context, ex, HttpStatusCode.NotFound );
            }
            catch (UnauthorizedAccessException ex) {
                await HandleApplicationException( context, ex, HttpStatusCode.Unauthorized );
            }
            catch (ForbiddenAccessException ex) {
                await HandleApplicationException( context, ex, HttpStatusCode.Forbidden );
            }
            catch (Exception ex) {
                await HandleApplicationException( context, ex, HttpStatusCode.InternalServerError );
            }
        }

        private async Task HandleApplicationException( HttpContext context, Exception ex, HttpStatusCode ErrorCode ) {
            context.Response.StatusCode = (int)ErrorCode;
            _logger.LogError( ex, "An error occurred in {Context}: {ErrorMessage}: \n\tStackTrace:{StackTrace}", context, ex.Message, ex.StackTrace );

            var errorResponse = new ErrorResponse( ex.GetType().Name, (int)ErrorCode, [ ex.Message ] );

            await context.Response.WriteAsJsonAsync( errorResponse );
        }
    }
}
