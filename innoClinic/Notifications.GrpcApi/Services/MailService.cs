using Grpc.Core;
using Notifications.Application;

namespace Notifications.GrpcApi.Services {
    public class MailService: GrpcApi.MailService.MailServiceBase {
        private readonly ILogger<MailService> _logger;
        private readonly IEmailSender _sender;
        public MailService( ILogger<MailService> logger, IEmailSender sender ) {
            _logger = logger;
            _sender = sender;   
        }

        public override async Task<Response> SendMessage( Message request, ServerCallContext context ) {
            _logger.LogInformation("Message has sent as {sender} to {To}", request.NameFrom, request.To.ToArray());
            return (await _sender.SendEmail(request.ToDomainMessage(), request.NameFrom)).ToGrpcResponse();
        }
    }

}
