using MassTransit;
using Shared.Events.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents.Application {
    public class Consumer: IConsumer<SaveDocumentRequest> {
        public Task Consume( ConsumeContext<SaveDocumentRequest> context ) {
            throw new NotImplementedException();
        }
    }
}
