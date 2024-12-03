using FluentValidation;
using Offices.Application.Exceptions;
using System.Net;

namespace Offices.Api.Middleware {
    public class ExceptionHandlingMiddleware: IMiddleware {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware( ILogger<ExceptionHandlingMiddleware> logger ) {
            _logger = logger;
        }

        public async Task InvokeAsync( HttpContext context, RequestDelegate next ) {
            try {
                await next( context );
            }
            catch (OfficeNotFoundException ex) {
                await HandleApplicationException( context, ex, HttpStatusCode.NotFound );
            }
            catch (ValidationException ex) {
                await HandleValidationException( context, ex );
            }
            catch (Exception ex) {
                await HandleApplicationException( context, ex, HttpStatusCode.InternalServerError );
            }
        }

        private async Task HandleApplicationException( HttpContext context, Exception ex, HttpStatusCode ErrorCode ) {
            context.Response.StatusCode = (int)ErrorCode;
            _logger.LogError( ex, "An error occurred in {Context}: {ErrorMessage}: \n\tStackTrace:{StackTrace}", context, ex.Message, ex.StackTrace );

            var errorResponse = new ErrorResponse( ex.GetType().Name, (int)ErrorCode, [ex.Message] );

            await context.Response.WriteAsJsonAsync( errorResponse );
        }
        private async Task HandleValidationException( HttpContext context, ValidationException ex ) {
            int errorCode = (int)HttpStatusCode.BadRequest;
            _logger.LogError( ex, "An error occurred in {Context}: {ErrorMessage}: \n\tStackTrace:{StackTrace}", context, ex.Message, ex.StackTrace );

            var errors = new List<string>();
            foreach (var error in ex.Errors) {
                errors.Add( error.ErrorMessage );
            }
            var errorResponse = new ErrorResponse( ex.GetType().Name, errorCode, errors.ToArray() );

            await context.Response.WriteAsJsonAsync( errorResponse );
        }
    }
}
