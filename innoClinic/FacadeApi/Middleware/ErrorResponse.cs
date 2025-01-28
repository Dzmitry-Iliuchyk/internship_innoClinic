namespace FacadeApi.Middleware {
    public class ErrorResponse {
        public string Name { get; set; }
        public int ErrorCode { get; set; }
        public string[] Messages { get; set; }

        public ErrorResponse( string name, int errorCode, string[] messages ) {
            this.Name = name;
            this.ErrorCode = errorCode;
            this.Messages = messages;
        }
    }
}
