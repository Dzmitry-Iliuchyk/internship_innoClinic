using Notifications.Domain;

namespace Notifications.Application {
    public interface IEmailSender {
        Task<string> SendEmail( Message message, string nameFrom );
    }

}
