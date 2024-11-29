using Authorization.Application.Exceptions;
using FluentValidation;
using System.Net;

namespace Authorization.Api.Middleware {
    public class ExceptionHandlingMiddleware: IMiddleware {
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
            catch (ValidationException ex) {
                await HandleValidationException( context, ex );
            }
            catch (Exception ex) {
                await HandleApplicationException( context, ex, HttpStatusCode.InternalServerError );
            }
        }

        private static async Task HandleApplicationException( HttpContext context, Exception ex, HttpStatusCode ErrorCode ) {
            context.Response.StatusCode = (int)ErrorCode;

            var errorResponse = new ErrorResponse( ex.GetType().Name, (int)ErrorCode,  ex.Message  );

            await context.Response.WriteAsJsonAsync( errorResponse );
        }
        private static async Task HandleValidationException( HttpContext context, ValidationException ex ) {
            int errorCode = (int)HttpStatusCode.BadRequest;
            context.Response.StatusCode = errorCode;
            var errors = new List<string>();
            foreach (var error in ex.Errors) {
                errors.Add( error.ErrorMessage );
            }
            var errorResponse = new ErrorResponse( ex.GetType().Name, errorCode, string.Join("\n\r", errors ));

            await context.Response.WriteAsJsonAsync( errorResponse );
        }
    }
}
