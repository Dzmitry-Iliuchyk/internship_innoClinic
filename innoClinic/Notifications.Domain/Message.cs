    using MimeKit;
namespace Notifications.Domain {

    public class Message {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message( IEnumerable<string> to, string subject, string content ) {
            To = new List<MailboxAddress>();

            To.AddRange( to.Select( x => new MailboxAddress( x.Split( "@" )[ 0 ], x ) ) );
            Subject = subject;
            Content = content;
        }
        public Message( IEnumerable<MailboxAddress> to, string subject, string content ) {
            To = to.ToList();
            Subject = subject;
            Content = content;
        }
    }
}
