using FacadeApi.Exceptions;
using Grpc.Core;
using System.Net;

namespace FacadeApi.Middleware {
    public class ExceptionHandlingMiddleware: IMiddleware {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware( ILogger<ExceptionHandlingMiddleware> logger ) {
            _logger = logger;
        }

        public async Task InvokeAsync( HttpContext context, RequestDelegate next ) {
            try {
                await next( context );
            }
            catch (NotSuccessHttpRequest ex) {
                await HandleHttpRequestException( context, ex);
            }
            catch (RpcException ex) {
                await HandleRpcException( context, ex );
            }
            catch (Exception ex) {
                await HandleException( context, ex );
            }
        }

        private async Task HandleHttpRequestException( HttpContext context, NotSuccessHttpRequest ex ) {
            context.Response.StatusCode = (int)ex.httpResult.StatusCode;
            _logger.LogError( ex, "An error occurred in httpRequest with requestMessage: {RequestMessage}\n\tContext: {Context} \n\t Error Message {ErrorMessage}: \n\tStackTrace:{StackTrace}", ex.httpResult.RequestMessage, context, ex.Message, ex.StackTrace );

            await context.Response.WriteAsJsonAsync( await ex.httpResult.Content.ReadAsStreamAsync() );
        }
        private async Task HandleRpcException( HttpContext context, RpcException ex ) {
            int errorCode = (int)ex.StatusCode;
            context.Response.StatusCode = errorCode;
            _logger.LogError( ex, "An error occurred in rpc request\n\tContext: {Context} \n\t Error Message {ErrorMessage}: \n\tStackTrace:{StackTrace}", context, ex.Message, ex.StackTrace );
            string name = ex.StatusCode.ToString();

            string[] messages = ex.Trailers.Select( t => t.Value ).ToArray();
            if (messages.Length == 0) {
                messages = new string[] { ex.Status.Detail };
            }

            var errorRespomse = new ErrorResponse( name, errorCode, messages );
            await context.Response.WriteAsJsonAsync( errorRespomse );
        }
        private async Task HandleException( HttpContext context, Exception ex ) {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            _logger.LogError( ex, "An error occurred in {Context}: {ErrorMessage}: \n\tStackTrace:{StackTrace}", context, ex.Message, ex.StackTrace );

            var errorResponse = new ErrorResponse( "Oooups", (int)HttpStatusCode.InternalServerError, [ "Something went wrong! " ] );

            await context.Response.WriteAsJsonAsync( errorResponse );
        }
    }
}
