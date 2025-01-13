using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Notifications.Domain;

namespace Notifications.Application {
    public class EmailSender: IEmailSender {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender( IOptions<EmailConfiguration> emailConfig ) {
            _emailConfig = emailConfig.Value;
        }

        public async Task<string> SendEmail( Message message, string nameFrom ) {
            var emailMessage = CreateEmailMessage( message, nameFrom );

            return await Send( emailMessage );
        }

        private MimeMessage CreateEmailMessage( Message message, string nameFrom ) {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add( new MailboxAddress( nameFrom, _emailConfig.From ) );
            emailMessage.Bcc.AddRange( message.To );
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart( MimeKit.Text.TextFormat.Html ) { Text = message.Content };

            return emailMessage;
        }

        private async Task<string> Send( MimeMessage mailMessage ) {
            using (var client = new SmtpClient()) {
                try {
                    
                    await client.ConnectAsync( _emailConfig.SmtpServer, _emailConfig.Port, true );
                    client.AuthenticationMechanisms.Remove( "XOAUTH2" );
                    await client.AuthenticateAsync( _emailConfig.UserName, _emailConfig.Password );

                    return await client.SendAsync( mailMessage );
                }
                catch {
                    throw;
                }
                finally {
                    await client.DisconnectAsync( true );
                    client.Dispose();
                }
            }
        }
    }

}
