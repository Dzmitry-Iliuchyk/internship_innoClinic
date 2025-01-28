    using Grpc.Core;
    using Grpc.Core.Interceptors;
    using Microsoft.Extensions.Logging;

namespace Documents.GrpcApi.Interceptors {

    public class ExceptionHandlingInterceptor: Interceptor {
        private readonly ILogger<ExceptionHandlingInterceptor> _logger;

        public ExceptionHandlingInterceptor( ILogger<ExceptionHandlingInterceptor> logger ) {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation ) {
            try {

                return await continuation( request, context );
            }
            catch (Exception ex) {

                _logger.LogError( ex, "Произошло необработанное исключение." );

                throw new RpcException( new Status( StatusCode.Internal, "Внутренняя ошибка сервера. Пожалуйста, попробуйте позже." ) );
            }
        }

    }

}
