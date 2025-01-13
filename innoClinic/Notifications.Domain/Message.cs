    using MimeKit;
namespace Notifications.Domain {

    public class Message {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string HtmlBodyContent { get; set; }
        public File? File { get; set; }
 
        public Message( IEnumerable<string> to, string subject, string content, File? file = null ) {
            To = new List<MailboxAddress>();

            To.AddRange( to.Select( x => new MailboxAddress( x.Split( "@" )[ 0 ], x ) ) );
            Subject = subject;
            HtmlBodyContent = content;
            File = file;
        }
        public Message( IEnumerable<MailboxAddress> to, string subject, string content, File? file = null ) {
            To = to.ToList();
            Subject = subject;
            HtmlBodyContent = content;
            File = file;
        }
    }
    public class File {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileContent { get; set; }
        public File( string fileName, string fileType, byte[] fileContent ) {
            this.FileName = fileName;
            this.FileType = fileType;
            this.FileContent = fileContent;
        }

    }
}
