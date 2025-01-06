using Services.Application.Exceptions;
using System.Net;

namespace Services.Api.Middlewares {
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
            catch (BadRequestException ex) {
                await HandleApplicationException( context, ex, HttpStatusCode.BadRequest );
            }
            catch (Exception ex) {
                await HandleException( context, ex);
            }
        }

        private async Task HandleApplicationException( HttpContext context, Exception ex, HttpStatusCode ErrorCode ) {
            context.Response.StatusCode = (int)ErrorCode;
            _logger.LogError( ex, "An error occurred in {Context}: {ErrorMessage}: \n\tStackTrace:{StackTrace}", context, ex.Message, ex.StackTrace );

            var errorResponse = new ErrorResponse( ex.GetType().Name, (int)ErrorCode, [ ex.Message ] );

            await context.Response.WriteAsJsonAsync( errorResponse );
        }
        private async Task HandleException( HttpContext context, Exception ex) {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            _logger.LogError( ex, "An error occurred in {Context}: {ErrorMessage}: \n\tStackTrace:{StackTrace}", context, ex.Message, ex.StackTrace );

            var errorResponse = new ErrorResponse( "Oooups", (int)HttpStatusCode.InternalServerError, [ "Something went wrong! " ] );

            await context.Response.WriteAsJsonAsync( errorResponse );
        }
    }
}
