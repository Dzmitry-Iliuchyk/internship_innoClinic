namespace Authorization.Api.Middleware {
    public class ErrorResponse {
        public string Name { get; set; }
        public int ErrorCode { get; set; }
        public string Description { get; set; }

        public ErrorResponse( string name, int errorCode, string description ) {
            this.Name = name;
            this.ErrorCode = errorCode;
            this.Description = description;
        }
    }
}
