using Notifications.GrpcApi;

public static class Extensions {
    public static Response ToGrpcResponse( this string responce ) {
        return new Response() { Message = responce };
    }
    public static Notifications.Domain.Message ToDomainMessage( this Notifications.GrpcApi.Message responce ) {
        return new Notifications.Domain.Message(responce.To.ToArray(), responce.Subject, responce.Content);
    }
}
