namespace FacadeApi.Offices.Exceptions {

    public class NotSuccessHttpRequest: Exception {
        public HttpResponseMessage httpResult;

        public NotSuccessHttpRequest( HttpResponseMessage httpResult ) {
            this.httpResult = httpResult;
        }
    }
}
