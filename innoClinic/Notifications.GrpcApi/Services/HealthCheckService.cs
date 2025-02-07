using Grpc.Core;
using Grpc.Health.V1;

namespace Notifications.GrpcApi.Services {
    public class HealthCheckService: Health.HealthBase {
        public override Task<HealthCheckResponse> Check( HealthCheckRequest request, ServerCallContext context ) {
            Console.WriteLine( $"This is {nameof( HealthCheckService )} Check " );
            //Todo: Check Logic
            return Task.FromResult( new HealthCheckResponse() { Status = HealthCheckResponse.Types.ServingStatus.Serving } );
        }

        public override async Task Watch( HealthCheckRequest request, IServerStreamWriter<HealthCheckResponse> responseStream, ServerCallContext context ) {
            //Todo: Check Logic
            await responseStream.WriteAsync( new HealthCheckResponse() { Status = HealthCheckResponse.Types.ServingStatus.Serving } );
        }
    }
}
