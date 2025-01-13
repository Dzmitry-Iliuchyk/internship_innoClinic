using MassTransit;
using Notifications.Application.Interfaces;
using Notifications.Domain;
using Shared.Events.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Application.Consumers {
    public class SendEmailConsumer: IConsumer<SendEmailRequest> {
        private readonly IEmailSender _sender;

        public SendEmailConsumer( IEmailSender sender ) {
            this._sender = sender;
        }

        public async Task Consume( ConsumeContext<SendEmailRequest> context ) {
            var file = context.Message.File != null ? new Domain.File( context.Message.File.FileName, context.Message.File.FileContentType, context.Message.File.FileContent ): null;
            var result = await _sender.SendEmail(new Message( context.Message.To, context.Message.Subject, context.Message.TextContent, file), context.Message.NameFrom);
            await context.RespondAsync(new SendEmailResponse() { Message = result } );
        }
    }
}
