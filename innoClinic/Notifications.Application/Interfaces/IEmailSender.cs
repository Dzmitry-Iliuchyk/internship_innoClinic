using Notifications.Domain;

namespace Notifications.Application.Interfaces {
    public interface IEmailSender {
        Task<string> SendEmail( Message message, string nameFrom );
    }

}
