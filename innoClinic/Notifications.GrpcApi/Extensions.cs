using Notifications.GrpcApi;

public static class Extensions {
    public static Response ToGrpcResponse( this string responce ) {
        return new Response() { Message = responce };
    }
    public static Notifications.Domain.Message ToDomainMessage( this Notifications.GrpcApi.Message mes ) {
        var file = mes.File != null ? new Notifications.Domain.File( mes.File.FileName, mes.File.FileType, mes.File.Content.ToArray() ) : null;

        return new Notifications.Domain.Message( mes.To.ToArray(), mes.Subject, mes.Content, file );
    }
}
